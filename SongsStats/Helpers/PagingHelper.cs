namespace SongsStats.Helpers
{
    public class PagingHelper
    {
        public static int GetTotalPages(int works, int limit) 
        {
            return works % limit == 0 
                ? works / limit
                : works / limit + 1; 
        }
    }
}
