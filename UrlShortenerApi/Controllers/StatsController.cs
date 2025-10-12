using Microsoft.AspNetCore.Mvc;
using UrlShortenerApi.Services;

namespace UrlShortenerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly Services.IUrlShortenerService _service;
        public StatsController(Services.IUrlShortenerService service) => _service = service;

        [HttpGet("{shortCode}")]
        public IActionResult GetStats(string shortCode)
        {
            var stats = _service.GetStats(shortCode);
            if (stats == null) return NotFound();
            return Ok(new { stats.ShortCode, stats.OriginalUrl, stats.Clicks });
        }
    }
}