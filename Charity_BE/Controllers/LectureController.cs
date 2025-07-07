using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.LectureDTOs;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly ILectureService _lectureService;

        public LectureController(ILectureService lectureService)
        {
            _lectureService = lectureService;
        }

        // GET: api/lecture
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<LectureDTO>>>> GetAllLectures()
        {
            try
            {
                var lectures = await _lectureService.GetAllLecturesAsync();
                return Ok(ApiResponse<List<LectureDTO>>.SuccessResult(lectures));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<LectureDTO>>.ErrorResult("فشل في استرجاع المحاضرات", 500));
            }
        }

        // GET: api/lecture/published
        [HttpGet("published")]
        public async Task<ActionResult<ApiResponse<List<LectureDTO>>>> GetPublishedLectures()
        {
            try
            {
                var lectures = await _lectureService.GetPublishedLecturesAsync();
                return Ok(ApiResponse<List<LectureDTO>>.SuccessResult(lectures));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<LectureDTO>>.ErrorResult("فشل في استرجاع المحاضرات المنشورة", 500));
            }
        }

        // GET: api/lecture/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<LectureDTO>>> GetLectureById(int id)
        {
            try
            {
                var lecture = await _lectureService.GetLectureByIdAsync(id);
                if (lecture == null)
                    return NotFound(ApiResponse<LectureDTO>.ErrorResult($"المحاضرة برقم {id} غير موجودة", 404));

                return Ok(ApiResponse<LectureDTO>.SuccessResult(lecture));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<LectureDTO>.ErrorResult("فشل في استرجاع المحاضرة", 500));
            }
        }

        // POST: api/lecture
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<LectureDTO>>> CreateLecture([FromBody] CreateLectureDTO createLectureDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<LectureDTO>.ErrorResult("بيانات غير صحيحة", 400, 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                var adminId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(adminId))
                    return Unauthorized(ApiResponse<LectureDTO>.ErrorResult("المدير غير مصادق عليه", 401));

                var lecture = await _lectureService.CreateLectureAsync(adminId, createLectureDto);
                return CreatedAtAction(nameof(GetLectureById), new { id = lecture.Id }, 
                    ApiResponse<LectureDTO>.SuccessResult(lecture, "تم إنشاء المحاضرة بنجاح"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<LectureDTO>.ErrorResult("فشل في إنشاء المحاضرة", 500));
            }
        }

        // POST: api/lecture/upload
        [HttpPost("upload")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<LectureDTO>>> UploadVideo([FromForm] LectureUploadDTO uploadDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<LectureDTO>.ErrorResult("بيانات غير صحيحة", 400, 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                var adminId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(adminId))
                    return Unauthorized(ApiResponse<LectureDTO>.ErrorResult("المدير غير مصادق عليه", 401));

                // Validate video file
                if (!await _lectureService.ValidateVideoFileAsync(uploadDto.VideoFile))
                    return BadRequest(ApiResponse<LectureDTO>.ErrorResult("ملف الفيديو غير صالح", 400));

                var lecture = await _lectureService.UploadVideoAsync(adminId, uploadDto);
                return CreatedAtAction(nameof(GetLectureById), new { id = lecture.Id }, 
                    ApiResponse<LectureDTO>.SuccessResult(lecture, "تم رفع الفيديو بنجاح"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<LectureDTO>.ErrorResult("فشل في رفع الفيديو", 500));
            }
        }

        // PUT: api/lecture/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<LectureDTO>>> UpdateLecture(int id, [FromBody] UpdateLectureDTO updateLectureDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<LectureDTO>.ErrorResult("بيانات غير صحيحة", 400));

            try
            {
                var lecture = await _lectureService.UpdateLectureAsync(id, updateLectureDto);
                if (lecture == null)
                    return NotFound(ApiResponse<LectureDTO>.ErrorResult($"المحاضرة برقم {id} غير موجودة", 404));

                return Ok(ApiResponse<LectureDTO>.SuccessResult(lecture, "تم تحديث المحاضرة بنجاح"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<LectureDTO>.ErrorResult("فشل في تحديث المحاضرة", 500));
            }
        }

        // DELETE: api/lecture/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteLecture(int id)
        {
            try
            {
                var result = await _lectureService.DeleteLectureAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"المحاضرة برقم {id} غير موجودة", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "تم حذف المحاضرة بنجاح"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("فشل في حذف المحاضرة", 500));
            }
        }

        // PUT: api/lecture/{id}/publish
        [HttpPut("{id}/publish")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<LectureDTO>>> PublishLecture(int id)
        {
            try
            {
                var lecture = await _lectureService.PublishLectureAsync(id);
                if (lecture == null)
                    return NotFound(ApiResponse<LectureDTO>.ErrorResult($"المحاضرة برقم {id} غير موجودة", 404));

                return Ok(ApiResponse<LectureDTO>.SuccessResult(lecture, "تم نشر المحاضرة بنجاح"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<LectureDTO>.ErrorResult("فشل في نشر المحاضرة", 500));
            }
        }

        // PUT: api/lecture/{id}/unpublish
        [HttpPut("{id}/unpublish")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<LectureDTO>>> UnpublishLecture(int id)
        {
            try
            {
                var lecture = await _lectureService.UnpublishLectureAsync(id);
                if (lecture == null)
                    return NotFound(ApiResponse<LectureDTO>.ErrorResult($"المحاضرة برقم {id} غير موجودة", 404));

                return Ok(ApiResponse<LectureDTO>.SuccessResult(lecture, "تم إلغاء نشر المحاضرة بنجاح"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<LectureDTO>.ErrorResult("فشل في إلغاء نشر المحاضرة", 500));
            }
        }

        // GET: api/lecture/search
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<LectureDTO>>>> SearchLectures([FromQuery] LectureSearchDTO searchDto)
        {
            try
            {
                var lectures = await _lectureService.SearchLecturesAsync(searchDto);
                return Ok(ApiResponse<PaginatedResponse<LectureDTO>>.SuccessResult(lectures));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PaginatedResponse<LectureDTO>>.ErrorResult("فشل في البحث عن المحاضرات", 500));
            }
        }

        // GET: api/lecture/consultation/{consultationId}
        [HttpGet("consultation/{consultationId}")]
        public async Task<ActionResult<ApiResponse<List<LectureDTO>>>> GetLecturesByConsultation(int consultationId)
        {
            try
            {
                var lectures = await _lectureService.GetLecturesByConsultationAsync(consultationId);
                return Ok(ApiResponse<List<LectureDTO>>.SuccessResult(lectures));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<LectureDTO>>.ErrorResult("فشل في استرجاع محاضرات الاستشارة", 500));
            }
        }

        // GET: api/lecture/type/{type}
        [HttpGet("type/{type}")]
        public async Task<ActionResult<ApiResponse<List<LectureDTO>>>> GetLecturesByType(LectureType type)
        {
            try
            {
                var lectures = await _lectureService.GetLecturesByTypeAsync(type);
                return Ok(ApiResponse<List<LectureDTO>>.SuccessResult(lectures));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<LectureDTO>>.ErrorResult("فشل في استرجاع المحاضرات حسب النوع", 500));
            }
        }

        // GET: api/lecture/{id}/related
        [HttpGet("{id}/related")]
        public async Task<ActionResult<ApiResponse<List<LectureDTO>>>> GetRelatedLectures(int id)
        {
            try
            {
                var lectures = await _lectureService.GetRelatedLecturesAsync(id);
                return Ok(ApiResponse<List<LectureDTO>>.SuccessResult(lectures));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<LectureDTO>>.ErrorResult("فشل في استرجاع المحاضرات ذات الصلة", 500));
            }
        }

        // PUT: api/lecture/{id}/view
        [HttpPut("{id}/view")]
        public async Task<ActionResult<ApiResponse<bool>>> IncrementViewCount(int id)
        {
            try
            {
                var result = await _lectureService.IncrementViewCountAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"المحاضرة برقم {id} غير موجودة", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "تم تحديث عدد المشاهدات"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("فشل في تحديث عدد المشاهدات", 500));
            }
        }

        // PUT: api/lecture/{id}/download
        [HttpPut("{id}/download")]
        public async Task<ActionResult<ApiResponse<bool>>> IncrementDownloadCount(int id)
        {
            try
            {
                var result = await _lectureService.IncrementDownloadCountAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"المحاضرة برقم {id} غير موجودة", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "تم تحديث عدد التحميلات"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("فشل في تحديث عدد التحميلات", 500));
            }
        }

        // GET: api/lecture/{id}/stream
        [HttpGet("{id}/stream")]
        public async Task<ActionResult<ApiResponse<string>>> GetVideoStreamUrl(int id)
        {
            try
            {
                var streamUrl = await _lectureService.GetVideoStreamUrlAsync(id);
                if (string.IsNullOrEmpty(streamUrl))
                    return NotFound(ApiResponse<string>.ErrorResult($"فيديو المحاضرة برقم {id} غير موجود", 404));

                return Ok(ApiResponse<string>.SuccessResult(streamUrl));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResult("فشل في الحصول على رابط الفيديو", 500));
            }
        }

        // GET: api/lecture/statistics
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> GetLectureStatistics()
        {
            try
            {
                var statistics = await _lectureService.GetLectureStatisticsAsync();
                return Ok(ApiResponse<object>.SuccessResult(statistics));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("فشل في استرجاع إحصائيات المحاضرات", 500));
            }
        }

        // GET: api/lecture/statistics/consultation/{consultationId}
        [HttpGet("statistics/consultation/{consultationId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> GetLectureStatisticsByConsultation(int consultationId)
        {
            try
            {
                var statistics = await _lectureService.GetLectureStatisticsByConsultationAsync(consultationId);
                return Ok(ApiResponse<object>.SuccessResult(statistics));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("فشل في استرجاع إحصائيات محاضرات الاستشارة", 500));
            }
        }

        // POST: api/lecture/{id}/thumbnail
        [HttpPost("{id}/thumbnail")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> GenerateThumbnail(int id)
        {
            try
            {
                var thumbnailUrl = await _lectureService.GenerateThumbnailAsync(id);
                if (string.IsNullOrEmpty(thumbnailUrl))
                    return NotFound(ApiResponse<string>.ErrorResult($"المحاضرة برقم {id} غير موجودة", 404));

                return Ok(ApiResponse<string>.SuccessResult(thumbnailUrl, "تم إنشاء الصورة المصغرة بنجاح"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResult("فشل في إنشاء الصورة المصغرة", 500));
            }
        }

        // PUT: api/lecture/{id}/thumbnail
        [HttpPut("{id}/thumbnail")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateThumbnail(int id, [FromBody] string thumbnailUrl)
        {
            try
            {
                var result = await _lectureService.UpdateThumbnailAsync(id, thumbnailUrl);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"المحاضرة برقم {id} غير موجودة", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "تم تحديث الصورة المصغرة بنجاح"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("فشل في تحديث الصورة المصغرة", 500));
            }
        }
    }
} 