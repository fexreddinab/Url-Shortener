using UrlShortenerApi.Models;

namespace UrlShortenerApi.Services.Abstractions;

public interface IUrlShortenerService
{
    Task<ShortUrl> ShortenAsync(string originalUrl);
    Task<string?> GetOriginalUrlAsync(string shortCode);
    Task<ShortUrl> GetStatsAsync(string shortCode);
}