namespace TrickyMultiplayerPlus
{
    class MultiplayerTallestModeCrazyFactory : AbstractMulitplayerTallestModeFactory
	{
		public override TallestGameModeFactory Create()
		{
			return new TallestGameModeFactory()
			{
				dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
				brickPickerFactory = new SharedRandomNamedGroupBrickPickerFactory(null, -1, 3, 35),
				startSpell = "IVY",
				brickLimit = 35,
				floorFactory = new ProPuzzleFloorFactory("FLOOR_PUZZLE_PRO", 5f),
				windStrengthMax = 0f,
				windStrengthMin = 0f
			};
		}
	}
}
