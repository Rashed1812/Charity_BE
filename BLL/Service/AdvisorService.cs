using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.ServiceAbstraction;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.RepositoryClasses;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.AdvisorDTOs;

namespace BLL.Service
{
    public class AdvisorService(AdvisorRepository _advisorRepo, IUserRepository _userRepo) : IAdvisorService
    {
        public async Task<IEnumerable<AdvisorDTO>> GetAllAdvisorsAsync()
        {
            var advisors = await _advisorRepo.GetAllAsync();

            var advisorDtos = new List<AdvisorDTO>();

            foreach (var advisor in advisors)
            {
                var user = await _userRepo.GetByIdAsync(advisor.UserId);

                advisorDtos.Add(new AdvisorDTO
                {
                    Id = advisor.Id,
                    FullName = advisor.FullName,
                    Email = user?.Email,
                    PhoneNumber = user?.PhoneNumber,
                    Specialty = advisor.Specialty,
                    Description = advisor.Description,
                    ZoomRoomUrl = advisor.ZoomRoomUrl,
                    ConsultationName = advisor.Consultation?.ConsultationName,
                    Availabilities = advisor.Availabilities?.Select(a => new AdvisorAvailabilityDTO
                    {
                        Id = a.Id,
                        Date = a.Date,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime,
                        IsBooked = a.IsBooked
                    }).ToList() ?? new List<AdvisorAvailabilityDTO>()
                });
            }

            return advisorDtos;
        }
        public async Task<AdvisorDTO> GetAdvisorByIdAsync(int id)
        {
            var advisor = await _advisorRepo.GetByIdAsync(id);
            if (advisor == null) return null;
            var user = await _userRepo.GetByIdAsync(advisor.UserId);
            return new AdvisorDTO
            {
                Id = advisor.Id,
                FullName = advisor.FullName,
                Email = user?.Email,
                PhoneNumber = user?.PhoneNumber,
                Specialty = advisor.Specialty,
                Description = advisor.Description,
                ZoomRoomUrl = advisor.ZoomRoomUrl,
                ConsultationName = advisor.Consultation?.ConsultationName,
                Availabilities = advisor.Availabilities?.Select(a => new AdvisorAvailabilityDTO
                {
                    Id = a.Id,
                    Date = a.Date,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    IsBooked = a.IsBooked
                }).ToList() ?? new List<AdvisorAvailabilityDTO>()
            };

        }

        public async Task<IEnumerable<AdvisorDTO>> GetAllAdvisorsWithRelatedDataAsync()
        {
            var advisors = await _advisorRepo.GetAllWithIncludesAsync();

            var advisorDtos = advisors.Select(advisor => new AdvisorDTO
            {
                Id = advisor.Id,
                FullName = advisor.FullName,
                Email = advisor.User?.Email,
                PhoneNumber = advisor.User?.PhoneNumber,
                Specialty = advisor.Specialty,
                Description = advisor.Description,
                ZoomRoomUrl = advisor.ZoomRoomUrl,
                ConsultationName = advisor.Consultation?.ConsultationName,
                Availabilities = advisor.Availabilities?.Select(a => new AdvisorAvailabilityDTO
                {
                    Id = a.Id,
                    Date = a.Date,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    IsBooked = a.IsBooked
                }).ToList() ?? new List<AdvisorAvailabilityDTO>()
            }).ToList();

            return advisorDtos;
        }

    }
}
