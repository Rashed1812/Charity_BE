using BLL.ServiceAbstraction;
using DAL.Data.Models;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.HelpDTOs;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class HelpRequestService : IHelpRequestService
    {
        private readonly IHelpRequestRepository _repository;
        private readonly IHelpTypeRepository _typeRepository;
        private readonly IMapper _mapper;
        public HelpRequestService(IHelpRequestRepository repository, IHelpTypeRepository typeRepository, IMapper mapper)
        {
            _repository = repository;
            _typeRepository = typeRepository;
            _mapper = mapper;
        }

        public async Task<List<HelpRequestDTO>> GetAllAsync()
        {
            var requests = await _repository.GetAllAsync();
            return _mapper.Map<List<HelpRequestDTO>>(requests);
        }

        public async Task<HelpRequestDTO> CreateAsync(CreateHelpRequestDTO dto)
        {
            var entity = _mapper.Map<HelpRequest>(dto);
            entity.CreatedAt = System.DateTime.UtcNow;
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<HelpRequestDTO>(created);
        }
    }
} 