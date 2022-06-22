using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;

namespace TrickyTowersMod
{
	[BepInPlugin("trickytowersmod", "Game Mode Tweaks Mod", "1.0.0.0")]
	public class CustomRaceModePlugin : BaseUnityPlugin
	{
		void Awake()
		{
			UnityEngine.Debug.Log("Hello, world!");
			var harmony = new Harmony("trickytowersmod");
			harmony.PatchAll();
		}
	}

	[HarmonyPatch(typeof(InitResourcesCommand))]
	[HarmonyPatch("Execute")]
	class RegisterNewResourcesPatch
	{
		static bool Prefix(InitResourcesCommand __instance)
		{
			Singleton<ResourceManager>.instance.RegisterResource("MODE_SELECT_MODE_ITEM_TALLEST", "UI/State/ModeSelect/ModeItem01", false);
			return true;
		}
	}

	[HarmonyPatch(typeof(LanguageManager))]
	[HarmonyPatch("LoadLanguage")]
	class RegisterNewStringsPatch
	{
		static void Postfix(LanguageManager __instance)
		{
			UnityEngine.Debug.Log("Patching strings.");
			__instance.RegisterLanguage("MODE_TALLEST", "Tallest", false, true);
			__instance.RegisterLanguage("EXPLANATION_TITLE_MULTIPLAYER_TALLEST", "Tallest Battle", false, true);
			__instance.RegisterLanguage("EXPLANATION_MULTIPLAYER_TALLEST", "Tallest tower with limited blocks wins!", false, true);
			return;
		}
	}

	[HarmonyPatch(typeof(InitExplanationsCommand))]
	[HarmonyPatch("Execute")]
	class RegisterNewExplanationsPatch
	{
		static bool Prefix(InitResourcesCommand __instance)
		{
			ExplanationManager instance = Singleton<ExplanationManager>.instance;
			string text = "EXPLANATION_ICON_MAGIC_DISABLED";
			string text2 = "EXPLANATION_ICON_IVY";
			string text3 = "EXPLANATION_ICON_UNDO";
			instance.AddExplanation("MULTIPLAYER_TALLEST_EASY", new ExplanationStruct("EXPLANATION_TITLE_MULTIPLAYER_TALLEST", "EXPLANATION_MULTIPLAYER_TALLEST", "EXPLANATION_MODE_IMAGE_RACE", true, "EASY", null, null, text));
			instance.AddExplanation("MULTIPLAYER_TALLEST_NORMAL", new ExplanationStruct("EXPLANATION_TITLE_MULTIPLAYER_TALLEST", "EXPLANATION_MULTIPLAYER_TALLEST", "EXPLANATION_MODE_IMAGE_RACE", true, "NORMAL", null, null, null));
			instance.AddExplanation("MULTIPLAYER_TALLEST_PRO", new ExplanationStruct("EXPLANATION_TITLE_MULTIPLAYER_TALLEST", "EXPLANATION_MULTIPLAYER_TALLEST", "EXPLANATION_MODE_IMAGE_RACE", true, "PRO", null, null, null));
			instance.AddExplanationOverview(new ExplanationOverviewModel("EXPLANATION_TITLE_MULTIPLAYER_TALLEST", "EXPLANATION_MULTIPLAYER_TALLEST", "EXPLANATION_MODE_IMAGE_RACE", true, null, null, text, null, null, null, null, null, null));
			return true;
		}
	}

	[HarmonyPatch(typeof(InitMultiplayerRaceModeCommand))]
	[HarmonyPatch("Execute")]
	class AddNewRaceAndNewModesPatch
	{
		static AccessTools.FieldRef<InitMultiplayerRaceModeCommand, List<SelectModel>> localMultiplayerModesRef =
			AccessTools.FieldRefAccess<InitMultiplayerRaceModeCommand, List<SelectModel>>("_localMultiPlayerWorlds");

