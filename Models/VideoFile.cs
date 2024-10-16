namespace MediaServer.Models
{
    public class VideoFile
    {
        public required string Path { get; set; }
        public required string Title { get; set; }
        public required string Director { get; set; }
        public required string Genre { get; set; }
        public required int Year { get; set; }
        public required MediaType MediaType { get; set; } // Neues Feld
    }

    public enum MediaType
    {
        Movie,
        Series
    }
}