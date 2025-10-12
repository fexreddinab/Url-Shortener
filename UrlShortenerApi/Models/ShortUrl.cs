namespace UrlShortenerApi.Models
{
    public class ShortUrl
    {
        public string ShortCode { get; set; }
        public string OriginalUrl { get; set; }
        public int Clicks { get; set; }
    }
}