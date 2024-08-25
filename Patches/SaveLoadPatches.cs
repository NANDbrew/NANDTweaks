using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NANDTweaks.Patches
{
    internal class SaveLoadPatches
    {
        [HarmonyPatch(typeof(SaveLoadManager))]
        private static class SaveLoadManagerPatches
        {
            [HarmonyPatch("Awake")]
            [HarmonyPostfix]
            public static void AwakePatch(SaveLoadManager __instance)
            {
                if (Plugin.saveLoadThumbs.Value)
                {
                    __instance.gameObject.AddComponent<Shotter3>();
                }
            }

            [HarmonyPatch("DoSaveGame")]
            [HarmonyPostfix]
            public static void SavePatch(SaveLoadManager __instance)
            {
                if (__instance.GetComponent<Shotter3>() is Shotter3 shotter)
                {
                    shotter.SaveThumbnail(SaveSlots.GetCurrentSavePath());
                }
            }
        }
    }
}
