# Url Shortener (local docker-compose)

This repository contains a simple Url Shortener API (`UrlShortenerApi`) with Prometheus metrics and Grafana for visualization. The compose stack includes Prometheus, Grafana and Alertmanager.

Quick start (macOS / zsh):

1. Build and start services:

```bash
cd /path/to/Url-Shortener
docker-compose up -d --build
```

2. App endpoints:
- API: http://localhost:8080
- Prometheus UI: http://localhost:9090
- Grafana: http://localhost:3000 (default credentials: admin / admin)

3. Test metrics quickly:

```bash
# create a short url
curl -X POST -H "Content-Type: application/json" -d '"https://example.com"' http://localhost:8080/api/shorten

# view metrics exposed by the app
curl http://localhost:8080/metrics | grep shorten_requests_total || true

# query Prometheus for a metric
curl 'http://localhost:9090/api/v1/query?query=shorten_requests_total' | jq .
```

How to add this project to GitHub (local steps):

1. Initialize git, commit, and push to a new GitHub repository.

```bash
cd /path/to/Url-Shortener
git init
git add .
git commit -m "Initial commit: UrlShortener with Prometheus & Grafana compose stack"

# create repo on GitHub (example using gh CLI)
# gh repo create <your-username>/Url-Shortener --public --source=. --remote=origin --push

# or using plain git if you create remote via GitHub UI
# git remote add origin git@github.com:<your-username>/Url-Shortener.git
# git push -u origin main
```

If you want, I can create a small GitHub Actions workflow for CI (dotnet build + unit tests) and add it to `.github/workflows/ci.yml`. Tell me if you'd like that.
