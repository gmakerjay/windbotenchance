"""
audit_card_ids.py — ตรวจ Card ID ในซอร์ส C# เทียบกับ cards.cdb (ทุก .cdb ใน EdoGame\\WindBot)

ใช้:
  python tools/audit_card_ids.py windbot-fork/ExecutorBase          # ตรวจ core
  python tools/audit_card_ids.py windbot-fork/Game/AI/Decks         # ตรวจเด็คทั้งหมด
  python tools/audit_card_ids.py windbot-fork/Game/AI/Decks/XExecutor.cs   # ตรวจไฟล์เดียว

รายงาน:
  NOTFOUND  — ตัวเลข 5–9 หลักที่ไม่มีใน cdb (ID ปลอม / พิมพ์ผิด)  [ข้ามเลข < 100000 ที่ไม่มีคอมเมนต์]
  MISMATCH  — บรรทัดที่มี ID เดียว + คอมเมนต์ชื่อการ์ด แต่ชื่อใน cdb ไม่ตรงคอมเมนต์ → แสดง ID ที่ถูก (ถ้าค้นชื่อเจอ)

หมายเหตุ: ค่าที่ไม่ใช่ ID (คะแนน 100000/999999, token ที่ไม่มีใน cdb) จะขึ้น NOTFOUND ได้ — ตรวจด้วยตา
"""
import sqlite3, glob, re, sys, os

HERE = os.path.dirname(os.path.abspath(__file__))
CDB_GLOBS = [
    os.path.join(HERE, '..', '..', '..', 'WindBot', '*.cdb'),   # EdoGame\WindBot\*.cdb (deployed)
    os.path.join(HERE, '..', 'cards.cdb'),                        # repo cards.cdb
    os.path.join(HERE, '..', 'windbot-fork', '*.cdb'),
]

def load_cards():
    rows = {}
    for g in CDB_GLOBS:
        for db in sorted(glob.glob(g)):
            try:
                for i, n, a in sqlite3.connect(db).execute(
                        "select t.id, t.name, d.alias from texts t join datas d on d.id=t.id"):
                    rows.setdefault(i, (n.strip("'\""), a))
            except Exception:
                pass
    return rows

def norm(s):
    return re.sub(r'[^a-z0-9]', '', s.lower())

def main():
    if hasattr(sys.stdout, 'reconfigure'):
        sys.stdout.reconfigure(encoding='utf-8', errors='replace')
    if len(sys.argv) < 2:
        print(__doc__); sys.exit(1)
    rows = load_cards()
    if not rows:
        print('ERROR: no cdb found'); sys.exit(2)
    byname = {}
    for i, (n, a) in rows.items():
        if a == 0:
            byname.setdefault(norm(n), []).append(i)
    num = re.compile(r'(?<![\w.])(\d{5,9})(?![\w.])')
    target = sys.argv[1]
    out_file = None
    if '--output' in sys.argv:
        idx = sys.argv.index('--output')
        out_file = sys.argv[idx + 1]
    elif '-o' in sys.argv:
        idx = sys.argv.index('-o')
        out_file = sys.argv[idx + 1]
    out_lines = []
    def log(msg):
        print(msg)
        if out_file:
            out_lines.append(msg + '\n')
    files = [target] if os.path.isfile(target) else [
        os.path.join(dp, f) for dp, _, fn in os.walk(target)
        if '\\obj' not in dp and '/obj' not in dp and '\\bin' not in dp and '/bin' not in dp
        for f in fn if f.endswith('.cs')]
    count = 0
    IGNORED_NUMS = {10000, 12000, 15000, 18000, 20000, 25000, 30000, 40000, 50000, 99999, 100000, 200000, 500000, 999999, 16777218}
    ALIASES = {
        norm('bls'): norm('black luster soldier'),
        norm('maximus dragma'): norm('dogmatika maximus'),
        norm('spenta'): norm('spoon, the seal of magistus'),
        norm('spenta, the magistus sealer'): norm('spoon, the seal of magistus'),
        norm('blue-eyes sage'): norm('sage with eyes of blue'),
        norm('blue-eyes melody'): norm('the melody of awakening dragon'),
        norm('blue-eyes mausoleum'): norm('mausoleum of white'),
        norm('altergeist spoofing'): norm('personal spoofing')
    }

    for p in files:
        for ln, line in enumerate(open(p, encoding='utf-8', errors='ignore'), 1):
            code, _, comment = line.partition('//')
            ids = [int(x) for x in num.findall(code)]
            comment = comment.strip()
            for cid in ids:
                if cid in IGNORED_NUMS:
                    continue
                if cid in rows:
                    if len(ids) == 1 and comment:
                        real = rows[cid][0]
                        base = re.split(r'\s*[\(\[]|\s+[—-]\s+|\s*/\s*|:\s', comment)[0].strip()
                        rn, cn = norm(real), norm(comment)
                        bn = norm(base)

                        # Check variable name in code
                        vmatch = re.search(r'(\w+)\s*=\s*' + str(cid), code)
                        vn = norm(vmatch.group(1)) if vmatch else ''

                        matched = (
                            rn[:10] in cn or bn[:10] in rn or rn in cn or
                            (vn and (vn in rn or rn[:8] in vn or (len(vn) >= 4 and vn[:6] in rn))) or
                            any(a in cn and b in rn for a, b in ALIASES.items())
                        )

                        if not matched:
                            fix = (byname.get(bn) or [None])[0]
                            log(f"MISMATCH {p}:{ln} {cid} cdb='{real}' // {comment[:60]}  => {fix}")
                            count += 1
                elif not (cid < 100000 and not comment):
                    log(f"NOTFOUND {p}:{ln} {cid} // {comment[:60]}")
                    count += 1
    log(f"\n{count} issue(s) in {len(files)} file(s)")
    if out_file:
        with open(out_file, 'w', encoding='utf-8') as f:
            f.writelines(out_lines)

if __name__ == '__main__':
    main()
