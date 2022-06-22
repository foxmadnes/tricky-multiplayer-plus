namespace TrickyMultiplayerPlus
{
    using System.Collections.Generic;

    internal class MultiPlayerTallestGameModeIntroController : AbstractMultiPlayerGameModeIntroController, IHUDInjectable, IGameModelInjectable, IGameDataInjectable, IBrickPickerInjectable, IInjectable
	{
		public MultiPlayerTallestGameModeIntroController(string gameType, bool skipIntroduction = false, bool skipModeTitle = false) : base(gameType, skipIntroduction, skipModeTitle)
		{
			this._skipIntroduction = skipIntroduction;
			this._huds = new List<AbstractHUD>();
			this._brickPickers = new List<IBrickPicker>();
			this._brickLeftModels = new List<DataModelInt>();
			this._nextBrickModels = new List<DataModelString>();
			this._gameData = new List<GameData>();

		}

		public override void Enable(string prevStateName, object data = null)
		{
			base.Enable(prevStateName, data);
			if (!this._skipIntroduction)
			{
				for (int i = 0; i < this._huds.Count; i++)
				{
					AbstractHUD abstractHUD = this._huds[i];
					BricksToPlaceView view = abstractHUD.GetView<BricksToPlaceView>();
					if (view != null)
					{
						view.ForceShowValue(0);
					}
				}
				int value = this._brickLeftModels[0].value;
				for (int j = 0; j < this._huds.Count; j++)
				{
					bool waitUntilComplete = j == this._brickLeftModels.Count - 1;
					HudShowEffect hudShowEffect = new HudShowEffect(waitUntilComplete);
					hudShowEffect.SetHud(this._huds[j]);
					this._effectRunner.AddEffect(hudShowEffect);
				}
				for (int k = 0; k < this._brickLeftModels.Count; k++)
				{
					this._nextBrickModels[k].value = this._brickPickers[k].ChooseBrick();
					bool flag = k == this._brickLeftModels.Count - 1;
					AnimateBrickCountEffect animateBrickCountEffect = new AnimateBrickCountEffect(this._nextBrickModels[k], value, flag, flag);
					animateBrickCountEffect.SetHud(this._huds[k]);
					animateBrickCountEffect.SetGameData(this._gameData[k]);
					this._effectRunner.AddEffect(animateBrickCountEffect);
				}
				this._effectRunner.AddEffect(new DelayEffect(0.3f, true));
			}
			this._effectRunner.AddEffect(new DelegateEffect(new DelegateEffect.EffectDelegate(this._GotoCountdownState)));
		}

		public void SetBrickPicker(IBrickPicker brickPicker)
		{
			if (!this._brickPickers.Contains(brickPicker))
			{
				this._brickPickers.Add(brickPicker);
			}
		}

		public void SetHud(AbstractHUD hud)
		{
			if (!this._huds.Contains(hud))
			{
				this._huds.Add(hud);
			}
		}

		public void SetGameData(GameData gameData)
		{
			if (!this._gameData.Contains(gameData))
			{
				this._gameData.Add(gameData);
			}
		}

		public void SetGameModel(GameModel gameModel)
		{
			DataModelInt dataModel = gameModel.GetDataModel<DataModelInt>("BRICK_LEFT");
			DataModelString dataModel2 = gameModel.GetDataModel<DataModelString>("NEXT_BRICK");
			if (!this._brickLeftModels.Contains(dataModel))
			{
				this._brickLeftModels.Add(dataModel);
			}
			if (!this._nextBrickModels.Contains(dataModel2))
			{
				this._nextBrickModels.Add(dataModel2);
			}
		}

		private void _GotoCountdownState()
		{
			((StateMachineFlowController)this._stateFlowController).stateMachine.ChangeState("COUNTDOWN", null);
		}

		private const float _DELAY = 0.3f;

		private List<AbstractHUD> _huds;

		private List<IBrickPicker> _brickPickers;

		private List<DataModelInt> _brickLeftModels;

		private List<DataModelString> _nextBrickModels;

		private List<GameData> _gameData;
	}


}
