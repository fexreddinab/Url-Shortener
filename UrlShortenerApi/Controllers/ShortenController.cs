using Microsoft.AspNetCore.Mvc;
using UrlShortenerApi.Services;

namespace UrlShortenerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortenController : ControllerBase
    {
        private readonly Services.IUrlShortenerService _service;
        public ShortenController(Services.IUrlShortenerService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> ShortenUrl()
        {
            using var sr = new StreamReader(Request.Body);
            var body = await sr.ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(body)) return BadRequest(new { errors = new { originalUrl = new[] { "The originalUrl field is required." } } });

            string originalUrl = null;
            try
            {
                // Try parse as JSON string: "https://..."
                var doc = System.Text.Json.JsonDocument.Parse(body);
                if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.String)
                {
                    originalUrl = doc.RootElement.GetString();
                }
                else if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Object && doc.RootElement.TryGetProperty("originalUrl", out var prop))
                {
                    originalUrl = prop.GetString();
                }
            }
            catch
            {
                // ignore parse errors
            }

            if (string.IsNullOrWhiteSpace(originalUrl)) return BadRequest(new { errors = new { originalUrl = new[] { "The originalUrl field is required." } } });

            var shortUrl = _service.Shorten(originalUrl);

            var scheme = Request.Scheme;          // http
            var host = Request.Host.Value;        // url.shortener:80 və ya localhost:8080
            var fullUrl = $"{scheme}://{host}/{shortUrl.ShortCode}";

            return Ok(new { shortUrl = fullUrl });
        }
    }
}