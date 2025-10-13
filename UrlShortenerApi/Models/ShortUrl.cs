using System.ComponentModel.DataAnnotations;

namespace UrlShortenerApi.Models
{
    public class ShortUrl
    {
        [Key]
        public string ShortCode { get; set; }

        [Required]
        public string OriginalUrl { get; set; }

        public int Clicks { get; set; }
    }
}