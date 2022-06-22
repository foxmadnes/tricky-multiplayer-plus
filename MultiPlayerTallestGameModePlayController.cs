namespace TrickyMultiplayerPlus
{
    using System.Collections.Generic;
    using UnityEngine;

    public class MultiPlayerTallestGameModePlayController : MultiPlayerGameModePlayController
	{
		public MultiPlayerTallestGameModePlayController(DataModelFloat winningPlayerXPosModel, DataModelFloat highestTowerModel, DataModelFloat lowestTowerModel, DropSpeedController dropSpeedController, int brickLimit) : base(highestTowerModel, lowestTowerModel, dropSpeedController)
		{
			this._winningPlayerXPosModel = winningPlayerXPosModel;
			this._highestTowerModel = highestTowerModel;
			this._brickLimit = brickLimit;
		}

		public override void UpdateController()
		{
			base.UpdateController();
		}

		private void _EndGame()
		{
			Debug.Log("Game ended");
		}

		protected override void _CheckGamePlayRules()
		{
			base._CheckGamePlayRules();
			foreach (AbstractGameController abstractGameController in this._gameControllers)
			{
				if (!abstractGameController.finished)
				{
					GameModel gameModel = this._gameModels[abstractGameController.id];
					TowerHeightModel dataModel = gameModel.GetDataModel<TowerHeightModel>("TOWER_HEIGHT");
					if (dataModel.value == this._highestTowerModel.value)
					{
						this._winningPlayerXPosModel.value = abstractGameController.zoomableCamera.camera.transform.position.x;
					}
				}
			}

			List<GameModel> list = new List<GameModel>();
			foreach (AbstractGameController abstractGameController2 in this._gameControllers)
			{
				GameModel item = this._gameModels[abstractGameController2.id];
				list.Add(item);
			}

			foreach (AbstractGameController abstractGameController in this._gameControllers)
			{
				if (!abstractGameController.finished)
				{
					GameModel gameModel = this._gameModels[abstractGameController.id];
					int num2 = gameModel.GetDataModel<DataModelInt>("BRICKS_USED").value;
					if (num2 >= this._brickLimit && !this._gameControllersInCountDown.Contains(abstractGameController))
					{
						if (abstractGameController is LocalGameController)
						{
							((LocalGameController)abstractGameController).DisableBrickSpawning();
						}
						this._StartCountDown(abstractGameController, false, false);
						this._gameControllersInCountDown.Add(abstractGameController);
					}
				}
			}

			this._UpdateTowerHeightInner(list);
			this._UpdateRankInner(list);
		}

		protected override void _UpdateMatchRank(int rank = 1)
		{
		}

		protected override float _GetMatchResultValue(string id)
		{
			return this._gameModels[id].GetDataModel<TowerHeightModel>("TOWER_HEIGHT").value;
		}

		private void _UpdateTowerHeightInner(List<GameModel> list)
        {
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = 0; j < list.Count - 1; j++)
				{
					if (list[j].GetDataModel<TowerHeightModel>("TOWER_HEIGHT").value < list[j + 1].GetDataModel<TowerHeightModel>("TOWER_HEIGHT").value)
					{
						GameModel value = list[j + 1];
						list[j + 1] = list[j];
						list[j] = value;
					}
				}
			}
		}

		private void _UpdateRankInner(List<GameModel> list)
        {
			int value2 = 0;
			int num = 1;
			float num2 = float.MaxValue;
			for (int k = 0; k < list.Count; k++)
			{
				float value3 = list[k].GetDataModel<TowerHeightModel>("TOWER_HEIGHT").value;
				if (value3 != num2)
				{
					value2 = num;
				}
				num2 = value3;
				list[k].GetDataModel<DataModelInt>("RANK").value = value2;
				list[k].GetDataModel<DataModelInt>("MATCH_RANK").value = value2;
				num++;
			}
		}

		private int _brickLimit;

		private DataModelFloat _winningPlayerXPosModel;

		private List<AbstractGameController> _gameControllersInCountDown = new List<AbstractGameController>();
	}

}
