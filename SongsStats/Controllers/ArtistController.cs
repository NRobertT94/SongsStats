using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SongsStats.Services;
using System;
using System.Threading.Tasks;

namespace SongsStats.Controllers
{
    public class ArtistController : Controller
    {

        private readonly IArtistService _artistService;
        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService ?? throw new ArgumentNullException(nameof(artistService));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetArtistSongsStats(string artist) 
        {
            if (string.IsNullOrWhiteSpace(artist))
            {
                return BadRequest("Please provide the artist name");
            }

            try
            {
                var result = await _artistService.GetArtistSongsStats(artist);

                return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }        
        }

       
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("pong");
        }
    }
}
