using Xunit;
using FluentAssertions;
using Shared.DTOS.ConsultationDTOs;

namespace Charity_BE.Tests.ControllerTests
{
    public class ConsultationControllerTests
    {
        [Fact]
        public void ConsultationDTO_WithValidData_ShouldBeValid()
        {
            // Arrange
            var consultationDto = new ConsultationDTO
            {
                Id = 1,
                ConsultationName = "Test Consultation",
                Description = "Test Description",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Act & Assert
            consultationDto.Should().NotBeNull();
            consultationDto.Id.Should().Be(1);
            consultationDto.ConsultationName.Should().Be("Test Consultation");
            consultationDto.Description.Should().Be("Test Description");
            consultationDto.IsActive.Should().BeTrue();
        }

        [Fact]
        public void CreateConsultationDTO_WithValidData_ShouldBeValid()
        {
            // Arrange
            var createConsultationDto = new CreateConsultationDTO
            {
                ConsultationName = "New Consultation"
            };

            // Act & Assert
            createConsultationDto.Should().NotBeNull();
            createConsultationDto.ConsultationName.Should().Be("New Consultation");
        }

        [Fact]
        public void UpdateConsultationDTO_WithValidData_ShouldBeValid()
        {
            // Arrange
            var updateConsultationDto = new UpdateConsultationDTO
            {
                ConsultationName = "Updated Consultation",
                Description = "Updated Description",
                IsActive = false
            };

            // Act & Assert
            updateConsultationDto.Should().NotBeNull();
            updateConsultationDto.ConsultationName.Should().Be("Updated Consultation");
            updateConsultationDto.Description.Should().Be("Updated Description");
            updateConsultationDto.IsActive.Should().BeFalse();
        }

        [Fact]
        public void UpdateConsultationDTO_WithPartialData_ShouldBeValid()
        {
            // Arrange
            var updateConsultationDto = new UpdateConsultationDTO
            {
                ConsultationName = "Updated Consultation"
            };

            // Act & Assert
            updateConsultationDto.Should().NotBeNull();
            updateConsultationDto.ConsultationName.Should().Be("Updated Consultation");
            updateConsultationDto.Description.Should().BeNull();
            updateConsultationDto.IsActive.Should().BeNull();
        }

        [Fact]
        public void ConsultationDTO_WithNullDescription_ShouldBeValid()
        {
            // Arrange
            var consultationDto = new ConsultationDTO
            {
                Id = 1,
                ConsultationName = "Test Consultation",
                Description = null,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            // Act & Assert
            consultationDto.Should().NotBeNull();
            consultationDto.Id.Should().Be(1);
            consultationDto.Description.Should().BeNull();
            consultationDto.UpdatedAt.Should().BeNull();
        }
    }
} 