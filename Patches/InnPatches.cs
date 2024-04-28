using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(Tavern))]
    internal static class InnPatches
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void Postfix(Tavern __instance) 
        {
            __instance.gameObject.AddComponent<InteriorEffectsTrigger>();

        }
    }
}
