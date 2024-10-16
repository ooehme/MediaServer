using System.Collections.Generic;
using System.IO;
using MediaServer.Models;
using MediaServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediaServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly MediaService _mediaService;

        public MediaController(MediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet("music")]
        public ActionResult<List<MediaFile>> GetAllMediaFiles()
        {
            return _mediaService.GetMediaFiles();
        }

        [HttpGet("music/artist/{artist}")]
        public ActionResult<List<MediaFile>> GetMediaFilesByArtist(string artist)
        {
            return _mediaService.GetMediaFilesByArtist(artist);
        }

        [HttpGet("music/album/{album}")]
        public ActionResult<List<MediaFile>> GetMediaFilesByAlbum(string album)
        {
            return _mediaService.GetMediaFilesByAlbum(album);
        }

        [HttpGet("music/play/{title}")]
        public IActionResult PlayMediaFile(string title)
        {
            var mediaFile = _mediaService.GetMediaFileByTitle(title);
            if (mediaFile == null)
            {
                return NotFound();
            }

            var fileStream = new FileStream(mediaFile.Path, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, "audio/mpeg");
        }

        [HttpGet("video")]
        public ActionResult<List<VideoFile>> GetAllVideoFiles()
        {
            return _mediaService.GetVideoFiles();
        }

        [HttpGet("video/director/{director}")]
        public ActionResult<List<VideoFile>> GetVideoFilesByDirector(string director)
        {
            return _mediaService.GetVideoFilesByDirector(director);
        }

        [HttpGet("video/genre/{genre}")]
        public ActionResult<List<VideoFile>> GetVideoFilesByGenre(string genre)
        {
            return _mediaService.GetVideoFilesByGenre(genre);
        }

        [HttpGet("video/play/{title}")]
        public IActionResult PlayVideoFile(string title)
        {
            var videoFile = _mediaService.GetVideoFileByTitle(title);
            if (videoFile == null)
            {
                return NotFound();
            }

            var fileStream = new FileStream(videoFile.Path, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, "video/mp4");
        }
    }
}