		static bool Prefix(InitMultiplayerRaceModeCommand __instance)
		{
			UnityEngine.Debug.Log("Patched race modes!");
			//RaceGameModeFactory gameModeFactoryIn = new MultiplayerRaceModeEasyFactory().Create();
			RaceGameModeFactory gameModeFactoryIn2 = new MultiplayerRaceModeNormalFactory().Create();
			RaceGameModeFactory gameModeFactoryIn3 = new MultiplayerRaceModeProFactory().Create();
			RaceGameModeFactory customEpicGameFactory = new MultiplayerRaceModeEpicFactory().Create();
			BackgroundsFactory backgroundsFactoryIn = new BackgroundsFactory(new Type[]
			{
			typeof(RaceBackground),
			typeof(RaceForeground)
			});
			BackgroundsFactory backgroundsFactoryIn2 = new BackgroundsFactory(new Type[]
			{
			typeof(RaceProBackground),
			typeof(RaceProForeground)
			});
			WorldModel raceItem = new WorldModel("RACE", "MRW", "Race World", new List<SelectModel>
		{
			//MODE_SELECT_DIFFICULTY_ITEM_EPIC
			//new MultiplayerGameModeModel("RACE_EASY", "Race Easy", gameModeFactoryIn, backgroundsFactoryIn, "EASY", "MULTIPLAYER_RACE_EASY"),
			new MultiplayerGameModeModel("RACE_NORMAL", "Race Normal", gameModeFactoryIn2, backgroundsFactoryIn, "NORMAL", "MULTIPLAYER_RACE_NORMAL"),
			new MultiplayerGameModeModel("RACE_PRO", "Race Pro", gameModeFactoryIn3, backgroundsFactoryIn2, "PRO", "MULTIPLAYER_RACE_PRO"),
			new MultiplayerGameModeModel("RACE_EASY", "Race Epic", customEpicGameFactory, backgroundsFactoryIn2, "EASY", "MULTIPLAYER_RACE_EASY")
		}, 0);

			//TallestGameModeFactory tallestGameEasy = MultiplayerTallestGameModeEasyFactory().create();

			WorldModel tallestItem = new WorldModel("TALLEST", "MTW", "Tallest World", new List<SelectModel>
		{
			new MultiplayerGameModeModel("TALLEST_EASY", "Tallest Easy", customEpicGameFactory, backgroundsFactoryIn, "EASY", "MULTIPLAYER_TALLEST_EASY"),
			new MultiplayerGameModeModel("TALLEST_NORMAL", "Tallest Normal", customEpicGameFactory, backgroundsFactoryIn, "NORMAL", "MULTIPLAYER_TALLEST_NORMAL"),
			new MultiplayerGameModeModel("TALLEST_PRO", "Tallest Pro", customEpicGameFactory, backgroundsFactoryIn2, "PRO", "MULTIPLAYER_TALLEST_PRO"),
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
			PuzzleGameModeFactory customEpicGameFactory = new MultiplayerPuzzleModeEpicFactory().Create();
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
			//new MultiplayerGameModeModel("PUZZLE_EASY", "Puzzle Easy", gameModeFactoryIn, backgroundsFactoryIn, "EASY", "MULTIPLAYER_PUZZLE_EASY"),
			new MultiplayerGameModeModel("PUZZLE_NORMAL", "Puzzle Normal", gameModeFactoryIn2, backgroundsFactoryIn, "NORMAL", "MULTIPLAYER_PUZZLE_NORMAL"),
			new MultiplayerGameModeModel("PUZZLE_PRO", "Puzzle Pro", gameModeFactoryIn3, backgroundsFactoryIn2, "PRO", "MULTIPLAYER_PUZZLE_PRO"),
			new MultiplayerGameModeModel("PUZZLE_EASY", "Puzzle Epic", customEpicGameFactory, backgroundsFactoryIn2, "EASY", "MULTIPLAYER_PUZZLE_EASY")
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
			//SurvivalGameModeFactory gameModeFactoryIn = new MultiplayerSurvivalModeEasyFactory().Create();
			SurvivalGameModeFactory gameModeFactoryIn2 = new MultiplayerSurvivalModeNormalFactory().Create();
			SurvivalGameModeFactory gameModeFactoryIn3 = new MultiplayerSurvivalModeProFactory().Create();
			SurvivalGameModeFactory customEpicGameFactory = new MultiplayerSurvivalModeEpicFactory().Create();
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
			//new MultiplayerGameModeModel("SURVIVAL_EASY", "Survival Easy", gameModeFactoryIn, backgroundsFactoryIn, "EASY", "MULTIPLAYER_SURVIVAL_EASY"),
			new MultiplayerGameModeModel("SURVIVAL_NORMAL", "Survival Normal", gameModeFactoryIn2, backgroundsFactoryIn, "NORMAL", "MULTIPLAYER_SURVIVAL_NORMAL"),
			new MultiplayerGameModeModel("SURVIVAL_PRO", "Survival Pro", gameModeFactoryIn3, backgroundsFactoryIn2, "PRO", "MULTIPLAYER_SURVIVAL_PRO"),
			new MultiplayerGameModeModel("SURVIVAL_EASY", "Survival Epic", customEpicGameFactory, backgroundsFactoryIn2, "EASY", "MULTIPLAYER_SURVIVAL_EASY")
		}, 0);
			localMultiplayerModesRef(__instance).Add(item);
			return false;
		}
	}

	[HarmonyPatch(typeof(AbstractInitSpellsCommand))]
	[HarmonyPatch("Execute")]
	class AddNewSpellsPatch
	{

