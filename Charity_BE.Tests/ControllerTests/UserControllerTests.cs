using Xunit;
using FluentAssertions;
using Shared.DTOS.UserDTO;

namespace Charity_BE.Tests.ControllerTests
{
    public class UserControllerTests
    {
        [Fact]
        public void UserDTO_WithValidData_ShouldBeValid()
        {
            // Arrange
            var userDto = new UserDTO
            {
                Id = "1",
                FullName = "Test User",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Address = "Test Address",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Roles = new List<string> { "Client" }
            };

            // Act & Assert
            userDto.Should().NotBeNull();
            userDto.Id.Should().Be("1");
            userDto.FullName.Should().Be("Test User");
            userDto.Email.Should().Be("test@example.com");
            userDto.IsActive.Should().BeTrue();
            userDto.Roles.Should().Contain("Client");
        }

        [Fact]
        public void UpdateUserDTO_WithValidData_ShouldBeValid()
        {
            // Arrange
            var updateUserDto = new UpdateUserDTO
            {
                FullName = "Updated User",
                Email = "updated@example.com",
                PhoneNumber = "9876543210",
                Address = "Updated Address"
            };

            // Act & Assert
            updateUserDto.Should().NotBeNull();
            updateUserDto.FullName.Should().Be("Updated User");
            updateUserDto.Email.Should().Be("updated@example.com");
            updateUserDto.PhoneNumber.Should().Be("9876543210");
            updateUserDto.Address.Should().Be("Updated Address");
        }

        [Fact]
        public void UpdateUserDTO_WithPartialData_ShouldBeValid()
        {
            // Arrange
            var updateUserDto = new UpdateUserDTO
            {
                FullName = "Updated User"
            };

            // Act & Assert
            updateUserDto.Should().NotBeNull();
            updateUserDto.FullName.Should().Be("Updated User");
            updateUserDto.Email.Should().BeNull();
            updateUserDto.PhoneNumber.Should().BeNull();
            updateUserDto.Address.Should().BeNull();
        }

        [Fact]
        public void UserDTO_WithNullRoles_ShouldBeValid()
        {
            // Arrange
            var userDto = new UserDTO
            {
                Id = "1",
                FullName = "Test User",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Address = "Test Address",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Roles = null
            };

            // Act & Assert
            userDto.Should().NotBeNull();
            userDto.Id.Should().Be("1");
            userDto.Roles.Should().BeNull();
        }

        [Fact]
        public void UserDTO_WithEmptyRoles_ShouldBeValid()
        {
            // Arrange
            var userDto = new UserDTO
            {
                Id = "1",
                FullName = "Test User",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Address = "Test Address",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Roles = new List<string>()
            };

            // Act & Assert
            userDto.Should().NotBeNull();
            userDto.Id.Should().Be("1");
            userDto.Roles.Should().BeEmpty();
        }
    }
} 