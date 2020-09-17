using SongsStats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongsStats.Services
{
    public interface IMusicBrainzService
    {
        Task<Artist> GetArtist(string artistName);
        Task<IEnumerable<Work>> GetArtistWorks(string artistId);
    }
}
