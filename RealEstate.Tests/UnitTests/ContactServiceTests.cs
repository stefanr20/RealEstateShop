using Xunit;
using Moq;
using FluentAssertions;
using RealEstate.BLL.Services;
using RealEstate.Domain.Entities;
using RealEstate.Data.UnitOfWork;
using RealEstate.Data.Repositories;

namespace RealEstate.Tests.UnitTests
{
    public class ContactServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<Contact>> _mockContactRepo;
        private readonly ContactService _contactService;

        public ContactServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockContactRepo = new Mock<IGenericRepository<Contact>>();
            _mockUnitOfWork.Setup(u => u.Contacts).Returns(_mockContactRepo.Object);
            _contactService = new ContactService(_mockUnitOfWork.Object);
        }

        private Contact MakeContact(int id, string email) => new Contact
        {
            Id = id,
            Email = email,
            Phone = "070000000"
        };

        [Fact]
        public void GetAll_ReturnsAllContacts()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                MakeContact(1, "stefan@test.com"),
                MakeContact(2, "ana@test.com"),
                MakeContact(3, "marko@test.com")
            };
            _mockContactRepo.Setup(r => r.GetAll()).Returns(contacts);

            // Act
            var result = _contactService.GetAll();

            // Assert
            result.Should().HaveCount(3);
        }

        [Fact]
        public void GetById_ValidId_ReturnsContact()
        {
            // Arrange
            var contact = MakeContact(1, "stefan@test.com");
            _mockContactRepo.Setup(r => r.GetById(1)).Returns(contact);

            // Act
            var result = _contactService.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be("stefan@test.com");
        }

        [Fact]
        public void GetById_InvalidId_ReturnsNull()
        {
            // Arrange
            _mockContactRepo.Setup(r => r.GetById(999)).Returns((Contact)null);

            // Act
            var result = _contactService.GetById(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Create_ValidContact_CallsInsertAndComplete()
        {
            // Arrange
            var contact = MakeContact(0, "stefan@test.com");
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _contactService.Create(contact);

            // Assert
            _mockContactRepo.Verify(r => r.Insert(contact), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_ValidContact_CallsUpdateAndComplete()
        {
            // Arrange
            var contact = MakeContact(1, "stefan@test.com");
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _contactService.Update(contact);

            // Assert
            _mockContactRepo.Verify(r => r.Update(contact), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_ValidId_CallsDeleteAndComplete()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _contactService.Delete(1);

            // Assert
            _mockContactRepo.Verify(r => r.Delete(1), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public void GetAll_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            _mockContactRepo.Setup(r => r.GetAll()).Returns(new List<Contact>());

            // Act
            var result = _contactService.GetAll();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
