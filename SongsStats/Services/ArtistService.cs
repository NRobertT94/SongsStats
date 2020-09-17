using SongsStats.Helpers;
using SongsStats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SongsStats.Services
{
    public class ArtistService : IArtistService
    {      
        private readonly IMusicBrainzService _musicBrainzService;
        private readonly ILyricsOvhService _lyricsOvhService;
        
        public ArtistService(IMusicBrainzService musicBrainzService, ILyricsOvhService lyricsService)
        {          
            _musicBrainzService = musicBrainzService ?? throw new ArgumentNullException(nameof(musicBrainzService));
            _lyricsOvhService = lyricsService ?? throw new ArgumentNullException(nameof(lyricsService));
        }

        public async Task<ArtistSongsStats> GetArtistSongsStats(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var artist = await _musicBrainzService.GetArtist(name);
            var artistWorks = await _musicBrainzService.GetArtistWorks(artist?.Id ?? string.Empty);

            var songsWithLyrics = await GetLyricsInParallelWithinBatches(artist, artistWorks);
         
            return ExtractStatsFromSongs(songsWithLyrics);      
        }

        private async Task<IEnumerable<Song>> GetLyricsInParallelWithinBatches(Artist artist, IEnumerable<Work> works)
        {
            var tasks = new HashSet<Task<IEnumerable<Song>>>();
            var batchSize = 25;
            var numberOfBatches = (int)Math.Ceiling((double)works.Count() / batchSize);

            for (var i = 0; i < numberOfBatches; i++)
            {
                var currentWorks = works.Skip(i * batchSize).Take(batchSize);
                tasks.Add(GetSongLyricsForBatch(artist, currentWorks));
            }

            return (await Task.WhenAll(tasks)).SelectMany(s => s);           
        }

        private async Task<IEnumerable<Song>> GetSongLyricsForBatch(Artist artist, IEnumerable<Work> works)
        {
            var songsWithLyrics = new HashSet<Song>();
            
            foreach (var work in works)
            {
                var song = await GetSongLyrics(artist, work);

                if (SongHelper.HasLyrics(song) && !SongHelper.IsInstrumental(song))
                {
                    songsWithLyrics.Add(song);
                }              
            }

            return songsWithLyrics;
        }

        private ArtistSongsStats ExtractStatsFromSongs(IEnumerable<Song> songs)
        {
            if (songs == null || !songs.Any())
            {
                return new ArtistSongsStats();
            }

            var shortestSongWordCount = songs.Min(x => SongHelper.CountWords(x));
            var shortestSong = songs.Where(x => SongHelper.CountWords(x) == shortestSongWordCount).FirstOrDefault();

            var longestSongWordCount = songs.Max(x => SongHelper.CountWords(x));
            var longestSong = songs.Where(x => SongHelper.CountWords(x) == longestSongWordCount).FirstOrDefault();

            return new ArtistSongsStats
            {
                SongsCount = songs.Count(),
                AverageWordsInSongs = (int)songs.Average(s => SongHelper.CountWords(s)),
                ShortestSongWordCount = shortestSongWordCount,
                LongestSongWordCount = longestSongWordCount,
                ShortestSong = shortestSong.Work.Title,
                LongestSong = longestSong.Work.Title
            };
        }

        private async Task<Song> GetSongLyrics(Artist artist, Work work)
        {         
            var lyricsOvhResponse = await _lyricsOvhService.GetLyrics(artist.Name, work.Title);

            if (string.IsNullOrWhiteSpace(lyricsOvhResponse?.Lyrics ?? string.Empty))
            {
                return null;
            }

            return new Song
            {
                Artist = artist,
                Work = work,
                Lyrics = lyricsOvhResponse.Lyrics
            };        
        }      
    }
}
