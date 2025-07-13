using DAL.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.FileService
{
    public class FileService : IFileService
    {
        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
            {
                if (file == null || file.Length == 0)
                    return null;

                // Validate folder name
                if (string.IsNullOrWhiteSpace(folderName))
                    throw new ArgumentException("Folder name cannot be empty");

                // Create uploads folder if it doesn't exist
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique filename
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return $"/{folderName}/{uniqueFileName}";
            }

        public bool DeleteFile(string fileUrl)
            {
                if (string.IsNullOrWhiteSpace(fileUrl))
                    return false;

                try
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), ("wwwroot"+
                                                                     fileUrl).TrimStart('/'));

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        return true;
                    }
                    return false;
                }
                catch
                {
                    // Log error if needed
                    return false;
                }
            }
        }
    
}
