using Microsoft.Extensions.Options;
using API_Demo.Configuration;
using API_Demo.Dto;
using API_Demo.Model;
using API_Demo.Settings;

namespace API_Demo.Services
{
    public class FileService : IFileService
    {
        private readonly UploadsSettings _options;
        private readonly IWebHostEnvironment _webHost;
        public FileService(IOptions<UploadsSettings> options, IWebHostEnvironment webHost)
        {
            _options = options.Value;
            _webHost = webHost;
        }
        public async Task<MetadataDto> UploadImageAsync(IFormFile file)
        {
            if (file == null)
                return new MetadataDto() { Message = "File Can't be null" };

            var extention = Path.GetExtension(file.FileName).ToLower();
            bool allowed = IsAllowedExtention(extention, _options.ImageExtentionAllowed);
            if (!allowed)
                return new MetadataDto() { Message = $"Extention Is Not Valid =>Must be one of this: {string.Join(',', _options.ImageExtentionAllowed)}" };

            //file size
            if (file.Length <= 0 || file.Length > _options.ImageSizeInMB * 1024 * 1024)
                return new MetadataDto() { Message = $"Size must be Greather Than 0 and less than {_options.ImageSizeInMB}MB " };

            var ImageFolderPath = Path.Combine(_webHost.WebRootPath, _options.UploadFolder, _options.ImageFolder);
            var newName = await Convert(file, ImageFolderPath, extention);
            return new MetadataDto
            {
                Message = "Saved Image Successfully",
                Extention = extention,
                FackName = newName,
                OriginalName = file.FileName,
                IsConverted = true,
                SizeInByte = file.Length,
                ContentType = file.ContentType,
                FullPath = Path.Combine(ImageFolderPath, newName),
            };
        }
        public async Task<MetadataDto> UploadVideoAsync(IFormFile file)
        {
            if (file == null)
                return new MetadataDto() { Message = "File Can't be null" };
            var extention = Path.GetExtension(file.FileName).ToLower();
            bool allowed = IsAllowedExtention(extention, _options.VideoExtentionAllowed);
            if (!allowed)
                return new MetadataDto() { Message = $"Extention Is Not Valid =>Must be one of this: {string.Join(',', _options.VideoExtentionAllowed)}" };

            //file size
            if (file.Length <= 0 || file.Length > _options.VideoSizeInMB * 1024 * 1024)
                return new MetadataDto() { Message = $"Size must be Greather Than 0 and less than {_options.VideoSizeInMB}MB " };

            var videoFolderPath = Path.Combine(_webHost.WebRootPath, _options.UploadFolder, _options.VideoFolder);
            var newName = await Convert(file, videoFolderPath, extention);
            return new MetadataDto
            {
                Message = "Saved Video Successfully",
                Extention = extention,
                FackName = newName,
                OriginalName = file.FileName,
                IsConverted = true,
                SizeInByte = file.Length,
                ContentType = file.ContentType,
                FullPath = Path.Combine(videoFolderPath, newName),
            };

        }
        public async Task<MetadataDto> UploadFileAsync(IFormFile file)
        {
            if (file == null)
                return new MetadataDto() { Message = "File Can't be null" };
            var extention = Path.GetExtension(file.FileName).ToLower();
            bool allowed = IsAllowedExtention(extention, _options.FileExtentionAllowed);
            if (!allowed)
                return new MetadataDto() { Message = $"Extention Is Not Valid =>Must be one of this: {string.Join(',', _options.FileExtentionAllowed)}" };

            //file size
            if (file.Length <= 0 || file.Length > _options.FileSizeInMB * 1024 * 1024)
                return new MetadataDto() { Message = $"Size must be Greather Than 0 and less than {_options.FileSizeInMB}MB " };

            var fileFolderPath = Path.Combine(_webHost.WebRootPath, _options.UploadFolder, _options.FileFolder);
            var newName = await Convert(file, fileFolderPath, extention);
            return new MetadataDto
            {
                Message = "Saved File Successfully",
                Extention = extention,
                FackName = newName,
                OriginalName = file.FileName,
                IsConverted = true,
                SizeInByte = file.Length,
                ContentType = file.ContentType,
                FullPath = Path.Combine(fileFolderPath, newName),
            };
        }
        public async Task<MetadataDto> UploadGeneralAsync(IFormFile file, string type)
        {
            if (file == null)
                return new MetadataDto() { Message = "File Can't be null" };

            string extention = Path.GetExtension(file.FileName).ToLower();
            string folder = "";
            List<string> allowedExtension;
            int maxSizeMB;

            switch (type.ToLower())
            {
                case "image":
                    allowedExtension = _options.ImageExtentionAllowed;
                    maxSizeMB = _options.ImageSizeInMB;
                    folder = _options.ImageFolder;
                    break;
                case "video":
                    allowedExtension = _options.VideoExtentionAllowed;
                    maxSizeMB = _options.VideoSizeInMB;
                    folder = _options.VideoFolder;
                    break;
                case "file":
                    allowedExtension = _options.FileExtentionAllowed;
                    maxSizeMB = _options.FileSizeInMB;
                    folder = _options.FileFolder;
                    break;
                default:
                    return new MetadataDto() { Message = "Unsupported type." };
            }

            if (!allowedExtension.Contains(extention))
                return new MetadataDto() { Message = $"Extension not allowed. Allowed: {string.Join(", ", allowedExtension)}" };

            if (file.Length <= 0 || file.Length > maxSizeMB * 1024 * 1024)
                return new MetadataDto { Message = $"File size must be > 0 and < {maxSizeMB}MB" };

            var savePath = Path.Combine(_webHost.WebRootPath, _options.UploadFolder, folder);
            var newName = await Convert(file, savePath, extention);

            return new MetadataDto
            {
                Message = "Saved Video Successfully",
                Extention = extention,
                FackName = newName,
                OriginalName = file.FileName,
                IsConverted = true,
                SizeInByte = file.Length,
                ContentType = file.ContentType,
                FullPath = Path.Combine(savePath, newName)
            };
        }
        public async Task<FileDownloadDto?> DownloadFileAsync(Metadata metadata)
        {
            if (metadata == null)
                return null;
            var fileBytes = await File.ReadAllBytesAsync(metadata.FullPath);
            return new FileDownloadDto
            {
                FileBytes = fileBytes,
                ContentType = metadata.ContentType,
                DownloadName = metadata.OriginalName,
            };

        }

