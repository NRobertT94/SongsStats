using SongsStats.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SongsStats.Services
{
    public class LyricsOvhService : ILyricsOvhService
    {
        private readonly HttpClient _httpClient;
       
        public LyricsOvhService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<LyricsOvhResponse> GetLyrics(string artist, string song)
        {         
            var uri = $"{artist}/{song}";

            var response = await _httpClient.GetAsync(uri);
                
            if (response.IsSuccessStatusCode)
            {
                await using var stream = await response.Content.ReadAsStreamAsync();

                var lyrics = await JsonSerializer.DeserializeAsync<LyricsOvhResponse>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return lyrics;
            }
               
            return null;
        }
    }
}
