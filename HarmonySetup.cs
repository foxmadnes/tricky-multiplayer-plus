using BepInEx;
using HarmonyLib;

namespace TrickyMultiplayerPlus
{

    [BepInPlugin("trickymultiplayerplus", "TrickyMultiplayerPlus", "1.0")]
	public class TrickyTowersModPlugin : BaseUnityPlugin
	{

		void Awake()
		{
			var harmony = new Harmony("trickymultiplayerplus");
			harmony.PatchAll();
		}
	}
}