using System;

namespace TrickyMultiplayerPlus
{
    public class TallestGameModeFactory : AbstractSinglePlayerGameModeFactory
	{
		public TallestGameModeFactory()
		{
			this.ambientAudio = new string[]
			{
				"AMBIENCE_BREEZE"
			};
			this.musicAudio = new MusicStruct[]
			{
			new MusicStruct("MUSIC_RACE", 1f)
			};
			backgroundFactory = new BackgroundsFactory(new Type[]
			{
					typeof(TallestBackground),
					typeof(WinnerHighlightBackground),
					typeof(MistForeground),
					typeof(LoserHighlightBackground)
			});
			this.worldId = 0;
			this.floorFactory = new FloorFactory("FLOOR_LWS", 12.5f);
		}

		protected override AbstractGameMode _CreateGameMode()
		{
			return new TallestGameMode
			{
				brickLimit = this.brickLimit,
				startSpell = this.startSpell,
				ambientAudio = this.ambientAudio
			};
		}

		public string startSpell;

		public int brickLimit;
	}

}
