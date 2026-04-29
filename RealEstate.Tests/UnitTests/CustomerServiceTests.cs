using Xunit;
using Moq;
using FluentAssertions;
using RealEstate.BLL.Services;
using RealEstate.Domain.Entities;
using RealEstate.Data.UnitOfWork;
using RealEstate.Data.Repositories;

namespace RealEstate.Tests.UnitTests
{
    public class CustomerServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<Customer>> _mockCustomerRepo;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCustomerRepo = new Mock<IGenericRepository<Customer>>();
            _mockUnitOfWork.Setup(u => u.Customers).Returns(_mockCustomerRepo.Object);
            _customerService = new CustomerService(_mockUnitOfWork.Object);
        }

        private Customer MakeCustomer(int id, string firstName, string lastName) => new Customer
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            Email = $"{firstName.ToLower()}@test.com",
            Phone = "070000000"
        };

        [Fact]
        public void GetAll_ReturnsAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                MakeCustomer(1, "Stefan", "Ristevski"),
                MakeCustomer(2, "Ana", "Stoeva"),
                MakeCustomer(3, "Marko", "Popov")
            };
            _mockCustomerRepo.Setup(r => r.GetAll()).Returns(customers);

            // Act
            var result = _customerService.GetAll();

            // Assert
            result.Should().HaveCount(3);
        }

        [Fact]
        public void GetById_ValidId_ReturnsCustomer()
        {
            // Arrange
            var customer = MakeCustomer(1, "Stefan", "Ristevski");
            _mockCustomerRepo.Setup(r => r.GetById(1)).Returns(customer);

            // Act
            var result = _customerService.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("Stefan");
        }

        [Fact]
        public void GetById_InvalidId_ReturnsNull()
        {
            // Arrange
            _mockCustomerRepo.Setup(r => r.GetById(999)).Returns((Customer)null);

            // Act
            var result = _customerService.GetById(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Create_ValidCustomer_CallsInsertAndComplete()
        {
            // Arrange
            var customer = MakeCustomer(0, "Stefan", "Ristevski");
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _customerService.Create(customer);

            // Assert
            _mockCustomerRepo.Verify(r => r.Insert(customer), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_ValidCustomer_CallsUpdateAndComplete()
        {
            // Arrange
            var customer = MakeCustomer(1, "Stefan", "Ristevski");
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _customerService.Update(customer);

            // Assert
            _mockCustomerRepo.Verify(r => r.Update(customer), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_ValidId_CallsDeleteAndComplete()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _customerService.Delete(1);

            // Assert
            _mockCustomerRepo.Verify(r => r.Delete(1), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public void GetAll_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            _mockCustomerRepo.Setup(r => r.GetAll()).Returns(new List<Customer>());

            // Act
            var result = _customerService.GetAll();

            // Assert
            result.Should().BeEmpty();
        }
    }
}