        //Using Memory Stream not better
        public async Task<FileDownloadDto?> DownloadFileAsync1(Metadata metadata)
        {
            if (metadata == null || !File.Exists(metadata.FullPath))
                return null;

            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                using (var fileStream = new FileStream(metadata.FullPath, FileMode.Open, FileAccess.Read))
                    await fileStream.CopyToAsync(ms);
                fileBytes = ms.ToArray();
            }
            return new FileDownloadDto
            {
                FileBytes = fileBytes,
                ContentType = metadata.ContentType,
                DownloadName = metadata.OriginalName,
            };

        }
        public bool RemoveFile(Metadata metadata)
        {
            if (!File.Exists(metadata.FullPath))
                return false;

            File.Delete(metadata.FullPath);
            return true;
        }
        public bool RemoveAll()
        {
            var uploadFolder = Path.Combine(_webHost.WebRootPath, _options.UploadFolder);
            if (Directory.Exists(uploadFolder))
            {
                Directory.Delete(uploadFolder, true);
                return true;
            }
            return false;
        }
        public bool RemoveAllImage()
        {
            var imageFolder = Path.Combine(_webHost.WebRootPath, _options.UploadFolder, _options.ImageFolder);
            if (Directory.Exists(imageFolder))
            {
                Directory.Delete(imageFolder, true);
                return true;
            }
            return false;
        }
        public bool RemoveAllVideo()
        {
            var videoFolder = Path.Combine(_webHost.WebRootPath, _options.UploadFolder, _options.VideoFolder);
            if (Directory.Exists(videoFolder))
            {
                Directory.Delete(videoFolder, true);
                return true;
            }
            return false;
        }
        public bool RemoveAllFile()
        {
            var fileFolder = Path.Combine(_webHost.WebRootPath, _options.UploadFolder, _options.FileFolder);
            if (Directory.Exists(fileFolder))
            {
                Directory.Delete(fileFolder, true);
                return true;
            }
            return false;
        }

        private bool IsAllowedExtention(string extention, List<string> allowedExtensions)
        {
            return allowedExtensions.Contains(extention);
        }
        private async Task<string> Convert(IFormFile file, string path, string extention)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string newName = $"{Guid.NewGuid().ToString()}{extention}";
            string fullPath = Path.Combine(path, newName);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
                await file.CopyToAsync(fileStream);
            return newName;
        }
    }
}
