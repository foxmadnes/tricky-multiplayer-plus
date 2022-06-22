using UnityEngine;

namespace TrickyMultiplayerPlus
{
    class CrazyRaceFloorFactory : FloorFactory, IResettable, ISeedable
	{
		public CrazyRaceFloorFactory(string resourceNameIn, float heightIn) : base(resourceNameIn, heightIn)
		{
			this.Reset();
		}

		public int seed { get; set; }

		public void Reset()
		{
			this.seed = UnityEngine.Random.Range(1, 100000000);
		}

		public override Floor CreateFloor(GameObject gameWorldContainer)
		{
			return new CrazyRandomRaceFloor(this.seed, base.height, gameWorldContainer);
		}
	}
}
