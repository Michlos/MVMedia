namespace MVMedia.Adm.DTOs
{
    public class ClientWithMediaFileDTO
    {
        public required ClientSummaryDTO Client { get; set; }
        public List<MediaFileLIstItemDTO>? MediaFiles { get; set; }
    }
}
