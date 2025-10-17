using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using UrlShortenerApi.Data;
using UrlShortenerApi.Models;
using UrlShortenerApi.Services.Abstractions;

namespace UrlShortenerApi.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly UrlShortenerDbContext _db;
        private readonly IConnectionMultiplexer _redis;

        public UrlShortenerService(UrlShortenerDbContext db, IConnectionMultiplexer redis)
        {
            _db = db;
            _redis = redis;
        }

        public async Task<ShortUrl> ShortenAsync(string originalUrl)
        {
            var redisDb = _redis.GetDatabase();

            var cached = await redisDb.StringGetAsync(originalUrl);

            if (cached.HasValue)
            {
                return new ShortUrl
                {
                    ShortCode = cached,
                    OriginalUrl = originalUrl,
                    Clicks = 0
                };
            }

            var url = await _db.ShortUrls.FirstOrDefaultAsync(x => x.OriginalUrl == originalUrl);

            if (url != null)
            {
                return url;
            }

            var shortCode = Guid.NewGuid().ToString()[..6];
            
            var shortUrl = new ShortUrl
            {
                ShortCode = shortCode,
                OriginalUrl = originalUrl,
                Clicks = 0
            };

            await redisDb.StringSetAsync(shortCode, originalUrl, TimeSpan.FromHours(1));
            await redisDb.StringSetAsync(originalUrl, shortCode, TimeSpan.FromHours(1));

            _db.ShortUrls.Add(shortUrl);
            await _db.SaveChangesAsync();
            return shortUrl;
        }

        public async Task<string?> GetOriginalUrlAsync(string shortCode)
        {
            var redisDb = _redis.GetDatabase();

            var cached = await redisDb.StringGetAsync(shortCode);

            if (cached.HasValue)
            {
                await incrementClickCountAsync(new ShortUrl { ShortCode = shortCode, OriginalUrl = cached });
                return cached;
            }

            var shortUrl = await _db.ShortUrls.FirstOrDefaultAsync(x => x.ShortCode == shortCode);

            if (shortUrl == null)
            {
                return null;
            }

            await incrementClickCountAsync(shortUrl);

            await redisDb.StringSetAsync(shortCode, shortUrl.OriginalUrl, TimeSpan.FromHours(1));
            await redisDb.StringSetAsync(shortUrl.OriginalUrl, shortCode, TimeSpan.FromHours(1));

            return shortUrl.OriginalUrl;
        }

        public async Task<ShortUrl> GetStatsAsync(string shortCode)
        {
            return await _db.ShortUrls.FirstOrDefaultAsync(x => x.ShortCode == shortCode);
        }

        private async Task incrementClickCountAsync(ShortUrl shortUrl)
        {
            shortUrl.Clicks++;
            await _db.SaveChangesAsync();
        }
    }
}
