namespace API_Demo.Settings
{
    public class UploadsSettings
    {
        public int ImageSizeInMB { get; set; }
        public int VideoSizeInMB { get; set; }
        public int FileSizeInMB { get; set; }
        public string UploadFolder { get; set; }
        public string ImageFolder { get; set; }
        public string VideoFolder { get; set; }
        public string FileFolder { get; set; }

        public List<string> ImageExtentionAllowed { get; set; } = new List<string>();
        public List<string> FileExtentionAllowed { get; set; } = new List<string>();
        public List<string> VideoExtentionAllowed { get; set; } = new List<string>();
    }
}
