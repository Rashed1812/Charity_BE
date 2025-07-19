using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.NewsDTOs;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;
using Shared.DTOS.ServiceOfferingDTOs;
using System.Security.Claims;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        // GET: api/news
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<NewsItemDTO>>>> GetAllNews()
        {
            try
            {
                var news = await _newsService.GetAllNewsAsync();
                return Ok(ApiResponse<List<NewsItemDTO>>.SuccessResult(news));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<NewsItemDTO>>.ErrorResult(ex.Message, 500));
            }
        }

        // GET: api/news/active
        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<List<NewsItemDTO>>>> GetActiveNews()
        {
            try
            {
                var news = await _newsService.GetActiveNewsAsync();
                return Ok(ApiResponse<List<NewsItemDTO>>.SuccessResult(news));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<NewsItemDTO>>.ErrorResult("Failed to retrieve active news", 500));
            }
        }

        // GET: api/news/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<NewsItemDTO>>> GetNewsById(int id)
        {
            try
            {
                var news = await _newsService.GetNewsByIdAsync(id);
                if (news == null)
                    return NotFound(ApiResponse<NewsItemDTO>.ErrorResult($"News with ID {id} not found", 404));

                return Ok(ApiResponse<NewsItemDTO>.SuccessResult(news));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<NewsItemDTO>.ErrorResult("Failed to retrieve news", 500));
            }
        }

        // POST: api/news
        [HttpPost]
        //[Authorize(Roles = "Admin")]ذ
        public async Task<ActionResult<ApiResponse<NewsItemDTO>>> CreateNews([FromForm] CreateNewsItemDTO createNewsDto, [FromQuery] string adminId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<NewsItemDTO>.ErrorResult("Invalid input data", 400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                if (string.IsNullOrEmpty(adminId))
                    return BadRequest(ApiResponse<NewsItemDTO>.ErrorResult("Admin ID is required", 400));

                var news = await _newsService.CreateNewsAsync(adminId, createNewsDto);
                return CreatedAtAction(nameof(GetNewsById), new { id = news.Id },
                    ApiResponse<NewsItemDTO>.SuccessResult(news, "News created successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<NewsItemDTO>.ErrorResult("Failed to create news", 500));
            }
        }

        // PUT: api/news/{id}
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<NewsItemDTO>>> UpdateNews(int id, [FromForm] UpdateNewsItemDTO updateNewsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<NewsItemDTO>.ErrorResult("Invalid input data", 400));

            try
            {
                var news = await _newsService.UpdateNewsAsync(id, updateNewsDto);
                if (news == null)
                    return NotFound(ApiResponse<NewsItemDTO>.ErrorResult($"News with ID {id} not found", 404));

                return Ok(ApiResponse<NewsItemDTO>.SuccessResult(news, "News updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<NewsItemDTO>.ErrorResult("Failed to update news", 500));
            }
        }

        // DELETE: api/news/{id}
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteNews(int id)
        {
            try
            {
                var result = await _newsService.DeleteNewsAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"News with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "News deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult(ex.Message, 500));
            }
        }

        // PUT: api/news/{id}/view
        [HttpPut("{id}/view")]
        public async Task<ActionResult<ApiResponse<bool>>> IncrementViewCount(int id)
        {
            try
            {
                var result = await _newsService.IncrementViewCountAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"News with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "View count incremented"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to increment view count", 500));
            }
        }

        // GET: api/news/statistics
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> GetNewsStatistics()
        {
            try
            {
                var statistics = await _newsService.GetNewsStatisticsAsync();
                return Ok(ApiResponse<object>.SuccessResult(statistics));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("Failed to retrieve statistics", 500));
            }
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceOfferingService _serviceOfferingService;

        public ServiceController(IServiceOfferingService serviceOfferingService)
        {
            _serviceOfferingService = serviceOfferingService;
        }

        // GET: api/service
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

        // GET: api/service/active
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

        // GET: api/service/{id}
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

        // POST: api/service
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

        // PUT: api/service/{id}
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

        // DELETE: api/service/{id}
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

        // PUT: api/service/{id}/click
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
    }
} 