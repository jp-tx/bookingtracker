# BookingTracker - Quick Deployment Guide

## Automated Deployment (Recommended)

### Step 1: Install GitHub CLI (if not already installed)

```powershell
winget install --id GitHub.cli
```

### Step 2: Run the deployment script

```powershell
cd C:\Users\jb.JP-TX\bookingtracker

# If you get "execution policy" error, run this first:
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

# Run the deployment script
.\deploy.ps1
```

The script will:
1. ✅ Check GitHub CLI installation
2. ✅ Login to GitHub (if needed)
3. ✅ Create private repository: `jp-tx/bookingtracker`
4. ✅ Push code to GitHub
5. ✅ Build Docker image for linux/amd64
6. ✅ Login to ghcr.io
7. ✅ Push image to: `ghcr.io/jp-tx/bookingtracker:latest`

---

## Manual Deployment (Alternative)

### Option A: Using GitHub CLI

```bash
# Install and login
winget install --id GitHub.cli
gh auth login

# Create repository
gh repo create bookingtracker --private --description "Musician booking pipeline tracker" --source . --remote origin --push

# Build and push Docker image
docker build -t ghcr.io/jp-tx/bookingtracker:latest --platform linux/amd64 .

# Login to GitHub Container Registry
echo YOUR_GITHUB_PAT | docker login ghcr.io -u jp-tx --password-stdin

# Push image
docker push ghcr.io/jp-tx/bookingtracker:latest
```

### Option B: Using Git + GitHub Web UI

1. **Create repository on GitHub.com**
   - Go to https://github.com/new
   - Name: `bookingtracker`
   - Make it **Private**
   - Don't initialize with README
   - Click "Create repository"

2. **Push code**
   ```bash
   cd C:\Users\jb.JP-TX\bookingtracker
   git branch -M main
   git remote add origin https://github.com/jp-tx/bookingtracker.git
   git push -u origin main
   ```

3. **Build and push Docker image**
   ```bash
   # Build
   docker build -t ghcr.io/jp-tx/bookingtracker:latest --platform linux/amd64 .

   # Create GitHub Personal Access Token:
   # https://github.com/settings/tokens/new
   # Scopes needed: write:packages, read:packages

   # Login to ghcr.io
   docker login ghcr.io -u jp-tx
   # Enter your PAT as the password

   # Push
   docker push ghcr.io/jp-tx/bookingtracker:latest
   ```

---

## Server Deployment

### On your server (Linux):

1. **Install Docker**
   ```bash
   curl -fsSL https://get.docker.com -o get-docker.sh
   sudo sh get-docker.sh
   ```

2. **Login to GitHub Container Registry**
   ```bash
   # Use the same GitHub PAT
   docker login ghcr.io -u jp-tx
   ```

3. **Create docker-compose.yml**
   ```bash
   mkdir -p ~/bookingtracker
   cd ~/bookingtracker

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
EOF
   ```

4. **Start the application**
   ```bash
   docker-compose up -d
   ```

5. **Access the application**
   - Visit: `http://YOUR_SERVER_IP:5000`

---

## Quick Commands

### View logs
```bash
docker-compose logs -f
```

### Update to latest version
```bash
docker-compose pull
docker-compose up -d
```

### Backup database
```bash
tar -czf backup-$(date +%Y%m%d).tar.gz data/
```

### Restart
```bash
docker-compose restart
```

---

## Repository URLs

After deployment:
- **Repository**: https://github.com/jp-tx/bookingtracker
- **Container**: https://github.com/jp-tx/bookingtracker/pkgs/container/bookingtracker
- **Image**: `ghcr.io/jp-tx/bookingtracker:latest`

---

## Make Package Accessible

After pushing to ghcr.io:

1. Go to: https://github.com/jp-tx/bookingtracker/pkgs/container/bookingtracker
2. Click "Package settings"
3. Either:
   - Make it **Public** (anyone can pull)
   - Or add your server's GitHub account to "Manage access"

---

## Troubleshooting

**Error: "unauthorized: unauthenticated"**
- Make sure you're logged in: `docker login ghcr.io -u jp-tx`
- Verify your PAT has `read:packages` scope

**Error: "manifest unknown"**
- Image hasn't been pushed yet
- Check image name matches exactly

**Cannot access on server**
- Check firewall: `sudo ufw allow 5000`
- Check container is running: `docker ps`
- Check logs: `docker-compose logs`

---

For detailed information, see **DEPLOYMENT.md**
