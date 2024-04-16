using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Patches
{
    internal static class SleepPatches
    {
        [HarmonyPatch(typeof(Sleep))]
        private static class SleepPatch
        {
            static float defaultSleepTimeScale;

            [HarmonyPostfix]
            [HarmonyPatch("Awake")]
            private static void Patch1(float ___sleepTimescale)
            {
                defaultSleepTimeScale = ___sleepTimescale;
            }

            [HarmonyPostfix]
            [HarmonyPatch("EnterBed")]
            private static void Patch2(Sleep __instance, ref float ___sleepTimescale)
            {
                if (!Plugin.anchorSleep.Value) return;
                if (GameState.currentBoat)
                {                
                    Anchor anchor = GameState.currentBoat.parent.GetComponent<BoatMooringRopes>().GetAnchorController().joint.gameObject.GetComponent<Anchor>();

                    if (anchor.IsSet())
                    {
                        Plugin.logSource.LogInfo("set sleepTimeScale from " + ___sleepTimescale + " to 24");
                        ___sleepTimescale = 24f;

                    }
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("LeaveBed")]
            private static void Patch3(ref float ___sleepTimescale)
            {
                if (!Plugin.anchorSleep.Value) return;

                ___sleepTimescale = defaultSleepTimeScale;

            }

            [HarmonyPostfix]
            [HarmonyPatch("CurrentBoatIsMoored")]
            private static void Patch4(ref bool __result)
            {
                if (!Plugin.anchorSleep.Value) return;

                if (GameState.currentBoat) return;

                __result = true;
                

            }
        }
    }
}
