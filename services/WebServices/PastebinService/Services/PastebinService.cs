using System;
using System.Threading.Tasks;
using HttpService.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PastbinService.Config;
using PastebinService.Interfaces;

namespace PastebinService.Services
{
    public class PastebinService : IPastebinService
    {
        private readonly IHttpService _http;
        private readonly ILogger<PastebinService> _log;
        private readonly PastbinSettings _config;

        private static readonly string scrapeUrl = "https://scrape.pastebin.com/api_scraping.php";
        private DateTime _lastFetched;

        public PastebinService(ILogger<PastebinService> log, IOptions<PastbinSettings> config, IHttpService http)
        {
            _http = http;
            _log = log;
            _config = config.Value;
        }

        public async Task<string> FetchFeed()
        {
            return await HandleGet(scrapeUrl);
        }

        public async Task<string> FetchSingle(string url)
        {
            return await HandleGet(url);
        }

        private async Task<string> HandleGet(string url)
        {
            string content = string.Empty;
            try
            {
                var webResponse = await _http.GetAsync(url);
                content = await webResponse.Content.ReadAsStringAsync();
                _lastFetched = DateTime.Now;
            }
            catch (Exception ex)
            {
                _log.LogCritical($"Error fetching raw feed data at URL : {url}{Environment.NewLine}Exception{ex.Message}");
            }
            return content;
        }

        public DateTime LastFetch()
        {
            return _lastFetched;
        }
    }
}
