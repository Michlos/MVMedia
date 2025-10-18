using MVMedia.Web.Models;

namespace MVMedia.Web.Service.Interfaces
{
    public interface IMediaService
    {
        Task<IEnumerable<MediaViewModel>> GetAllMedia();
    }
}
