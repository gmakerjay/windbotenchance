# ============================================================================
# SYNC_AGENT_ASSETS.ps1
# Two-way "newer wins" sync of agent-facing assets between:
#   Workspace root : C:\Users\admin\Documents\EdoGame\            (loaded by the IDE agent)
#   Git repo       : C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN\  (pushed to GitHub)
#
# Synced items:
#   .agents\skills\   (yugioh-executor SKILL.md + references)
#   Docs\             (reports / playbooks / architecture)
#   PROGRESS.md
#   AGENTS.md
#
# Run AFTER `git pull` (repo -> workspace) and BEFORE `git add` (workspace -> repo).
# Deletions are NOT propagated (safe by design).
# ============================================================================
$ErrorActionPreference = 'Stop'

$Repo = $PSScriptRoot
$Root = (Resolve-Path (Join-Path $Repo '..\..')).Path

function Invoke-Robo([string]$src, [string]$dst, [string[]]$extra) {
    if (-not (Test-Path $src)) { return }
    $roboArgs = @($src, $dst) + $extra + @('/XO', '/R:1', '/W:1', '/NJH', '/NJS', '/NDL', '/NP')
    $out = & robocopy @roboArgs
    if ($LASTEXITCODE -ge 8) { throw "robocopy failed ($LASTEXITCODE): $src -> $dst" }
    $out | Where-Object { $_.Trim() } | ForEach-Object { "    $($_.Trim())" }
}

function Sync-Dir([string]$rel) {
    $a = Join-Path $Root $rel; $b = Join-Path $Repo $rel
    Write-Host "  [DIR ] $rel" -ForegroundColor Cyan
    Invoke-Robo $a $b @('/E')
    Invoke-Robo $b $a @('/E')
}

function Sync-File([string]$name) {
    Write-Host "  [FILE] $name" -ForegroundColor Cyan
    Invoke-Robo $Root $Repo @($name)
    Invoke-Robo $Repo $Root @($name)
}

Write-Host "Syncing agent assets (newer wins)" -ForegroundColor Yellow
Write-Host "  Workspace: $Root"
Write-Host "  Repo     : $Repo"
Sync-Dir  '.agents\skills'
Sync-Dir  'Docs'
Sync-File 'PROGRESS.md'
Sync-File 'AGENTS.md'
Write-Host "OK: agent assets in sync" -ForegroundColor Green
exit 0
