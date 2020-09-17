using FluentAssertions;
using Moq;
using SongsStats.Models;
using SongsStats.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SongsStats.Tests.Services
{
    public class ArtistServiceTests
    {
        private readonly Mock<IMusicBrainzService> _musicBrainzServiceMock;
        private readonly Mock<ILyricsOvhService> _lyricsOvhServiceMock;

        private readonly IArtistService _artistService;
 
        public ArtistServiceTests()
        {
            _musicBrainzServiceMock = new Mock<IMusicBrainzService>();
            _lyricsOvhServiceMock = new Mock<ILyricsOvhService>();
            
            _artistService = new ArtistService(_musicBrainzServiceMock.Object, _lyricsOvhServiceMock.Object);
        }

        [Fact]
        public void Constructor_WhenMusicBrainzServiceIsNull_ThrowsArgumentNullException()
        {
            Action act = () => new ArtistService(null, _lyricsOvhServiceMock.Object);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WhenLyricsOvhServiceIsNull_ThrowsArgumentNullException()
        {
            Action act = () => new ArtistService(_musicBrainzServiceMock.Object, null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task GetArtistSongsStats_WhenArtistNameIsNull_ShouldReturnNull()
        {
            var result = await _artistService.GetArtistSongsStats(null);

            result.Should().Be(null);
        }

        [Fact]
        public async Task GetArtistSongsStats_WhenArtistNameIsEmpty_ShouldReturnNull()
        {
            var result = await _artistService.GetArtistSongsStats("");

            result.Should().Be(null);
        }

        [Fact]
        public async Task GetArtistSongsStats_WhenArtistNameIsCorrect_ShouldReturnTypeArtistsSongsStats()
        {
            var artistName = "John Smith";

            SetupDependencies(artistName, GenerateStubSongs());

            var result = await _artistService.GetArtistSongsStats(artistName);
          
            result.Should().BeOfType<ArtistSongsStats>();
        }

        [Fact]
        public async Task GetArtistSongsStats_WhenSongsContainLyrics_ShouldReturnArtistsSongsStats()
        {
            var artistName = "John Smith";
            var expected = new ArtistSongsStats
            {
                SongsCount = 2,
                AverageWordsInSongs = 3,
                ShortestSong = "Song1",
                ShortestSongWordCount = 2,
                LongestSong = "Song2",
                LongestSongWordCount = 4,
            };

            SetupDependencies(artistName, GenerateStubSongs());

            var result = await _artistService.GetArtistSongsStats(artistName);

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetArtistSongsStats_WhenWorksContainInstrumental_ShouldFilterOutInstrumentalSongs()
        {
            var artistName = "John Smith";
            var expected = new ArtistSongsStats
            {
                SongsCount = 1,
                AverageWordsInSongs = 2,
                ShortestSong = "Song1",
                ShortestSongWordCount = 2,
                LongestSong = "Song1",
                LongestSongWordCount = 2,
            };

            SetupDependencies(artistName, GenerateStubSongsWithOneInstrumental());

            var result = await _artistService.GetArtistSongsStats(artistName);

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetArtistSongsStats_WhenWorksContainNoLyrics_ShouldFilterOutSongsWithoutLyrics()
        {
            var artistName = "John Smith";
            var expected = new ArtistSongsStats();
            
            SetupDependencies(artistName, GenerateStubSongsWithNoLyrics());

            var result = await _artistService.GetArtistSongsStats(artistName);

            result.Should().BeEquivalentTo(expected);
        }

        private void SetupDependencies(string artistName, List<Song> stubSongs)
        {
            var sampleArtist = new Artist { Id = "123", Name = artistName };

            var works = stubSongs.Select(x => x.Work);
               
            _musicBrainzServiceMock.Setup(x => x.GetArtist(It.IsAny<string>())).Returns(Task.FromResult(sampleArtist));
            _musicBrainzServiceMock.Setup(x => x.GetArtistWorks(It.IsAny<string>())).Returns(Task.FromResult(works));

            foreach(var song in stubSongs)
            {                
                _lyricsOvhServiceMock
                    .Setup(x => x.GetLyrics(It.IsAny<string>(), song.Work.Title))
                    .Returns(Task.FromResult(new LyricsOvhResponse { Lyrics = song.Lyrics}));
            }
           
        }

        private List<Song> GenerateStubSongs()
        {
            var result = new List<Song> {
                new Song
                {
                    Artist = new Artist { Id= "a1", Name = "John Smith"},
                    Work = new Work {Id = "w1", Title = "Song1", Type = "Song"},
                    Lyrics = "Hello World"
                },
                new Song
                {
                    Artist = new Artist { Id= "a1", Name = "John Smith"},
                    Work = new Work {Id = "w2", Title = "Song2", Type = "Song"},
                    Lyrics = "Second Hello World Song"
                }
            };

            return result;
        }

        private List<Song> GenerateStubSongsWithOneInstrumental()
        {
            var result = new List<Song> {
                new Song
                {
                    Artist = new Artist { Id= "a1", Name = "John Smith"},
                    Work = new Work {Id = "w1", Title = "Song1", Type = "Song"},
                    Lyrics = "Hello World"
                },
                new Song
                {
                    Artist = new Artist { Id= "a1", Name = "John Smith"},
                    Work = new Work {Id = "w2", Title = "Song2", Type = "Song"},
                    Lyrics = "instrumental"
                }
            };

            return result;
        }
        private List<Song> GenerateStubSongsWithNoLyrics()
        {
            var result = new List<Song> {
                new Song
                {
                    Artist = new Artist { Id= "a1", Name = "John Smith"},
                    Work = new Work {Id = "w1", Title = "Song1", Type = "Song"},
                    Lyrics = ""
                },
                new Song
                {
                    Artist = new Artist { Id= "a1", Name = "John Smith"},
                    Work = new Work {Id = "w2", Title = "Song2", Type = "Song"},
                    Lyrics = ""
                }
            };

            return result;
        }

    }
}
