using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NANDTweaks.Patches
{
    internal static class ElixirPatches
    {


        [HarmonyPatch(typeof(ShipItem), "UpdateLookText")]
        private static class UpdateLookTextPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ShipItem __instance)
            {
                if (!Plugin.elixirText.Value) return;
                if (__instance.sold && __instance is ShipItemElixir)
                {
                    __instance.description = __instance.name;
                }

            }
        }

        [HarmonyPatch(typeof(ShipItemElixir), "OnAltActivate")]
        private static class OnAltActivatePatch
        {
            [HarmonyPrefix]
            public static void Prefix(ShipItemElixir __instance)
            {
                if (__instance.sold)
                {
                    Refs.playerMouthCol.PlayDrinkSound();
                }

            }
        }
    }
}
