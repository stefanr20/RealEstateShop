using Xunit;
using Moq;
using FluentAssertions;
using RealEstate.BLL.Services;
using RealEstate.Domain.Entities;
using RealEstate.Data.UnitOfWork;
using RealEstate.Data.Repositories;

namespace RealEstate.Tests.UnitTests
{
    public class AddressServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<Address>> _mockAddressRepo;
        private readonly AddressService _addressService;

        public AddressServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockAddressRepo = new Mock<IGenericRepository<Address>>();
            _mockUnitOfWork.Setup(u => u.Addresses).Returns(_mockAddressRepo.Object);
            _addressService = new AddressService(_mockUnitOfWork.Object);
        }

        private Address MakeAddress(int id, string city) => new Address
        {
            Id = id,
            City = city,
            Street = "Test Street",
            Country = "MK"
        };

        [Fact]
        public void GetAll_ReturnsAllAddresses()
        {
            // Arrange
            var addresses = new List<Address>
            {
                MakeAddress(1, "Skopje"),
                MakeAddress(2, "Ohrid"),
                MakeAddress(3, "Bitola")
            };
            _mockAddressRepo.Setup(r => r.GetAll()).Returns(addresses);

            // Act
            var result = _addressService.GetAll();

            // Assert
            result.Should().HaveCount(3);
        }

        [Fact]
        public void GetById_ValidId_ReturnsAddress()
        {
            // Arrange
            var address = MakeAddress(1, "Skopje");
            _mockAddressRepo.Setup(r => r.GetById(1)).Returns(address);

            // Act
            var result = _addressService.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.City.Should().Be("Skopje");
        }

        [Fact]
        public void GetById_InvalidId_ReturnsNull()
        {
            // Arrange
            _mockAddressRepo.Setup(r => r.GetById(999)).Returns((Address)null);

            // Act
            var result = _addressService.GetById(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Create_ValidAddress_CallsInsertAndComplete()
        {
            // Arrange
            var address = MakeAddress(0, "Skopje");
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _addressService.Create(address);

            // Assert
            _mockAddressRepo.Verify(r => r.Insert(address), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_ValidAddress_CallsUpdateAndComplete()
        {
            // Arrange
            var address = MakeAddress(1, "Skopje");
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _addressService.Update(address);

            // Assert
            _mockAddressRepo.Verify(r => r.Update(address), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_ValidId_CallsDeleteAndComplete()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _addressService.Delete(1);

            // Assert
            _mockAddressRepo.Verify(r => r.Delete(1), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public void GetAll_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            _mockAddressRepo.Setup(r => r.GetAll()).Returns(new List<Address>());

            // Act
            var result = _addressService.GetAll();

            // Assert
            result.Should().BeEmpty();
        }
    }
}