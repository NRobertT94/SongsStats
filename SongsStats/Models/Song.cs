namespace SongsStats.Models
{
    public class Song
    {
        public Artist Artist { get; set; }
        public Work Work { get; set; }
        public string Lyrics { get; set; }
    }
}
