using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.ReconcileRequestDTOs;
using DAL.Data.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BLL.Service
{
    public class ReconcileRequestService : IReconcileRequestService
    {
        private readonly IReconcileRequestRepository _repository;
        private readonly IMapper _mapper;
        public ReconcileRequestService(IReconcileRequestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ReconcileRequestDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<ReconcileRequestDTO>>(entities);
        }

        public async Task<List<ReconcileRequestDTO>> GetByUserIdAsync(string userId)
        {
            var entities = await _repository.GetByUserIdAsync(userId);
            return _mapper.Map<List<ReconcileRequestDTO>>(entities);
        }

        public async Task<ReconcileRequestDTO> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<ReconcileRequestDTO>(entity);
        }

        public async Task<ReconcileRequestDTO> CreateAsync(string userId, CreateReconcileRequestDTO dto)
        {
            var entity = _mapper.Map<ReconcileRequest>(dto);
            entity.UserId = userId;
            entity.CreatedAt = System.DateTime.UtcNow;
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<ReconcileRequestDTO>(created);
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null || (entity.UserId != null && entity.UserId != userId))
                return false;
            await _repository.DeleteAsync(entity);
            return true;
        }
    }
} 