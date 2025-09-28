namespace MVMedia.Adm.DTOs
{
    public class ClientWithMediaDTO
    {
        public ClientSummaryDTO Client { get; set; }
        public List<MediaListItemDTO> Medias { get; set; }
    }
}
