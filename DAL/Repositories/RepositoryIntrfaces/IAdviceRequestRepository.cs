using DAL.Data.Models;
using DAL.Repositries.GenericRepositries;
using Shared.DTOS.AdvisorDTOs;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IAdviceRequestRepository : IGenericRepository<AdviceRequest>
    {
        Task<List<AdviceRequest>> GetAllRequestsWithDetailsAsync();
        Task<List<AdviceRequest>> GetRequestsByUserAsync(string userId);
        Task<AdviceRequest> GetRequestByIdWithDetailsAsync(int id);
        Task<List<AdviceRequest>> GetRequestsByStatusAsync(ConsultationStatus status);
        Task<List<AdviceRequest>> GetRequestsByAdvisorAsync(int advisorId);
        Task<List<AdviceRequest>> GetRequestsByConsultationAsync(int consultationId);
        Task<int> GetTotalRequestsCountAsync();
        Task<int> GetPendingRequestsCountAsync();
        Task<int> GetConfirmedRequestsCountAsync();
        Task<int> GetCompletedRequestsCountAsync();
        Task<int> GetCancelledRequestsCountAsync();
        Task<int> GetRequestsCountByConsultationAsync(int consultationId);
        Task<int> GetPendingRequestsCountByConsultationAsync(int consultationId);
        Task<int> GetCompletedRequestsCountByConsultationAsync(int consultationId);
        Task<List<object>> GetRequestsByMonthAsync(int months);
        Task<List<object>> GetRequestsByConsultationAndMonthAsync(int consultationId, int months);
        Task<List<object>> GetTopAdvisorsAsync(int count);
        Task<bool> HasActiveConsultationsAsync(string userId);
        Task<List<AdviceRequest>> GetRecentConsultationsAsync(int count);
        Task<List<AdviceRequest>> GetActiveRequestsByAdvisorAsync(int advisorId);
        Task<int> GetTotalConsultationsByAdvisorAsync(int advisorId);
        Task<int> GetPendingRequestsByAdvisorAsync(int advisorId);
        Task<int> GetCompletedConsultationsByAdvisorAsync(int advisorId);
        Task<List<AdviceRequest>> GetByUserIdAsync(string userId);
        Task<List<AdviceRequest>> GetByAdvisorIdAsync(int advisorId);
        Task<List<AdviceRequest>> GetByConsultationIdAsync(int consultationId);
        Task<List<AdviceRequest>> GetByStatusAsync(ConsultationStatus status);
        Task<List<AdviceRequest>> GetPendingRequestsAsync();
        Task<List<AdviceRequest>> GetCompletedRequestsAsync();
        Task<int> GetTotalConsultationsByUserAsync(string userId);
        Task<int> GetPendingConsultationsByUserAsync(string userId);
        Task<int> GetCompletedConsultationsByUserAsync(string userId);
    }
} 