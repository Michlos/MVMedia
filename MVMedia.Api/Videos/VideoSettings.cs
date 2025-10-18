namespace MVMedia.Api.Videos;
public class VideoSettings
    {
        public string VideoPath { get; set; }
        public int MaxFileSizeMB { get; set; }
        public List<string> AllowFileTypes { get; set; }
    }
