using System;
using System.Collections.Generic;
using HarmonyLib;

/**
 * Adding new Game Modes is easier in an existing InitMultiplayer command due to the use of local variables. Currently new games modes are in the Race Patch.
 * 
 * Tweaking existing game modes is done by copying the entire method and not running the original.
 */
namespace TrickyMultiplayerPlus
{
    [HarmonyPatch(typeof(InitMultiplayerRaceModeCommand))]
	[HarmonyPatch("Execute")]
	class AddNewRaceAndNewModesPatch
	{
		static AccessTools.FieldRef<InitMultiplayerRaceModeCommand, List<SelectModel>> localMultiplayerModesRef =
			AccessTools.FieldRefAccess<InitMultiplayerRaceModeCommand, List<SelectModel>>("_localMultiPlayerWorlds");

		static bool Prefix(InitMultiplayerRaceModeCommand __instance)
		{
			UnityEngine.Debug.Log("Patched race modes!");
			RaceGameModeFactory gameModeFactoryIn = new MultiplayerRaceModeEasyFactory().Create();
			RaceGameModeFactory gameModeFactoryIn2 = new MultiplayerRaceModeNormalFactory().Create();
			RaceGameModeFactory gameModeFactoryIn3 = new MultiplayerRaceModeProFactory().Create();
			RaceGameModeFactory customHeroicGameFactory = new MultiplayerRaceModeHeroicFactory().Create();
			RaceGameModeFactory crazyGameFactory = new MultiplayerRaceModeCrazyFactory().Create();
			BackgroundsFactory raceBackgroundsFactoryIn = new BackgroundsFactory(new Type[]
			{
			typeof(RaceBackground),
			typeof(RaceForeground)
			});
			BackgroundsFactory raceBackgroundsFactoryIn2 = new BackgroundsFactory(new Type[]
			{
			typeof(RaceProBackground),
			typeof(RaceProForeground)
			});
			WorldModel raceItem = new WorldModel("RACE", "MRW", "Race World", new List<SelectModel>
		{
			new MultiplayerGameModeModel("RACE_NORMAL", "Race Normal", gameModeFactoryIn2, raceBackgroundsFactoryIn, "NORMAL", "MULTIPLAYER_RACE_NORMAL"),
			new MultiplayerGameModeModel("RACE_PRO", "Race Pro", gameModeFactoryIn3, raceBackgroundsFactoryIn2, "PRO", "MULTIPLAYER_RACE_PRO"),
			new MultiplayerGameModeModel("RACE_HEROIC", "Race Heroic", customHeroicGameFactory, raceBackgroundsFactoryIn2, "HEROIC", "MULTIPLAYER_RACE_HEROIC"),
			new MultiplayerGameModeModel("RACE_CRAZY", "Race Crazy", crazyGameFactory, raceBackgroundsFactoryIn, "CRAZY", "MULTIPLAYER_RACE_CRAZY")
		}, 0);

			UnityEngine.Debug.Log("Adding tallest modes!");

			TallestGameModeFactory tallestGameHeroic = new MultiplayerTallestModeHeroicFactory().Create();
			TallestGameModeFactory tallestGameMedium = new MultiplayerTallestModeMediumFactory().Create();
			TallestGameModeFactory tallestGamePro = new MultiplayerTallestModeProFactory().Create();
			TallestGameModeFactory tallestGameCrazy = new MultiplayerTallestModeCrazyFactory().Create();

			BackgroundsFactory tallestBackgroundsFactoryIn = new BackgroundsFactory(new Type[]
			{
			typeof(TallestBackground),
			typeof(MistForeground)
			});

			BackgroundsFactory tallestWindyBackgroundsFactoryIn = new BackgroundsFactory(new Type[]
{
			typeof(TallestBackground),
			typeof(MistyWindForeground)
});

			WorldModel tallestItem = new WorldModel("TALLEST", "MTW", "Tallest World", new List<SelectModel>
		{
			new MultiplayerGameModeModel("TALLEST_NORMAL", "Tallest Normal", tallestGameMedium, tallestBackgroundsFactoryIn, "NORMAL", "MULTIPLAYER_TALLEST_NORMAL"),
			new MultiplayerGameModeModel("TALLEST_PRO", "Tallest Pro", tallestGamePro, tallestBackgroundsFactoryIn, "PRO", "MULTIPLAYER_TALLEST_PRO"),
			new MultiplayerGameModeModel("TALLEST_HEROIC", "Tallest Heroic", tallestGameHeroic, tallestWindyBackgroundsFactoryIn, "HEROIC", "MULTIPLAYER_TALLEST_HEROIC"),
			new MultiplayerGameModeModel("TALLEST_CRAZY", "Tallest Crazy", tallestGameCrazy, tallestBackgroundsFactoryIn, "CRAZY", "MULTIPLAYER_TALLEST_CRAZY")
		}, 0);
			localMultiplayerModesRef(__instance).Add(raceItem);
			localMultiplayerModesRef(__instance).Add(tallestItem);
			return false;
		}
	}

	[HarmonyPatch(typeof(InitMultiPlayerPuzzleModeCommand))]
	[HarmonyPatch("Execute")]
	class AddNewPuzzleModesPatch
	{
		static AccessTools.FieldRef<InitMultiPlayerPuzzleModeCommand, List<SelectModel>> localMultiplayerModesRef =
			AccessTools.FieldRefAccess<InitMultiPlayerPuzzleModeCommand, List<SelectModel>>("_localMultiPlayerWorlds");

