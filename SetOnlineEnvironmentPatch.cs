using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;

namespace TrickyTowersMod
{
    [HarmonyPatch(typeof(GameSettings))]
    [HarmonyPatch(MethodType.Constructor)]
    class SetOnlineEnvironmentPatch
    {
        static AccessTools.FieldRef<GameSettings, Dictionary<string, Dictionary<string, object>>> _settingsRef =
            AccessTools.FieldRefAccess<GameSettings, Dictionary<string, Dictionary<string, object>>>("_settings");
         

        public static void Postfix(GameSettings __instance) 
        {
            Dictionary<string, object> dictionary = _settingsRef(__instance)["DEFAULT"];
            dictionary.Add("_NETWORK_ENVIRONMENT", "Tricky Towers " + "TrickTowerMod " + TrickyTowersModPlugin.modVersion);
        }

    }
}
