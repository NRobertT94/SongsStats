using SongsStats.Models;
using System.Threading.Tasks;

namespace SongsStats.Services
{
    public interface ILyricsOvhService
    {
        Task<LyricsOvhResponse> GetLyrics(string artist, string song);     
    }
}
