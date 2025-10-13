using UrlShortenerApi.Data;
using UrlShortenerApi.Models;
using UrlShortenerApi.Services.Abstractions;

namespace UrlShortenerApi.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly UrlShortenerDbContext _db;

        public UrlShortenerService(UrlShortenerDbContext db)
        {
            _db = db;
        }

        public ShortUrl Shorten(string originalUrl)
        {
            var shortCode = Guid.NewGuid().ToString()[..6];
            var shortUrl = new ShortUrl
            {
                ShortCode = shortCode,
                OriginalUrl = originalUrl,
                Clicks = 0
            };
            _db.ShortUrls.Add(shortUrl);
            _db.SaveChanges();
            return shortUrl;
        }

        public string GetOriginalUrl(string shortCode)
        {
            var shortUrl = _db.ShortUrls.FirstOrDefault(x => x.ShortCode == shortCode);
            if (shortUrl == null) return null;
            shortUrl.Clicks++;
            _db.SaveChanges();
            return shortUrl.OriginalUrl;
        }

        public ShortUrl GetStats(string shortCode)
        {
            return _db.ShortUrls.FirstOrDefault(x => x.ShortCode == shortCode);
        }
    }
}
