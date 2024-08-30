using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cakeslice;

namespace NANDTweaks.Patches
{
    // made by Seelöwe back in 2022
    [HarmonyPatch(typeof(GoPointerButton), "UpdateColor")]
    [HarmonyPriority(700)]
    internal static class NoOutlinesPatch
    {
        public static bool Prefix(ref Outline ___outline, bool ___overrideEnableOutline)
        {
            if (!Plugin.noOutlines.Value)
            {
                return true;
            }
            if (___overrideEnableOutline)
            {
                ___outline.color = 1;
                ___outline.enabled = true;
                return false;
            }
            ___outline.color = 2;
            ___outline.enabled = false;
            return false;
        }
    }
}
