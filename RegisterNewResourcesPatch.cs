using HarmonyLib;

namespace TrickyMultiplayerPlus
{
    [HarmonyPatch(typeof(InitResourcesCommand))]
	[HarmonyPatch("Execute")]
	class RegisterNewResourcesPatch
	{
		static bool Prefix(InitResourcesCommand __instance)
		{
			// Images will later be replaced.
			Singleton<ResourceManager>.instance.RegisterResource("MODE_SELECT_MODE_ITEM_TALLEST", "UI/State/ModeSelect/ModeItem01", false);
			Singleton<ResourceManager>.instance.RegisterResource("MODE_SELECT_MODE_ITEM_HEROIC", "UI/State/ModeSelect/ModeItem01", false);
			Singleton<ResourceManager>.instance.RegisterResource("MODE_SELECT_DIFFICULTY_ITEM_CRAZY", "UI/State/ModeSelect/DifficultyItem03", false);
			Singleton<ResourceManager>.instance.RegisterResource("EXPLANATION_DIFFICULTY_IMAGE_CRAZY", "UI/Popup/Explanation/DifficultyImage/Pro", false);
			Singleton<ResourceManager>.instance.RegisterResource("MODE_SELECT_DIFFICULTY_ITEM_HEROIC", "UI/State/ModeSelect/DifficultyItem03", false);
			Singleton<ResourceManager>.instance.RegisterResource("EXPLANATION_DIFFICULTY_IMAGE_HEROIC", "UI/Popup/Explanation/DifficultyImage/Pro", false);
			Singleton<ResourceManager>.instance.RegisterResource("MODE_TITLE_CRAZY", "UI/State/Game/ModeTitle/ModeTitleMessageSpecial", false);
			Singleton<ResourceManager>.instance.RegisterResource("MODE_TITLE_HEROIC", "UI/State/Game/ModeTitle/ModeTitleMessageSpecial", false);

			return true;
		}
	}
}
