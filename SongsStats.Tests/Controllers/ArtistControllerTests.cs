using SongsStats.Controllers;
using SongsStats.Models;
using SongsStats.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SongsStats.Tests.Controllers
{
    public class ArtistControllerTests
    {
        private readonly Mock<IArtistService> _artistServiceMock;     
        private readonly ArtistController _artistController;

        public ArtistControllerTests()
        {
            _artistServiceMock = new Mock<IArtistService>();
            _artistController = new ArtistController(_artistServiceMock.Object);
        }
        
        [Fact]
        public void Constructor_WhenArtistServiceIsNull_ThrowsArgumentNullException()
        {
            Action act = () => new ArtistController(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task GetArtistSongsStats_WhenArtistNameIsEmpty_ShouldReturnBadRequest()
        {
            _artistServiceMock.Setup(_ => _.GetArtistSongsStats(It.IsAny<string>())).Returns(Task.FromResult(new ArtistSongsStats()));
            
            var response = await _artistController.GetArtistSongsStats("");

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetArtistSongsStats_WhenArtistNameIsCorrect_ShouldReturnBadRequest()
        {
            _artistServiceMock.Setup(_ => _.GetArtistSongsStats(It.IsAny<string>())).Returns(Task.FromResult(new ArtistSongsStats()));

            var response = await _artistController.GetArtistSongsStats("John Smith");

            response.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetArtistSongsStats_WhenArtistServiceThrowsException_ShouldThrowInternalServerError()
        {
            _artistServiceMock.Setup(_ => _.GetArtistSongsStats(It.IsAny<string>())).Throws<Exception>();

            var response = await _artistController.GetArtistSongsStats("John Smith");

            response.Should().BeOfType<StatusCodeResult>();
            var statusCodeResult = response as StatusCodeResult;

            statusCodeResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task GetArtistSongsStats_WhenArtistNameIsGiven_ShouldCallArtistService()
        {
            await _artistController.GetArtistSongsStats("John Smith");

            _artistServiceMock.Verify(_ => _.GetArtistSongsStats(It.IsAny<string>()), Times.Once);
        }
     
    }
}
