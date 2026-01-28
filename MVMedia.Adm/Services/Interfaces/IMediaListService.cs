using Microsoft.AspNetCore.Mvc;
using MVMedia.Adm.Models;

namespace MVMedia.Adm.Services.Interfaces;

public interface IMediaListService
{
    Task<MediaListViewModel> AddMediaList(IEnumerable<MediaListViewModel> mediaList);
    Task AddMediaList(ActionResult<IEnumerable<MediaFileViewModel>> mediaList);
    Task<MediaListViewModel> GetMediaList();
}
