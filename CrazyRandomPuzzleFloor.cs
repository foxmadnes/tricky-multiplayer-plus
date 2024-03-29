﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyMultiplayerPlus
{
	/** 
	 * Unity scripts expect all floors to be a prefab floor that ultimately are ProRandomPuzzleFloors, so new floors are subclasses 
	 * of ProRandomPuzzleFloor that clear all work done by ProRandomPuzzleFloor.
	**/
	class CrazyRandomPuzzleFloor : ProRandomPuzzleFloor
	{
		public CrazyRandomPuzzleFloor(int seedIn, float heightIn, GameObject startParent = null) : base(seedIn, heightIn, startParent)
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

			ArrayList alreadySeenX = new ArrayList();
			for (int i = 0; i < random.Next(3) + 3; i++)
            {
				int x = random.Next(10) - 5;
				int y = random.Next(this.puzzleHeight - _TOP_MARGIN);
				this._CreateBlock(gameObject, random, x, y);
			}

			WBTools.SetLayerRecursively(gameObject, this._gameObject.layer, WBTools.LayerType.ANY);
		}

		private void _CreateBlock(GameObject colliderObject, System.Random random, int x, int y)
		{
			GameObject gameObject;
			gameObject = Singleton<ResourceManager>.instance.InstantiateByName("FLOOR_PUZZLE_PRO_PIECE_TOP");
			gameObject.transform.parent = colliderObject.transform;
			gameObject.transform.localPosition = new Vector3((float)x, (float)(-(float)y));
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

		private const int _NUM_RANDOM_BLOCKS = 3;

		private const int _TOP_MARGIN = 3;

		private int _seed;

		public int puzzleHeight;

		private string[] _floorResources;
	}
}
