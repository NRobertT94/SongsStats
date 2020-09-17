using System.Collections.Generic;

namespace SongsStats.Models
{
    public class ArtistQueryResponse
    {
        public IEnumerable<Artist> Artists { get; set; }
    }
}
