$dir = 'c:\Users\admin\Documents\YugiohTH\Source_Project\YGO_AI_PLATFORM\windbot-fork\Game\AI\Decks'

$tier2 = @(
    @{File='_2026_ArtMageExecutor.cs'; B1='CardId.SprightElf'; B2='CardId.AccesscodeTalker'; B3='CardId.IPMasquerena'},
    @{File='_2026_DreadnoughtExecutor.cs'; B1='CardId.SuperDreadnoughtRailCannonGustav'; B2='CardId.SuperDreadnoughtRailCannonJuggernautLiber'; B3='CardId.NumberC9ChaosDysonSphere'},
    @{File='_2026_FirekingExecutor.cs'; B1='CardId.GarunixEternity'; B2='CardId.PrometheanPrincess'; B3='CardId.SalamangreatRagingPhoenix'},
    @{File='_2026_LabrynthExecutor.cs'; B1='CardId.LabrynthStovieTorbie'; B2='CardId.LadyLabrynthOfTheSilverCastle'; B3='CardId.LovelyLabrynthOfTheSilverCastle'},
    @{File='_2026_MagnetExecutor.cs'; B1='CardId.TellusionTheMagnaWarrior'; B2='CardId.NaturiaBeast'; B3='CardId.ConductionWarriorPlasmaMagnet'},
    @{File='_2026_MalissExecutor.cs'; B1='CardId.MalissInWonderland'; B2='CardId.CheshireCatOfTheGhostForest'; B3='CardId.HarpiesConductor'},
    @{File='_2026_PurrelyExecutor.cs'; B1='CardId.ExpurrellyNoir'; B2='CardId.ExpurrellyGrandeNoir'; B3='CardId.Purrely'},
    @{File='_2026_TrueDracoExecutor.cs'; B1='CardId.MasterPeaceTheTrueDracoslayingKing'; B2='CardId.DinomightKnightTheTrueDracofighter'; B3='CardId.InspectorBoarder'}
)

# Template with placeholders
$boardTemplate = @'

        protected override bool IsBoardStrongEnough__OPEN____CLOSE__
        __OBRACE__
            if __OPEN__Bot.HasInMonstersZone__OPEN____BOSS1____CLOSE____CLOSE__ return true;
            if __OPEN__Bot.HasInMonstersZone__OPEN____BOSS2____CLOSE____CLOSE__ return true;
            if __OPEN__Bot.HasInMonstersZone__OPEN____BOSS3____CLOSE____CLOSE__ return true;
            return base.IsBoardStrongEnough__OPEN____CLOSE__;
        __CBRACE__
'@

$stopTemplate = @'

        protected override bool ShouldStopExtending__OPEN____CLOSE__
        __OBRACE__
            if __OPEN__Bot.HasInMonstersZone__OPEN____BOSS1____CLOSE__
                __PIPE____PIPE__ Bot.HasInMonstersZone__OPEN____BOSS2____CLOSE____CLOSE__
                return base.ShouldStopExtending__OPEN____CLOSE__;
            return false;
        __CBRACE__
'@

foreach ($d in $tier2) {
    $path = Join-Path $dir $d.File
    $content = Get-Content $path -Raw

    $hasBoard = $content -match 'IsBoardStrongEnough'
    $hasStop = $content -match 'ShouldStopExtending'

    if ($hasBoard -and $hasStop) {
        Write-Output "SKIP $($d.File)"
        continue
    }

    $inject = ''
    if (-not $hasBoard) {
        $b = $boardTemplate.Replace('__BOSS1__', $d.B1).Replace('__BOSS2__', $d.B2).Replace('__BOSS3__', $d.B3)
        $b = $b.Replace('__OPEN__', '(').Replace('__CLOSE__', ')').Replace('__OBRACE__', '{').Replace('__CBRACE__', '}').Replace('__PIPE____PIPE__', '||')
        $inject += $b
    }
    if (-not $hasStop) {
        $s = $stopTemplate.Replace('__BOSS1__', $d.B1).Replace('__BOSS2__', $d.B2)
        $s = $s.Replace('__OPEN__', '(').Replace('__CLOSE__', ')').Replace('__OBRACE__', '{').Replace('__CBRACE__', '}').Replace('__PIPE____PIPE__', '||')
        $inject += $s
    }

    if ($inject -ne '') {
        # Find the IsAceCard method end: "        }" after "public override bool IsAceCard"
        $aceIdx = $content.IndexOf('public override bool IsAceCard')
        if ($aceIdx -ge 0) {
            # Find matching close brace - count braces from aceIdx
            $searchStart = $content.IndexOf('{', $aceIdx)
            $braceCount = 0
            $endIdx = $searchStart
            for ($i = $searchStart; $i -lt $content.Length; $i++) {
                if ($content[$i] -eq '{') { $braceCount++ }
                if ($content[$i] -eq '}') { $braceCount-- }
                if ($braceCount -eq 0) { $endIdx = $i + 1; break }
            }
            $newContent = $content.Substring(0, $endIdx) + $inject + $content.Substring($endIdx)
            [System.IO.File]::WriteAllText($path, $newContent, [System.Text.Encoding]::UTF8)
            Write-Output "DONE $($d.File)"
        } else {
            Write-Output "FAIL $($d.File) -- no IsAceCard"
        }
    }
}
