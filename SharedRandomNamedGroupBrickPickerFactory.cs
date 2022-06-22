namespace TrickyMultiplayerPlus
{
    class SharedRandomNamedGroupBrickPickerFactory : RandomNamedGroupBrickPickerFactory
	{
		public SharedRandomNamedGroupBrickPickerFactory(string[] bricks = null, int forcedSeed = -1, int groupSize = 4, int numBricksToSpawn = -1) : base(bricks, forcedSeed, groupSize, numBricksToSpawn)
		{
		}

		public override IBrickPicker CreateBrickPicker()
		{
			return new RandomBagGroupBrickNamePicker(this._bricks, base.seed, this._groupSize, this._numBricksToSpawn);
		}
	}
}
