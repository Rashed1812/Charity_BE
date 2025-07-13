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
    public class HelpTypeController : ControllerBase
    {
        private readonly IHelpTypeService _service;
        public HelpTypeController(IHelpTypeService service)
        {
            _service = service;
        }

        // GET: api/helptype
        [HttpGet]
        public async Task<ActionResult<List<HelpTypeDTO>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        // GET: api/helptype/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<HelpTypeDTO>> GetById(int id)
        {
            var type = await _service.GetByIdAsync(id);
            if (type == null) return NotFound();
            return Ok(type);
        }

        // POST: api/helptype
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<HelpTypeDTO>> Create([FromBody] CreateHelpTypeDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/helptype/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<HelpTypeDTO>> Update(int id, [FromBody] CreateHelpTypeDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/helptype/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
} 