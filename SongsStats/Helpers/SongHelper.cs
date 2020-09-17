using SongsStats.Models;

namespace SongsStats.Helpers
{
    public static class SongHelper
    {
        public static int CountWords(Song song)
        {
            if (!HasLyrics(song))
            {
                return 0;
            }

            return song.Lyrics.Split(" ").Length;
        }

        public static bool IsInstrumental(Song song)
        {
            if (!HasLyrics(song))
            {
                return false;
            }

            if (song.Lyrics.ToLower().Contains("instrumental") && (song.Lyrics.Split(" ").Length == 1))
            {
                return true;
            }

            return false;
        }

        public static bool HasLyrics(Song song)
        {
            return !string.IsNullOrWhiteSpace(song?.Lyrics ?? string.Empty);
        }

    }
}
