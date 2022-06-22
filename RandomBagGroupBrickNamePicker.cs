namespace TrickyMultiplayerPlus
{
    using System;
    using System.Collections.Generic;

    public class RandomBagGroupBrickNamePicker : IBrickPicker
	{
		public RandomBagGroupBrickNamePicker(string[] possibleBrickNames, int seed = -1, int groupSize = 4, int numBricksToSpawn = -1)
		{
			this._brickNameBag = new List<string>();
			this._seed = seed;
			this._possibleBrickNames = possibleBrickNames;
			this._numBricksToSpawn = numBricksToSpawn;
			this._bricksSpawnedCount = 0;
			this._brickGroupSize = groupSize;
			this._bricksLeftInGroup = 0;
			this._currentBrick = null;
			if (this._seed == -1)
			{
				this._seed = RandomBagGroupBrickNamePicker._RANDOM_SEEDER.Next();
			}
			this._random = new Random(this._seed);
		}

		public string ChooseBrick()
		{
			UnityEngine.Debug.Log("Choosing bricks in group brick picker!");
			if (this._numBricksToSpawn > 0 && this._bricksSpawnedCount >= this._numBricksToSpawn)
			{
				return null;
			}

			if (_bricksLeftInGroup > 0)
            {
				this._bricksSpawnedCount++;
				this._bricksLeftInGroup -= 1;
				return _currentBrick;
			}

			if (this._brickNameBag.Count == 0)
			{
				this.ResetBrickPicker();
			}

			int index = this._random.Next(this._brickNameBag.Count);
			string result = this._brickNameBag[index];
			this._currentBrick = result;
			this._bricksLeftInGroup = _brickGroupSize - 1;
			this._brickNameBag.RemoveAt(index);
			this._bricksSpawnedCount++;
			return result;
		}

		public void ResetBrickPicker()
		{
			this._brickNameBag.Clear();
			foreach (string item in this._possibleBrickNames)
			{
				this._brickNameBag.Add(item);
			}
		}

		public IBrickPicker Clone()
		{
			return new RandomBagBrickNamePicker(this._possibleBrickNames, this._seed, this._numBricksToSpawn);
		}

		public static readonly Random _RANDOM_SEEDER = new Random();

		private string[] _possibleBrickNames;

		private List<string> _brickNameBag;

		private Random _random;

		private int _seed;

		private int _numBricksToSpawn;

		private int _bricksSpawnedCount;

		private string _currentBrick;

		private int _bricksLeftInGroup;

		private int _brickGroupSize;
	}

}
