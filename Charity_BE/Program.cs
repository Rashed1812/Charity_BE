using System.Threading.Tasks;
using BLL.ServiceAbstraction;
using DAL.Data;
using DAL.Data.DataSeed;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.RepositoryClasses;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BLL.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using BLL.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace Charity_BE
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Charity API", Version = "v1" });
                
                // Add JWT Authentication to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });

            // Repository Registrations
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IAdvisorRepository, AdvisorRepository>();
            builder.Services.AddScoped<IAdvisorAvailabilityRepository, AdvisorAvailabilityRepository>();
            builder.Services.AddScoped<IAdviceRequestRepository, AdviceRequestRepository>();
            builder.Services.AddScoped<IComplaintRepository, ComplaintRepository>();
            builder.Services.AddScoped<IComplaintMessageRepository, ComplaintMessageRepository>();
            builder.Services.AddScoped<IVolunteerApplicationRepository, VolunteerApplicationRepository>();
            builder.Services.AddScoped<INewsItemRepository, NewsItemRepository>();
            builder.Services.AddScoped<IServiceOfferingRepository, ServiceOfferingRepository>();
            builder.Services.AddScoped<ILectureRepository, LectureRepository>();
            builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IVolunteerRepository, VolunteerRepository>();


            // Service Registrations
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IAdvisorService, AdvisorService>();
            builder.Services.AddScoped<IConsultationService, ConsultationService>();
            builder.Services.AddScoped<IAdviceRequestService, AdviceRequestService>();
            builder.Services.AddScoped<IComplaintService, ComplaintService>();
            builder.Services.AddScoped<IVolunteerService, VolunteerService>();
            builder.Services.AddScoped<INewsService, NewsService>();
            builder.Services.AddScoped<IServiceOfferingService, ServiceOfferingService>();
            builder.Services.AddScoped<ILectureService, LectureService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IUserService, UserService>();
            // Data Seed
            builder.Services.AddScoped<DataSeed>();

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var seeder = services.GetRequiredService<DataSeed>();
                    await seeder.IdentityDataSeedAsync();
                    await seeder.DataSeedAsync();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during database seeding.");
                }
            }

            app.Run();
        }
    }
}
