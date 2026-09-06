$dir = "c:\Users\admin\Documents\YugiohTH\Source_Project\YGO_AI_PLATFORM\windbot-fork\Game\AI\Decks"
$targets = @(
    '_2026_YummyExecutor.cs','_2026_EneaCraftExecutor.cs',
    '_2026_ArtMageExecutor.cs','_2026_DreadnoughtExecutor.cs',
    '_2026_FirekingExecutor.cs','_2026_LabrynthExecutor.cs',
    '_2026_MagnetExecutor.cs','_2026_MalissExecutor.cs',
    '_2026_PurrelyExecutor.cs','_2026_TrueDracoExecutor.cs'
)

foreach ($f in $targets) {
    $path = Join-Path $dir $f
    $content = Get-Content $path -Raw
    $lineArr = $content -split "`n"
    
    # Find IsAceCard line number
    for ($i = 0; $i -lt $lineArr.Count; $i++) {
        if ($lineArr[$i] -match "IsAceCard") {
            Write-Output ("=== {0} === IsAceCard at L{1}: {2}" -f $f, ($i+1), $lineArr[$i].Trim())
            break
        }
    }
    
    # Find end of class (last } before namespace closing })
    $lastBrace = 0
    for ($i = $lineArr.Count - 1; $i -ge 0; $i--) {
        if ($lineArr[$i].Trim() -eq "}") {
            $lastBrace = $i + 1
            break
        }
    }
    Write-Output ("  EndClass ~L{0}, Total={1}" -f $lastBrace, $lineArr.Count)
}
