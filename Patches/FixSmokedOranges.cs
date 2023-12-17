using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using SailwindModdingHelper;

namespace TweaksAndFixes.Patches
{
    internal class FixSmokedOranges
    {
        [HarmonyPatch(typeof(ShipItemCrate), "OnLoad")]
        private static class OrangeCratePatch
        {
           [HarmonyPrefix]
           public static bool Prefix(ShipItemCrate __instance)
            {
                if (__instance.name.Contains("oranges"))
                {
                    __instance.smokedFood = false;
                    //__instance.name = "Bruh";
                    //ModLogger.Log(Main.mod,"We did a thing!");
                }
                //ModLogger.Log(Main.mod, "Did we do a thing?");

                return true;
            }

        }

    }
}
