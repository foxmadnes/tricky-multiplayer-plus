using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

/*
 * This patch replaces some existing images with new mod images after they're loaded.
 */
namespace TrickyMultiplayerPlus
{
    [HarmonyPatch(typeof(ResourceManager))]
    [HarmonyPatch(MethodType.Normal)]
    [HarmonyPatch("InstantiateByName")]
    [HarmonyPatch(new Type[] {typeof(string), typeof(Vector3), typeof(GameObject) })]
    public class ResourceManagerSubstitutionsPatch
    {

        public static void Postfix(string resourceName, ref GameObject __result)
        {
            if (resourceName == "MODE_SELECT_MODE_ITEM_TALLEST")
            {
                replaceSprite("assets/tallest/gph_menuModeTallest.png", __result);
            }
            else if (resourceName == "MODE_SELECT_DIFFICULTY_ITEM_CRAZY")
            {
                replaceDifficultySprite("assets/difficulties/gph_difficultyCrazy.png", __result);
            }
            else if (resourceName == "MODE_SELECT_DIFFICULTY_ITEM_HEROIC")
            {
                replaceDifficultySprite("assets/difficulties/gph_difficultyHeroic.png", __result);
            }
        }

        private static void replaceDifficultySprite(string newImagePath, GameObject __result)
        {

            Debug.Log("Trying to get image of " + __result);
            Image[] images = __result.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                if (image.name == "Difficulty")
                {
                    AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Application.dataPath), "BepInEx/plugins/TrickyMultiplayerPlus/trickymultiplayerplus"));
                    if (myLoadedAssetBundle == null)
                    {
                        Debug.Log("Failed to load AssetBundle!");
                        return;
                    }

                    Sprite overrideSprite = myLoadedAssetBundle.LoadAsset<Sprite>(newImagePath);

                    Debug.Log(image == null);
                    Debug.Log(overrideSprite == null);
                    image.overrideSprite = overrideSprite;

                    myLoadedAssetBundle.Unload(false);
                }
            }

        }

        private static void replaceSprite(string newImagePath, GameObject __result)
        {
            AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Application.dataPath), "BepInEx/plugins/TrickyMultiplayerPlus/trickymultiplayerplus"));
            if (myLoadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                return;
            }

            Sprite overrideSprite = myLoadedAssetBundle.LoadAsset<Sprite>(newImagePath);
            Debug.Log("Trying to get image of " + __result);
            Image image = __result.GetComponentInChildren<Image>();
            Debug.Log(image == null);
            Debug.Log(overrideSprite == null);
            image.overrideSprite = overrideSprite;

            myLoadedAssetBundle.Unload(false);
        }
    }
}
