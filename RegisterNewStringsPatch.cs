using HarmonyLib;

namespace TrickyMultiplayerPlus
{
    [HarmonyPatch(typeof(LanguageManager))]
	[HarmonyPatch("LoadLanguage")]
	class RegisterNewStringsPatch
	{
		static void Postfix(LanguageManager __instance)
		{
			UnityEngine.Debug.Log("Patching strings.");
			__instance.RegisterLanguage("MODE_TALLEST", "Tallest", false, true);
			__instance.RegisterLanguage("EXPLANATION_TITLE_MULTIPLAYER_TALLEST", "Tallest Battle", false, true);
			__instance.RegisterLanguage("EXPLANATION_MULTIPLAYER_TALLEST", "Tallest tower with limited blocks wins!", false, true);

			__instance.RegisterLanguage("DIFFICULTY_HEROIC", "Heroic", false, true);
			__instance.RegisterLanguage("DIFFICULTY_CRAZY", "Crazy", false, true);
			__instance.RegisterLanguage("EXPLANATION_MULTIPLAYER_RACE_CRAZY_SUBTITLE", "Bridge the gap!", false, true);
			__instance.RegisterLanguage("COUNTDOWN_TALLEST", "Go!", false, true);

			return;
		}
	}
}
