using HarmonyLib;
using MonoMod.Core.Utils;
using System.Linq;
using UnityEngine;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(Tavern))]
    internal static class InnPatches
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void Postfix(Tavern __instance) 
        {
            int index = __instance.transform.root.GetComponent<IslandSceneryScene>().parentIslandIndex;
            if (InnTriggers.triggerLocs.Keys.Contains(index))
            {
                AddInteriorTrigger(__instance.transform, index);
            }
            else
            {
                var trigger = __instance.gameObject.AddComponent<InteriorEffectsTrigger>();
                trigger.doors = new GPButtonTrapdoor[0];
                trigger.semiIndoor = true;
            }
        }
        public static void AddInteriorTrigger(Transform parent, int index)
        {
            GameObject interiorTrigger = UnityEngine.Object.Instantiate(PortOfficePatches.refTrigger, parent);
            interiorTrigger.name = "inn interior trigger " + index;
            interiorTrigger.transform.localPosition = InnTriggers.triggerLocs[index];
            interiorTrigger.transform.localRotation = InnTriggers.triggerRotations[index];
            interiorTrigger.GetComponent<BoxCollider>().size = InnTriggers.colSizes[index];
        }
    }
}
