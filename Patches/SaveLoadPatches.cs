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
            public static void Postfix(SaveLoadManager __instance)
            {
                if (Plugin.saveLoadThumbs.Value)
                {
                    __instance.gameObject.AddComponent<Shotter3>();
                }
            }

            [HarmonyPatch("DoSaveGame")]
            [HarmonyPrefix]
            public static void Prefix(SaveLoadManager __instance)
            {
                if (__instance.GetComponent<Shotter3>() is Shotter3 shotter)
                {
                    shotter.SaveThumbnail(SaveSlots.GetCurrentSavePath());
                }
            }
        }
    }
}
