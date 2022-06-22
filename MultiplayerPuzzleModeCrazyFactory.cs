using System;

namespace TrickyMultiplayerPlus
{
    public class MultiplayerPuzzleModeCrazyFactory : AbstractMulitplayerPuzzleModeFactory
	{
		public override PuzzleGameModeFactory Create()
		{
			return new PuzzleGameModeFactory(2, 3, -1f)
			{
				dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
				brickPickerFactory = new SharedRandomNamedBrickPickerFactory(null, -1, -1),
				floorFactory = new CrazyPuzzleFloorFactory("FLOOR_PUZZLE_PRO", 10f),
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
}
