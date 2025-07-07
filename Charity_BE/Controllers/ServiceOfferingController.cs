using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.ServiceOfferingDTOs;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceOfferingController : ControllerBase
    {
        private readonly IServiceOfferingService _serviceOfferingService;

        public ServiceOfferingController(IServiceOfferingService serviceOfferingService)
        {
            _serviceOfferingService = serviceOfferingService;
        }

        // GET: api/serviceoffering
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ServiceOfferingDTO>>>> GetAllServices()
        {
            try
            {
                var services = await _serviceOfferingService.GetAllServicesAsync();
                return Ok(ApiResponse<List<ServiceOfferingDTO>>.SuccessResult(services));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ServiceOfferingDTO>>.ErrorResult("Failed to retrieve services", 500));
            }
        }

        // GET: api/serviceoffering/active
        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<List<ServiceOfferingDTO>>>> GetActiveServices()
        {
            try
            {
                var services = await _serviceOfferingService.GetActiveServicesAsync();
                return Ok(ApiResponse<List<ServiceOfferingDTO>>.SuccessResult(services));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ServiceOfferingDTO>>.ErrorResult("Failed to retrieve active services", 500));
            }
        }

        // GET: api/serviceoffering/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ServiceOfferingDTO>>> GetServiceById(int id)
        {
            try
            {
                var service = await _serviceOfferingService.GetServiceByIdAsync(id);
                if (service == null)
                    return NotFound(ApiResponse<ServiceOfferingDTO>.ErrorResult($"Service with ID {id} not found", 404));

                return Ok(ApiResponse<ServiceOfferingDTO>.SuccessResult(service));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ServiceOfferingDTO>.ErrorResult("Failed to retrieve service", 500));
            }
        }

        // POST: api/serviceoffering
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ServiceOfferingDTO>>> CreateService([FromBody] CreateServiceOfferingDTO createServiceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<ServiceOfferingDTO>.ErrorResult("Invalid input data", 400, 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                var service = await _serviceOfferingService.CreateServiceAsync(createServiceDto);
                return CreatedAtAction(nameof(GetServiceById), new { id = service.Id }, 
                    ApiResponse<ServiceOfferingDTO>.SuccessResult(service, "Service created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ServiceOfferingDTO>.ErrorResult("Failed to create service", 500));
            }
        }

        // PUT: api/serviceoffering/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ServiceOfferingDTO>>> UpdateService(int id, [FromBody] UpdateServiceOfferingDTO updateServiceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<ServiceOfferingDTO>.ErrorResult("Invalid input data", 400));

            try
            {
                var service = await _serviceOfferingService.UpdateServiceAsync(id, updateServiceDto);
                if (service == null)
                    return NotFound(ApiResponse<ServiceOfferingDTO>.ErrorResult($"Service with ID {id} not found", 404));

                return Ok(ApiResponse<ServiceOfferingDTO>.SuccessResult(service, "Service updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ServiceOfferingDTO>.ErrorResult("Failed to update service", 500));
            }
        }

        // DELETE: api/serviceoffering/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteService(int id)
        {
            try
            {
                var result = await _serviceOfferingService.DeleteServiceAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"Service with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "Service deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to delete service", 500));
            }
        }

        // PUT: api/serviceoffering/{id}/click
        [HttpPut("{id}/click")]
        public async Task<ActionResult<ApiResponse<bool>>> IncrementClickCount(int id)
        {
            try
            {
                var result = await _serviceOfferingService.IncrementClickCountAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"Service with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "Click count incremented"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to increment click count", 500));
            }
        }

        //// GET: api/serviceoffering/statistics
        //[HttpGet("statistics")]
        //[Authorize(Roles = "Admin")]
        //public async Task<ActionResult<ApiResponse<object>>> GetServiceStatistics()
        //{
        //    try
        //    {
        //        var statistics = await _serviceOfferingService.GetServiceStatisticsAsync();
        //        return Ok(ApiResponse<object>.SuccessResult(statistics));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ApiResponse<object>.ErrorResult("Failed to retrieve statistics", 500));
        //    }
        //}
    }
} 