using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(StartMenu), "Start")]
    internal static class FPatcher
    {
        [HarmonyPostfix]
        public static void UpdatePatch(ref bool ___fPressed)
        {
            ___fPressed = true;

        }
    }

}
