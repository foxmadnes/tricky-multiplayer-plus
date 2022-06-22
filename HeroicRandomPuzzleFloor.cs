using UnityEngine;

namespace TrickyMultiplayerPlus
{
    class HeroicRandomPuzzleFloor : ProRandomPuzzleFloor
	{
		public HeroicRandomPuzzleFloor(int seedIn, float heightIn, GameObject startParent = null) : base(seedIn, heightIn, startParent)
		{
			this.seed = seedIn;
			this.puzzleHeight = (int)heightIn;
		}

		public int seed
		{
			get
			{
				return this._seed;
			}
			set
			{
				this._SetSeed(value);
			}
		}

		protected override void _Init()
		{
			base._Init();
			this._ReplaceFloor();
			this._GenerateFloor();
		}

		private void _ReplaceFloor()
		{
			if (this._gameObject.transform.childCount > 0)
			{
				base._ClearColliders();
				GameObject[] childGameObjects = WBTools.GetChildGameObjects(this._gameObject);
				foreach (GameObject obj in childGameObjects)
				{
					UnityEngine.Object.DestroyImmediate(obj);
				}
			}
			this._GenerateFloor();
			base._UpdateColliders();
		}

		private void _GenerateFloor()
		{
			if (this._seed == 0)
			{
				return;
			}
			System.Random random = new System.Random(this.seed);
			GameObject gameObject = new GameObject();
			gameObject.name = "Collider";
			gameObject.transform.parent = this._gameObject.transform;
			gameObject.transform.localPosition = Vector3.zero;
			this._CreateSides(gameObject, random, 6);
			this._CreateSides(gameObject, random, -5);
			this._CreateSides(gameObject, random, 1);

			WBTools.SetLayerRecursively(gameObject, this._gameObject.layer, WBTools.LayerType.ANY);
		}

		private void _CreateSides(GameObject colliderObject, System.Random random, int x)
		{
			bool flag = true;
			for (int i = 0; i < this.puzzleHeight - 3; i++)
			{
				if (random.Next(100) > 60)
				{
					GameObject gameObject;
					if (flag)
					{
						flag = false;
						gameObject = Singleton<ResourceManager>.instance.InstantiateByName("FLOOR_PUZZLE_PRO_PIECE_TOP");
					}
					else
					{
						gameObject = Singleton<ResourceManager>.instance.InstantiateByName("FLOOR_PUZZLE_PRO_PIECE_" + UnityEngine.Random.Range(1, 12));
					}
					gameObject.transform.parent = colliderObject.transform;
					gameObject.transform.localPosition = new Vector3((float)x, (float)(-(float)i));
				}
			}
		}

		private void _SetSeed(int value)
		{
			if (value != this._seed && value != 0)
			{
				this._seed = value;
				if (base.hasGameObject)
				{
					this._ReplaceFloor();
				}
			}
		}

		private const int _BOTTOM_MARGIN = 3;

		private int _seed;

		public int puzzleHeight;

		private string[] _floorResources;
	}
}
