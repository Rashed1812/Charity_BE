using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComplaintMessages_AspNetUsers_SenderId",
                table: "ComplaintMessages");

            migrationBuilder.DropColumn(
                name: "AreaOfInterest",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "RedirectUrl",
                table: "ServiceOfferings");

            migrationBuilder.DropColumn(
                name: "MessageText",
                table: "ComplaintMessages");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "AdviceRequests");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "VolunteerApplications",
                newName: "Education");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "ServiceOfferings",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PublishDate",
                table: "NewsItems",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "NewsItems",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "SentAt",
                table: "ComplaintMessages",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "ComplaintMessages",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IsFromAdmin",
                table: "ComplaintMessages",
                newName: "IsAdmin");

            migrationBuilder.RenameIndex(
                name: "IX_ComplaintMessages_SenderId",
                table: "ComplaintMessages",
                newName: "IX_ComplaintMessages_UserId");

            migrationBuilder.RenameColumn(
                name: "IsBooked",
                table: "AdvisorAvailabilities",
                newName: "IsAvailable");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "AdvisorAvailabilities",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "AppointmentTime",
                table: "AdviceRequests",
                newName: "RequestDate");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "VolunteerApplications",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "VolunteerApplications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "VolunteerApplications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "VolunteerApplications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppliedAt",
                table: "VolunteerApplications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Availability",
                table: "VolunteerApplications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "VolunteerApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "VolunteerApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "VolunteerApplications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Experience",
                table: "VolunteerApplications",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "VolunteerApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "VolunteerApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Motivation",
                table: "VolunteerApplications",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewedAt",
                table: "VolunteerApplications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewedBy",
                table: "VolunteerApplications",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "VolunteerApplications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "ServiceOfferings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ServiceOfferings",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ServiceOfferings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ClickCount",
                table: "ServiceOfferings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContactInfo",
                table: "ServiceOfferings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cost",
                table: "ServiceOfferings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ServiceOfferings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "ServiceOfferings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Requirements",
                table: "ServiceOfferings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ServiceOfferings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "NewsItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "NewsItems",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "NewsItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "NewsItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "NewsItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "NewsItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "NewsItems",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "NewsItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "NewsItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ConsultationName",
                table: "Consultations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Consultations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Consultations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Consultations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Consultations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Complaints",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Complaints",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Complaints",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Complaints",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Resolution",
                table: "Complaints",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResolvedAt",
                table: "Complaints",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Complaints",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "ComplaintMessages",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ZoomRoomUrl",
                table: "Advisors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Advisors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Advisors",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ConsultationId",
                table: "Advisors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Advisors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Advisors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Advisors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Advisors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Advisors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "AdvisorAvailabilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "AdvisorAvailabilities",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AdvisorAvailabilities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdvisorId",
                table: "AdviceRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                table: "AdviceRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedDate",
                table: "AdviceRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AdviceRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AdviceRequests",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "AdviceRequests",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "AdviceRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Response",
                table: "AdviceRequests",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "AdviceRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AdviceRequests",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Admins",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Admins",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Admins",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Admins",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Admins",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Admins",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Admins",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Speaker = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    FileFormat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    DownloadCount = table.Column<int>(type: "int", nullable: false),
                    ConsultationId = table.Column<int>(type: "int", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lectures_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lectures_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lectures_Consultations_ConsultationId",
                        column: x => x.ConsultationId,
                        principalTable: "Consultations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_ApplicationUserId",
                table: "Lectures",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_ConsultationId",
                table: "Lectures",
                column: "ConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_CreatedBy",
                table: "Lectures",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintMessages_AspNetUsers_UserId",
                table: "ComplaintMessages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComplaintMessages_AspNetUsers_UserId",
                table: "ComplaintMessages");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "AppliedAt",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "Availability",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "City",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "Experience",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "Motivation",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "ReviewedAt",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "ReviewedBy",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "VolunteerApplications");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "ServiceOfferings");

            migrationBuilder.DropColumn(
                name: "ClickCount",
                table: "ServiceOfferings");

            migrationBuilder.DropColumn(
                name: "ContactInfo",
                table: "ServiceOfferings");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "ServiceOfferings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ServiceOfferings");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "ServiceOfferings");

            migrationBuilder.DropColumn(
                name: "Requirements",
                table: "ServiceOfferings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ServiceOfferings");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Resolution",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "ResolvedAt",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "ComplaintMessages");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastLoginAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Advisors");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Advisors");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Advisors");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Advisors");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Advisors");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "AdvisorAvailabilities");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "AdvisorAvailabilities");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AdvisorAvailabilities");

            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "AdviceRequests");

            migrationBuilder.DropColumn(
                name: "ConfirmedDate",
                table: "AdviceRequests");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AdviceRequests");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AdviceRequests");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "AdviceRequests");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "AdviceRequests");

            migrationBuilder.DropColumn(
                name: "Response",
                table: "AdviceRequests");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "AdviceRequests");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "AdviceRequests");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "Education",
                table: "VolunteerApplications",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ServiceOfferings",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "NewsItems",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "NewsItems",
                newName: "PublishDate");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ComplaintMessages",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "IsAdmin",
                table: "ComplaintMessages",
                newName: "IsFromAdmin");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ComplaintMessages",
                newName: "SentAt");

            migrationBuilder.RenameIndex(
                name: "IX_ComplaintMessages_UserId",
                table: "ComplaintMessages",
                newName: "IX_ComplaintMessages_SenderId");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "AdvisorAvailabilities",
                newName: "IsBooked");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "AdvisorAvailabilities",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "RequestDate",
                table: "AdviceRequests",
                newName: "AppointmentTime");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "VolunteerApplications",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "VolunteerApplications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AreaOfInterest",
                table: "VolunteerApplications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "VolunteerApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "ServiceOfferings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ServiceOfferings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<string>(
                name: "RedirectUrl",
                table: "ServiceOfferings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "NewsItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "NewsItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000);

            migrationBuilder.AlterColumn<string>(
                name: "ConsultationName",
                table: "Consultations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "MessageText",
                table: "ComplaintMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ZoomRoomUrl",
                table: "Advisors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Advisors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Advisors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ConsultationId",
                table: "Advisors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdvisorId",
                table: "AdviceRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "AdviceRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Admins",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Admins",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintMessages_AspNetUsers_SenderId",
                table: "ComplaintMessages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
