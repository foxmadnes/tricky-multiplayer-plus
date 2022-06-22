using System;
using TMPro;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

/*
 * Replacing title in mode introduction because of oversight that makes new game modes otherwise say RACE
 */
namespace TrickyMultiplayerPlus
{

    [HarmonyPatch(typeof(ShowModeTitleEffect))]
	[HarmonyPatch("_HandlePreAnimationDelayComplete")]
	class AddGameModeTitlePatch
    {

         static AccessTools.FieldRef<ShowModeTitleEffect, string> gameTypeRef =
			AccessTools.FieldRefAccess<ShowModeTitleEffect, string>("_gameType");

		static AccessTools.FieldRef<ShowModeTitleEffect, string> difficultyRef =
			AccessTools.FieldRefAccess<ShowModeTitleEffect, string>("_difficulty");

		static AccessTools.FieldRef<ShowModeTitleEffect, GameObject> modeTitleRef =
			AccessTools.FieldRefAccess<ShowModeTitleEffect, GameObject>("_modeTitle");

		static AccessTools.FieldRef<ShowModeTitleEffect, bool> overtimeRef =
			AccessTools.FieldRefAccess<ShowModeTitleEffect, bool>("_overtime");

		static AccessTools.FieldRef<ShowModeTitleEffect, AnimationEventHandler> animationEventHandlerRef =
			AccessTools.FieldRefAccess<ShowModeTitleEffect, AnimationEventHandler>("_animationEventHandler");

		static bool Prefix(ref Timer timer, ShowModeTitleEffect __instance)
        {
			MethodInfo handlePreAnimationDelayCompleteMethod = AccessTools.DeclaredMethod(typeof(ShowModeTitleEffect), "_HandlePreAnimationDelayComplete");
			Action<Timer> handlePreAnimationDelayAction = (Action<Timer>) Delegate.CreateDelegate(typeof(Action<Timer>), __instance, handlePreAnimationDelayCompleteMethod);
			timer.complete -= handlePreAnimationDelayAction;
			string id = "MODE_" + gameTypeRef(__instance);

			GameObject parent = GameObject.Find("UI HUD/Container");
			string id2 = difficultyRef(__instance) != null ? "DIFFICULTY_" + difficultyRef(__instance) : "DIFFICULTY_EASY";
			string difficulty = difficultyRef(__instance);

			if (difficulty == "PRO")
			{
				modeTitleRef(__instance) = Singleton<ResourceManager>.instance.InstantiateByName("MODE_TITLE_SPECIAL", new Vector3(0f, -40f), parent);
			} else if (difficulty == "NORMAL")
            {
				modeTitleRef(__instance) = Singleton<ResourceManager>.instance.InstantiateByName("MODE_TITLE_" + difficultyRef(__instance), Vector3.zero, parent);
			} else
            {
				modeTitleRef(__instance) = Singleton<ResourceManager>.instance.InstantiateByName("MODE_TITLE_" + difficultyRef(__instance), new Vector3(0f, -40f), parent);
			}

			UnityEngine.Debug.Log("Fetching title for difficulty: " + id2 + " and mode " + id);
			TextMeshProUGUI component = WBTools.FindChildByName(modeTitleRef(__instance), "Background/Label").GetComponent<TextMeshProUGUI>();
			component.text = Singleton<LanguageManager>.instance.GetById(id) + " " + Singleton<LanguageManager>.instance.GetById(id2);
			if (!overtimeRef(__instance))
			{
				WBTools.FindChildByName(modeTitleRef(__instance), "Background/OvertimeLabel").SetActive(false);
			}
			animationEventHandlerRef(__instance) = modeTitleRef(__instance).GetComponent<AnimationEventHandler>();

			MethodInfo handleAnimationEventMethod = AccessTools.DeclaredMethod(typeof(ShowModeTitleEffect), "_HandleAnimationEvent");
			Action<string> handleAnimationEventAction = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), __instance, handleAnimationEventMethod);
			animationEventHandlerRef(__instance).animationEvent += handleAnimationEventAction;

			MonoBehaviourSingleton<AudioManager>.instance.PlaySfx("SFX_COUNTDOWN", 1f, 0f, 1f, false, false);

			return false;
		}
    }
}
