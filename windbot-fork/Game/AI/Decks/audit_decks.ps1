$deckDir = $PSScriptRoot
$decks = Get-ChildItem -Path $deckDir -Filter '_2026_*.cs' | Select-Object -ExpandProperty Name
$total = $decks.Count

Write-Host "=== TOTAL 2026 DECKS: $total ==="
Write-Host ""

# A1: IsBoardStrongEnough
$hasBoard = @(Get-ChildItem -Path $deckDir -Filter '_2026_*.cs' | Select-String -Pattern 'override.*IsBoardStrongEnough' -List | Select-Object -ExpandProperty Filename)
$missingBoard = $decks | Where-Object { $_ -notin $hasBoard }
Write-Host "--- A1: IsBoardStrongEnough override ($($hasBoard.Count)/$total have it) ---"
$missingBoard | ForEach-Object { Write-Host "  MISSING: $_" }
Write-Host ""

# A2: ShouldStopExtending
$hasStop = @(Get-ChildItem -Path $deckDir -Filter '_2026_*.cs' | Select-String -Pattern 'override.*ShouldStopExtending' -List | Select-Object -ExpandProperty Filename)
$missingStop = $decks | Where-Object { $_ -notin $hasStop }
Write-Host "--- A2: ShouldStopExtending override ($($hasStop.Count)/$total have it) ---"
$missingStop | ForEach-Object { Write-Host "  MISSING: $_" }
Write-Host ""

# A3: ComboRouter lines count
Write-Host "--- A3: ComboRouter.RegisterLine count per deck ---"
foreach ($deck in $decks) {
    $path = Join-Path $deckDir $deck
    $count = @(Select-String -Path $path -Pattern 'ComboRouter\.RegisterLine').Count
    if ($count -lt 2) {
        Write-Host "  LOW ($count lines): $deck"
    }
}
Write-Host ""

# A4: IsAceCard
$hasAce = @(Get-ChildItem -Path $deckDir -Filter '_2026_*.cs' | Select-String -Pattern 'override.*IsAceCard' -List | Select-Object -ExpandProperty Filename)
$missingAce = $decks | Where-Object { $_ -notin $hasAce }
Write-Host "--- A4: IsAceCard override ($($hasAce.Count)/$total have it) ---"
$missingAce | ForEach-Object { Write-Host "  MISSING: $_" }
Write-Host ""

# A5: GetMaterialPriority
$hasMat = @(Get-ChildItem -Path $deckDir -Filter '_2026_*.cs' | Select-String -Pattern 'override.*GetMaterialPriority' -List | Select-Object -ExpandProperty Filename)
$missingMat = $decks | Where-Object { $_ -notin $hasMat }
Write-Host "--- A5: GetMaterialPriority override ($($hasMat.Count)/$total have it) ---"
$missingMat | ForEach-Object { Write-Host "  MISSING: $_" }
