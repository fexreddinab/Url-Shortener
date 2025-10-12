using Prometheus;

namespace UrlShortenerApi
{
    public static class MetricRegistry
    {
        // Counters
        public static readonly Counter ApiRequests = Metrics.CreateCounter(
            "api_requests_total",
            "Total API requests",
            new CounterConfiguration { LabelNames = new[] { "endpoint", "method", "status" } }
        );

        public static readonly Counter ShortenRequests = Metrics.CreateCounter(
            "shorten_requests_total",
            "Total shorten requests"
        );

        public static readonly Counter Redirects = Metrics.CreateCounter(
            "redirects_total",
            "Total redirects served"
        );

        public static readonly Counter StatsRequests = Metrics.CreateCounter(
            "stats_requests_total",
            "Total stats requests"
        );

        // Gauges
        public static readonly Gauge ActiveShortUrls = Metrics.CreateGauge(
            "active_short_urls",
            "Number of short urls currently stored"
        );
    }
}
