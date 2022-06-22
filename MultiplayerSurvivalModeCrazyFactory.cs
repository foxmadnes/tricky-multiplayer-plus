using System.Collections.Generic;

namespace TrickyMultiplayerPlus
{
	public class MultiplayerSurvivalModeCrazyFactory : AbstractMultiplayerSurvivalModeFactory
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
			//new SpellStruct("GHOST", 10),
			new SpellStruct("FAST_NO_ROTATE_REPEATING", 1),
			new SpellStruct("NO_ROTATE_MYSTERY", 1),
			new SpellStruct("LARGE_NO_ROTATE", 1)
			});

			return new SurvivalGameModeFactory(1, 66)
			{
				dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
				spellContainerSpawnerFactory = new MoveDownSpellContainerSpawnerFactory(0.25f, 0.8f, 8f, 5f),
				brickPickerFactory = new RandomNamedBrickPickerFactory(null, -1, 66),
				spellPickerFactory = new RubberBandingSpellPickerFactory(new SpellSetRuleFactory[]
				{

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
