using Xunit;
using FluentAssertions;
using Shared.DTOS.AuthDTO;

namespace Charity_BE.Tests.ControllerTests
{
    public class AuthenticationControllerTests
    {
        [Fact]
        public void RegisterDTO_WithValidData_ShouldBeValid()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                FullName = "Test User",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Password = "Test123!",
                ConfirmPassword = "Test123!"
            };

            // Act & Assert
            registerDto.Should().NotBeNull();
            registerDto.FullName.Should().Be("Test User");
            registerDto.Email.Should().Be("test@example.com");
            registerDto.Password.Should().Be("Test123!");
            registerDto.ConfirmPassword.Should().Be("Test123!");
        }

        [Fact]
        public void LoginDTO_WithValidData_ShouldBeValid()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Email = "test@example.com",
                Password = "Test123!"
            };

            // Act & Assert
            loginDto.Should().NotBeNull();
            loginDto.Email.Should().Be("test@example.com");
            loginDto.Password.Should().Be("Test123!");
        }

        [Fact]
        public void RegisterAdminDTO_WithValidData_ShouldBeValid()
        {
            // Arrange
            var registerAdminDto = new RegisterAdminDTO
            {
                FullName = "Admin User",
                Email = "admin@example.com",
                PhoneNumber = "1234567890",
                Department = "IT",
                Position = "Manager",
                Password = "Admin123!",
                ConfirmPassword = "Admin123!"
            };

            // Act & Assert
            registerAdminDto.Should().NotBeNull();
            registerAdminDto.FullName.Should().Be("Admin User");
            registerAdminDto.Department.Should().Be("IT");
            registerAdminDto.Position.Should().Be("Manager");
        }

        [Fact]
        public void RegisterAdvisorDTO_WithValidData_ShouldBeValid()
        {
            // Arrange
            var registerAdvisorDto = new RegisterAdvisorDTO
            {
                FullName = "Advisor User",
                Email = "advisor@example.com",
                PhoneNumber = "1234567890",
                Specialty = "Technology",
                Description = "Expert in technology",
                ZoomRoomUrl = "https://zoom.us/j/123456",
                Password = "Advisor123!",
                ConfirmPassword = "Advisor123!"
            };

            // Act & Assert
            registerAdvisorDto.Should().NotBeNull();
            registerAdvisorDto.FullName.Should().Be("Advisor User");
            registerAdvisorDto.Specialty.Should().Be("Technology");
            registerAdvisorDto.ZoomRoomUrl.Should().Be("https://zoom.us/j/123456");
        }

        [Fact]
        public void AuthResponseDTO_WithValidData_ShouldBeValid()
        {
            // Arrange
            var authResponseDto = new AuthResponseDTO
            {
                FullName = "Test User",
                Email = "test@example.com",
                Role = "Client",
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
            };

            // Act & Assert
            authResponseDto.Should().NotBeNull();
            authResponseDto.FullName.Should().Be("Test User");
            authResponseDto.Email.Should().Be("test@example.com");
            authResponseDto.Role.Should().Be("Client");
            authResponseDto.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void CurrentUserDTO_WithValidData_ShouldBeValid()
        {
            // Arrange
            var currentUserDto = new CurrentUserDTO
            {
                Id = "1",
                FullName = "Test User",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Address = "Test Address",
                Role = new List<string> { "Client" }
            };

            // Act & Assert
            currentUserDto.Should().NotBeNull();
            currentUserDto.Id.Should().Be("1");
            currentUserDto.FullName.Should().Be("Test User");
            currentUserDto.Email.Should().Be("test@example.com");
            currentUserDto.Role.Should().Contain("Client");
        }
    }
} 