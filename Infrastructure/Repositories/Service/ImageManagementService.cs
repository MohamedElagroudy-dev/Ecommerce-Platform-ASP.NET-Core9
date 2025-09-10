using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Service
{
    public class ImageManagementService : IImageManagementService
    {
        private readonly IFileProvider fileProvider;
        private readonly IConfiguration _configuration;
        public ImageManagementService(IFileProvider fileProvider,IConfiguration configuration)
        {
            this.fileProvider = fileProvider;
            _configuration = configuration;
        }

        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            List<string> SaveImageSrc = new List<string>();

            var ImageDirctory = Path.Combine("wwwroot", "Images", src);

            if (!Directory.Exists(ImageDirctory))
            {
                Directory.CreateDirectory(ImageDirctory);
            }

            // Get base URL from appsettings
            string baseUrl = _configuration["ApiSettings:BaseUrl"];

            foreach (var item in files)
            {
                if (item.Length > 0)
                {
                    var ImageName = item.FileName;

                    var root = Path.Combine(ImageDirctory, ImageName);

                    using (FileStream stream = new FileStream(root, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }

                    // Add full URL for frontend
                    var ImageSrc = $"{baseUrl}/Images/{src}/{Uri.EscapeDataString(ImageName)}";
                    SaveImageSrc.Add(ImageSrc);
                }
            }
            return SaveImageSrc;
        }




        public void DeleteImageAsync(string src)
        {
            if (string.IsNullOrWhiteSpace(src))
                return; 

            if (Uri.TryCreate(src, UriKind.Absolute, out var uri))
                src = uri.AbsolutePath; 

            var filePath = Path.Combine("wwwroot", src.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

    }
}
