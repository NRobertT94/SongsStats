using SongsStats.Models;
using FluentAssertions;
using SongsStats.Helpers;
using Xunit;

namespace SongsStats.Tests.Helpers
{
    public class SongHelperTests
    {
        [Fact]
        public void CountWords_WhenSongIsNull_ShouldReturnZero()
        {
            var result = SongHelper.CountWords(null);

            result.Should().Be(0);
        }

        [Fact]
        public void CountWords_WhenSongLyricsIsNull_ShouldReturnZero()
        {
            var song = new Song();

            var result = SongHelper.CountWords(song);

            result.Should().Be(0);
        }

        [Fact]
        public void CountWords_WhenSongLyricsIsEmpty_ShouldReturnZero()
        {
            var song = new Song { Lyrics = string.Empty };

            var result = SongHelper.CountWords(song);

            result.Should().Be(0);
        }

        [Fact]
        public void CountWords_WhenSongHasLyrics_ShouldWordCount()
        {
            var song = new Song { Lyrics = "hello world" };

            var result = SongHelper.CountWords(song);

            result.Should().Be(2);
        }

        [Fact]
        public void CountWords_WhenSongIsInstrumental_ShouldWordCount()
        {
            var song = new Song { Lyrics = "instrumental" };

            var result = SongHelper.CountWords(song);

            result.Should().Be(1);
        }

        [Fact]
        public void IsInstrumental_WhenSongIsInstrumental_ShouldReturnTrue()
        {
            var song = new Song
            {
                Lyrics = "(instrumental)"
            };

            var result = SongHelper.IsInstrumental(song);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsInstrumental_WhenSongIsNotInstrumental_ShouldReturnFalse()
        {
            var song = new Song
            {
                Lyrics = "hello world"
            };

            var result = SongHelper.IsInstrumental(song);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsInstrumental_WhenSongHasOneWord_ShouldReturnFalse()
        {
            var song = new Song
            {
                Lyrics = "test"
            };

            var result = SongHelper.IsInstrumental(song);

            result.Should().BeFalse();
        }

        [Fact]
        public void HasLyrics_WhenSongIsNull_ShouldReturnFalse()
        {        
            var result = SongHelper.HasLyrics(null);

            result.Should().BeFalse();
        }


        [Fact]
        public void HasLyrics_WhenSongHasEmptyLyrics_ShouldReturnFalse()
        {
            var song = new Song
            {
                Lyrics = string.Empty

            };
            var result = SongHelper.HasLyrics(song);

            result.Should().BeFalse();
        }

        [Fact]
        public void HasLyrics_WhenSongHasLyrics_ShouldReturnTrue()
        {
            var song = new Song
            {
                Lyrics = "hello world"

            };
            var result = SongHelper.HasLyrics(song);

            result.Should().BeTrue();
        }
    }
}
