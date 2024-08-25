using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SailwindModdingHelper;

namespace NANDTweaks.Patches
{
    internal class ShipItemScrollPatches
    {
        [HarmonyPatch(typeof(ShipItem), "OnAltActivate")]
        private static class OnAltActivatePatch
        {
            [HarmonyPostfix]
            public static void Postfix(ShipItem __instance, ref float ___holdDistance)
            {
                if (__instance is ShipItemScroll)
                {
                    if (___holdDistance > 0.4)
                    {
                        __instance.OnPickup();
                    }
                    else
                    {
                        __instance.OnDrop();
                    }

                }
            }
        }
        [HarmonyPatch(typeof(ShipItem), "Awake")]
        private static class AwakePatch
        {
            [HarmonyPostfix]
            public static void Postfix(ShipItem __instance)
            {
                if (__instance is ShipItemScroll scroll)
                {
                    scroll.InvokePrivateMethod("HideArrows");

                }
            }
        }

        [HarmonyPatch(typeof(ShipItemScroll))]
        private static class PickupDropPatch
        {
            [HarmonyPatch("OnPickup")]
            [HarmonyPostfix]
            public static void Postfix(ShipItemScroll __instance)
            {
                __instance.holdDistance = 0.35f;
            }
        
            [HarmonyPatch("OnDrop")]
            [HarmonyPrefix]
            public static void Prefix(ShipItemScroll __instance)
            {
                __instance.holdDistance = 0.8f;
            }
        }
    }
}
