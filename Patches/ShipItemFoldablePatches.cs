using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NANDTweaks.Patches
{
    internal class ShipItemFoldablePatches
    {
        [HarmonyPatch(typeof(ShipItemFoldable))]
        private static class FoldUnfoldPatch
        {
            [HarmonyPatch("Fold")]
            [HarmonyPostfix]
            public static void FoldPatch(ref float ___holdDistance)
            {
                ___holdDistance = 0.8f;

            }

            [HarmonyPatch("Unfold")]
            [HarmonyPostfix]
            public static void UnfoldPatch(ref float ___holdDistance)
            {
                ___holdDistance = 0.5f;

            }
        
        }
    }
}
