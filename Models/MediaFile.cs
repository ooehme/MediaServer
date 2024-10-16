namespace MediaServer.Models
{
    public class MediaFile
    {
        public required string Path { get; set; }
        public required string Artist { get; set; }
        public required string Album { get; set; }
        public required string Title { get; set; }
        public required string Genre { get; set; }
        public required int Year { get; set; }
    }
}