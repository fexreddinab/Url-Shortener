UrlShortenerApi metrics
======================

This project exposes Prometheus metrics at the /metrics endpoint (via prometheus-net).

Metrics added:

- api_requests_total{endpoint,method,status}: total HTTP requests
- shorten_requests_total: total requests to shorten URLs
- redirects_total: total redirects performed
- stats_requests_total: total stats requests
- active_short_urls: gauge with number of stored short URLs

How to scrape:

Configure Prometheus to scrape http://<host>:<port>/metrics for this service.
