using HarmonyLib;

/*
 * This match fixes a bug where the selected mode is used by Tricky Towers into the index of the selected difficulty list.
 * 
 * In addition, patching the location where the actual indexing happens triggers a Harmony/Monomod bug, so patching in the setter instead.
 * If the Harmony/Monomod bug is fixed, the transpiler code below can be used to use the correct local variable with minimal tweaking.
 */
namespace TrickyMultiplayerPlus
{
    [HarmonyPatch(typeof(GameModeSelectStateController))]
    [HarmonyPatch(MethodType.Normal)]
    [HarmonyPatch("Enable")]
    public class GameModeIndexPatch
    {

        static AccessTools.FieldRef<GameModeSelectStateController, ModeSelectStats> localModeSelectStatsRef =
    AccessTools.FieldRefAccess<GameModeSelectStateController, ModeSelectStats>("_modeSelectStats");

        public static void Postfix(GameModeSelectStateController __instance)
        {
            localModeSelectStatsRef(__instance).selectedMode = 0;
        }
    }
}
