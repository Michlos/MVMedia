namespace MVMedia.Adm.DTOs
{
    public class ClientWithMediaFileDTO
    {
        public ClientSummaryDTO Client { get; set; }
        public List<MediaFileDTO> MediaFiles { get; set; }
    }
}
