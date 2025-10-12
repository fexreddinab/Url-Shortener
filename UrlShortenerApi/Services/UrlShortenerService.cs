using UrlShortenerApi.Models;

namespace UrlShortenerApi.Services
{
    public interface IUrlShortenerService
    {
        ShortUrl Shorten(string originalUrl);
        string GetOriginalUrl(string shortCode);
        ShortUrl GetStats(string shortCode);
    }

    public class UrlShortenerService : IUrlShortenerService
    {
        // In-memory store for demo
        private readonly Dictionary<string, ShortUrl> _store = new();
        public ShortUrl Shorten(string originalUrl)
        {
            var shortCode = Guid.NewGuid().ToString()[..6];
            var shortUrl = new ShortUrl { ShortCode = shortCode, OriginalUrl = originalUrl, Clicks = 0 };
            _store[shortCode] = shortUrl;
            return shortUrl;
        }
        public string GetOriginalUrl(string shortCode)
        {
            if (_store.TryGetValue(shortCode, out var shortUrl))
            {
                shortUrl.Clicks++;
                return shortUrl.OriginalUrl;
            }
            return null;
        }
        public ShortUrl GetStats(string shortCode)
        {
            _store.TryGetValue(shortCode, out var shortUrl);
            return shortUrl;
        }
    }
}