		static bool Prefix(AbstractInitSpellsCommand __instance)
		{
			MethodInfo methodInfo = typeof(AbstractInitSpellsCommand).GetMethod("_AddLightCurrentBrickSpell", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(string), typeof(string), typeof(AbstractEffect[]) }, null);
			var parameters = new object[] {"LARGE_SELF", "LARGE_SPELL_ICON", new AbstractEffect[]
				{
				new GetCurrentBrickEffect(),
				new ApplyModifierEffect(new LargeBrickModifier(), true, false, null)
				}
			};
			methodInfo.Invoke(__instance, parameters);
			return true;
		}
	}
	public class MultiplayerRaceModeEpicFactory : AbstractMulitplayerRaceModeFactory
	{
		// Token: 0x060006B8 RID: 1720 RVA: 0x00064AD8 File Offset: 0x00062CD8
		public override RaceGameModeFactory Create()
		{
			RubberBandingSpellPickerFactory spellPickerFactory = new RubberBandingSpellPickerFactory(new SpellSetRuleFactory[]
			{
			new SpellSetRuleFactory(new DifferenceCompareConditionFloatFactory("TARGET_HEIGHT", "TOWER_HEIGHT", 15, ComparisonType.LESS_THAN_OR_EQUAL, ValueDirection.FREE), new SpellSet(new SpellStruct[]
			{
				new SpellStruct("IVY", 1),
				new SpellStruct("UNDO", 1)
			}), this._darkSpellMappingNormal),
			new SpellSetRuleFactory(new DifferenceCompareConditionFloatFactory("HIGHEST_TOWER", "TOWER_HEIGHT", 20, ComparisonType.GREATER_THAN_OR_EQUAL, ValueDirection.FREE), new SpellSet(new SpellStruct[]
			{
				new SpellStruct("AUTO_BUILD", 2),
				new SpellStruct("PETRIFY", 1),
				new SpellStruct("MINIBASE", 1),
			}), this._darkSpellMappingFarBehind),
			new SpellSetRuleFactory(new DifferenceCompareConditionFloatFactory("HIGHEST_TOWER", "TOWER_HEIGHT", 8, ComparisonType.GREATER_THAN_OR_EQUAL, ValueDirection.FREE), new SpellSet(new SpellStruct[]
			{
				new SpellStruct("PETRIFY", 1),
				new SpellStruct("MINIBASE", 1),
				new SpellStruct("LARGE_SELF", 1),
			}), this._darkSpellMappingBehind),
			new SpellSetRuleFactory(new DifferenceCompareConditionFloatFactory("HIGHEST_TOWER", "TOWER_HEIGHT", 8, ComparisonType.LESS_THAN, ValueDirection.FREE), new SpellSet(new SpellStruct[]
			{
				new SpellStruct("IVY", 10),
				new SpellStruct("UNDO", 5),
				new SpellStruct("PETRIFY", 3),
				new SpellStruct("MINIBASE", 3),
				new SpellStruct("LARGE_SELF", 5),
			}), this._darkSpellMappingNormal)
			});
			return new RaceGameModeFactory(38f, 120f, 0.2f)
			{
				ambientAudio = new string[]
				{
				"AMBIENCE_STORM"
				},
				musicAudio = new MusicStruct[]
				{
				new MusicStruct("MUSIC_RACE_PRO", 1f)
				},
				dropSpeedControllerFactory = new DropSpeedControllerFactory(5f),
				spellPickerFactory = spellPickerFactory,
				brickDroppedCheckerFactory = new BrickDroppedCheckerFactory(false),
				brickPickerFactory = new SharedRandomNamedBrickPickerFactory(null, -1, -1),
				backgroundFactory = new BackgroundsFactory(new Type[]
				{
				typeof(RaceProBackground),
				typeof(WinnerHighlightBackground),
				typeof(FinishLineBackground),
				typeof(RaceProForeground),
				typeof(LoserHighlightBackground)
				}),
				worldId = 0,
				spellContainerSpawnerFactory = new MoveDownSpellContainerSpawnerFactory(0.25f, 0.75f, 9f, 9f),
				windStrengthMin = 3f,
				windStrengthMax = 5f,
				randomInitialWindDirection = true
			};
		}
	}

	public class MultiplayerPuzzleModeEpicFactory : AbstractMulitplayerPuzzleModeFactory
	{
		public override PuzzleGameModeFactory Create()
		{
			return new PuzzleGameModeFactory(5, 13, -1f)
			{
				dropSpeedControllerFactory = new DropSpeedControllerFactory(3f),
				brickPickerFactory = new SharedRandomNamedBrickPickerFactory(null, -1, -1),
				floorFactory = new ProPuzzleFloorFactory("FLOOR_PUZZLE_PRO", 10f),
				startSpell = "IVY",
				backgroundFactory = new BackgroundsFactory(new Type[]
				{
				typeof(DesertProBackgroundSun),
				typeof(DesertProBackground),
				typeof(DesertDistortionBackground),
				typeof(DesertMultiplayerLaserBackground),
				typeof(WinnerHighlightBackground),
				typeof(DesertForeground),
				typeof(LoserHighlightBackground)
				}),
				windStrengthMax = 0f,
				windStrengthMin = 0f
			};
		}
	}

	public class MultiplayerSurvivalModeEpicFactory : AbstractMultiplayerSurvivalModeFactory
	{
		public override SurvivalGameModeFactory Create()
		{
			Dictionary<string, SpellSet> dictionary = new Dictionary<string, SpellSet>();
			dictionary["IVY"] = new SpellSet(new SpellStruct[]
			{
			new SpellStruct("LARGE", 10),
			new SpellStruct("MYSTERY", 10),
			new SpellStruct("NO_ROTATE", 5)
			});
			dictionary["PETRIFY"] = new SpellSet(new SpellStruct[]
			{
			new SpellStruct("NO_ROTATE", 7),
			new SpellStruct("FAST", 5),
			new SpellStruct("FAST_NO_ROTATE_REPEATING", 3),
			new SpellStruct("NO_ROTATE_MYSTERY", 4),
			new SpellStruct("LARGE_NO_ROTATE", 1)
			});
			dictionary["MINIBASE"] = new SpellSet(new SpellStruct[]
			{
			new SpellStruct("NO_ROTATE", 7),
			new SpellStruct("FAST", 10),
			new SpellStruct("GHOST", 10),
			new SpellStruct("FAST_NO_ROTATE_REPEATING", 1),
			new SpellStruct("NO_ROTATE_MYSTERY", 1),
			new SpellStruct("LARGE_NO_ROTATE", 1)
			});
			dictionary["EXTRA_LIFE"] = new SpellSet(new SpellStruct[]
			{
			new SpellStruct("LARGE_NO_ROTATE_MYSTERY", 5),
			new SpellStruct("FAST", 7),
			new SpellStruct("GHOST", 10),
			new SpellStruct("FAST_NO_ROTATE_REPEATING", 4),
			new SpellStruct("NO_ROTATE_MYSTERY", 4),
			new SpellStruct("LARGE_NO_ROTATE", 4)
			});
			Dictionary<string, SpellSet> dictionary2 = new Dictionary<string, SpellSet>();
			dictionary2["IVY"] = dictionary["IVY"];
			dictionary2["PETRIFY"] = dictionary["PETRIFY"];
			dictionary2["MINIBASE"] = dictionary["MINIBASE"];
			dictionary2["EXTRA_LIFE"] = new SpellSet(new SpellStruct[]
			{
			new SpellStruct("FAST_NO_ROTATE_REPEATING", 4),
			new SpellStruct("NO_ROTATE_MYSTERY", 4),
			new SpellStruct("LARGE_NO_ROTATE", 4),
			new SpellStruct("LARGE_NO_ROTATE_MYSTERY", 3)
			});
			return new SurvivalGameModeFactory(Settings.game.maxHealth, 66)
			{
				dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
				spellContainerSpawnerFactory = new MoveDownSpellContainerSpawnerFactory(0.25f, 0.8f, 8f, 5f),
				brickPickerFactory = new RandomNamedBrickPickerFactory(null, -1, 122),
				spellPickerFactory = new RubberBandingSpellPickerFactory(new SpellSetRuleFactory[]
				{
				new SpellSetRuleFactory(new CompareConditionIntFactory("HEALTH", 1, ComparisonType.EQUAL, ValueDirection.FREE), new SpellSet(new SpellStruct[]
				{
					new SpellStruct("EXTRA_LIFE", 6),
					new SpellStruct("IVY", 3),
					new SpellStruct("MINIBASE", 1),
					new SpellStruct("PETRIFY", 1),
				}), dictionary2),
				new SpellSetRuleFactory(new CompareConditionIntFactory("HEALTH", 3, ComparisonType.LESS_THAN, ValueDirection.FREE), new SpellSet(new SpellStruct[]
				{
					new SpellStruct("EXTRA_LIFE", 4),
					new SpellStruct("IVY", 3),
					new SpellStruct("MINIBASE", 1),
					new SpellStruct("PETRIFY", 1),
				}), dictionary),
				new SpellSetRuleFactory(null, new SpellSet(new SpellStruct[]
				{
					new SpellStruct("IVY", 3),
					new SpellStruct("MINIBASE", 1),
					new SpellStruct("PETRIFY", 1),
				}), dictionary)
				})
			};
		}
	}
}