		static bool Prefix(InitMultiPlayerPuzzleModeCommand __instance)
		{
			UnityEngine.Debug.Log("Patched puzzle modes!");
			PuzzleGameModeFactory gameModeFactoryIn = new MultiplayerPuzzleModeEasyFactory().Create();
			PuzzleGameModeFactory gameModeFactoryIn2 = new MultiplayerPuzzleModeNormalFactory().Create();
			PuzzleGameModeFactory gameModeFactoryIn3 = new MultiplayerPuzzleModeProFactory().Create();
			PuzzleGameModeFactory customHeroicGameFactory = new MultiplayerPuzzleModeHeroicFactory().Create();
			PuzzleGameModeFactory customCrazyGameFactory = new MultiplayerPuzzleModeCrazyFactory().Create();

			BackgroundsFactory backgroundsFactoryIn = new BackgroundsFactory(new Type[]
			{
			typeof(DesertBackgroundSun),
			typeof(DesertBackground),
			typeof(DesertForeground)
			});
			BackgroundsFactory backgroundsFactoryIn2 = new BackgroundsFactory(new Type[]
			{
			typeof(DesertProBackgroundSun),
			typeof(DesertProBackground),
			typeof(DesertForeground)
			});
			WorldModel item = new WorldModel("PUZZLE", "MPW", "Puzzle World", new List<SelectModel>
		{
			new MultiplayerGameModeModel("PUZZLE_NORMAL", "Puzzle Normal", gameModeFactoryIn2, backgroundsFactoryIn, "NORMAL", "MULTIPLAYER_PUZZLE_NORMAL"),
			new MultiplayerGameModeModel("PUZZLE_PRO", "Puzzle Pro", gameModeFactoryIn3, backgroundsFactoryIn2, "PRO", "MULTIPLAYER_PUZZLE_PRO"),
			new MultiplayerGameModeModel("PUZZLE_HEROIC", "Puzzle Heroic", customHeroicGameFactory, backgroundsFactoryIn2, "HEROIC", "MULTIPLAYER_PUZZLE_HEROIC"),
			new MultiplayerGameModeModel("PUZZLE_CRAZY", "Puzzle Crazy", customCrazyGameFactory, backgroundsFactoryIn2, "CRAZY", "MULTIPLAYER_PUZZLE_CRAZY")
		}, 0);
			localMultiplayerModesRef(__instance).Add(item);
			return false;
		}
	}

	[HarmonyPatch(typeof(InitMultiplayerSurvivalModeCommand))]
	[HarmonyPatch("Execute")]
	class AddNewSurvivalModesPatch
	{
		static AccessTools.FieldRef<InitMultiplayerSurvivalModeCommand, List<SelectModel>> localMultiplayerModesRef =
	AccessTools.FieldRefAccess<InitMultiplayerSurvivalModeCommand, List<SelectModel>>("_localMultiPlayerWorlds");

		static bool Prefix(InitMultiplayerSurvivalModeCommand __instance)
		{
			UnityEngine.Debug.Log("Patched survival modes!");
			SurvivalGameModeFactory gameModeFactoryIn2 = new MultiplayerSurvivalModeNormalFactory().Create();
			SurvivalGameModeFactory gameModeFactoryIn3 = new MultiplayerSurvivalModeProFactory().Create();
			SurvivalGameModeFactory customHeroicGameFactory = new MultiplayerSurvivalModeHeroicFactory().Create();
			SurvivalGameModeFactory customCrazyGameFactory = new MultiplayerSurvivalModeCrazyFactory().Create();
			BackgroundsFactory backgroundsFactoryIn = new BackgroundsFactory(new Type[]
			{
			typeof(SurvivalBackground),
			typeof(MistForeground)
			});
			BackgroundsFactory backgroundsFactoryIn2 = new BackgroundsFactory(new Type[]
			{
			typeof(SurvivalProBackground),
			typeof(MistForeground)
			});
			WorldModel item = new WorldModel("SURVIVAL", "MSW", "Survival World", new List<SelectModel>
		{
			new MultiplayerGameModeModel("SURVIVAL_NORMAL", "Survival Normal", gameModeFactoryIn2, backgroundsFactoryIn, "NORMAL", "MULTIPLAYER_SURVIVAL_NORMAL"),
			new MultiplayerGameModeModel("SURVIVAL_PRO", "Survival Pro", gameModeFactoryIn3, backgroundsFactoryIn2, "PRO", "MULTIPLAYER_SURVIVAL_PRO"),
			new MultiplayerGameModeModel("SURVIVAL_HEROIC", "Survival Heroic", customHeroicGameFactory, backgroundsFactoryIn2, "HEROIC", "MULTIPLAYER_SURVIVAL_HEROIC"),
			new MultiplayerGameModeModel("SURVIVAL_CRAZY", "Survival Crazy", customCrazyGameFactory, backgroundsFactoryIn2, "CRAZY", "MULTIPLAYER_SURVIVAL_CRAZY")
		}, 0);
			localMultiplayerModesRef(__instance).Add(item);
			return false;
		}
	}
}
