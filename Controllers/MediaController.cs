using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaServer.Data;
using MediaServer.Models;
using MediaServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

namespace MediaServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class MediaController : ControllerBase
    {
        private readonly MediaService _mediaService;
        private readonly IndexingService _indexingService;
        private readonly AppDbContext _dbContext;

        public MediaController(MediaService mediaService, IndexingService indexingService, AppDbContext dbContext)
        {
            _mediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
            _indexingService = indexingService ?? throw new ArgumentNullException(nameof(indexingService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpPost("index")]
        public async Task<IActionResult> StartIndexing()
        {
            try
            {
                await _indexingService.StartIndexingAsync();
                return Ok("Indexing started");
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 error
                return StatusCode(500, $"An error occurred while starting indexing: {ex.Message}");
            }
        }

        [HttpGet("music")]
        public async Task<ActionResult<List<MediaFile>>> GetAllMediaFiles()
        {
            try
            {
                return await _mediaService.GetMediaFilesAsync(_dbContext);
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 error
                return StatusCode(500, $"An error occurred while retrieving media files: {ex.Message}");
            }
        }

        [HttpGet("music/artist/{artist}")]
        public async Task<ActionResult<List<MediaFile>>> GetMediaFilesByArtist(string artist)
        {
            try
            {
                return await _mediaService.GetMediaFilesByArtistAsync(_dbContext, artist);
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 error
                return StatusCode(500, $"An error occurred while retrieving media files by artist: {ex.Message}");
            }
        }

        [HttpGet("music/album/{album}")]
        public async Task<ActionResult<List<MediaFile>>> GetMediaFilesByAlbum(string album)
        {
            try
            {
                return await _mediaService.GetMediaFilesByAlbumAsync(_dbContext, album);
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 error
                return StatusCode(500, $"An error occurred while retrieving media files by album: {ex.Message}");
            }
        }

        [HttpGet("music/play/{title}")]
        public async Task<IActionResult> PlayMediaFile(string title)
        {
            try
            {
                var fileStream = await _mediaService.GetMediaFileStreamAsync(_dbContext, title);
                if (fileStream == null)
                {
                    return NotFound();
                }
                return File(fileStream, "audio/mpeg");
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 error
                return StatusCode(500, $"An error occurred while playing media file: {ex.Message}");
            }
        }

        [HttpGet("video")]
        public async Task<ActionResult<List<VideoFile>>> GetAllVideoFiles()
        {
            try
            {
                return await _mediaService.GetVideoFilesAsync(_dbContext);
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 error
                return StatusCode(500, $"An error occurred while retrieving video files: {ex.Message}");
            }
        }

        [HttpGet("video/director/{director}")]
        public async Task<ActionResult<List<VideoFile>>> GetVideoFilesByDirector(string director)
        {
            try
            {
                return await _mediaService.GetVideoFilesByDirectorAsync(_dbContext, director);
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 error
                return StatusCode(500, $"An error occurred while retrieving video files by director: {ex.Message}");
            }
        }

        [HttpGet("video/genre/{genre}")]
        public async Task<ActionResult<List<VideoFile>>> GetVideoFilesByGenre(string genre)
        {
            try
            {
                return await _mediaService.GetVideoFilesByGenreAsync(_dbContext, genre);
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 error
                return StatusCode(500, $"An error occurred while retrieving video files by genre: {ex.Message}");
            }
        }

        [HttpGet("video/play/{title}")]
        public async Task<IActionResult> PlayVideoFile(string title)
        {
            try
            {
                var fileStream = await _mediaService.GetVideoFileStreamAsync(_dbContext, title);
                if (fileStream == null)
                {
                    return NotFound();
                }
                return File(fileStream, "video/mp4");
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 error
                return StatusCode(500, $"An error occurred while playing video file: {ex.Message}");
            }
        }
    }
}