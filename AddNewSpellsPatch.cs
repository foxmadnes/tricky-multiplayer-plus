using System;
using HarmonyLib;
using System.Reflection;

namespace TrickyMultiplayerPlus
{
    [HarmonyPatch(typeof(AbstractInitSpellsCommand))]
	[HarmonyPatch("Execute")]
	class AddNewSpellsPatch
	{

		static bool Prefix(AbstractInitSpellsCommand __instance)
		{
			MethodInfo methodInfo = typeof(AbstractInitSpellsCommand).GetMethod("_AddLightCurrentBrickSpell", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(string), typeof(string), typeof(AbstractEffect[]) }, null);
			var parameters = new object[] {"LARGE_SELF", "LARGE_SPELL_ICON", new AbstractEffect[]
				{
				new GetCurrentBrickEffect(),
				new ApplyModifierEffect(new LargeBrickModifier(), true, false, null)
				}
			};
			methodInfo.Invoke(__instance, parameters);
			return true;
		}
	}
}
