namespace SongsStats.Models
{
    public class ArtistSongsStats
    {
        public int AverageWordsInSongs { get; set; }

        public string ShortestSong { get; set; }

        public string LongestSong { get; set; }

        public int ShortestSongWordCount { get; set; }

        public int LongestSongWordCount { get; set; }

        public double StandardDeviation { get; set; }

        public int SongsCount { get; set; }
    }
}
