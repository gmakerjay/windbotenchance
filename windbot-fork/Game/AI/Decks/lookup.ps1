$dir = 'c:\Users\admin\Documents\YugiohTH\Source_Project\YGO_AI_PLATFORM\windbot-fork\Game\AI\Decks'
$targets = @(
    @{File='_2026_ArtMageExecutor.cs'; Search='BossMonsters'},
    @{File='_2026_DreadnoughtExecutor.cs'; Search='AceCardIds'},
    @{File='_2026_PurrelyExecutor.cs'; Search='BossMonsters'}
)

foreach ($t in $targets) {
    Write-Output ""
    Write-Output "=== $($t.File) ($($t.Search)) ==="
    $content = Get-Content (Join-Path $dir $t.File) -Raw
    $lines = $content -split "`n"
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match $t.Search -and $lines[$i] -match '(static|readonly)') {
            for ($j = $i; $j -lt [Math]::Min($i + 15, $lines.Count); $j++) {
                Write-Output ("L{0}: {1}" -f ($j+1), $lines[$j].TrimEnd())
                if ($lines[$j].Trim() -match '^\};' -or ($lines[$j].Trim() -eq '};')) { break }
            }
            break
        }
    }
}
