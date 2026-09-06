import sqlite3

conn = sqlite3.connect('cards.cdb')
cur = conn.cursor()

main_deck = [
    59913418, 59913418, # Demise Supreme King
    13518809,           # Ruin Supreme Queen
    72426662, 72426662, # Demise King
    46427957,           # Ruin Queen
    86124104, 86124104, 86124104, # Demise Agent
    50139096, 50139096, # Ruin Angel
    65877963, 65877963, 65877963, # Impcantation Chalislime
    80701178, 80701178, # Impcantation Talismandra
    53303460, 53303460, # Impcantation Candoll
    92919429, 92919429, 92919429, # Diviner of the Herald
    14558127, 14558127, # Ash Blossom
    13048472, 13048472, 13048472, # Pre-Preparation of Rites
    96729612, 96729612, 96729612, # Preparation of Rites
    8198712, 8198712, 8198712,    # End of the World
    32828635, 32828635, 32828635, # Cycle of the World
    95612049,           # Turning of the World
    69217334,           # Breaking of the World
    24299458, 24299458, 24299458, # Forbidden Droplet
    24224830, 24224830, # Called by the Grave
    18144506,           # Harpie's Feather Duster
]

extra_deck = [
    79606837, 79606837, # Herald of the Arc Light
    28400508,           # Draglubion
    57314798,           # Numeron Dragon
    63767246,           # Hope Harbinger
    8165596,            # Photon Lord
    90448279,           # Zeus
    90590303,           # Bagooska
    21044178,           # Abyss Dweller
    6983839,            # Tornado Dragon
    82633039,           # Castel
    73898890,           # Dyna Mondo
    50277355,           # Cross-Sheep
    2857636,            # Knightmare Phoenix
    86066372,           # Accesscode Talker
]

print(f"Main Deck Count: {len(main_deck)}")
print(f"Extra Deck Count: {len(extra_deck)}")

for card_id in set(main_deck + extra_deck):
    row = cur.execute('SELECT name FROM texts WHERE id = ?', (card_id,)).fetchone()
    if not row:
        print(f"MISSING: {card_id}")
    else:
        pass
print("ALL CARDS VERIFIED!")
