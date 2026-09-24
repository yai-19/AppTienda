using AppTienda.Infrastructure.Shared.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace AppTienda.Infrastructure.Shared.Services
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IWebHostEnvironment _env;

        public UploadFileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string?> UploadFileAsync(IFormFile? file, string folderName, string? existingFilePath = null)
        {
            if (file == null || file.Length == 0)
            {
                return existingFilePath;
            }

            // Remove previous file if exists
            if (!string.IsNullOrEmpty(existingFilePath))
            {
                DeleteFile(existingFilePath);
            }

            var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsFolder = Path.Combine(webRoot, "uploads", folderName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var extension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{folderName}/{uniqueFileName}";
        }

        public void DeleteFile(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return;

            try
            {
                var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var cleanRelative = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                var fullPath = Path.Combine(webRoot, cleanRelative);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch
            {
                // Silently ignore if file deletion fails
            }
        }
    }
}
