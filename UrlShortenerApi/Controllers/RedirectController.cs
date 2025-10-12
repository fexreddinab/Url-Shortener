using Microsoft.AspNetCore.Mvc;
using UrlShortenerApi.Services;

namespace UrlShortenerApi.Controllers
{
    [ApiController]
    [Route("{shortCode}")]
    public class RedirectController : ControllerBase
    {
        private readonly Services.IUrlShortenerService _service;
        public RedirectController(Services.IUrlShortenerService service) => _service = service;

        [HttpGet]
        public IActionResult RedirectToOriginal(string shortCode)
        {
            var original = _service.GetOriginalUrl(shortCode);
            if (original == null) return NotFound();
            if (!original.StartsWith("http://") && !original.StartsWith("https://"))
            {
                original = "http://" + original;
            }
            return Redirect(original);
        }
    }
}