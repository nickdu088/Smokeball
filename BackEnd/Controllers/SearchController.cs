using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using BackEnd;
using BackEnd.SearchService;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private readonly ISearchService _searchService;
        public SearchController(ILogger<SearchController> logger, ISearchService searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        // GET Search/keywords/url
        [HttpGet("{keywords}/{url}")]
        public async Task<SearchItem> GetAsync(string keywords, string url)
        {
            if (string.IsNullOrEmpty(keywords.Trim()) || string.IsNullOrEmpty(url.Trim()))
            {
                _logger.Log(LogLevel.Error, "keyword or url can not be empty or null");
                throw new ArgumentNullException();
            }

            _logger.Log(LogLevel.Information, $"Search keyword '{keywords}' with '{url}'");

            var html = await _searchService.GetRawResult(keywords);
            string result = _searchService.Parse(html, url);

            SearchItem cacheEntry = new SearchItem()
            {
                Keywords = keywords,
                Url = url,
                Result = result
            };

            return cacheEntry;
        }
    }
}
