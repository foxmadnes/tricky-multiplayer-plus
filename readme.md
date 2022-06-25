# TRICKY MULTIPLAYER PLUS

This is a BepInEx mod of Tricky Towers. To Install, unzip a [mod release](https://github.com/foxmadnes/tricky-multiplayer-plus/releases) zip into your tricky towers install location so the BepInEx folder is alongside the TrickyTowers.exe file.

This mod was developed on Windows and likely will only work on Windows for now. I developed this for me and my friends, so the code could probably use cleanup, and there is only English support.


![Race Crazy](screenshots/RaceCrazy.jpg?raw=true "Race Crazy")


An expanded multiplayer mod for Tricky Towers. 

Note: This mod is not compatible with vanilla tricky towers users, please do not try to queue for public games with this mod enabled.


Adds one new game mode (Tallest), and two new difficulties (Heroic and Crazy) for all new and existing game modes!

## New Game mode: Tallest

![Tallest Title](screenshots/TallestTitle.jpg?raw=true "Tallest Title")

With the same blocks, be on the one to build the highest tower! Once your tower is finished, the tower will freeze in place and the game will wait for all other towers to finish. Only starting powerups are enabled.


## New Difficulty: Heroic

Harder or more competitive versions of existing game modes.

![Tallest Heroic Menu](screenshots/TallestHeroicMenu.jpg?raw=true "Tallest Heroic Menu")

In puzzle, two disjoint thin towers are the base.
In race, most positive spells are removed, and negative spells are nastier.
In survival, negative spells are boosted and spells are more common.
In tallest, more blocks are available and an ivy spell for strategic placement.

## New Difficulty: Crazy

Twists on existing game modes.

![Survival Crazy Menu](screenshots/SurvivalCrazyMenu.jpg?raw=true "Survival Crazy Menu") 

In puzzle: only a few random single blocks are available as a base.
In race: the base is two disjoint columns, and petrify and island powerups are disabled.
In survival: single life with tricky negative spells give tough choices for spells at each point.
In tallest: blocks come in sets of 4 of the same block, how will your tower shape up?


### Compilation

To compile the mod yourself, use Visual Studio and link the game dll files as well as the dlls in BepInEx_x86_5.4.15.0.
Assets are compiled from the pngs in the assets folder in an empty unity project with an AssetBundle named trickymultiplayerplus.

