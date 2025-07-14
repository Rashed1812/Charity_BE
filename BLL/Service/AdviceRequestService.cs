using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using DAL.Data.Models;
using Shared.DTOS.AdviceRequestDTOs;
using Shared.DTOS.AdvisorDTOs;

namespace BLL.Service
{
    public class AdviceRequestService : IAdviceRequestService
    {
        private readonly IAdviceRequestRepository _adviceRequestRepository;
        private readonly IAdvisorRepository _advisorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConsultationRepository _consultationRepository;
        private readonly IMapper _mapper;

        public AdviceRequestService(
            IAdviceRequestRepository adviceRequestRepository,
            IAdvisorRepository advisorRepository,
            IUserRepository userRepository,
            IConsultationRepository consultationRepository,
            IMapper mapper)
        {
            _adviceRequestRepository = adviceRequestRepository;
            _advisorRepository = advisorRepository;
            _userRepository = userRepository;
            _consultationRepository = consultationRepository;
            _mapper = mapper;
        }

        public async Task<List<AdviceRequestDTO>> GetAllRequestsAsync()
        {
            var requests = await _adviceRequestRepository.GetAllAsync();
            return _mapper.Map<List<AdviceRequestDTO>>(requests);
        }

        public async Task<List<AdviceRequestDTO>> GetUserRequestsAsync(string userId)
        {
            var requests = await _adviceRequestRepository.GetByUserIdAsync(userId);
            return _mapper.Map<List<AdviceRequestDTO>>(requests);
        }

        public async Task<AdviceRequestDTO> GetRequestByIdAsync(int id)
        {
            var request = await _adviceRequestRepository.GetByIdAsync(id);
            return _mapper.Map<AdviceRequestDTO>(request);
        }

        public async Task<AdviceRequestDTO> CreateRequestAsync(string userId, CreateAdviceRequestDTO createRequestDto)
        {
            var availability = await _advisorRepository.GetAvailabilityByIdAsync(createRequestDto.AdvisorAvailabilityId);
            if (availability == null)
                throw new InvalidOperationException("الموعد المتاح غير موجود.");
            if (availability.IsBooked)
                throw new InvalidOperationException("هذا الموعد تم حجزه بالفعل.");

            var request = _mapper.Map<AdviceRequest>(createRequestDto);
            request.UserId = userId;
            request.Status = ConsultationStatus.Pending;
            request.RequestDate = DateTime.UtcNow;
            request.AdvisorAvailabilityId = createRequestDto.AdvisorAvailabilityId;
            request.AdvisorId = availability.AdvisorId;

            availability.IsBooked = true;

            availability.AdviceRequestId = null;

            var createdRequest = await _adviceRequestRepository.AddAsync(request);
            availability.AdviceRequestId = createdRequest.Id;
            await _advisorRepository.UpdateAvailabilityAsync(availability);

            return _mapper.Map<AdviceRequestDTO>(createdRequest);
        }

        public async Task<AdviceRequestDTO> UpdateRequestAsync(int id, string userId, UpdateAdviceRequestDTO updateRequestDto)
        {
            var request = await _adviceRequestRepository.GetByIdAsync(id);
            if (request == null || request.UserId != userId)
                return null;

            // Only allow updates if request is still pending
            if (request.Status != ConsultationStatus.Pending)
                throw new InvalidOperationException("Cannot update request that is not pending");

            // Update properties
            if (!string.IsNullOrEmpty(updateRequestDto.Title))
                request.Title = updateRequestDto.Title;

            if (!string.IsNullOrEmpty(updateRequestDto.Description))
                request.Description = updateRequestDto.Description;

            if (!string.IsNullOrEmpty(updateRequestDto.Priority))
                request.Priority = updateRequestDto.Priority;

            var updatedRequest = await _adviceRequestRepository.UpdateAsync(request);
            return _mapper.Map<AdviceRequestDTO>(updatedRequest);
        }

        public async Task<bool> CancelRequestAsync(int id, string userId)
        {
            var request = await _adviceRequestRepository.GetByIdAsync(id);
            if (request == null || request.UserId != userId)
                return false;

            // Only allow cancellation if request is still pending
            if (request.Status != ConsultationStatus.Pending)
                throw new InvalidOperationException("Cannot cancel request that is not pending");

            request.Status = ConsultationStatus.Cancelled;
            await _adviceRequestRepository.UpdateAsync(request);
            return true;
        }

        public async Task<AdviceRequestDTO> ConfirmRequestAsync(int id, string advisorId)
        {
            var request = await _adviceRequestRepository.GetByIdAsync(id);
            if (request == null)
                return null;

            // Only allow confirmation if request is pending
            if (request.Status != ConsultationStatus.Pending)
                throw new InvalidOperationException("Cannot confirm request that is not pending");

            request.AdvisorId = int.Parse(advisorId); // This should be the advisor's ID, not user ID
            request.Status = ConsultationStatus.Confirmed;
            request.ConfirmedDate = DateTime.UtcNow;

            var updatedRequest = await _adviceRequestRepository.UpdateAsync(request);
            return _mapper.Map<AdviceRequestDTO>(updatedRequest);
        }

        public async Task<AdviceRequestDTO> CompleteRequestAsync(int id, string advisorId, CompleteRequestDTO completeRequestDto)
        {
            var request = await _adviceRequestRepository.GetByIdAsync(id);
            if (request == null)
                return null;

            // Only allow completion if request is confirmed
            if (request.Status != ConsultationStatus.Confirmed)
                throw new InvalidOperationException("Cannot complete request that is not confirmed");

            request.Response = completeRequestDto.Response;
            request.Status = ConsultationStatus.Completed;
            request.CompletedDate = DateTime.UtcNow;

            var updatedRequest = await _adviceRequestRepository.UpdateAsync(request);
            return _mapper.Map<AdviceRequestDTO>(updatedRequest);
        }

        public async Task<List<AdviceRequestDTO>> GetRequestsByStatusAsync(ConsultationStatus status)
        {
            var requests = await _adviceRequestRepository.GetByStatusAsync(status);
            return _mapper.Map<List<AdviceRequestDTO>>(requests);
        }

        public async Task<List<AdviceRequestDTO>> GetRequestsByAdvisorAsync(int advisorId)
        {
            var requests = await _adviceRequestRepository.GetByAdvisorIdAsync(advisorId);
            return _mapper.Map<List<AdviceRequestDTO>>(requests);
        }

        public async Task<List<AdviceRequestDTO>> GetRequestsByConsultationAsync(int consultationId)
        {
            var requests = await _adviceRequestRepository.GetByConsultationIdAsync(consultationId);
            return _mapper.Map<List<AdviceRequestDTO>>(requests);
        }

        public async Task<object> GetRequestStatisticsAsync()
        {
            var requests = await _adviceRequestRepository.GetAllAsync();
            
            return new
            {
                TotalRequests = requests.Count(),
                PendingRequests = requests.Count(r => r.Status == ConsultationStatus.Pending),
                ConfirmedRequests = requests.Count(r => r.Status == ConsultationStatus.Confirmed),
                CompletedRequests = requests.Count(r => r.Status == ConsultationStatus.Completed),
                CancelledRequests = requests.Count(r => r.Status == ConsultationStatus.Cancelled)
            };
        }
    }
} 