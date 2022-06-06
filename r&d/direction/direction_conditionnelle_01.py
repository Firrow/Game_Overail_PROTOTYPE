# region import et objet utile pour la suite
from os import system

class Point:
    def __init__(self, x: int, y: int) -> None:
        self.x = x
        self.y = y

DEBUG = False
# endregion


# Exemple créé sur cette map :
STRING_TILEMAP = """  A B C
1 ╔═══╗
2 ║ ╔═╣
3 ╚═╩═╝
"""

# La map est modélisé avec un simple attribut qui relate les directions possible depuis une tuile
TILEMAP = [
#     ╔     ═      ╗
    ["ES", "EO",  "SO"],
#     ║     ╔      ╣
    ["NS", "ES",  "NSO"],
#     ╚     ╩      ╝
    ["NE", "NEO", "NO"],
]
# Paramètre réglable dans le futur : permet d'inverser G et D si le train est tête en bas
# à -1 on inverse, à 1 on garde la G et D du >> train <<
# Pour les commandes façon Manon il faut inverser
INVERSION_NORD = -1

# position de départ du train en B2
position = Point(1, 1)
# le train regarde à gauche, donc il "vient" de la case C2
from_direction = "E"
# la sortie que le train doit prendre
go_direction = ""  # inconnu pour l'instant
# utilisation d'une valeur par défaut pour la direction choisie par le joueur
player_direction = 1

loop = True
while loop:
    system("cls")

    print(STRING_TILEMAP)
    print(f"{position.x+1}.{'ABC'[position.y]}")

    # choix entre G et D du joueur
    player_input = input().upper()
    # G vaut 1 et D vaut -1
    if player_input in ["G", "D"]:
        player_direction = 1 if player_input == "G" else -1
    if DEBUG: print(f"player_direction = {player_direction}")

    # récupération des directions possible de la case actuelle
    tile_directions = TILEMAP[position.x][position.y]
    if DEBUG: print(f"tile_directions = {tile_directions}")

    # index de la direction dont provient le train
    from_index = tile_directions.index(from_direction)
    if DEBUG: print(f"from_index = {from_index}")

    # on lit la case index+player_direction (1 si G ou -1 si D) modulo len pour boucler la liste sur elle même
    # /!\ si le train à la tête en bas on applique la préférence INVERSION_NORD du joueur
    go_index = from_index + player_direction*(INVERSION_NORD if from_direction == "N" else 1)
    if DEBUG: print(f"go_index = {go_index} = {from_index} + {player_direction}*({INVERSION_NORD} if {from_direction} == 'N' else 1)")
    go_direction = tile_directions[go_index % len(tile_directions)]
    if DEBUG: print(f"go_direction = {go_direction}")

    # Il reste plus qu'à appliquer le déplacement
    # On garde en mémoire la case d'où l'on vient en prenant l'inverse du go_direction
    if go_direction == "N":
        position.x -= 1
        from_direction = "S"
    if go_direction == "E":
        position.y += 1
        from_direction = "O"
    if go_direction == "S":
        position.x += 1
        from_direction = "N"
    if go_direction == "O":
        position.y -= 1
        from_direction = "E"

    # Affichage de la nouvelle position
    print(f"{position.x+1}.{'ABC'[position.y]}")

    # Encore un tour ?
    loop = input("Continue ?\n") == ""
