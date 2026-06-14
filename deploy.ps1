# BookingTracker Deployment Script
# This script creates the GitHub repo, builds, and pushes the Docker image

param(
    [Parameter(Mandatory=$false)]
    [string]$GitHubToken,

    [Parameter(Mandatory=$false)]
    [string]$GitHubUsername = "jp-tx",

    [Parameter(Mandatory=$false)]
    [string]$RepoName = "bookingtracker"
)

$ErrorActionPreference = "Stop"

Write-Host "==================================" -ForegroundColor Cyan
Write-Host "BookingTracker Deployment Script" -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Check if GitHub CLI is installed
Write-Host "[1/6] Checking GitHub CLI..." -ForegroundColor Yellow
$ghInstalled = Get-Command gh -ErrorAction SilentlyContinue

if (-not $ghInstalled) {
    Write-Host "GitHub CLI not found. Please install it:" -ForegroundColor Red
    Write-Host "  winget install --id GitHub.cli" -ForegroundColor White
    Write-Host "  Or visit: https://cli.github.com/" -ForegroundColor White
    Write-Host ""
    Write-Host "After installation, run: gh auth login" -ForegroundColor White
    exit 1
}

# Step 2: Check GitHub authentication
Write-Host "[2/6] Checking GitHub authentication..." -ForegroundColor Yellow
$authStatus = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "Not logged in to GitHub. Running gh auth login..." -ForegroundColor Yellow
    gh auth login
    if ($LASTEXITCODE -ne 0) {
        Write-Host "GitHub authentication failed!" -ForegroundColor Red
        exit 1
    }
}
Write-Host "Authenticated with GitHub" -ForegroundColor Green

# Step 3: Create GitHub repository (if it doesn't exist)
Write-Host "[3/6] Creating GitHub repository..." -ForegroundColor Yellow
$repoExists = gh repo view "$GitHubUsername/$RepoName" 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "Creating new private repository..." -ForegroundColor Yellow
    gh repo create $RepoName --private --description "Musician booking pipeline tracker" --source . --remote origin --push
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Failed to create repository!" -ForegroundColor Red
        exit 1
    }
    Write-Host "Repository created successfully" -ForegroundColor Green
} else {
    Write-Host "Repository already exists" -ForegroundColor Green

    # Check if remote is set
    $remote = git remote get-url origin 2>$null
    if (-not $remote) {
        Write-Host "Adding remote origin..." -ForegroundColor Yellow
        git remote add origin "https://github.com/$GitHubUsername/$RepoName.git"
    }

    # Push to GitHub
    Write-Host "Pushing to GitHub..." -ForegroundColor Yellow
    git branch -M main
    git push -u origin main
}

# Step 4: Build Docker image
Write-Host "[4/6] Building Docker image..." -ForegroundColor Yellow
Write-Host "This may take several minutes..." -ForegroundColor Gray

docker build -t "ghcr.io/$GitHubUsername/$RepoName:latest" --platform linux/amd64 .
if ($LASTEXITCODE -ne 0) {
    Write-Host "Docker build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "Docker image built successfully" -ForegroundColor Green

# Step 5: Login to GitHub Container Registry
Write-Host "[5/6] Logging in to GitHub Container Registry..." -ForegroundColor Yellow

if ($GitHubToken) {
    echo $GitHubToken | docker login ghcr.io -u $GitHubUsername --password-stdin
} else {
    # Try to use gh to get a token
    Write-Host "Getting GitHub token from gh CLI..." -ForegroundColor Gray
    $token = gh auth token
    if ($token) {
        echo $token | docker login ghcr.io -u $GitHubUsername --password-stdin
    } else {
        Write-Host "Please enter your GitHub Personal Access Token (PAT):" -ForegroundColor Yellow
        Write-Host "Create one at: https://github.com/settings/tokens" -ForegroundColor Gray
        Write-Host "Required scopes: write:packages, read:packages" -ForegroundColor Gray
        $secureToken = Read-Host -AsSecureString
        $BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureToken)
        $token = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
        echo $token | docker login ghcr.io -u $GitHubUsername --password-stdin
    }
}

if ($LASTEXITCODE -ne 0) {
    Write-Host "Docker login failed!" -ForegroundColor Red
    exit 1
}
Write-Host "Logged in to ghcr.io successfully" -ForegroundColor Green

# Step 6: Push Docker image
Write-Host "[6/6] Pushing Docker image to ghcr.io..." -ForegroundColor Yellow
docker push "ghcr.io/$GitHubUsername/$RepoName:latest"
if ($LASTEXITCODE -ne 0) {
    Write-Host "Docker push failed!" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "==================================" -ForegroundColor Green
Write-Host "Deployment Successful!" -ForegroundColor Green
Write-Host "==================================" -ForegroundColor Green
Write-Host ""
Write-Host "Repository: https://github.com/$GitHubUsername/$RepoName" -ForegroundColor Cyan
Write-Host "Image: ghcr.io/$GitHubUsername/$RepoName:latest" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Go to https://github.com/$GitHubUsername/$RepoName/settings" -ForegroundColor White
Write-Host "2. Make the package public or give your server access" -ForegroundColor White
Write-Host "3. On your server, create docker-compose.yml and run:" -ForegroundColor White
Write-Host "   docker-compose up -d" -ForegroundColor Gray
Write-Host ""
Write-Host "See DEPLOYMENT.md for detailed server setup instructions" -ForegroundColor White
