using AutoMapper;
using DAL.Data.Models;
using DAL.Data.Models.IdentityModels;
using Shared.DTOS.AdminDTOs;
using Shared.DTOS.AdvisorDTOs;
using Shared.DTOS.AdviceRequestDTOs;
using Shared.DTOS.AuthDTO;
using Shared.DTOS.ComplaintDTOs;
using Shared.DTOS.ConsultationDTOs;
using Shared.DTOS.LectureDTOs;
using Shared.DTOS.NewsDTOs;
using Shared.DTOS.ServiceOfferingDTOs;
using Shared.DTOS.UserDTO;
using Shared.DTOS.VolunteerDTOs;
using Shared.DTOS.NotificationDTOs;
using Shared.DTOS.MediationDTOs;

namespace BLL.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Admin Mappings
            CreateMap<Admin, AdminDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
            CreateMap<CreateAdminDTO, Admin>();
            CreateMap<UpdateAdminDTO, Admin>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Advisor Mappings
            CreateMap<Advisor, AdvisorDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.ConsultationName, opt => opt.MapFrom(src => src.Consultation != null ? src.Consultation.ConsultationName : ""))
                .ForMember(dest => dest.ConsultationType, opt => opt.MapFrom(src => src.ConsultationType));

            CreateMap<CreateAdvisorDTO, Advisor>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.ConsultationType, opt => opt.MapFrom(src => src.ConsultationType));

            CreateMap<UpdateAdvisorDTO, Advisor>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.ConsultationType, opt => opt.MapFrom(src => src.ConsultationType));

            // Advisor Availability Mappings
            CreateMap<AdvisorAvailability, AdvisorAvailabilityDTO>()
                .ForMember(dest => dest.AdvisorName, opt => opt.MapFrom(src => src.Advisor.FullName));
            CreateMap<CreateAvailabilityDTO, AdvisorAvailability>()
                .ForMember(dest => dest.ConsultationType, opt => opt.MapFrom(src => src.ConsultationType));
            CreateMap<UpdateAvailabilityDTO, AdvisorAvailability>()
                .ForMember(dest => dest.ConsultationType, opt => opt.MapFrom(src => src.ConsultationType));

            // Advice Request Mappings
            CreateMap<AdviceRequest, AdviceRequestDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.AdvisorName, opt => opt.MapFrom(src => src.Advisor.FullName))
                .ForMember(dest => dest.ConsultationName, opt => opt.MapFrom(src => src.Consultation.ConsultationName))
                .ForMember(dest => dest.ConsultationType, opt => opt.MapFrom(src => src.ConsultationType));
            CreateMap<CreateAdviceRequestDTO, AdviceRequest>()
                .ForMember(dest => dest.ConsultationType, opt => opt.MapFrom(src => src.ConsultationType));
            CreateMap<UpdateAdviceRequestDTO, AdviceRequest>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Complaint Mappings
            CreateMap<Complaint, ComplaintDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName));
            CreateMap<CreateComplaintDTO, Complaint>();
            CreateMap<UpdateComplaintDTO, Complaint>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Consultation Mappings
            CreateMap<Consultation, ConsultationDTO>();
            CreateMap<CreateConsultationDTO, Consultation>();
            CreateMap<UpdateConsultationDTO, Consultation>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Lecture Mappings
            CreateMap<Lecture, LectureDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => DeserializeTags(src.Tags)))
                .ForMember(dest => dest.ConsultationName, opt => opt.MapFrom(src => src.Consultation != null ? src.Consultation.ConsultationName : null))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.FullName : null));
            CreateMap<CreateLectureDTO, Lecture>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => SerializeTags(src.Tags)));
            CreateMap<LectureDTO, Lecture>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<LectureType>(src.Type)))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => SerializeTags(src.Tags)));
            CreateMap<UpdateLectureDTO, Lecture>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // News Item Mappings
            CreateMap<NewsItem, NewsItemDTO>();
            CreateMap<CreateNewsItemDTO, NewsItem>();
            CreateMap<UpdateNewsItemDTO, NewsItem>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Service Offering Mappings
            CreateMap<ServiceOffering, ServiceOfferingDTO>();
            CreateMap<CreateServiceOfferingDTO, ServiceOffering>();
            CreateMap<UpdateServiceOfferingDTO, ServiceOffering>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // User Mappings
            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
            CreateMap<UpdateUserDTO, ApplicationUser>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Volunteer Application Mappings
            CreateMap<VolunteerApplication, VolunteerApplicationDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName));
            CreateMap<CreateVolunteerApplicationDTO, VolunteerApplication>();
            CreateMap<UpdateVolunteerApplicationDTO, VolunteerApplication>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Auth Mappings
            CreateMap<ApplicationUser, CurrentUserDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
            CreateMap<RegisterDTO, ApplicationUser>();
            CreateMap<RegisterAdminDTO, ApplicationUser>();
            CreateMap<RegisterAdvisorDTO, ApplicationUser>();

            // Notification Mappings
            CreateMap<Notification, NotificationDTO>();
            CreateMap<NotificationCreateDTO, Notification>();

            // Mediation Mappings
            CreateMap<Mediation, Shared.DTOS.MediationDTOs.MediationDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
            CreateMap<Shared.DTOS.MediationDTOs.CreateMediationDTO, Mediation>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
            CreateMap<Shared.DTOS.MediationDTOs.UpdateMediationDTO, Mediation>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

        private static List<string> DeserializeTags(string tags)
        {
            return string.IsNullOrEmpty(tags)
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(tags);
        }

        private static string SerializeTags(List<string> tags)
        {
            return tags != null
                ? System.Text.Json.JsonSerializer.Serialize(tags)
                : null;
        }
    }
} 