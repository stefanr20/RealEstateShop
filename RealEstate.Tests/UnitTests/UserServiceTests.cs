using Xunit;
using Moq;
using FluentAssertions;
using RealEstate.BLL.Services;
using RealEstate.Domain.Entities;
using RealEstate.Data.UnitOfWork;
using RealEstate.Data.Repositories;

namespace RealEstate.Tests.UnitTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<User>> _mockUserRepo;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepo = new Mock<IGenericRepository<User>>();
            _mockUnitOfWork.Setup(u => u.Users).Returns(_mockUserRepo.Object);
            _userService = new UserService(_mockUnitOfWork.Object);
        }

        private User MakeUser(int id, string username, string plainPassword, string role = "user") => new User
        {
            Id = id,
            Username = username,
            Email = $"{username}@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword(plainPassword),
            Role = role
        };

        [Fact]
        public void GetAll_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                MakeUser(1, "stefan", "pass123"),
                MakeUser(2, "ana", "pass456")
            };
            _mockUserRepo.Setup(r => r.GetAll()).Returns(users);

            // Act
            var result = _userService.GetAll();

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetById_ValidId_ReturnsUser()
        {
            // Arrange
            var user = MakeUser(1, "stefan", "pass123");
            _mockUserRepo.Setup(r => r.GetById(1)).Returns(user);

            // Act
            var result = _userService.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("stefan");
        }

        [Fact]
        public void GetById_InvalidId_ReturnsNull()
        {
            // Arrange
            _mockUserRepo.Setup(r => r.GetById(999)).Returns((User)null);

            // Act
            var result = _userService.GetById(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Create_HashesPasswordBeforeInserting()
        {
            // Arrange
            var user = new User { Id = 1, Username = "stefan", Email = "stefan@test.com", Password = "plainpassword", Role = "user" };
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _userService.Create(user);

            // Assert
            user.Password.Should().NotBe("plainpassword");
            BCrypt.Net.BCrypt.Verify("plainpassword", user.Password).Should().BeTrue();
            _mockUserRepo.Verify(r => r.Insert(user), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_ValidId_CallsDeleteAndComplete()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _userService.Delete(1);

            // Assert
            _mockUserRepo.Verify(r => r.Delete(1), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var users = new List<User> { MakeUser(1, "stefan", "pass123") };
            _mockUserRepo.Setup(r => r.GetAll()).Returns(users);

            // Act
            var result = _userService.Login("stefan", "pass123");

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("stefan");
        }

        [Fact]
        public void Login_WrongPassword_ReturnsNull()
        {
            // Arrange
            var users = new List<User> { MakeUser(1, "stefan", "pass123") };
            _mockUserRepo.Setup(r => r.GetAll()).Returns(users);

            // Act
            var result = _userService.Login("stefan", "wrongpassword");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Login_NonExistentUser_ReturnsNull()
        {
            // Arrange
            _mockUserRepo.Setup(r => r.GetAll()).Returns(new List<User>());

            // Act
            var result = _userService.Login("nobody", "pass123");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void UsernameExists_ExistingUsername_ReturnsTrue()
        {
            // Arrange
            var users = new List<User> { MakeUser(1, "stefan", "pass123") };
            _mockUserRepo.Setup(r => r.GetAll()).Returns(users);

            // Act
            var result = _userService.UsernameExists("stefan");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void UsernameExists_NonExistentUsername_ReturnsFalse()
        {
            // Arrange
            _mockUserRepo.Setup(r => r.GetAll()).Returns(new List<User>());

            // Act
            var result = _userService.UsernameExists("nobody");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateRole_ValidId_UpdatesRoleAndSaves()
        {
            // Arrange
            var user = MakeUser(1, "stefan", "pass123", "user");
            _mockUserRepo.Setup(r => r.GetById(1)).Returns(user);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _userService.UpdateRole(1, "admin");

            // Assert
            user.Role.Should().Be("admin");
            _mockUserRepo.Verify(r => r.Update(user), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateRole_InvalidId_DoesNothing()
        {
            // Arrange
            _mockUserRepo.Setup(r => r.GetById(999)).Returns((User)null);

            // Act
            await _userService.UpdateRole(999, "admin");

            // Assert
            _mockUserRepo.Verify(r => r.Update(It.IsAny<User>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }
    }
}