using DG.Tweening;
using UnityEngine;

/*
 * This class copied as is because it is inaccesible by visibility from the dlls but is needed for new classes.
 */
namespace TrickyMultiplayerPlus
{

    internal class AnimateBrickCountEffect : AbstractEffect, IHUDInjectable, IGameDataInjectable, IInjectable
	{
		public AnimateBrickCountEffect(DataModelString nextBrick, int orginalBricksLeft, bool playSound = false, bool waitUntilComplete = true) : base(false, null)
		{
			this._originalBricksLeft = orginalBricksLeft;
			this._visualBricksToCount = new DataModelInt(false);
			this._bricksLeftCopy = new DataModelInt(false);
			this._nextBrick = nextBrick;
			this._playSound = playSound;
			this._waitUntilComplete = waitUntilComplete;
			this._brickPicker = new RandomBagBrickNamePicker(BrickHelper.NORMAL_BRICKS, -1, -1);
		}

		protected override void _Cleanup()
		{
			if (this._bricksLeftCopy != null)
			{
				this._bricksLeftCopy.valueChanged -= this._HandleBrickCountChanged;
			}
			if (this._visualBricksToCount != null)
			{
				this._visualBricksToCount.valueChanged -= this._HandleNextBrickChange;
			}
			DoTweenUtil.CleanupTweens(this);
			base._Cleanup();
		}

		public void SetHud(AbstractHUD hud)
		{
			this._survivalHUD = hud;
		}

		public void SetGameData(GameData gameData)
		{
			this._gameData = gameData;
		}

		public override AbstractEffect Clone()
		{
			return new AnimateBrickCountEffect(this._nextBrick, this._originalBricksLeft, this._playSound, this._waitUntilComplete);
		}

		protected override void _Start()
		{
			if (this._playSound)
			{
				if (this._originalBricksLeft == 33)
				{
					this._audio = new LocalAudioEffect(AudioName.COUNT_UP_SHORT, 1f, 0f, 1f, false, false, true);
				}
				else
				{
					this._audio = new LocalAudioEffect(AudioName.COUNT_UP_LONG, 1f, 0f, 1f, false, false, true);
				}
				this._audio.Start();
			}
			this._bricksToPlaceView = this._survivalHUD.GetView<BricksToPlaceView>();
			this._nextBrickView = this._survivalHUD.GetView<NextBrickView>();
			this._bricksLeftCopy.value = 0;
			this._bricksLeftCopy.valueChanged += this._HandleBrickCountChanged;
			this._visualBricksToCount.value = 0;
			this._visualBricksToCount.valueChanged += this._HandleNextBrickChange;
			float duration = 2f;
			if (this._originalBricksLeft == 33)
			{
				duration = 1f;
			}
			DoTweenUtil.AddTween(this, DOTween.To(() => this._bricksLeftCopy.value, delegate (int x)
			{
				this._bricksLeftCopy.value = x;
			}, this._originalBricksLeft, duration).SetEase(Ease.Linear).OnComplete(new TweenCallback(this._HandleCountUpCompleted)));
			DoTweenUtil.AddTween(this, DOTween.To(() => this._visualBricksToCount.value, delegate (int x)
			{
				this._visualBricksToCount.value = x;
			}, Mathf.FloorToInt((float)(this._originalBricksLeft / 2)), duration).SetEase(Ease.Linear));
			if (!this._waitUntilComplete)
			{
				base._OnComplete();
			}
		}

		private void _HandleBrickCountChanged(int newValue, int prevValue)
		{
			this._bricksToPlaceView.ForceShowValue(newValue);
		}

		private void _HandleNextBrickChange(int newValue, int oldValue)
		{
			string brickName = this._brickPicker.ChooseBrick();
			this._nextBrickView.ForceShowBrick(brickName);
		}

		private void _HandleCountUpCompleted()
		{
			if (this._audio != null)
			{
				this._audio.Stop();
				this._audio = null;
			}
			this._visualBricksToCount.valueChanged -= this._HandleNextBrickChange;
			this._nextBrickView.ForceShowBrick(this._nextBrick.value);
			this._gameData.nextBrick = this._nextBrick;
			if (this._waitUntilComplete)
			{
				base._CompleteAndFinish();
			}
			else
			{
				base._OnFinish();
			}
		}

		private const float _TWEEN_TIME_SHORT = 1f;

		private const float _TWEEN_TIME_LONG = 2f;

		private bool _waitUntilComplete;

		private AbstractHUD _survivalHUD;

		private DataModelInt _bricksLeftCopy;

		private DataModelString _nextBrick;

		private RandomBagBrickNamePicker _brickPicker;

		private DataModelInt _visualBricksToCount;

		private int _originalBricksLeft;

		private GameData _gameData;

		private BricksToPlaceView _bricksToPlaceView;

		private NextBrickView _nextBrickView;

		private bool _playSound;

		private AudioEffect _audio;
	}

}
