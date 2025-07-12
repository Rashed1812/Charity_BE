using BLL.ServiceAbstraction;
using DAL.Data.Models;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.HelpDTOs;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class HelpTypeService : IHelpTypeService
    {
        private readonly IHelpTypeRepository _repository;
        private readonly IMapper _mapper;
        public HelpTypeService(IHelpTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<HelpTypeDTO>> GetAllAsync()
        {
            var types = await _repository.GetAllAsync();
            return _mapper.Map<List<HelpTypeDTO>>(types);
        }

        public async Task<HelpTypeDTO> GetByIdAsync(int id)
        {
            var type = await _repository.GetByIdAsync(id);
            return _mapper.Map<HelpTypeDTO>(type);
        }

        public async Task<HelpTypeDTO> CreateAsync(CreateHelpTypeDTO dto)
        {
            var entity = _mapper.Map<HelpType>(dto);
            entity.CreatedAt = System.DateTime.UtcNow;
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<HelpTypeDTO>(created);
        }

        public async Task<HelpTypeDTO> UpdateAsync(int id, CreateHelpTypeDTO dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            _mapper.Map(dto, entity);
            entity.UpdatedAt = System.DateTime.UtcNow;
            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<HelpTypeDTO>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;
            return await _repository.DeleteAsync(entity);
        }
    }
} 