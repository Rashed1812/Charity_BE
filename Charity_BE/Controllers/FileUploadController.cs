using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.Common;
using System.IO;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploadController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        // POST: api/fileupload/advisor-image
        [HttpPost("advisor-image")]
        [Authorize(Roles = "Admin,Advisor")]
        public async Task<ActionResult<ApiResponse<string>>> UploadAdvisorImage(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(ApiResponse<string>.ErrorResult("No file uploaded", 400));

                // التحقق من نوع الملف
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                    return BadRequest(ApiResponse<string>.ErrorResult("Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.", 400));

                // التحقق من حجم الملف (5MB كحد أقصى)
                if (file.Length > 5 * 1024 * 1024)
                    return BadRequest(ApiResponse<string>.ErrorResult("File size too large. Maximum size is 5MB.", 400));

                // إنشاء مجلد الصور إذا لم يكن موجود
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "advisors");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // إنشاء اسم فريد للملف
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // حفظ الملف
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // إرجاع الرابط النسبي للملف
                var imageUrl = $"/images/advisors/{fileName}";

                return Ok(ApiResponse<string>.SuccessResult(imageUrl, "Image uploaded successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResult("Failed to upload image", 500));
            }
        }

        // DELETE: api/fileupload/advisor-image
        [HttpDelete("advisor-image")]
        [Authorize(Roles = "Admin,Advisor")]
        public ActionResult<ApiResponse<bool>> DeleteAdvisorImage([FromQuery] string imageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                    return BadRequest(ApiResponse<bool>.ErrorResult("Image URL is required", 400));

                // التحقق من أن الرابط يبدأ بـ /images/advisors/
                if (!imageUrl.StartsWith("/images/advisors/"))
                    return BadRequest(ApiResponse<bool>.ErrorResult("Invalid image URL", 400));

                // حذف الملف
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
                
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return Ok(ApiResponse<bool>.SuccessResult(true, "Image deleted successfully"));
                }
                else
                {
                    return NotFound(ApiResponse<bool>.ErrorResult("Image not found", 404));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to delete image", 500));
            }
        }
    }
} 