namespace TrickyMultiplayerPlus
{
    public class MultiplayerTallestModeProFactory : AbstractMulitplayerTallestModeFactory
	{
		public override TallestGameModeFactory Create()
		{
			return new TallestGameModeFactory()
			{
				dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
				brickPickerFactory = new SharedRandomNamedBrickPickerFactory(null, -1, 45),
				startSpell = "BUBBLE",
				brickLimit = 45,
				windStrengthMax = 0f,
				windStrengthMin = 0f
			};
		}
	}
}
