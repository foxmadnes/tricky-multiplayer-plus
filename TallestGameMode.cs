using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace TrickyMultiplayerPlus
{

    public class TallestGameMode : AbstractMultiPlayerGameMode
	{
		public TallestGameMode()
		{
			this._winningPlayerXPosModel = new DataModelFloat(false);
			this._bricksUsedControllers = new List<BricksUsedController>();
			this._bricksLeftControllers = new List<BricksLeftController>();
		}


		public int brickLimit { private get; set; }

		public string startSpell { private get; set; }

		public string[] ambientAudio { private get; set; }


		protected override void _Init()
		{
			base._Init();
			Shader.SetGlobalFloat("_WaterCutoff", -8.5f);
			Shader.SetGlobalColor("_WaterColor", ColorUtil.FromHex(2768553U));
			Shader.SetGlobalColor("_WaterLineColor", ColorUtil.FromHex(12106473U));
			Shader.EnableKeyword("WATER_ON");
			this._brickLimitEndCondition = new FirstCompoundCondition();
			this._endCondition = new FirstCompoundCondition();
			this._endCondition.AddCompareCondition(this._brickLimitEndCondition);
		}

		public override void Setup()
		{
			base.Setup();
		}

		protected override void _InitStateControllers()
		{
			base._InitStateControllers();
			this._gameModePlayController = new MultiPlayerTallestGameModePlayController(this._winningPlayerXPosModel, this._highestTowerModel, this._lowestTowerModel, this._dropSpeedController, this.brickLimit);
			this._gameModePlayController.countDownComplete += this._HandleCountDownComplete;
			this._gameModePlayController.countDownStarted += this._HandleCountDownStarted;
			this.AddStateController("EXPLANATION", new GameModeExplanationController(this._explanationId, this._showControls, "INTRO", this.skipExplanation));
			this.AddStateController("INTRO", new MultiPlayerTallestGameModeIntroController("TALLEST", this.skipIntroduction, this.skipModeTitle));
			this.AddStateController("COUNTDOWN", new RaceGameModeCountDownController(this._musicResources));
			this.AddStateController("PLAY", this._gameModePlayController);
		}

		protected override void _Cleanup()
		{
			if (this._gameModePlayController != null)
			{
				this._gameModePlayController.countDownComplete -= this._HandleCountDownComplete;
				this._gameModePlayController.countDownStarted -= this._HandleCountDownStarted;
			}
			if (this._gameControllers != null)
			{
				foreach (AbstractGameController abstractGameController in this._gameControllers)
				{
					abstractGameController.stateChange -= this._HandleGameControllerStateChanged;
				}
			}
			base._Cleanup();
		}

		public override void GetSetup(NetworkWriter writer)
		{
			base.GetSetup(writer);
		}

		public override void SetSetup(NetworkReader reader)
		{
			base.SetSetup(reader);
		}

		public override float ModifyHorizontalMoveLimit(float limit)
		{
			return limit * 1.2f;
		}

		protected override void _AddGameController(AbstractGameController gameController)
		{
			gameController.stateChange += this._HandleGameControllerStateChanged;
		}

		protected override void _FillGameModel(GameModel gameModel, AbstractGameController gameController)
		{
			base._FillGameModel(gameModel, gameController);
			DataModelInt dataModelInt = new DataModelInt(false);
			gameModel.AddDataModel("WINNING_X_POS", this._winningPlayerXPosModel);
			gameModel.GetDataModel<DataModelString>("SPELL").value = this.startSpell;
			gameModel.AddDataModel("RANK", new DataModelInt(false));

			TowerHeightModel dataModel = gameModel.GetDataModel<TowerHeightModel>("TOWER_HEIGHT");
			DataModelFloat dummyTargetHeightModel = new DataModelFloat(false);
			dummyTargetHeightModel.value = 25;
			CompareConditionFloat value = new CompareConditionFloat(dataModel,dummyTargetHeightModel, ComparisonType.GREATER_THAN_OR_EQUAL, ValueDirection.FREE);
			this._towerHeightModels.Add(gameController.id, value);

			CompareConditionInt compareConditionInt = new CompareConditionInt(dataModelInt, 0, ComparisonType.LESS_THAN_OR_EQUAL, ValueDirection.FREE);
			DataModelInt dataModelInt2 = new DataModelInt(false);
			dataModelInt2.value = 0;
			dataModelInt2.minValue = 0;
			gameModel.AddDataModel("BRICKS_USED", dataModelInt2);
			BricksUsedController bricksUsedController = new BricksUsedController(dataModelInt2);
			gameController.Inject(bricksUsedController);
			this._bricksUsedControllers.Add(bricksUsedController);
			if (this.brickLimit > 0)
			{
				DataModelInt dataModelInt3 = new DataModelInt(false);
				dataModelInt3.value = this.brickLimit;
				dataModelInt3.maxValue = this.brickLimit;
				dataModelInt3.minValue = 0;
				gameModel.AddDataModel("BRICK_LEFT", dataModelInt3);
				BricksLeftController bricksLeftController = new BricksLeftController(dataModelInt3);
				gameController.Inject(bricksLeftController);
				this._bricksLeftControllers.Add(bricksLeftController);
				CompareConditionInt compareConditionInt2 = new CompareConditionInt(dataModelInt3, 0, ComparisonType.LESS_THAN_OR_EQUAL, ValueDirection.FREE);
				this._brickLimitEndCondition.AddCompareCondition(compareConditionInt2);
				this._brickLeftModels.Add(gameController.id, compareConditionInt2);
			}
		}

		protected override void _SetCustomGameStateControllers(AbstractGameController gameController)
		{
			Dictionary<string, AbstractStateController> dictionary = new Dictionary<string, AbstractStateController>();
			GameFinishController value = new GameFinishController(false, true, true);
			dictionary.Add("FINISH", value);
			dictionary.Add("WIN_REQUESTED", value);
			dictionary.Add("ROOF", new LocalRoofController(this._roofResource, false));
			dictionary.Add("BASK", new GameBaskController(false));
			dictionary.Add("GAME", new LocalGamePlayController(false));
			gameController.customStateControllers = dictionary;
		}

		protected override void _CreateHud(GameModel gameModel, AbstractGameController gameController, Rect viewPort)
		{
			AbstractHUD hud = new SurvivalHUD(gameModel, viewPort, gameController.id);
			gameController.SetHud(hud);
		}

		protected override BrickGuide _CreateBrickGuide(GameModel gameModel)
		{
			return new BrickGuide(new Color(1f, 0.8666667f, 1f, 0.5f))
			{
				minBottom = -12.5f
			};
		}

		private string[] _GetWinners()
		{
			string winner = "";
			float num = -1;
			foreach (string text in this._gameModels.Keys)
			{

				GameModel gameModel = this._gameModels[text];
				TowerHeightModel dataModel = gameModel.GetDataModel<TowerHeightModel>("TOWER_HEIGHT");
				float value = dataModel.value;
				if (value > num)
				{
					num = value;
					winner = text;
				}
			}
			return new string[] { winner };
		}

		protected override void _HandleGameStates(Dictionary<AbstractGameController, string> gameStates)
		{
			List<AbstractGameController> list = new List<AbstractGameController>();
			List<AbstractGameController> list2 = new List<AbstractGameController>();
			foreach (AbstractGameController abstractGameController in gameStates.Keys)
			{
				string a = gameStates[abstractGameController];
				if (a == "FINISH" || a == "WIN_REQUESTED")
				{
					list.Add(abstractGameController);
				}
				else if (a == "FINISH_REQUESTED")
				{
					list2.Add(abstractGameController);
				}
			}
			foreach (AbstractGameController abstractGameController2 in list2)
			{
				string state = abstractGameController2.stateMachine.state;
				if (state != "FINISH" && state != "BASK" && state != "ROOF")
				{
					base._OnGameEnd(abstractGameController2.id, true);
				}
			}
			if (list.Count == gameStates.Count)
			{
				this._gameModeEnded = true;
				base._OnGameModeEnd(this._GetWinners(), true);
			}
		}

		protected override string _GetIdByCondition(AbstractCondition condition)
		{
			foreach (string text in this._brickLeftModels.Keys)
			{
				if (this._brickLeftModels[text] == condition)
				{
					return text;
				}
			}
			return base._GetIdByCondition(condition);
		}

		protected override float[] _GetTowerValues(string[] ids)
		{
			float[] array = new float[this._gameModels.Values.Count];
			int num = 0;
			foreach (GameModel gameModel in this._gameModels.Values)
			{
				float num2 = gameModel.GetDataModel<TowerHeightModel>("TOWER_HEIGHT").value;
				array[num] = num2;
				num++;
			}
			return array;
		}

		protected override void _HandleGameModeEndConditionSuccess(AbstractCondition condition)
		{
			foreach (AbstractGameController abstractGameController in this._gameControllers)
			{
				this._gameControllersToEndRequest.Add(abstractGameController.id);
			}
			this._gameModeEnded = true;
			base._OnGameModeEndRequest(this._GetWinners());
		}

		private void _HandleCountDownComplete(AbstractGameController gameController)
		{
			base._OnGameEndRequest(gameController.id);
		}

		private void _HandleGameControllerStateChanged(string stateName, string prevStateName)
		{
			if (stateName == "FINISH")
			{
				bool flag = true;
				foreach (AbstractGameController abstractGameController in this._gameControllers)
				{
					if (!abstractGameController.finished)
					{
						flag = false;
					}
				}
				if (flag)
				{
					this._gameModeEnded = true;
					base._OnGameModeEndRequest(this._GetWinners());
				}
			}
		}

		private List<BricksUsedController> _bricksUsedControllers;

		private List<BricksLeftController> _bricksLeftControllers;

		private Dictionary<string, AbstractCondition> _brickLeftModels = new Dictionary<string, AbstractCondition>();

		private AbstractCompoundCondition _brickLimitEndCondition;

		private MultiPlayerTallestGameModePlayController _gameModePlayController;

		private Dictionary<string, AbstractCondition> _towerHeightModels = new Dictionary<string, AbstractCondition>();

		private DataModelFloat _winningPlayerXPosModel;
	}

}
