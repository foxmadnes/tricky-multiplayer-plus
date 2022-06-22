using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace TrickyTowersMod
{

	// Token: 0x020003B3 RID: 947
	public class TallestGameMode : AbstractMultiPlayerGameMode
	{
		// Token: 0x06001274 RID: 4724 RVA: 0x0000EF66 File Offset: 0x0000D166
		public TallestGameMode()
		{
			this._allowWizardMoveDownInBask = false;
		}


		public int brickLimit { private get; set; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600127B RID: 4731 RVA: 0x0000EFBE File Offset: 0x0000D1BE
		// (set) Token: 0x0600127C RID: 4732 RVA: 0x0000EFC6 File Offset: 0x0000D1C6
		public string startSpell { private get; set; }


		// Token: 0x0600127D RID: 4733 RVA: 0x00088874 File Offset: 0x00086A74
		protected override void _Init()
		{
			base._Init();
			base._Init();
			Shader.SetGlobalFloat("_WaterCutoff", -8.5f);
			Shader.SetGlobalColor("_WaterColor", ColorUtil.FromHex(2768553U));
			Shader.SetGlobalColor("_WaterLineColor", ColorUtil.FromHex(12106473U));
			Shader.EnableKeyword("WATER_ON");
			this._brickLimitEndCondition = new FirstCompoundCondition();
			this._endCondition = new FirstCompoundCondition();
			this._endCondition.AddCompareCondition(this._brickLimitEndCondition);
			base._SetGameModeEndCondition(this._endCondition);
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x0000EFCF File Offset: 0x0000D1CF
		public override void Setup()
		{
			base.Setup();
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x0008894C File Offset: 0x00086B4C
		protected override void _InitStateControllers()
		{
			base._InitStateControllers();
			this._gameModePlayController = new MultiPlayerPuzzleGameModePlayController(this.time, this._timeModel, this._progressModel, this._widestTowerModel, this._highestTowerModel, this._lowestTowerModel, this._dropSpeedController);
			this._gameModePlayController.countDownComplete += this._HandleCountDownComplete;
			this._gameModePlayController.countDownStarted += this._HandleCountDownStarted;
			this.AddStateController("EXPLANATION", new GameModeExplanationController(this._explanationId, this._showControls, "INTRO", this.skipExplanation));
			this.AddStateController("INTRO", new MultiPlayerPuzzleGameModeIntroController("PUZZLE", this.skipIntroduction, this.skipModeTitle));
			this.AddStateController("COUNTDOWN", new PuzzleGameModeCountdownController());
			this.AddStateController("PLAY", this._gameModePlayController);
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00088A2C File Offset: 0x00086C2C
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

		// Token: 0x06001281 RID: 4737 RVA: 0x0000EFD7 File Offset: 0x0000D1D7
		public override void GetSetup(NetworkWriter writer)
		{
			base.GetSetup(writer);
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x0000EFEC File Offset: 0x0000D1EC
		public override void SetSetup(NetworkReader reader)
		{
			base.SetSetup(reader);
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x0000F012 File Offset: 0x0000D212
		public override float ModifyHorizontalMoveLimit(float limit)
		{
			return limit * 1.2f;
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x0000F01B File Offset: 0x0000D21B
		protected override void _AddGameController(AbstractGameController gameController)
		{
			gameController.stateChange += this._HandleGameControllerStateChanged;
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00088B34 File Offset: 0x00086D34
		protected override void _FillGameModel(GameModel gameModel, AbstractGameController gameController)
		{
			base._FillGameModel(gameModel, gameController);
			DataModelInt dataModelInt = new DataModelInt(false);
			gameModel.AddDataModel("WINNING_X_POS", this._winningPlayerXPosModel);
			gameModel.GetDataModel<DataModelString>("SPELL").value = this.startSpell;

			TowerHeightModel dataModel = gameModel.GetDataModel<TowerHeightModel>("TOWER_HEIGHT");
			CompareConditionFloat value = new CompareConditionFloat(dataModel, this._targetHeightModel, ComparisonType.GREATER_THAN_OR_EQUAL, ValueDirection.FREE);
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
			base._SetEndCondition(gameController, compareConditionInt);
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x00088BDC File Offset: 0x00086DDC
		protected override void _SetCustomGameStateControllers(AbstractGameController gameController)
		{
			Dictionary<string, AbstractStateController> dictionary = new Dictionary<string, AbstractStateController>();
			GameFinishController value = new GameFinishController(true, true, true);
			dictionary.Add("FINISH", value);
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

		protected override AbstractTowerCamera _CreateTowerCamera(GameModel gameModel)
		{
			return new TowerCamera();
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00088C80 File Offset: 0x00086E80
		private string _GetWinner()
		{
			string winner = "";
			int num = -1;
			foreach (string text in this._gameModels.Keys)
			{
				AbstractGameController abstractGameController = base._GetGameControllerById(text);

				
				GameModel gameModel = this._gameModels[text];
				int value = this._towerHeightModels[text].value;
				if (value > num)
				{
					num = value;
					winner = text;
				}
			}
			//this._mostBricksModel.value = num;
			return winner;
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x00088D60 File Offset: 0x00086F60
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
				base._OnGameModeEnd(this._GetWinners(), false);
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

		// Token: 0x06001291 RID: 4753 RVA: 0x00088EC8 File Offset: 0x000870C8
		protected override float[] _GetTowerValues(string[] ids)
		{
			float[] array = new float[this._gameModels.Values.Count];
			int num = 0;
			foreach (GameModel gameModel in this._gameModels.Values)
			{
				float num2 = (float)gameModel.GetDataModel<DataModelInt>("TOWER_HEIGHT").value;
				array[num] = num2;
				num++;
			}
			return array;
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x00088F58 File Offset: 0x00087158
		protected override void _HandleGameModeEndConditionSuccess(AbstractCondition condition)
		{
			foreach (AbstractGameController abstractGameController in this._gameControllers)
			{
				this._gameControllersToEndRequest.Add(abstractGameController.id);
			}
			base._OnGameEndRequest(this._GetWinner());
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x0000F058 File Offset: 0x0000D258
		private void _HandleCountDownComplete(AbstractGameController gameController)
		{
			base._OnGameEndRequest(gameController.id);
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x00088FD4 File Offset: 0x000871D4
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

		private MultiPlayerPuzzleGameModePlayController _gameModePlayController;

		private Dictionary<string, AbstractCondition> _towerHeightModels = new Dictionary<string, AbstractCondition>();
	}

}
