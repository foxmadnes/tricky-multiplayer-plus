using UnityEngine;

namespace TrickyMultiplayerPlus
{
    class CrazyPuzzleFloorFactory : FloorFactory, IResettable, ISeedable
	{
		public CrazyPuzzleFloorFactory(string resourceNameIn, float heightIn) : base(resourceNameIn, heightIn)
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
			return new CrazyRandomPuzzleFloor(this.seed, base.height, gameWorldContainer);
		}
	}
}
