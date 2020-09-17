using FluentAssertions;
using SongsStats.Helpers;
using Xunit;

namespace SongsStats.Tests.Helpers
{
    public class PagingHelperTests
    {

        [Fact]
        public void GetOffset_WhenWorkCountDivisibleByLimit_ShouldReturnNextOffset()
        {          
            var limit = 100;
            var workCount = 900;
            var expected = 9;

            var result = PagingHelper.GetTotalPages(workCount, limit);

            result.Should().Be(expected);
        }

        [Fact]
        public void GetOffset_WhenWorkCountNotDivisibleByLimit_ShouldReturnNextOffset()
        {           
            var limit = 100;
            var workCount = 999;
            var expected = 10;

            var result = PagingHelper.GetTotalPages(workCount, limit);

            result.Should().Be(expected);
        }
    }
}
