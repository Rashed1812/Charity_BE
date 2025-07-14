using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DAL.Data.Models.IdentityModels;
using DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.DTOS.NotificationDTOs;

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
                // 1. إضافة Consultation
                if (!_DbContext.Set<Consultation>().Any())
                {
                    var consultationDataPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\DAL\Data\DataSeed\FilesData\Consultation.json"));
                    Console.WriteLine($"Looking for Consultation.json at: {consultationDataPath}");
                    
                    if (!File.Exists(consultationDataPath))
                    {
                        var alternativePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\DAL\Data\DataSeed\FilesData\Consultation.json"));
                        Console.WriteLine($"Trying alternative path: {alternativePath}");
                        if (File.Exists(alternativePath))
                        {
                            consultationDataPath = alternativePath;
                        }
                    }
                    
                    if (File.Exists(consultationDataPath))
                    {
                        var data = File.OpenRead(consultationDataPath);
                        var list = await JsonSerializer.DeserializeAsync<List<Consultation>>(data);
                        if (list is not null && list.Any())
                        {
                            await _DbContext.Set<Consultation>().AddRangeAsync(list);
                            Console.WriteLine($"Added {list.Count} consultations to database");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Consultation.json file not found at both paths");
                    }
                }
                else
                {
                    Console.WriteLine("Consultations already exist in database, skipping...");
                }
                await _DbContext.SaveChangesAsync();

                // 2. إضافة ApplicationUser
                if (!_DbContext.Set<ApplicationUser>().Any())
                {
                    var userDataPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\DAL\Data\DataSeed\FilesData\User.json"));
                    Console.WriteLine($"Looking for User.json at: {userDataPath}");
                    
                    if (!File.Exists(userDataPath))
                    {
                        var alternativePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\DAL\Data\DataSeed\FilesData\User.json"));
                        Console.WriteLine($"Trying alternative path: {alternativePath}");
                        if (File.Exists(alternativePath))
                        {
                            userDataPath = alternativePath;
                        }
                    }
                    
                    if (File.Exists(userDataPath))
                    {
                        var data = File.OpenRead(userDataPath);
                        var list = await JsonSerializer.DeserializeAsync<List<ApplicationUser>>(data);
                        if (list is not null && list.Any())
                        {
                            await _DbContext.Set<ApplicationUser>().AddRangeAsync(list);
                            Console.WriteLine($"Added {list.Count} users to database");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"User.json file not found at both paths");
                    }
                }
                else
                {
                    Console.WriteLine("Users already exist in database, skipping...");
                }
                await _DbContext.SaveChangesAsync();

                // 3. إضافة Advisor مع ربط UserId تلقائيًا
                if (!_DbContext.Set<Advisor>().Any())
                {
                    var usersInDb = _DbContext.Users.ToList();
                    var advisorDataPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\DAL\Data\DataSeed\FilesData\Advisor.json"));
                    Console.WriteLine($"Looking for Advisor.json at: {advisorDataPath}");
                    
                    if (!File.Exists(advisorDataPath))
                    {
                        var alternativePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\DAL\Data\DataSeed\FilesData\Advisor.json"));
                        Console.WriteLine($"Trying alternative path: {alternativePath}");
                        if (File.Exists(alternativePath))
                        {
                            advisorDataPath = alternativePath;
                        }
                    }
                    
                    if (File.Exists(advisorDataPath))
                    {
                        var data = File.OpenRead(advisorDataPath);
                        var advisors = await JsonSerializer.DeserializeAsync<List<Advisor>>(data);
                        int addedCount = 0;
                        foreach (var advisor in advisors)
                        {
                            var user = usersInDb.FirstOrDefault(u => u.Email == advisor.Email);
                            if (user != null)
                            {
                                advisor.UserId = user.Id;
                                _DbContext.Advisors.Add(advisor);
                                addedCount++;
                            }
                            else
                            {
                                Console.WriteLine($"Warning: User with email {advisor.Email} not found. Skipping advisor creation.");
                            }
                        }
                        Console.WriteLine($"Added {addedCount} advisors to database");
                    }
                    else
                    {
                        Console.WriteLine($"Advisor.json file not found at both paths");
                    }
                }
                else
                {
                    Console.WriteLine("Advisors already exist in database, skipping...");
                }
                await _DbContext.SaveChangesAsync();

                // 4. إضافة AdvisorAvailability
                if (!_DbContext.Set<AdvisorAvailability>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\AdvisorAvailability.json");
                    var list = await JsonSerializer.DeserializeAsync<List<AdvisorAvailability>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<AdvisorAvailability>().AddRangeAsync(list);
                }
                await _DbContext.SaveChangesAsync();

                // 5. إضافة AdviceRequest
                if (!_DbContext.Set<AdviceRequest>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\AdviceRequest.json");
                    var list = await JsonSerializer.DeserializeAsync<List<AdviceRequest>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<AdviceRequest>().AddRangeAsync(list);
                }
                await _DbContext.SaveChangesAsync();

                if (!_DbContext.Set<Complaint>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\Complaint.json");
                    var list = await JsonSerializer.DeserializeAsync<List<Complaint>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<Complaint>().AddRangeAsync(list);
                }

                if (!_DbContext.Set<NewsItem>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\NewsItem.json");
                    var list = await JsonSerializer.DeserializeAsync<List<NewsItem>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<NewsItem>().AddRangeAsync(list);
                }
                if (!_DbContext.Set<HelpType>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\HelpType.json");
                    var list = await JsonSerializer.DeserializeAsync<List<HelpType>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<HelpType>().AddRangeAsync(list);
                }
                if (!_DbContext.Set<HelpRequest>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\HelpRequest.json");
                    var list = await JsonSerializer.DeserializeAsync<List<HelpRequest>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<HelpRequest>().AddRangeAsync(list);
                }
                if (!_DbContext.Set<Lecture>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\Lecture.json");
                    var list = await JsonSerializer.DeserializeAsync<List<Lecture>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<Lecture>().AddRangeAsync(list);
                }
                if (!_DbContext.Set<ReconcileRequest>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\ReconcileRequest.json");
                    var list = await JsonSerializer.DeserializeAsync<List<ReconcileRequest>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<ReconcileRequest>().AddRangeAsync(list);
                }
                if (!_DbContext.Set<ServiceOffering>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\ServiceOffering.json");
                    var list = await JsonSerializer.DeserializeAsync<List<ServiceOffering>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<ServiceOffering>().AddRangeAsync(list);
                }

                if (!_DbContext.Set<VolunteerApplication>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\VolunteerApplication.json");
                    var list = await JsonSerializer.DeserializeAsync<List<VolunteerApplication>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<VolunteerApplication>().AddRangeAsync(list);
                }

                if (!_DbContext.Set<Admin>().Any())
                {
                    var data = File.OpenRead(@"..\DAL\Data\DataSeed\FilesData\Admin.json");
                    var list = await JsonSerializer.DeserializeAsync<List<Admin>>(data);
                    if (list is not null && list.Any())
                        await _DbContext.Set<Admin>().AddRangeAsync(list);
                }

                if (!_DbContext.Set<Notification>().Any())
                {
                    var notifications = new List<Notification>
                    {
                        new Notification {
                            UserId = "148b2502-57b2-4957-9a88-fedaad0ca1e7",
                            Title = "تنبيه تجريبي للأدمن",
                            Message = "تمت إضافة شكوى جديدة.",
                            Type = NotificationType.Complaint,
                            CreatedAt = DateTime.UtcNow
                        },
                        new Notification {
                            UserId = "805062ec-7158-4062-a92f-5778f4b7332d",
                            Title = "تنبيه تجريبي للمستخدم",
                            Message = "تم تغيير حالة الشكوى الخاصة بك.",
                            Type = NotificationType.Complaint,
                            CreatedAt = DateTime.UtcNow
                        }
                    };
                    await _DbContext.Set<Notification>().AddRangeAsync(notifications);
                }

                await _DbContext.SaveChangesAsync();
                Console.WriteLine("Data seeding completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during data seeding: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
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
                    Path.Combine(Directory.GetCurrentDirectory(), @"..\DAL\Data\DataSeed\FilesData\User.json")
                );

                Console.WriteLine($"Looking for User.json at: {fullPath}");

                if (!File.Exists(fullPath))
                {
                    // Try alternative path
                    var alternativePath = Path.GetFullPath(
                        Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\DAL\Data\DataSeed\FilesData\User.json")
                    );
                    Console.WriteLine($"Trying alternative path: {alternativePath}");
                    
                    if (File.Exists(alternativePath))
                    {
                        fullPath = alternativePath;
                    }
                    else
                    {
                        Console.WriteLine($"User.json file not found at both paths");
                        return;
                    }
                }


                var jsonData = await File.ReadAllTextAsync(fullPath);
                var userSeedList = JsonSerializer.Deserialize<List<ApplicatiosUserSeedModel>>(jsonData);

                if (userSeedList is null || !userSeedList.Any())
                {
                    Console.WriteLine("No user data found in seed file.");
                    return;
                }

                Console.WriteLine($"Found {userSeedList.Count} users in seed file");

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
                Console.WriteLine("Identity data seeding completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding error: {ex.Message}");
            }
        }
    }
}
