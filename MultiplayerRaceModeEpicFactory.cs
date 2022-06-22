using System;
using System.Collections.Generic;

namespace TrickyMultiplayerPlus
{
    public class MultiplayerRaceModeHeroicFactory : AbstractMulitplayerRaceModeFactory
	{
		public override RaceGameModeFactory Create()
		{
			Dictionary<string, SpellSet> darkSpellDictionary = new Dictionary<string, SpellSet>();
			darkSpellDictionary["UNDO"] = new SpellSet(new SpellStruct[]
			{
				new SpellStruct("SLOW", 10),
				new SpellStruct("ICE", 10),
				new SpellStruct("LARGE_MYSTERY", 4),
				new SpellStruct("LARGE_AUTO_ROTATE", 4),
				new SpellStruct("LARGE_ICE_SINGLE", 4)
			});

			Dictionary<string, SpellSet> darkSpellDictionaryFarBehind = new Dictionary<string, SpellSet>();
			darkSpellDictionary["UNDO"] = new SpellSet(new SpellStruct[]
			{
				new SpellStruct("LARGE_MYSTERY", 1),
				new SpellStruct("LARGE_AUTO_ROTATE", 1),
				new SpellStruct("LARGE_ICE_SINGLE", 1),
				new SpellStruct("ICE_AUTO_ROTATE_MYSTERY", 1)
			});

			RubberBandingSpellPickerFactory spellPickerFactory = new RubberBandingSpellPickerFactory(new SpellSetRuleFactory[]
			{

			new SpellSetRuleFactory(new DifferenceCompareConditionFloatFactory("TARGET_HEIGHT", "TOWER_HEIGHT", 15, ComparisonType.LESS_THAN_OR_EQUAL, ValueDirection.FREE), new SpellSet(new SpellStruct[]
			{
				new SpellStruct("UNDO", 1),
			}), darkSpellDictionary),
			new SpellSetRuleFactory(new DifferenceCompareConditionFloatFactory("HIGHEST_TOWER", "TOWER_HEIGHT", 20, ComparisonType.GREATER_THAN_OR_EQUAL, ValueDirection.FREE), new SpellSet(new SpellStruct[]
			{
				new SpellStruct("UNDO", 1),
			}), darkSpellDictionaryFarBehind),
			new SpellSetRuleFactory(new DifferenceCompareConditionFloatFactory("HIGHEST_TOWER", "TOWER_HEIGHT", 8, ComparisonType.GREATER_THAN_OR_EQUAL, ValueDirection.FREE), new SpellSet(new SpellStruct[]
			{
				new SpellStruct("UNDO", 1),
			}), darkSpellDictionary),
			new SpellSetRuleFactory(new DifferenceCompareConditionFloatFactory("HIGHEST_TOWER", "TOWER_HEIGHT", 8, ComparisonType.LESS_THAN, ValueDirection.FREE), new SpellSet(new SpellStruct[]
			{
				new SpellStruct("UNDO", 1),
			}), darkSpellDictionary)
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
				spellContainerSpawnerFactory = new MoveDownSpellContainerSpawnerFactory(0.25f, 1f, 9f, 9f),
				windStrengthMin = 3f,
				windStrengthMax = 3f,
				randomInitialWindDirection = true
			};
		}
	}
}
