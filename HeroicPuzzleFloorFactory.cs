using UnityEngine;

namespace TrickyMultiplayerPlus
{
    class HeroicPuzzleFloorFactory : FloorFactory, IResettable, ISeedable
	{
		public HeroicPuzzleFloorFactory(string resourceNameIn, float heightIn) : base(resourceNameIn, heightIn)
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
			return new HeroicRandomPuzzleFloor(this.seed, base.height, gameWorldContainer);
		}
	}
}
