using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.FileService
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName);
        bool DeleteFile(string fileUrl);
    }
   
}
