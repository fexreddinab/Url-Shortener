using UrlShortenerApi.Models;

namespace UrlShortenerApi.Services.Abstractions;

public interface IUrlShortenerService
{
    ShortUrl Shorten(string originalUrl);
    string GetOriginalUrl(string shortCode);
    ShortUrl GetStats(string shortCode);
}