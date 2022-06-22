using System;

namespace TrickyMultiplayerPlus
{
    public abstract class AbstractMulitplayerTallestModeFactory
	{
		public virtual TallestGameModeFactory Create()
		{
			throw new NotImplementedException();
		}
	}
}
