$files = @(
    '_2026_GraveExecutor.cs',
    '_2026_MagistusFairyExecutor.cs',
    '_2026_PuppetExecutor.cs',
    '_2026_RunickExecutor.cs',
    '_2026_SolfachordExecutor.cs',
    '_2026_TellarknightExecutor.cs',
    '_2026_ExodiaExecutor.cs'
)
foreach ($f in $files) {
    Write-Host "=== $f ==="
    $p = Join-Path $PSScriptRoot $f
    Select-String -Path $p -Pattern 'ACE CARDS:|WIN CONDITION:|GOING 1ST END BOARD:|IsAceCard|AceCardIds' | ForEach-Object {
        Write-Host ("  L" + $_.LineNumber + ": " + $_.Line.Trim())
    }
    Write-Host ""
}
