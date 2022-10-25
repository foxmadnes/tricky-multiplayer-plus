using System.Collections.Generic;

namespace TrickyMultiplayerPlus
{
    public class MultiplayerSurvivalModeHeroicFactory : AbstractMultiplayerSurvivalModeFactory
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
			return new SurvivalGameModeFactory(Settings.game.maxHealth, 122)
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
