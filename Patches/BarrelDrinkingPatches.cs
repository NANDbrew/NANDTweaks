using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NANDTweaks.Patches
{
    internal class BarrelDrinkingPatches
    {
        [HarmonyPatch(typeof(ShipItemBottle))]
        private static class BarrelDrinkPatches
        {
            [HarmonyPatch("ExtraLateUpdate")]
            public static void Postfix(ref bool ___big, bool ___wasDrinking, bool ___drinking, float ___capacity)
            {
                if (!___drinking && !___wasDrinking)
                {
                    if (___capacity > 10f)
                    {
                        ___big = true;
                    }
                }
            }
        }
    }
}
