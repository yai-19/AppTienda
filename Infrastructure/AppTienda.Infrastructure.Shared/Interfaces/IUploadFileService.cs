using Microsoft.AspNetCore.Http;

namespace AppTienda.Infrastructure.Shared.Interfaces
{
    public interface IUploadFileService
    {
        Task<string?> UploadFileAsync(IFormFile? file, string folderName, string? existingFilePath = null);
        void DeleteFile(string? relativePath);
    }
}
