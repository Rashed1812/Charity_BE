using Xunit;
using FluentAssertions;
using DAL.Data.Models.IdentityModels;
using DAL.Data.Models;
using Shared.DTOS.ComplaintDTOs;

namespace Charity_BE.Tests.SimpleTests
{
    public class ModelTests
    {
        [Fact]
        public void ApplicationUser_Constructor_ShouldCreateValidUser()
        {
            // Arrange & Act
            var user = new ApplicationUser
            {
                Id = "1",
                FullName = "Test User",
                Email = "test@example.com",
                UserName = "test@example.com",
                PhoneNumber = "1234567890",
                Address = "Test Address",
                IsActive = true
            };

            // Assert
            user.Should().NotBeNull();
            user.Id.Should().Be("1");
            user.FullName.Should().Be("Test User");
            user.Email.Should().Be("test@example.com");
            user.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Admin_Constructor_ShouldCreateValidAdmin()
        {
            // Arrange & Act
            var admin = new Admin
            {
                Id = 1,
                UserId = "1",
                FullName = "Admin User",
                Email = "admin@example.com",
                PhoneNumber = "1234567890",
                IsActive = true
            };

            // Assert
            admin.Should().NotBeNull();
            admin.Id.Should().Be(1);
            admin.FullName.Should().Be("Admin User");
            admin.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Advisor_Constructor_ShouldCreateValidAdvisor()
        {
            // Arrange & Act
            var advisor = new Advisor
            {
                Id = 1,
                UserId = "1",
                FullName = "Advisor User",
                Specialty = "Technology",
                Description = "Expert in technology",
                ZoomRoomUrl = "https://zoom.us/j/123456",
                PhoneNumber = "1234567890",
                Email = "advisor@example.com",
                IsActive = true
            };

            // Assert
            advisor.Should().NotBeNull();
            advisor.Id.Should().Be(1);
            advisor.FullName.Should().Be("Advisor User");
            advisor.Specialty.Should().Be("Technology");
            advisor.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Consultation_Constructor_ShouldCreateValidConsultation()
        {
            // Arrange & Act
            var consultation = new Consultation
            {
                Id = 1,
                ConsultationName = "Test Consultation",
                Description = "Test Description",
                IsActive = true
            };

            // Assert
            consultation.Should().NotBeNull();
            consultation.Id.Should().Be(1);
            consultation.ConsultationName.Should().Be("Test Consultation");
            consultation.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Complaint_Constructor_ShouldCreateValidComplaint()
        {
            // Arrange & Act
            var complaint = new Complaint
            {
                Id = 1,
                UserId = "1",
                Title = "Test Complaint",
                Description = "Test Description",
                Category = ComplaintCategory.Other,
                Status = ComplaintStatus.Pending,
                Priority = "Medium"
            };

            // Assert
            complaint.Should().NotBeNull();
            complaint.Id.Should().Be(1);
            complaint.Title.Should().Be("Test Complaint");
            complaint.Status.Should().Be(ComplaintStatus.Pending);
            complaint.Priority.Should().Be("Medium");
        }
    }
} 