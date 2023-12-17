/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(IslandMarketWarehouseArea), "IsGoodValid")]
    internal static class MushroomCratePatches
    {
        [HarmonyPostfix]
        public static void MushroomCrateAmountPatch(Good good, ref bool __result)
        {
            if (good.name.Contains("forest mushroom") && good.GetComponent<ShipItemCrate>().amount >= 24)
            {
                __result = true;
            }
        }
    }
}
*/