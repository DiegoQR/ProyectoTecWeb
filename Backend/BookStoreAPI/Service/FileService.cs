using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Service
{
    public class FileService : IFileService
    {
        public string UploadFile(IFormFile file, string imageClass)
        {
            string imagePath = string.Empty;
            var folderName = string.Empty;
            switch (imageClass)
            {
                case "book":
                    folderName = Path.Combine("images", "book_images");
                    break;
                case "editorial":
                    folderName = Path.Combine("images", "editorials_images");
                    break;
            }
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if(file.Length > 0) 
            {
                string extension = Path.GetExtension(file.FileName);
                var fileName = $"{Guid.NewGuid().ToString()}{extension}";
                var fullPath = Path.Combine(pathToSave, fileName);
                imagePath = Path.Combine(folderName, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

            }

            return imagePath;
        }
    }
}
