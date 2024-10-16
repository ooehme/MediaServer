using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediaServer.Data;
using MediaServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TagLib;

namespace MediaServer.Services
{
    public class MediaService
    {
        private readonly string _mediaDirectory;
    
        public MediaService(IConfiguration configuration)
        {
            _mediaDirectory = configuration.GetSection("MediaSettings:MediaDirectory").Value;
            if (string.IsNullOrEmpty(_mediaDirectory))
            {
                throw new InvalidOperationException("Media directory path is not configured.");
            }
        }

        public async Task IndexMediaFilesAsync(AppDbContext dbContext)
        {
            var files = Directory.GetFiles(_mediaDirectory, "*.*", SearchOption.AllDirectories)
                                 .Where(file => file.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) ||
                                                file.EndsWith(".wav", StringComparison.OrdinalIgnoreCase) ||
                                                file.EndsWith(".flac", StringComparison.OrdinalIgnoreCase));

            foreach (var file in files)
            {
                try
                {
                    var tagFile = TagLib.File.Create(file);
                    var mediaFile = new MediaFile
                    {
                        Path = file,
                        Artist = tagFile.Tag.FirstPerformer ?? string.Empty,
                        Album = tagFile.Tag.Album ?? string.Empty,
                        Title = tagFile.Tag.Title ?? Path.GetFileNameWithoutExtension(file),
                        Genre = tagFile.Tag.FirstGenre ?? string.Empty,
                        Year = (int)tagFile.Tag.Year
                    };

                    var existingFile = await dbContext.MediaFiles.FindAsync(file);
                    if (existingFile == null)
                    {
                        dbContext.MediaFiles.Add(mediaFile);
                    }
                    else
                    {
                        dbContext.Entry(existingFile).CurrentValues.SetValues(mediaFile);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file {file}: {ex.Message}");
                }
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task IndexVideoFilesAsync(AppDbContext dbContext)
        {
            var files = Directory.GetFiles(_mediaDirectory, "*.*", SearchOption.AllDirectories)
                                 .Where(file => file.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase) ||
                                                file.EndsWith(".avi", StringComparison.OrdinalIgnoreCase) ||
                                                file.EndsWith(".mkv", StringComparison.OrdinalIgnoreCase));

            foreach (var file in files)
            {
                try
                {
                    var tagFile = TagLib.File.Create(file);
                    var videoFile = new VideoFile
                    {
                        Path = file,
                        Title = tagFile.Tag.Title ?? Path.GetFileNameWithoutExtension(file),
                        Director = tagFile.Tag.FirstPerformer ?? string.Empty,
                        Genre = tagFile.Tag.FirstGenre ?? string.Empty,
                        Year = (int)tagFile.Tag.Year
                    };

                    var existingFile = await dbContext.VideoFiles.FindAsync(file);
                    if (existingFile == null)
                    {
                        dbContext.VideoFiles.Add(videoFile);
                    }
                    else
                    {
                        dbContext.Entry(existingFile).CurrentValues.SetValues(videoFile);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file {file}: {ex.Message}");
                }
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<List<MediaFile>> GetMediaFilesAsync(AppDbContext dbContext)
        {
            return await dbContext.MediaFiles.ToListAsync();
        }

        public async Task<MediaFile> GetMediaFileByTitleAsync(AppDbContext dbContext, string title)
        {
            return await dbContext.MediaFiles.FirstOrDefaultAsync(f => EF.Functions.Like(f.Title, $"%{title}%"));
        }

        public async Task<List<MediaFile>> GetMediaFilesByArtistAsync(AppDbContext dbContext, string artist)
        {
            return await dbContext.MediaFiles
                .Where(f => EF.Functions.Like(f.Artist, $"%{artist}%"))
                .ToListAsync();
        }

        public async Task<List<MediaFile>> GetMediaFilesByAlbumAsync(AppDbContext dbContext, string album)
        {
            return await dbContext.MediaFiles
                .Where(f => EF.Functions.Like(f.Album, $"%{album}%"))
                .ToListAsync();
        }

        public async Task<List<VideoFile>> GetVideoFilesAsync(AppDbContext dbContext)
        {
            return await dbContext.VideoFiles.ToListAsync();
        }

        public async Task<VideoFile> GetVideoFileByTitleAsync(AppDbContext dbContext, string title)
        {
            return await dbContext.VideoFiles.FirstOrDefaultAsync(f => EF.Functions.Like(f.Title, $"%{title}%"));
        }

        public async Task<List<VideoFile>> GetVideoFilesByDirectorAsync(AppDbContext dbContext, string director)
        {
            return await dbContext.VideoFiles
                .Where(f => EF.Functions.Like(f.Director, $"%{director}%"))
                .ToListAsync();
        }

        public async Task<List<VideoFile>> GetVideoFilesByGenreAsync(AppDbContext dbContext, string genre)
        {
            return await dbContext.VideoFiles
                .Where(f => EF.Functions.Like(f.Genre, $"%{genre}%"))
                .ToListAsync();
        }

        public async Task<FileStream> GetMediaFileStreamAsync(AppDbContext dbContext, string title)
        {
            var mediaFile = await GetMediaFileByTitleAsync(dbContext, title);
            if (mediaFile == null)
            {
                return null;
            }

            return new FileStream(mediaFile.Path, FileMode.Open, FileAccess.Read);
        }

        public async Task<FileStream> GetVideoFileStreamAsync(AppDbContext dbContext, string title)
        {
            var videoFile = await GetVideoFileByTitleAsync(dbContext, title);
            if (videoFile == null)
            {
                return null;
            }

            return new FileStream(videoFile.Path, FileMode.Open, FileAccess.Read);
        }
    }
}