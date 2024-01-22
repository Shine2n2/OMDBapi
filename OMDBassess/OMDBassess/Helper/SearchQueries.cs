using Microsoft.Extensions.Caching.Memory;

namespace OMDBassess.Helper
{
    public class SearchQueries
    {
        private readonly IMemoryCache _cache;

        public SearchQueries(IMemoryCache cache)
        {
                _cache = cache;
        }
        public void SaveLatestSearchToCache(string query)
        {            
            var latestQueries = GetLatestQueriesFromCache();           
            latestQueries.Insert(0, query);            
            latestQueries = latestQueries.Take(5).ToList();            
            _cache.Set("LatestQueries", latestQueries);
        }

        public List<string> GetLatestQueriesFromCache()
        {            
            if (_cache.TryGetValue("LatestQueries", out List<string> latestQueries))
            {
                return latestQueries;
            }

            return new List<string>();
        }
    }
}
