using System;

namespace TrickyMultiplayerPlus
{
    public class MultiplayerTallestModeMediumFactory : AbstractMulitplayerTallestModeFactory
	{
		public override TallestGameModeFactory Create()
		{
			return new TallestGameModeFactory()
			{
				dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
				brickPickerFactory = new SharedRandomNamedBrickPickerFactory(null, -1, 35),
				startSpell = "IVY",
				brickLimit = 35,
				windStrengthMin = 0f,
				windStrengthMax = 0f
			};
		}
	}
}
