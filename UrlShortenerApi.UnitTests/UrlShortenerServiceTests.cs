using UrlShortenerApi.Services;
using UrlShortenerApi.Models;
using Xunit;

namespace UrlShortenerApi.UnitTests
{
    public class UrlShortenerServiceTests
    {
        [Fact]
        public void Shorten_ShouldReturnShortUrl()
        {
            var service = new UrlShortenerService();
            var result = service.Shorten("https://example.com");
            Assert.NotNull(result);
            Assert.Equal("https://example.com", result.OriginalUrl);
            Assert.False(string.IsNullOrEmpty(result.ShortCode));
        }

        [Fact]
        public void GetOriginalUrl_ShouldReturnOriginalUrl()
        {
            var service = new UrlShortenerService();
            var shortUrl = service.Shorten("https://example.com");
            var original = service.GetOriginalUrl(shortUrl.ShortCode);
            Assert.Equal("https://example.com", original);
        }

        [Fact]
        public void GetStats_ShouldReturnStats()
        {
            var service = new UrlShortenerService();
            var shortUrl = service.Shorten("https://example.com");
            service.GetOriginalUrl(shortUrl.ShortCode);
            var stats = service.GetStats(shortUrl.ShortCode);
            Assert.Equal(1, stats.Clicks);
        }
    }
}