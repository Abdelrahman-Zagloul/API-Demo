namespace API_Demo.Dto
{
    public class FileDownloadDto
    {
        public byte[] FileBytes { get; set; }
        public string ContentType { get; set; }
        public string DownloadName { get; set; }
    }
}
