# BookingTracker Deployment Guide

## Prerequisites

- Docker and Docker Compose installed on your server
- GitHub account with access to the repository
- GitHub Personal Access Token (PAT) with `read:packages` scope

## Step 1: Create GitHub Repository

```bash
# Install GitHub CLI (if not already installed)
# Windows: winget install --id GitHub.cli
# Or visit: https://cli.github.com/

# Login to GitHub
gh auth login

# Create private repository
gh repo create bookingtracker --private --description "Musician booking pipeline tracker"

# Push code
git branch -M main
git remote add origin https://github.com/jp-tx/bookingtracker.git
git push -u origin main
```

## Step 2: Build and Push Docker Image

### Build the Image

```bash
cd C:\Users\jb.JP-TX\bookingtracker

# Build for linux/amd64 (most servers)
docker build -t ghcr.io/jp-tx/bookingtracker:latest --platform linux/amd64 .

# Or build for both platforms
docker buildx build --platform linux/amd64,linux/arm64 -t ghcr.io/jp-tx/bookingtracker:latest .
```

### Login to GitHub Container Registry

```bash
# Create a GitHub Personal Access Token at:
# https://github.com/settings/tokens
# Scopes needed: write:packages, read:packages, delete:packages

# Login to ghcr.io
echo YOUR_GITHUB_PAT | docker login ghcr.io -u jp-tx --password-stdin
```

### Push the Image

```bash
docker push ghcr.io/jp-tx/bookingtracker:latest
```

## Step 3: Deploy on Server

### On Your Server

1. **Install Docker and Docker Compose**
   ```bash
   # Ubuntu/Debian
   curl -fsSL https://get.docker.com -o get-docker.sh
   sudo sh get-docker.sh
   sudo apt-get install docker-compose-plugin
   ```

2. **Login to GitHub Container Registry**
   ```bash
   echo YOUR_GITHUB_PAT | docker login ghcr.io -u jp-tx --password-stdin
   ```

3. **Create deployment directory**
   ```bash
   mkdir -p ~/bookingtracker
   cd ~/bookingtracker
   ```

4. **Create docker-compose.yml**
   ```bash
   cat > docker-compose.yml << 'EOF'
version: '3.8'

services:
  bookingtracker:
    image: ghcr.io/jp-tx/bookingtracker:latest
    container_name: bookingtracker
    restart: unless-stopped
    ports:
      - "5000:8080"
    volumes:
      - ./data:/data
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:8080
      - ConnectionStrings__DefaultConnection=Data Source=/data/app.db
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:8080/"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 40s
EOF
   ```

5. **Start the application**
   ```bash
   docker-compose pull
   docker-compose up -d
   ```

6. **View logs**
   ```bash
   docker-compose logs -f
   ```

## Step 4: Access the Application

Visit `http://YOUR_SERVER_IP:5000`

## Updating the Application

```bash
# On your development machine
docker build -t ghcr.io/jp-tx/bookingtracker:latest --platform linux/amd64 .
docker push ghcr.io/jp-tx/bookingtracker:latest

# On your server
cd ~/bookingtracker
docker-compose pull
docker-compose up -d
```

## Backup Data

Your database and uploaded files are in the `./data` directory.

```bash
# Backup
tar -czf bookingtracker-backup-$(date +%Y%m%d).tar.gz data/

# Restore
tar -xzf bookingtracker-backup-YYYYMMDD.tar.gz
```

## Troubleshooting

### View logs
```bash
docker-compose logs -f bookingtracker
```

### Restart container
```bash
docker-compose restart
```

### Rebuild from scratch
```bash
docker-compose down
docker-compose pull
docker-compose up -d
```

### Access container shell
```bash
docker-compose exec bookingtracker /bin/bash
```

## Environment Variables

You can customize these in docker-compose.yml:

- `ASPNETCORE_ENVIRONMENT` - Production, Development, Staging
- `ASPNETCORE_URLS` - Bind URL (default: http://+:8080)
- `ConnectionStrings__DefaultConnection` - Database connection string

## Using HTTPS with Reverse Proxy

For production, use nginx or Caddy as a reverse proxy:

### Caddy Example

```
bookingtracker.yourdomain.com {
    reverse_proxy localhost:5000
}
```

### Nginx Example

```nginx
server {
    listen 80;
    server_name bookingtracker.yourdomain.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```
