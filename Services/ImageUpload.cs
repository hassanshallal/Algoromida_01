using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

//Please implement all security measures: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-5.0
// Right now, we only check filesize and file extension
namespace Algoromida_01.Services
{
    public class ImageUpload : IImageUpload
    {
        private IWebHostEnvironment _webHostEnvironment;
        private readonly long _fileSizeLimit;
        List<string> acceptedExtensions = new List<string>(new string[] { ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".JPG", ".JPEG", ".PNG", ".GIF", ".TIFF" });

        public ImageUpload(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _fileSizeLimit = 2097152;
        }

        public bool validateUploadedFile(IFormFile fromFile)
        {
            if ((fromFile.Length > _fileSizeLimit) || (!acceptedExtensions.Any(fromFile.FileName.EndsWith)))
            {
                return false;
            }
            else {
                return true;
            }
        }

        public string getImageExtension(IFormFile fromFile)
        {
            string filename = fromFile.FileName.Trim('"');
            string ext = filename.Substring(filename.LastIndexOf("."));
            return ext;
        }

        public async void UploadImage(IFormFile fromFile, string newName)
        {
            long totalBytes = fromFile.Length;
            string filename = fromFile.FileName.Trim('"');
            
            filename = EnsureFileName(filename);
            byte[] buffer = new byte[16 * 1024];
            using (FileStream output = System.IO.File.Create(GetPathAndFilename(newName)))
            {
                using (Stream input = fromFile.OpenReadStream())
                {
                    int readBytes;
                    while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        await output.WriteAsync(buffer, 0, readBytes);
                        totalBytes += readBytes;
                    }
                }
            }
            
        }

        private string EnsureFileName(string filename)
        {
            if (filename.Contains("//"))
            {
                filename = filename.Substring(filename.LastIndexOf("//") + 1);
            }
            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            string path = _webHostEnvironment.WebRootPath + "//uploads//";
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string finalPath = path + filename;
            return finalPath;
        }
    }
}
