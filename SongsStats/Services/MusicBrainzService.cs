using Microsoft.CodeAnalysis;
using SongsStats.Helpers;
using SongsStats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SongsStats.Services
{
    public class MusicBrainzService : IMusicBrainzService
    {
        private readonly HttpClient _httpClient;
        private readonly int _limit = 100;

        public MusicBrainzService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<Artist> GetArtist(string artistName)
        {
            var uri = $"artist/?query=artist:{artistName}";
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                
                var artistsResponse = await JsonSerializer.DeserializeAsync<ArtistQueryResponse>(responseStream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                return artistsResponse.Artists.FirstOrDefault();
            }

            return null;
        }

        public async Task<IEnumerable<Work>> GetArtistWorks(string artistId)
        {
            if (string.IsNullOrWhiteSpace(artistId))
            {
                return new List<Work>();
            }

            var offset = 0;
            var works = new List<Work>();

            var result = await GetWorks(artistId, _limit, offset);
            works.AddRange(result.Works);

            var totalPages = PagingHelper.GetTotalPages(result.WorkCount, _limit);

            for (var page = 1; page < totalPages; page++)
            {
                var remaingingWorks = await GetWorks(artistId, _limit, (page * _limit));
                works.AddRange(remaingingWorks.Works);
            }

            return FilterSongsOnly(works);
        }

        private async Task<WorksResponse> GetWorks(string artistId, int limit, int offset)
        {
            if (string.IsNullOrWhiteSpace(artistId))
            {
                return null;
            }

            var uri = $"work?artist={artistId}&limit={limit}&offset={offset}";

            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                
                return await JsonSerializer.DeserializeAsync<WorksResponse>(responseStream,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }

            return null;
        }

        private IEnumerable<Work> FilterSongsOnly(IEnumerable<Work> works)
        {
            if (works == null || !works.Any())
            {
                return Enumerable.Empty<Work>();
            }

            return works.Where(w => (w?.Type ?? string.Empty).ToLower() == "song");
        }
    }
}
