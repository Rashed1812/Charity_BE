using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DAL.Data.Models.IdentityModels;
using DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data.DataSeed
{
    public class DataSeed(ApplicationDbContext _DbContext,
        UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager)
    {
        public async Task DataSeedAsync()
        {
            var pendingMigrations = await _DbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
                await _DbContext.Database.MigrateAsync();

            try
            {

                if (!_DbContext.Set<Admin>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\Admin.json");
                    var list = await JsonSerializer.DeserializeAsync<List<Admin>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<Admin>().AddRangeAsync(list);
                }

                if (!_DbContext.Set<Advisor>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\Advisor.json");
                    var list = await JsonSerializer.DeserializeAsync<List<Advisor>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<Advisor>().AddRangeAsync(list);
                }

                if (!_DbContext.Set<AdviceRequest>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\AdviceRequest.json");
                    var list = await JsonSerializer.DeserializeAsync<List<AdviceRequest>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<AdviceRequest>().AddRangeAsync(list);
                }

                if (!_DbContext.Set<AdvisorAvailability>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\AdvisorAvailability.json");
                    var list = await JsonSerializer.DeserializeAsync<List<AdvisorAvailability>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<AdvisorAvailability>().AddRangeAsync(list);
                }

                if (!_DbContext.Set<Complaint>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\Complaint.json");
                    var list = await JsonSerializer.DeserializeAsync<List<Complaint>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<Complaint>().AddRangeAsync(list);
                }

                if (!_DbContext.Set<ComplaintMessage>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\ComplaintMessage.json");
                    var list = await JsonSerializer.DeserializeAsync<List<ComplaintMessage>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<ComplaintMessage>().AddRangeAsync(list);
                }

                if (!_DbContext.Set<NewsItem>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\NewsItem.json");
                    var list = await JsonSerializer.DeserializeAsync<List<NewsItem>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<NewsItem>().AddRangeAsync(list);
                }

                if (!_DbContext.Set<ServiceOffering>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\ServiceOffering.json");
                    var list = await JsonSerializer.DeserializeAsync<List<ServiceOffering>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<ServiceOffering>().AddRangeAsync(list);
                }
                if (!_DbContext.Set<Consultation>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\Consultation.json");
                    var list = await JsonSerializer.DeserializeAsync<List<Consultation>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<Consultation>().AddRangeAsync(list);
                }

                if (!_DbContext.Set<ApplicationUser>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\User.json");
                    var list = await JsonSerializer.DeserializeAsync<List<ApplicationUser>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<ApplicationUser>().AddRangeAsync(list);
                }

                if (!_DbContext.Set<VolunteerApplication>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\VolunteerApplication.json");
                    var list = await JsonSerializer.DeserializeAsync<List<VolunteerApplication>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<VolunteerApplication>().AddRangeAsync(list);
                }

                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during data seeding: {ex.Message}");
            }
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                // Step 1: Create Roles if not exist
                string[] roles = { "Admin", "User", "Advisor" };
                foreach (var role in roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
                var fullPath = Path.GetFullPath(
                    Path.Combine(AppContext.BaseDirectory, @"..\DAL\Data\DataSeed\FilesData\User.json")
                );

                Console.WriteLine($"Looking for seed file at: {fullPath}");

                if (!File.Exists(fullPath))
                {
                    Console.WriteLine($"Seed file not found: {fullPath}");
                    return;
                }


                var jsonData = await File.ReadAllTextAsync(fullPath);
                var userSeedList = JsonSerializer.Deserialize<List<ApplicatiosUserSeedModel>>(jsonData);

                if (userSeedList is null || !userSeedList.Any())
                {
                    Console.WriteLine("No user data found in seed file.");
                    return;
                }

                foreach (var seed in userSeedList)
                {
                    var existingUser = await _userManager.FindByEmailAsync(seed.Email);
                    if (existingUser is null)
                    {
                        var user = new ApplicationUser
                        {
                            FullName = seed.FullName,
                            UserName = seed.UserName,
                            Email = seed.Email,
                            EmailConfirmed = true,
                            PhoneNumberConfirmed = true
                        };

                        var result = await _userManager.CreateAsync(user, seed.Password);

                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, seed.Role);
                            Console.WriteLine($"User created: {seed.Email} as {seed.Role}");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to create {seed.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"User already exists: {seed.Email}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding error: {ex.Message}");
            }
        }
    }
}
