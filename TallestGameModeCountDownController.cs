using System;
using TMPro;
using UnityEngine;

// Token: 0x02000288 RID: 648
public class TallestGameModeCountDownController : GameModeCountDownController
{
	// Token: 0x06000B63 RID: 2915 RVA: 0x0005E340 File Offset: 0x0005C540
	public TallestGameModeCountDownController(MusicStruct[] musicResources = null)
	{
		this._musicResources = musicResources;
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x0005E350 File Offset: 0x0005C550
	protected override void _SetupEffects(GameObject message, Animator animator)
	{
		this._effectRunner.AddEffect(new SetGameObjectActiveEffect(message, true));
		AnimationEffect animationEffect = new AnimationEffect(animator, "Countdown", "race", "PlayComplete");
		animationEffect.finish += this._HandleAnimationFinish;
		this._effectRunner.AddEffect(animationEffect);
		this._effectRunner.AddEffect(new DelegateEffect(new DelegateEffect.EffectDelegate(this._GotoPlayState)));
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x0005E3C0 File Offset: 0x0005C5C0
	protected override void _DestroyMessage()
	{
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x0005E3C4 File Offset: 0x0005C5C4
	protected override void _GotoPlayState()
	{
		if (this._musicResources != null)
		{
			foreach (MusicStruct musicStruct in this._musicResources)
			{
				new MusicPlayEffect(musicStruct.resource, "MUSIC", musicStruct.volume, true, 0f, true).Start();
			}
		}
		base._GotoPlayState();
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x0005E430 File Offset: 0x0005C630
	private void _HandleAnimationFinish(AbstractEffect effect)
	{
		effect.finish -= this._HandleAnimationFinish;
		UnityEngine.Object.Destroy(this._message);
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x0005E450 File Offset: 0x0005C650
	protected override void _HandleAnimationEvent(string eventName)
	{
		base._HandleAnimationEvent(eventName);
		if (eventName == "race")
		{
			MonoBehaviourSingleton<AudioManager>.instance.PlaySfx("SPEECH_YAY_05", 1f, 0f, 1f, false, false);
			TextMeshProUGUI label = this._label;
			string text = LanguageManager.Gets("COUNTDOWN_TALLEST");
			this._labelBack.text = text;
			label.text = text;
		}
	}

	// Token: 0x04000C83 RID: 3203
	private MusicStruct[] _musicResources;
}
