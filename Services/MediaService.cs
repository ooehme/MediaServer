using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaServer.Models;
using TagLib;

namespace MediaServer.Services
{
    public class MediaService
    {
        private readonly string _mediaDirectory;
        private List<MediaFile> _mediaFiles;
        private List<VideoFile> _videoFiles;

        public MediaService(string mediaDirectory)
        {
            _mediaDirectory = mediaDirectory;
            _mediaFiles = new List<MediaFile>();
            _videoFiles = new List<VideoFile>();
            IndexMediaFiles();
            IndexVideoFiles();
        }

        private void IndexMediaFiles()
        {
            _mediaFiles.Clear();
            var files = Directory.GetFiles(_mediaDirectory, "*.*", SearchOption.AllDirectories)
                                 .Where(file => file.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) ||
                                                file.EndsWith(".wav", StringComparison.OrdinalIgnoreCase) ||
                                                file.EndsWith(".flac", StringComparison.OrdinalIgnoreCase));
            foreach (var file in files)
            {
                try
                {
                    var tagFile = TagLib.File.Create(file);
                    _mediaFiles.Add(new MediaFile
                    {
                        Path = file,
                        Artist = tagFile.Tag.FirstPerformer ?? string.Empty,
                        Album = tagFile.Tag.Album ?? string.Empty,
                        Title = tagFile.Tag.Title ?? string.Empty,
                        Genre = tagFile.Tag.FirstGenre ?? string.Empty,
                        Year = (int)tagFile.Tag.Year
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file {file}: {ex.Message}");
                }
            }
        }

        private void IndexVideoFiles()
        {
            _videoFiles.Clear();
            var files = Directory.GetFiles(_mediaDirectory, "*.*", SearchOption.AllDirectories)
                                 .Where(file => file.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase) ||
                                                file.EndsWith(".avi", StringComparison.OrdinalIgnoreCase) ||
                                                file.EndsWith(".mkv", StringComparison.OrdinalIgnoreCase));
            foreach (var file in files)
            {
                try
                {
                    var tagFile = TagLib.File.Create(file);
                    _videoFiles.Add(new VideoFile
                    {
                        Path = file,
                        Title = tagFile.Tag.Title ?? string.Empty,
                        Director = tagFile.Tag.FirstPerformer ?? string.Empty,
                        Genre = tagFile.Tag.FirstGenre ?? string.Empty,
                        Year = (int)tagFile.Tag.Year
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file {file}: {ex.Message}");
                }
            }
        }

        public List<MediaFile> GetMediaFiles()
        {
            return _mediaFiles;
        }

        public MediaFile? GetMediaFileByTitle(string title)
        {
            return _mediaFiles.FirstOrDefault(f => f.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public List<MediaFile> GetMediaFilesByArtist(string artist)
        {
            return _mediaFiles.Where(f => f.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<MediaFile> GetMediaFilesByAlbum(string album)
        {
            return _mediaFiles.Where(f => f.Album.Equals(album, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<VideoFile> GetVideoFiles()
        {
            return _videoFiles;
        }

        public VideoFile? GetVideoFileByTitle(string title)
        {
            return _videoFiles.FirstOrDefault(f => f.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public List<VideoFile> GetVideoFilesByDirector(string director)
        {
            return _videoFiles.Where(f => f.Director.Equals(director, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<VideoFile> GetVideoFilesByGenre(string genre)
        {
            return _videoFiles.Where(f => f.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}