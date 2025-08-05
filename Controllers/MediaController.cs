using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API_Demo.Data;
using API_Demo.Dto;
using API_Demo.Model;
using API_Demo.Services;
namespace API_Demo.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]

    public class MediaController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        public MediaController(IFileService fileService, AppDbContext context, IMapper mapper)
        {
            _fileService = fileService;
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImageAsync(IFormFile file)
        {
            var dto = await _fileService.UploadImageAsync(file);
            if (dto.IsConverted)
            {
                bool isSaved = SaveToDataBase(dto);
                if (isSaved)
                    return Ok(dto);
                return BadRequest("Error While Saving in database");
            }

            return BadRequest(dto.Message);
        }

        [HttpPost("UploadVideo")]
        public async Task<IActionResult> UploadVideoAsync(IFormFile file)
        {
            var dto = await _fileService.UploadVideoAsync(file);
            if (dto.IsConverted)
            {
                bool isSaved = SaveToDataBase(dto);
                if (isSaved)
                    return Ok(dto);
                return BadRequest("Error While Saving in database");
            }

            return BadRequest(dto.Message);
        }
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            var dto = await _fileService.UploadFileAsync(file);
            if (dto.IsConverted)
            {
                bool isSaved = SaveToDataBase(dto);
                if (isSaved)
                    return Ok(dto);
                return BadRequest("Error While Saving in database");
            }

            return BadRequest(dto.Message);

        }
        [HttpPost("{type}")]
        public async Task<IActionResult> UploadGeneralAsync([FromRoute] string type, IFormFile file)
        {
            var dto = await _fileService.UploadGeneralAsync(file, type);
            if (dto.IsConverted)
            {
                bool isSaved = SaveToDataBase(dto);
                if (isSaved)
                    return Ok(dto);
                return BadRequest("Error While Saving in database");
            }

            return BadRequest(dto.Message);
        }

        [HttpGet("Download")]
        public async Task<IActionResult> DownloadFileAsync(string fileName)
        {
            var metaData = _context.Metadata.FirstOrDefault(x => x.OriginalName == fileName);
            if (metaData == null)
                return BadRequest("File Not Exist");

            var fileDto = await _fileService.DownloadFileAsync(metaData);
            if (fileDto == null)
                return BadRequest("File Not Exist");

            return File(fileDto.FileBytes, fileDto.ContentType, fileDto.DownloadName);
        }

        [HttpDelete("Remove File")]
        public IActionResult RemoveFile(string fileName)
        {
            var metaData = _context.Metadata.FirstOrDefault(x => x.OriginalName == fileName);
            if (metaData == null)
                return BadRequest("File Not Exist");

            bool deleted = _fileService.RemoveFile(metaData);
            if (deleted)
            {
                _context.Metadata.Remove(metaData);
                _context.SaveChanges();
                return Ok("Deleted Successfully");
            }
            return BadRequest("No File To Remove");

        }
        [HttpDelete("Remove All")]
        public IActionResult RemoveAll()
        {
            bool deleted = _fileService.RemoveAll();
            if (deleted)
            {
                var AllFiles = _context.Metadata.ToList();
                _context.Metadata.RemoveRange(AllFiles);
                _context.SaveChanges();
                return Ok("Deleted Successfully");
            }
            return BadRequest("No Files To Remove");
        }
        [HttpDelete("Remove Images")]
        public IActionResult RemoveAllImages()
        {

            bool deleted = _fileService.RemoveAllImage();
            if (deleted)
            {
                var AllImages = _context.Metadata.Where(x => x.FullPath.Contains("Images")).ToList();
                _context.Metadata.RemoveRange(AllImages);
                _context.SaveChanges();
                return Ok("Deleted Successfully");
            }
            return BadRequest("No Images To Remove");
        }

        [HttpDelete("Remove Videos")]
        public IActionResult RemoveAllVideos()
        {

            bool deleted = _fileService.RemoveAllVideo();
            if (deleted)
            {
                var AllVideo = _context.Metadata.Where(x => x.FullPath.Contains("Videos")).ToList();
                _context.Metadata.RemoveRange(AllVideo);
                _context.SaveChanges();
                return Ok("Deleted Successfully");
            }
            return BadRequest("No Videos To Remove");

        }
        [HttpDelete("Remove Files")]
        public IActionResult RemoveAllFiles()
        {

            bool deleted = _fileService.RemoveAllFile();
            if (deleted)
            {
                var AllVideo = _context.Metadata.Where(x => x.FullPath.Contains("Files")).ToList();
                _context.Metadata.RemoveRange(AllVideo);
                _context.SaveChanges();
                return Ok("Deleted Successfully");
            }
            return BadRequest("No Files To Remove");
        }
        private bool SaveToDataBase(MetadataDto dto)
        {
            var metaData = _mapper.Map<Metadata>(dto);
            try
            {
                _context.Metadata.Add(metaData);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

