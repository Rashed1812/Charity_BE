using Microsoft.AspNetCore.Mvc;
using BLL.ServiceAbstraction;
using Shared.DTOS.HelpDTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Charity_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelpRequestController : ControllerBase
    {
        private readonly IHelpRequestService _service;
        public HelpRequestController(IHelpRequestService service)
        {
            _service = service;
        }

        // POST: api/helprequest
        [HttpPost]
        public async Task<ActionResult<HelpRequestDTO>> Create([FromBody] CreateHelpRequestDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), null, created);
        }

        // GET: api/helprequest
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<HelpRequestDTO>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
    }
} 