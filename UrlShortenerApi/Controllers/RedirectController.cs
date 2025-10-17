using Microsoft.AspNetCore.Mvc;
using UrlShortenerApi.Services;

namespace UrlShortenerApi.Controllers
{
    [ApiController]
    [Route("{shortCode}")]
    public class RedirectController : ControllerBase
    {
        private readonly Services.Abstractions.IUrlShortenerService _service;
        public RedirectController(Services.Abstractions.IUrlShortenerService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {
            var original = await _service.GetOriginalUrlAsync(shortCode);
            if (original == null) return NotFound();
            if (!original.StartsWith("http://") && !original.StartsWith("https://"))
            {
                original = "https://" + original;
            }
            return Redirect(original);
        }
    }
}