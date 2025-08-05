using API_Demo.Dto;
using API_Demo.Model;

namespace API_Demo.Services
{
    public interface IFileService
    {
        Task<MetadataDto> UploadImageAsync(IFormFile file);
        Task<MetadataDto> UploadVideoAsync(IFormFile file);
        Task<MetadataDto> UploadFileAsync(IFormFile file);
        Task<MetadataDto> UploadGeneralAsync(IFormFile file, string type);
        Task<FileDownloadDto?> DownloadFileAsync(Metadata metadata);
        bool RemoveFile(Metadata metadata);
        bool RemoveAll();
        bool RemoveAllImage();
        bool RemoveAllVideo();
        bool RemoveAllFile();
    }
}
