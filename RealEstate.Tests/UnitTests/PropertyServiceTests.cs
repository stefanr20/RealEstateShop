using Xunit;
using Moq;
using FluentAssertions;
using RealEstate.BLL.Services;
using RealEstate.BLL.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Data.Context;
using RealEstate.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace RealEstate.Tests.UnitTests
{
    public class PropertyServiceTests
    {
        private readonly RealEstateDbContext _context;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly PropertyService _propertyService;

        public PropertyServiceTests()
        {
            var options = new DbContextOptionsBuilder<RealEstateDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new RealEstateDbContext(options);
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _propertyService = new PropertyService(_mockUnitOfWork.Object, _context);
        }

        private Property MakeProperty(int id, string title, string city) => new Property
        {
            Id = id,
            Title = title,
            Description = "Test description",
            Price = "€100,000",
            Photo = "photo.jpg",
            Type = "Apartment",
            HeatingType = "Central",
            Bedrooms = 2,
            Bathrooms = 1,
            Area = 80,
            Address = new Address { City = city, Street = "Test St", Country = "MK" }
        };

        [Fact]
        public void GetAll_ReturnsAllProperties()
        {
            // Arrange
            _context.Properties.AddRange(
                MakeProperty(1, "Luxury Apartment", "Skopje"),
                MakeProperty(2, "Modern Villa", "Ohrid")
            );
            _context.SaveChanges();

            // Act
            var result = _propertyService.GetAll();

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetById_ValidId_ReturnsProperty()
        {
            // Arrange
            _context.Properties.Add(MakeProperty(10, "Test Property", "Skopje"));
            _context.SaveChanges();

            // Act
            var result = _propertyService.GetById(10);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Test Property");
        }

        [Fact]
        public void GetById_InvalidId_ReturnsNull()
        {
            // Act
            var result = _propertyService.GetById(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Create_ValidProperty_CallsInsertAndComplete()
        {
            // Arrange
            var property = MakeProperty(0, "New Property", "Skopje");
            _mockUnitOfWork.Setup(u => u.Properties.Insert(property));
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _propertyService.Create(property);

            // Assert
            _mockUnitOfWork.Verify(u => u.Properties.Insert(property), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_ValidId_CallsDeleteAndComplete()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Properties.Delete(1));
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _propertyService.Delete(1);

            // Assert
            _mockUnitOfWork.Verify(u => u.Properties.Delete(1), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public void Search_ByCity_ReturnsMatchingProperties()
        {
            // Arrange
            _context.Properties.AddRange(
                MakeProperty(20, "Skopje Flat", "Skopje"),
                MakeProperty(21, "Ohrid House", "Ohrid")
            );
            _context.SaveChanges();

            // Act
            var result = _propertyService.Search("skopje", null, null, null, null);

            // Assert
            result.Should().HaveCount(1);
            result.First().Title.Should().Be("Skopje Flat");
        }

        [Fact]
        public void Search_EmptyQuery_ReturnsAllProperties()
        {
            // Arrange
            _context.Properties.AddRange(
                MakeProperty(30, "Property A", "Skopje"),
                MakeProperty(31, "Property B", "Bitola")
            );
            _context.SaveChanges();

            // Act
            var result = _propertyService.Search(null, null, null, null, null);

            // Assert
            result.Should().HaveCount(2);
        }
    }
}