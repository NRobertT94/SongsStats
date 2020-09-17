using SongsStats.Models;
using System.Threading.Tasks;

namespace SongsStats.Services
{
    public interface IArtistService
    {
        Task<ArtistSongsStats> GetArtistSongsStats(string name);
    }
}
