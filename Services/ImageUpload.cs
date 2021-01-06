using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Logging;
using nClam;

//for clamv: https://github.com/tekmaven/nClam
//Please implement all security measures: https://docs.microsoft.com/en-us/aspn$
// Right now, we only check filesize and file extension
namespace Algoromida_01.Services
{
    public class ImageUpload : IImageUpload
    {
        private IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ImageUpload> _imageUploadLogger;
        private readonly long _fileSizeLimit;
        private readonly ClamClient _clam;
        private readonly List<string> acceptedExtensions = new List<string>(new string[] { ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".JPG", ".JPEG", ".PNG", ".GIF", ".TIFF" });
        

        public ImageUpload(IWebHostEnvironment webHostEnvironment, ILogger<ImageUpload> imageUploadLogger)
        {
            _webHostEnvironment = webHostEnvironment;
            _imageUploadLogger = imageUploadLogger;
            _fileSizeLimit = 2097152;
            _clam = new ClamClient("localhost", 5893);
        }

        public async Task<bool> scanFile(string tempFileName)
        {
            var scanResult = await _clam.ScanFileOnServerAsync(tempFileName);
            switch (scanResult.Result)
            {
                case ClamScanResults.Clean:
                    _imageUploadLogger.LogInformation("The file is clean!");
                    return true;
                case ClamScanResults.VirusDetected:
                    _imageUploadLogger.LogInformation("Virus Found!");
                    _imageUploadLogger.LogInformation("Virus name: {0}", scanResult.InfectedFiles.First().VirusName);
                    return false;
                case ClamScanResults.Error:
                    _imageUploadLogger.LogError("an error occured! Error: {0}", scanResult.RawResult);
                    return false;
            }
            return false;
        }

        public bool validateUploadedFile(IFormFile fromFile)
        {
            if (fromFile.Length > _fileSizeLimit)
            {
                _imageUploadLogger.LogInformation("Oversized file.");
                return false;
            } else if (!acceptedExtensions.Any(fromFile.FileName.EndsWith))
            {
                _imageUploadLogger.LogInformation("Non-image file detected.");
                return false;
            }
            else
            {
                _imageUploadLogger.LogInformation("File validated in terms of size and extension.");
                return true;
            }
        }

        public string getImageExtension(IFormFile fromFile)
        {
            string filename = fromFile.FileName.Trim('"');
            string ext = filename.Substring(filename.LastIndexOf("."));
            _imageUploadLogger.LogInformation("Extension: {0}", ext);
            return ext;
        }

        public async Task<bool> UploadImage(IFormFile fromFile, string newName)
        {
            _imageUploadLogger.LogInformation("Start uploading");

            long totalBytes = fromFile.Length;
            string filename = fromFile.FileName.Trim('"');

            filename = EnsureFileName(filename);
            byte[] buffer = new byte[16 * 1024];
            string tempFileName = GetPathAndFilenameTemp(newName);
            using (FileStream output = System.IO.File.Create(tempFileName))
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
	     
            _imageUploadLogger.LogInformation("Start scanning");
            bool result = await scanFile(tempFileName);

            _imageUploadLogger.LogInformation("result: {0}", result);
            if (result)
            {
                string finalFileName = GetPathAndFilename(newName);
                File.Move(tempFileName, finalFileName);
                _imageUploadLogger.LogInformation("Passed & moved");
                return true;
            }
            else
            {
                File.Delete(tempFileName);
                _imageUploadLogger.LogInformation("Unpassed & deleted");
                return false;
            }
        }
   
        private string EnsureFileName(string filename)
        {
            if (filename.Contains("//"))
            {
                filename = filename.Substring(filename.LastIndexOf("//") + 1);
            }
            _imageUploadLogger.LogInformation("Ensured filename: {0}", filename);
            return filename;
        }

        private string GetPathAndFilenameTemp(string filename)
        {
            string path = _webHostEnvironment.WebRootPath + "/temp/";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string finalPath = path + filename;
            _imageUploadLogger.LogInformation("GetPathAndFilenameTemp: {0}", finalPath);
            return finalPath;
        }

        private string GetPathAndFilename(string filename)
        {
            string path = _webHostEnvironment.WebRootPath + "/uploads/";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string finalPath = path + filename;
            _imageUploadLogger.LogInformation("GetPathAndFilename: {0}", finalPath);
            return finalPath;
        }
    }

}
