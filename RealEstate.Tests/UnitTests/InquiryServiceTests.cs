using Xunit;
using Moq;
using FluentAssertions;
using RealEstate.BLL.Services;
using RealEstate.Domain.Entities;
using RealEstate.Data.Context;
using RealEstate.Data.UnitOfWork;
using RealEstate.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace RealEstate.Tests.UnitTests
{
    public class InquiryServiceTests
    {
        private readonly RealEstateDbContext _context;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<Inquiry>> _mockInquiryRepo;
        private readonly InquiryService _inquiryService;

        public InquiryServiceTests()
        {
            var options = new DbContextOptionsBuilder<RealEstateDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new RealEstateDbContext(options);
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockInquiryRepo = new Mock<IGenericRepository<Inquiry>>();
            _mockUnitOfWork.Setup(u => u.Inquiries).Returns(_mockInquiryRepo.Object);
            _inquiryService = new InquiryService(_mockUnitOfWork.Object, _context);
        }

        private Inquiry MakeInquiry(int id, string name) => new Inquiry
        {
            Id = id,
            Name = name,
            Email = "test@test.com",
            Phone = "070000000",
            Message = "Test message"
        };

        private Property MakeProperty(int id, string title) => new Property
        {
            Id = id,
            Title = title,
            Description = "Desc",
            Price = "€100,000",
            Photo = "photo.jpg",
            Type = "Apartment",
            HeatingType = "Central",
            Bedrooms = 2,
            Bathrooms = 1,
            Area = 80
        };

        [Fact]
        public void GetAll_ReturnsAllInquiries()
        {
            // Arrange
            var inquiries = new List<Inquiry>
            {
                MakeInquiry(1, "Stefan"),
                MakeInquiry(2, "Ana")
            };
            _mockInquiryRepo.Setup(r => r.GetAll()).Returns(inquiries);

            // Act
            var result = _inquiryService.GetAll();

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Create_ValidInquiry_CallsInsertAndComplete()
        {
            // Arrange
            var inquiry = MakeInquiry(0, "Stefan");
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _inquiryService.Create(inquiry);

            // Assert
            _mockInquiryRepo.Verify(r => r.Insert(inquiry), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public void GetAllWithProperties_ReturnsInquiriesWithProperties()
        {
            // Arrange
            _context.Properties.Add(MakeProperty(1, "Test Property"));
            _context.Inquiries.Add(new Inquiry
            {
                Id = 1,
                Name = "Stefan",
                Email = "stefan@test.com",
                Phone = "070000000",
                Message = "Interested",
                PropertyId = 1
            });
            _context.SaveChanges();

            // Act
            var result = _inquiryService.GetAllWithProperties();

            // Assert
            result.Should().HaveCount(1);
            result.First().Property.Should().NotBeNull();
            result.First().Property.Title.Should().Be("Test Property");
        }

        [Fact]
        public async Task Reply_ValidId_UpdatesAdminReply()
        {
            // Arrange
            _context.Inquiries.Add(MakeInquiry(5, "Stefan"));
            _context.SaveChanges();

            // Act
            await _inquiryService.Reply(5, "Thank you for your inquiry!");

            // Assert
            var updated = _context.Inquiries.Find(5);
            updated.AdminReply.Should().Be("Thank you for your inquiry!");
            updated.RepliedAt.Should().NotBeNull();
        }

        [Fact]
        public async Task Reply_InvalidId_DoesNotThrow()
        {
            // Act
            var act = async () => await _inquiryService.Reply(999, "Reply");

            // Assert
            await act.Should().NotThrowAsync();
        }
    }
}