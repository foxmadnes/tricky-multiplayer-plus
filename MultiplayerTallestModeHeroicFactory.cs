using System;

namespace TrickyMultiplayerPlus
{
    public class MultiplayerTallestModeHeroicFactory : AbstractMulitplayerTallestModeFactory
	{
		public override TallestGameModeFactory Create()
		{
			return new TallestGameModeFactory()
			{
				dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
				brickPickerFactory = new SharedRandomNamedBrickPickerFactory(null, -1, 35),
				startSpell = "LARGE_SELF",
				brickLimit = 35,
				//floorFactory = new ProPuzzleFloorFactory("FLOOR_PUZZLE_PRO", 5f),
				floorFactory = new CrazyRaceFloorFactory("FLOOR_PUZZLE_PRO", 10f),

				backgroundFactory = new BackgroundsFactory(new Type[]
				{
					typeof(TallestBackground),
					typeof(WinnerHighlightBackground),
					typeof(MistyWindForeground),
					typeof(LoserHighlightBackground)
				}),
				windStrengthMax = 0f,
				windStrengthMin = 0f,
				randomInitialWindDirection = true
			};
		}
	}
}
