using Microsoft.AspNetCore.Http;

namespace BookStoreAPI.Service
{
    public interface IFileService
    {
        string UploadFile(IFormFile file, string imageClass);
    }
}
