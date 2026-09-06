# ============================================================================
# BUILD_AND_DEPLOY.ps1 — YugiohTH WindBot & DashBot Build & Deploy Script
# ============================================================================
# Usage:
#   .\BUILD_AND_DEPLOY.ps1                       # Build & Deploy to C:\Users\admin\Documents\EdoGame
#   .\BUILD_AND_DEPLOY.ps1 -BuildOnly            # Build only, no deploy
#   .\BUILD_AND_DEPLOY.ps1 -DeployTarget "D:\MyGame"  # Custom deploy target
#
# Prerequisites:
#   - .NET 10 SDK installed
#   - Run from the YGO_SOURCE_CLEAN root directory
# ============================================================================

param(
    [switch]$BuildOnly,
    [string]$DeployTarget = "",
    [switch]$Help
)

$ErrorActionPreference = "Stop"

# --- Configuration ---
$ScriptDir = $PSScriptRoot
$WindBotCsproj = Join-Path $ScriptDir "windbot-fork\WindBot.csproj"
$DashBotCsproj = Join-Path $ScriptDir "dashbot\dashbot.csproj"
$PublishOutput = Join-Path $ScriptDir "windbot-fork\bin\Release\net10.0-publish"
$DashBotOutput = Join-Path $ScriptDir "dashbot\bin\Release\net10.0-windows"

# --- Default Deploy Target: EdoGame ---
$Parent1 = (Resolve-Path (Join-Path $ScriptDir "..") -ErrorAction SilentlyContinue).Path
$Parent2 = (Resolve-Path (Join-Path $ScriptDir "..\..") -ErrorAction SilentlyContinue).Path

$DefaultEdoGame = $Parent2
if ($Parent1 -and ((Test-Path (Join-Path $Parent1 "EDOPro.exe")) -or (Test-Path (Join-Path $Parent1 "WindBot")))) {
    $DefaultEdoGame = $Parent1
} elseif ($Parent2 -and ((Test-Path (Join-Path $Parent2 "EDOPro.exe")) -or (Test-Path (Join-Path $Parent2 "WindBot")))) {
    $DefaultEdoGame = $Parent2
} elseif ($Parent2) {
    $DefaultEdoGame = $Parent2
} else {
    $DefaultEdoGame = $Parent1
}

if ($Help) {
    Write-Host @"

BUILD_AND_DEPLOY.ps1 — YugiohTH WindBot & DashBot Builder
==========================================================
Usage:
  .\BUILD_AND_DEPLOY.ps1                              Build & Deploy to EdoGame
  .\BUILD_AND_DEPLOY.ps1 -BuildOnly                   Build only
  .\BUILD_AND_DEPLOY.ps1 -DeployTarget "D:\MyGame"    Deploy to custom game folder

Default Deploy Target:
  Game Runtime: $DefaultEdoGame

"@
    exit 0
}

function Write-Step($step, $msg) {
    Write-Host ""
    Write-Host "[$step] $msg" -ForegroundColor Cyan
    Write-Host ("=" * 60) -ForegroundColor DarkGray
}

function Write-OK($msg) { Write-Host "  OK: $msg" -ForegroundColor Green }
function Write-Fail($msg) { Write-Host "  FAIL: $msg" -ForegroundColor Red }

# ============================================================================
# STEP 1: Build WindBot (Self-Contained Release)
# ============================================================================
Write-Step "1/3" "Publishing WindBot (self-contained win-x64)"

dotnet publish $WindBotCsproj `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=false `
    -p:PublishReadyToRun=true `
    -o $PublishOutput

if ($LASTEXITCODE -ne 0) {
    Write-Fail "WindBot publish failed!"
    exit 1
}
Write-OK "WindBot published to: $PublishOutput"

# ============================================================================
# STEP 2: Build DashBot Launcher (WPF)
# ============================================================================
Write-Step "2/3" "Building DashBot Launcher (WPF Release)"

dotnet build $DashBotCsproj -c Release --no-incremental

if ($LASTEXITCODE -ne 0) {
    Write-Fail "DashBot build failed!"
    exit 1
}
Write-OK "DashBot built to: $DashBotOutput"

# ============================================================================
# STEP 3: Deploy to EdoGame
# ============================================================================
if ($BuildOnly) {
    Write-Host ""
    Write-Host "Build completed successfully. Skipping deployment (-BuildOnly)." -ForegroundColor Yellow
    exit 0
}

# Determine deploy target
$TargetDir = if ($DeployTarget) { $DeployTarget } else { $DefaultEdoGame }

Write-Step "3/3" "Deploying to Game Directory: $TargetDir"

# Ensure target directories exist
$dirs = @(
    (Join-Path $TargetDir "WindBot"),
    (Join-Path $TargetDir "WindBot\Decks"),
    (Join-Path $TargetDir "WindBot\Dialogs"),
    (Join-Path $TargetDir "deck")
)
foreach ($d in $dirs) {
    if (!(Test-Path $d)) { New-Item -ItemType Directory -Path $d -Force | Out-Null }
}

# Deploy WindBot binaries
$windbotFiles = @("ExecutorBase.dll", "WindBot.dll", "core.dll", "bots.json")
foreach ($f in $windbotFiles) {
    $src = Join-Path $PublishOutput $f
    if (Test-Path $src) {
        Copy-Item $src (Join-Path $TargetDir "WindBot\") -Force
        Copy-Item $src (Join-Path $TargetDir "") -Force
        Write-OK "Deployed $f"
    } else {
        Write-Fail "Missing: $f"
    }
}

# Deploy Decks & Dialogs
Copy-Item (Join-Path $PublishOutput "Decks\*") (Join-Path $TargetDir "WindBot\Decks\") -Recurse -Force
Copy-Item (Join-Path $PublishOutput "Dialogs\*") (Join-Path $TargetDir "WindBot\Dialogs\") -Recurse -Force
Copy-Item (Join-Path $ScriptDir "windbot-fork\Decks\*") (Join-Path $TargetDir "deck\") -Recurse -Force
Write-OK "Deployed Decks & Dialogs"

# Deploy cards.cdb (Ensure full custom/prerelease cards are available for WindBot and EdoGame)
$sourceCdb = Join-Path $ScriptDir "windbot-fork\cards.cdb"
if (Test-Path $sourceCdb) {
    Copy-Item $sourceCdb (Join-Path $TargetDir "WindBot\cards.cdb") -Force
    $targetRootCdb = Join-Path $TargetDir "cards.cdb"
    if ((Test-Path $targetRootCdb) -and (Get-Item $targetRootCdb).Length -lt 10000000) {
        Copy-Item $sourceCdb $targetRootCdb -Force
        Write-OK "Updated root cards.cdb with full prerelease/custom card database"
    }
    Write-OK "Deployed cards.cdb to WindBot"
}

# Deploy DashBot
Copy-Item "$DashBotOutput\*" "$TargetDir\" -Recurse -Force
Write-OK "Deployed DashBot Launcher"

# ============================================================================
# Summary
# ============================================================================
Write-Host ""
Write-Host "============================================" -ForegroundColor Green
Write-Host " BUILD & DEPLOY COMPLETE" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green
Write-Host "  WindBot:  $PublishOutput"
Write-Host "  DashBot:  $DashBotOutput"
Write-Host "  Deployed: $TargetDir"
Write-Host ""
