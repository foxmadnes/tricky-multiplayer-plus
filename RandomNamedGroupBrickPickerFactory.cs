using System;

namespace TrickyMultiplayerPlus
{
	class RandomNamedGroupBrickPickerFactory : AbstractNamedBrickPickerFactory, IResettable, ISeedable
	{
		public RandomNamedGroupBrickPickerFactory(string[] bricks = null, int forcedSeed = -1, int groupSize = 4, int numBricksToSpawn = -1) : base(bricks)
		{
			this._numBricksToSpawn = numBricksToSpawn;
			this._forcedSeed = forcedSeed;
			this._groupSize = 4;
			this.Reset();
		}

		public int seed { get; set; }

		public override IBrickPicker CreateBrickPicker()
		{
			this.Reset();
			return new RandomBagGroupBrickNamePicker(this._bricks, this.seed, this._groupSize, this._numBricksToSpawn);
		}

		public void Reset()
		{
			if (this._forcedSeed == -1)
			{
				this.seed = RandomBagGroupBrickNamePicker._RANDOM_SEEDER.Next();
			}
			else
			{
				this.seed = this._forcedSeed;
			}
		}

		private static readonly Random _RANDOM_SEEDER = new Random();

		protected int _forcedSeed;

		protected int _groupSize;

		protected int _numBricksToSpawn;
	}
}
