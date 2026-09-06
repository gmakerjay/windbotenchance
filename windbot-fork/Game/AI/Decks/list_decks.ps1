$dir = "c:\Users\admin\Documents\YugiohTH\Source_Project\YGO_AI_PLATFORM\windbot-fork\Game\AI\Decks"
$files = Get-ChildItem "$dir\_2026_*.cs"

foreach ($f in $files) {
    $name = $f.BaseName
    $content = Get-Content $f.FullName -Raw
    $lines = ($content -split "`n").Count

    $hasBoard = if ($content -match "IsBoardStrongEnough") { "YES" } else { "NO" }
    $hasStop  = if ($content -match "ShouldStopExtending") { "YES" } else { "NO" }
    $hasAce   = if ($content -match "override.*IsAceCard")  { "YES" } else { "NO" }
    $hasMat   = if ($content -match "override.*GetMaterialPriority") { "YES" } else { "NO" }
    
    $comboCount = ([regex]::Matches($content, "ComboRouter\.RegisterLine")).Count

    Write-Output "$name|$lines|$hasBoard|$hasStop|$comboCount|$hasAce|$hasMat"
}
