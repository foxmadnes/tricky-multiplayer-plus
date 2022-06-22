using HarmonyLib;

namespace TrickyMultiplayerPlus
{
    [HarmonyPatch(typeof(InitExplanationsCommand))]
	[HarmonyPatch("Execute")]
	class RegisterNewExplanationsPatch
	{
		static bool Prefix(InitResourcesCommand __instance)
		{
			ExplanationManager instance = Singleton<ExplanationManager>.instance;
			string text = "EXPLANATION_ICON_MAGIC_DISABLED";
			instance.AddExplanation("MULTIPLAYER_TALLEST_EASY", new ExplanationStruct("EXPLANATION_TITLE_MULTIPLAYER_TALLEST", "EXPLANATION_MULTIPLAYER_TALLEST", "EXPLANATION_MODE_IMAGE_RACE", true, "EASY", null, null, text));
			instance.AddExplanation("MULTIPLAYER_TALLEST_NORMAL", new ExplanationStruct("EXPLANATION_TITLE_MULTIPLAYER_TALLEST", "EXPLANATION_MULTIPLAYER_TALLEST", "EXPLANATION_MODE_IMAGE_RACE", true, "NORMAL", null, null, null));
			instance.AddExplanation("MULTIPLAYER_TALLEST_PRO", new ExplanationStruct("EXPLANATION_TITLE_MULTIPLAYER_TALLEST", "EXPLANATION_MULTIPLAYER_TALLEST", "EXPLANATION_MODE_IMAGE_RACE", true, "PRO", null, null, null));
			instance.AddExplanationOverview(new ExplanationOverviewModel("EXPLANATION_TITLE_MULTIPLAYER_TALLEST", "EXPLANATION_MULTIPLAYER_TALLEST", "EXPLANATION_MODE_IMAGE_RACE", true, null, null, text, null, null, null, null, null, null));

			// New Crazy mode explanations
			instance.AddExplanation("MULTIPLAYER_RACE_CRAZY", new ExplanationStruct("EXPLANATION_TITLE_MULTIPLAYER_RACE", "EXPLANATION_MULTIPLAYER_RACE", "EXPLANATION_MODE_IMAGE_RACE", true, "CRAZY", "EXPLANATION_MULTIPLAYER_RACE_CRAZY_SUBTITLE", null, null));
			instance.AddExplanation("MULTIPLAYER_PUZZLE_CRAZY", new ExplanationStruct("EXPLANATION_TITLE_MULTIPLAYER_PUZZLE", "EXPLANATION_MULTIPLAYER_PUZZLE", "EXPLANATION_MODE_IMAGE_PUZZLE", true, "CRAZY", null, null, null));
			instance.AddExplanation("MULTIPLAYER_TALLEST_CRAZY", new ExplanationStruct("EXPLANATION_TITLE_MULTIPLAYER_TALLEST", "EXPLANATION_MULTIPLAYER_TALLEST", "EXPLANATION_MODE_IMAGE_RACE", true, "CRAZY", null, null, null));
			instance.AddExplanation("MULTIPLAYER_SURVIVAL_CRAZY", new ExplanationStruct("EXPLANATION_TITLE_MULTIPLAYER_SURVIVAL", "EXPLANATION_MULTIPLAYER_SURVIVAL", "EXPLANATION_MODE_IMAGE_SURVIVAL", true, "CRAZY", null, null, null));

			// New Heroic mode explanations
			instance.AddExplanation("MULTIPLAYER_RACE_HEROIC", new ExplanationStruct("EXPLANATION_TITLE_MULTIPLAYER_RACE", "EXPLANATION_MULTIPLAYER_RACE", "EXPLANATION_MODE_IMAGE_RACE", true, "HEROIC", "EXPLANATION_MULTIPLAYER_RACE_CRAZY_SUBTITLE", null, null));
			instance.AddExplanation("MULTIPLAYER_PUZZLE_HEROIC", new ExplanationStruct("EXPLANATION_TITLE_MULTIPLAYER_PUZZLE", "EXPLANATION_MULTIPLAYER_PUZZLE", "EXPLANATION_MODE_IMAGE_PUZZLE", true, "HEROIC", null, null, null));
			instance.AddExplanation("MULTIPLAYER_TALLEST_HEROIC", new ExplanationStruct("EXPLANATION_TITLE_MULTIPLAYER_TALLEST", "EXPLANATION_MULTIPLAYER_TALLEST", "EXPLANATION_MODE_IMAGE_RACE", true, "HEROIC", null, null, null));
			instance.AddExplanation("MULTIPLAYER_SURVIVAL_HEROIC", new ExplanationStruct("EXPLANATION_TITLE_MULTIPLAYER_SURVIVAL", "EXPLANATION_MULTIPLAYER_SURVIVAL", "EXPLANATION_MODE_IMAGE_SURVIVAL", true, "HEROIC", null, null, null));

			return true;
		}
	}
}
