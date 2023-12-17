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
/*            [HarmonyPostfix]
            [HarmonyPatch("CurrentBoatIsMoored")]
            private static void Patch1(ref bool __result)
            {
                Anchor anchor = GameState.currentBoat.parent.GetComponent<BoatMooringRopes>().GetAnchorController().joint.gameObject.GetComponent<Anchor>();
                if (GameState.currentBoat && anchor.IsSet())
                {
                    SailwindModdingHelper.ModLogger.Log(Main.mod, "found a thing!");
                    __result = true;
                }
            }*/
            [HarmonyPrefix]
            [HarmonyPatch("FallAsleep")]
            private static void Patch2(ref float ___sleepTimescale)
            {
                Anchor anchor = GameState.currentBoat.parent.GetComponent<BoatMooringRopes>().GetAnchorController().joint.gameObject.GetComponent<Anchor>();
                if (GameState.currentBoat && anchor.IsSet())
                {
                    Plugin.logSource.LogInfo("set sleepTimeScale from " + ___sleepTimescale + " to 24");
                    ___sleepTimescale = 24f;
                }
                else ___sleepTimescale = 16f;
            }
        }
    }
}
