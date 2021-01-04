using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Algoromida_01.Services
{
    public interface IImageUpload
    {
        Task<bool> scanFile(string tempFileName);
	bool validateUploadedFile(IFormFile fromFile);
        string getImageExtension(IFormFile fromFile);
        void UploadImage(IFormFile fromFile, string newName);
    }
}
