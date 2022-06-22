namespace TrickyMultiplayerPlus
{
    class MultiplayerRaceModeCrazyFactory : AbstractMulitplayerRaceModeFactory
	{
		public override RaceGameModeFactory Create()
		{
			RubberBandingSpellPickerFactory spellPickerFactory = new RubberBandingSpellPickerFactory(new SpellSetRuleFactory[]
			{
			new SpellSetRuleFactory(new DifferenceCompareConditionFloatFactory("TARGET_HEIGHT", "TOWER_HEIGHT", 15, ComparisonType.LESS_THAN_OR_EQUAL, ValueDirection.FREE), new SpellSet(new SpellStruct[]
			{
				new SpellStruct("IVY", 1),
				new SpellStruct("UNDO", 1)
			}), this._darkSpellMappingNormal),
			new SpellSetRuleFactory(new DifferenceCompareConditionFloatFactory("HIGHEST_TOWER", "TOWER_HEIGHT", 15, ComparisonType.GREATER_THAN_OR_EQUAL, ValueDirection.FREE), new SpellSet(new SpellStruct[]
			{
				new SpellStruct("AUTO_BUILD", 2),
				new SpellStruct("PETRIFY",  1),
				new SpellStruct("LARGE_SELF", 1)
			}), this._darkSpellMappingFarBehind),
			new SpellSetRuleFactory(new DifferenceCompareConditionFloatFactory("HIGHEST_TOWER", "TOWER_HEIGHT", 8, ComparisonType.GREATER_THAN_OR_EQUAL, ValueDirection.FREE), new SpellSet(new SpellStruct[]
			{
			}), this._darkSpellMappingBehind),
			new SpellSetRuleFactory(new DifferenceCompareConditionFloatFactory("HIGHEST_TOWER", "TOWER_HEIGHT", 8, ComparisonType.LESS_THAN, ValueDirection.FREE), new SpellSet(new SpellStruct[]
			{
				new SpellStruct("IVY", 10),
				new SpellStruct("UNDO", 10),
			}), this._darkSpellMappingNormal)
			});
			float raceHeight = Settings.debug.raceHeight;
			return new RaceGameModeFactory((raceHeight <= 0f) ? 45f : raceHeight, 120f, 0.2f)
			{
				dropSpeedControllerFactory = new DropSpeedControllerFactory(2.5f),
				spellPickerFactory = spellPickerFactory,
				brickDroppedCheckerFactory = new BrickDroppedCheckerFactory(true),
				brickPickerFactory = new SharedRandomNamedBrickPickerFactory(null, -1, -1),
				spellContainerSpawnerFactory = new MoveDownSpellContainerSpawnerFactory(0.25f, 0.5f, 18f, 9f),
				floorFactory = new CrazyRaceFloorFactory("FLOOR_PUZZLE_PRO", 10f)
			};
		}
	}
}
