using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using OMDBassess.Helper;
using OMDBassess.Model;

namespace OMDBassess.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMemoryCache _cache;        
        private readonly SearchQueries _searchQuery;
        private readonly IConfiguration _config;
        private string ApiUrl;    
        private string OmdbApiKey;    

        public MovieController(IMemoryCache memoryCache, SearchQueries saveLatestSearch, IConfiguration config)
        {
            _cache = memoryCache;          
            _searchQuery = saveLatestSearch;
            _config = config;
        }

        [HttpGet("search/{title}")]
        public async Task<IActionResult> Search(string title)
        {
            ApiUrl = _config["ApiSettings:ApiBaseUrl"];
            OmdbApiKey = _config["ApiSettings:ApiKey"];

            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest("Title is required for the search.");
            }

            string cacheKey = $"SearchQuery_{title}";
           
            _searchQuery.SaveLatestSearchToCache(title);

            if (!_cache.TryGetValue(cacheKey, out string result))
            {
                using (var httpClient = new HttpClient())
                {                    
                    var response = await httpClient.GetStringAsync($"{ApiUrl}{OmdbApiKey}&s={title}&type=movie");
                    result = response;                    
                    _cache.Set(cacheKey, result, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });
                }                               
            }
            
            return Ok(JsonConvert.DeserializeObject<MovieSearchResponse>(result));
        }

        [HttpGet("latestQueries")]
        public IActionResult GetLatestQueries()
        {            
            var latestQueries = _searchQuery.GetLatestQueriesFromCache();
            return Ok(latestQueries);
        }        
    }
}
