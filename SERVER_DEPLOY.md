# BookingTracker - Server Deployment

## ✅ Deployment Complete!

- **Repository**: https://github.com/jp-tx/bookingtracker
- **Container Image**: ghcr.io/jp-tx/bookingtracker:latest
- **Container Registry**: https://github.com/jp-tx/bookingtracker/pkgs/container/bookingtracker

---

## Deploy on Your Server

### Step 1: Install Docker (if not already installed)

```bash
# Ubuntu/Debian
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Install Docker Compose
sudo apt-get update
sudo apt-get install docker-compose-plugin
```

### Step 2: Create Deployment Directory

```bash
mkdir -p ~/bookingtracker
cd ~/bookingtracker
```

### Step 3: Login to GitHub Container Registry

```bash
# You'll need your GitHub Personal Access Token
docker login ghcr.io -u jp-tx
# When prompted, paste your GitHub PAT (the one with read:packages scope)
```

### Step 4: Create docker-compose.yml

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
      test: ["CMD-SHELL", "curl -f http://localhost:8080/ || exit 1"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 40s
EOF
```

### Step 5: Start the Application

```bash
docker-compose up -d
```

### Step 6: Verify It's Running

```bash
# Check container status
docker-compose ps

# View logs
docker-compose logs -f

# Test the application
curl http://localhost:5000
```

### Step 7: Access the Application

- **Local**: http://localhost:5000
- **Remote**: http://YOUR_SERVER_IP:5000

---

## Firewall Configuration

If you can't access from outside, open the port:

```bash
# UFW (Ubuntu)
sudo ufw allow 5000/tcp

# firewalld (CentOS/RHEL)
sudo firewall-cmd --permanent --add-port=5000/tcp
sudo firewall-cmd --reload
```

---

## SSL/HTTPS with Reverse Proxy

### Using Caddy (Recommended - Automatic HTTPS)

```bash
# Install Caddy
sudo apt install -y debian-keyring debian-archive-keyring apt-transport-https
curl -1sLf 'https://dl.cloudsmith.io/public/caddy/stable/gpg.key' | sudo gpg --dearmor -o /usr/share/keyrings/caddy-stable-archive-keyring.gpg
curl -1sLf 'https://dl.cloudsmith.io/public/caddy/stable/debian.deb.txt' | sudo tee /etc/apt/sources.list.d/caddy-stable.list
sudo apt update
sudo apt install caddy

# Create Caddyfile
sudo nano /etc/caddy/Caddyfile
```

Add this content:
```
bookingtracker.yourdomain.com {
    reverse_proxy localhost:5000
}
```

```bash
# Restart Caddy
sudo systemctl restart caddy
```

### Using Nginx

```bash
sudo apt install nginx

sudo nano /etc/nginx/sites-available/bookingtracker
```

Add:
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

```bash
sudo ln -s /etc/nginx/sites-available/bookingtracker /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl restart nginx

# Get SSL certificate
sudo apt install certbot python3-certbot-nginx
sudo certbot --nginx -d bookingtracker.yourdomain.com
```

---

## Updating the Application

When you push a new version:

```bash
# On development machine
cd C:\Users\jb.JP-TX\bookingtracker
docker build -t ghcr.io/jp-tx/bookingtracker:latest --platform linux/amd64 .
docker push ghcr.io/jp-tx/bookingtracker:latest

# On server
cd ~/bookingtracker
docker-compose pull
docker-compose up -d
```

---

## Backup & Restore

### Backup

```bash
# Backup database and data
cd ~/bookingtracker
tar -czf backup-$(date +%Y%m%d-%H%M%S).tar.gz data/

# Copy backup to safe location
scp backup-*.tar.gz user@backup-server:/backups/
```

### Restore

```bash
# Stop application
docker-compose down

# Restore data
tar -xzf backup-YYYYMMDD-HHMMSS.tar.gz

# Start application
docker-compose up -d
```

### Automated Backups

Create a cron job:

```bash
crontab -e
```

Add:
```
# Backup every day at 2 AM
0 2 * * * cd ~/bookingtracker && tar -czf ~/backups/bookingtracker-$(date +\%Y\%m\%d).tar.gz data/ && find ~/backups -name "bookingtracker-*.tar.gz" -mtime +30 -delete
```

---

## Useful Commands

### View Logs
```bash
docker-compose logs -f bookingtracker
```

### Restart
```bash
docker-compose restart
```

### Stop
```bash
docker-compose stop
```

### Start
```bash
docker-compose start
```

### Rebuild from scratch
```bash
docker-compose down
docker-compose pull
docker-compose up -d
```

### Access container shell
```bash
docker-compose exec bookingtracker bash
```

### Check resource usage
```bash
docker stats bookingtracker
```

---

## Troubleshooting

### Container won't start
```bash
docker-compose logs bookingtracker
```

### Permission errors with database
```bash
sudo chown -R 1000:1000 data/
```

### Port already in use
```bash
# Change port in docker-compose.yml
ports:
  - "8080:8080"  # Use different external port
```

### Out of disk space
```bash
# Clean up old Docker images
docker system prune -a

# Check disk usage
df -h
du -sh ~/bookingtracker/data
```

---

## Monitoring

### Simple uptime monitoring

Create a monitoring script:

```bash
nano ~/check-bookingtracker.sh
```

```bash
#!/bin/bash
if ! curl -f http://localhost:5000 > /dev/null 2>&1; then
    echo "BookingTracker is down! Restarting..."
    cd ~/bookingtracker
    docker-compose restart
    echo "BookingTracker restarted at $(date)" >> ~/bookingtracker-restarts.log
fi
```

```bash
chmod +x ~/check-bookingtracker.sh

# Add to crontab (check every 5 minutes)
crontab -e
```

Add:
```
*/5 * * * * ~/check-bookingtracker.sh
```

---

## Support

For issues, check:
1. Container logs: `docker-compose logs -f`
2. Container status: `docker-compose ps`
3. Network connectivity: `curl http://localhost:5000`
4. Disk space: `df -h`
5. Database file: `ls -lh data/app.db`
