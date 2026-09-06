import sqlite3

conn = sqlite3.connect('cards.cdb')
cur = conn.cursor()

names = ['Dark Ruler No More', 'Forbidden Droplet', 'Lightning Storm', 'Super Polymerization', 'Triple Tactics Talent', 'Evenly Matched', 'Gameciel, the Sea Turtle Kaiju', 'Lava Golem', 'Raigeki']

for name in names:
    res = cur.execute('SELECT id, name FROM texts WHERE name = ?', (name,)).fetchall()
    print(f"{name}: {res}")
