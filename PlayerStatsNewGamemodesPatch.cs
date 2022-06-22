using System.Collections.Generic;
using HarmonyLib;

namespace TrickyMultiplayerPlus
{
    [HarmonyPatch(typeof(PlayerStats))]
	[HarmonyPatch(MethodType.Constructor)]
	class AddNewModesToPlayerStatsPatch
	{
		static AccessTools.FieldRef<PlayerStats, Dictionary<string, GamePlayStats>> localGameStatsByTypeRef =
	AccessTools.FieldRefAccess<PlayerStats, Dictionary<string, GamePlayStats>>("_gameStatsByType");

		public static void Postfix(PlayerStats __instance) 
        {
			UnityEngine.Debug.Log("Patching PlayerStats Constructor.");
			// Patch in Tallest stats
			foreach (string difficulty in PlayerStats.difficulties)
			{
				localGameStatsByTypeRef(__instance).Add("GP_" + "T" + difficulty.Substring(0, 1), new GamePlayStats());
			}

			// Create a list of all game modes including new ones
			List<string> allGameModes = new List<string>(PlayerStats.gameModes);
			allGameModes.Add("TALLEST");

			List<string> newDifficulties = new List<string>();
			newDifficulties.Add("HEROIC");
			newDifficulties.Add("CRAZY");

			foreach (string world in allGameModes)
			{
				foreach (string difficulty in newDifficulties)
				{
					localGameStatsByTypeRef(__instance).Add(HelperMethods.GetGamePlayStatsID(world, difficulty), new GamePlayStats());
				}
			}
		}
	}

	public class HelperMethods
    {
		public static string GetGamePlayStatsID(string world, string difficulty)
		{
			return "GP_" + world.Substring(0, 1) + difficulty.Substring(0, 1);
		}
	}
}
