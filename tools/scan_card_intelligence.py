import os
import re
import sqlite3
import time

SCRIPT_DIR = r"C:\Users\admin\Documents\EdoGame\script\official"
CDB_PATH = r"C:\Users\admin\Documents\EdoGame\cards.cdb"

def load_card_data(cdb_path):
    card_names = {}
    card_types = {}
    if os.path.exists(cdb_path):
        conn = sqlite3.connect(cdb_path)
        cur = conn.cursor()
        for row in cur.execute("SELECT id, name FROM texts"):
            card_names[row[0]] = row[1]
        for row in cur.execute("SELECT id, type FROM datas"):
            card_types[row[0]] = row[1]
        conn.close()
    return card_names, card_types

def scan_and_generate():
    print(f"Starting scan from: {SCRIPT_DIR}")
    start_time = time.time()
    
    card_names, card_types = load_card_data(CDB_PATH)
    print(f"Loaded {len(card_names)} card names from cards.cdb")

    target_immune = {}
    battle_immune = {}
    dangerous_battle = {}
    fusion_spells = {}

    # Regex patterns
    re_target_immune = re.compile(r'EFFECT_CANNOT_BE_EFFECT_TARGET')
    re_battle_indes = re.compile(r'EFFECT_INDESTRUCTABLE_BATTLE')
    re_reflect_damage = re.compile(r'EFFECT_REFLECT_BATTLE_DAMAGE')
    re_avoid_damage = re.compile(r'EFFECT_AVOID_BATTLE_DAMAGE')
    re_fusion_summon = re.compile(r'SUMMON_TYPE_FUSION|Fusion\.CreateSummonEff|CATEGORY_FUSION_SUMMON')

    if not os.path.exists(SCRIPT_DIR):
        print(f"Directory not found: {SCRIPT_DIR}")
        return

    files = [f for f in os.listdir(SCRIPT_DIR) if f.startswith('c') and f.endswith('.lua')]
    print(f"Found {len(files)} lua files to inspect...")

    for fname in files:
        card_id_str = fname[1:-4]
        if not card_id_str.isdigit():
            continue
        card_id = int(card_id_str)
        if card_id >= 100000000 and card_id not in (100000000, 100000010, 100000020):
            continue

        filepath = os.path.join(SCRIPT_DIR, fname)
        try:
            with open(filepath, 'r', encoding='utf-8', errors='ignore') as f:
                content = f.read()
        except Exception:
            continue

        name = card_names.get(card_id, f"Card {card_id}")
        ctype = card_types.get(card_id, 0)
        is_spell = (ctype & 2) != 0
        is_monster = (ctype & 1) != 0

        # Target Immune: check self-target immunity (single range / handler player)
        if re_target_immune.search(content) and is_monster:
            if 'aux.tgoval' in content or 'EFFECT_FLAG_SINGLE_RANGE' in content or 'tp~=e:GetHandlerPlayer()' in content or 'cannot be targeted by' in content.lower():
                target_immune[card_id] = name

        # Battle Immune: check monster indestructible by battle
        if re_battle_indes.search(content) and is_monster:
            if 'EFFECT_TYPE_SINGLE' in content or 'EFFECT_FLAG_SINGLE_RANGE' in content:
                battle_immune[card_id] = name

        # Dangerous Battle: reflect damage or avoid damage on 0-atk / reflection archetype
        if is_monster:
            if re_reflect_damage.search(content):
                dangerous_battle[card_id] = name
            elif re_avoid_damage.search(content) and any(k in name for k in ['Yubel', 'Mikanko', 'Timelord', 'Sphreeze', 'Lion Heart', 'Reaper']):
                dangerous_battle[card_id] = name

        # Fusion Spell: spell that creates fusion summon or uses SUMMON_TYPE_FUSION
        if is_spell and re_fusion_summon.search(content):
            fusion_spells[card_id] = name

    elapsed = time.time() - start_time
    print(f"Scan complete in {elapsed:.2f}s!")
    print(f"Target Immune monsters: {len(target_immune)}")
    print(f"Battle Immune monsters: {len(battle_immune)}")
    print(f"Dangerous Battle monsters: {len(dangerous_battle)}")
    print(f"Fusion Spells: {len(fusion_spells)}")

    # Write CardIntelligence.Generated.cs
    output_path = r"C:\Users\admin\Documents\EdoGame\src\YGO_SOURCE_CLEAN\windbot-fork\ExecutorBase\Game\AI\CardIntelligence.Generated.cs"
    with open(output_path, 'w', encoding='utf-8') as out:
        out.write("""using System.Collections.Generic;

namespace WindBot.Game.AI
{
    /// <summary>
    /// Auto-generated Card Intelligence Database extracted from official Lua scripts & cards.cdb.
    /// Run tools/scan_card_intelligence.py to update whenever new cards are released.
    /// </summary>
    public static partial class CardIntelligence
    {
        // ═══════════════════════════════════════════════════════════════
        //  AUTO-GENERATED: TARGET-IMMUNE MONSTERS
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> GeneratedTargetImmuneCards = new HashSet<int>
        {
""")
        for cid in sorted(target_immune.keys()):
            clean_name = target_immune[cid].replace('\n', ' ').replace('\r', '')
            out.write(f"            {cid},  // {clean_name}\n")
        out.write("""        };

        // ═══════════════════════════════════════════════════════════════
        //  AUTO-GENERATED: BATTLE-IMMUNE MONSTERS
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> GeneratedBattleImmuneCards = new HashSet<int>
        {
""")
        for cid in sorted(battle_immune.keys()):
            clean_name = battle_immune[cid].replace('\n', ' ').replace('\r', '')
            out.write(f"            {cid},  // {clean_name}\n")
        out.write("""        };

        // ═══════════════════════════════════════════════════════════════
        //  AUTO-GENERATED: DANGEROUS BATTLE MONSTERS (Reflect / Avoid)
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> GeneratedDangerousBattleMonsters = new HashSet<int>
        {
""")
        for cid in sorted(dangerous_battle.keys()):
            clean_name = dangerous_battle[cid].replace('\n', ' ').replace('\r', '')
            out.write(f"            {cid},  // {clean_name}\n")
        out.write("""        };

        // ═══════════════════════════════════════════════════════════════
        //  AUTO-GENERATED: FUSION SPELLS
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> GeneratedFusionSpells = new HashSet<int>
        {
""")
        for cid in sorted(fusion_spells.keys()):
            clean_name = fusion_spells[cid].replace('\n', ' ').replace('\r', '')
            out.write(f"            {cid},  // {clean_name}\n")
        out.write("""        };
    }
}
""")
    print(f"Successfully generated: {output_path}")

if __name__ == "__main__":
    scan_and_generate()

