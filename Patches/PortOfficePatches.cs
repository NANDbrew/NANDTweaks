using HarmonyLib;
using UnityEngine;

namespace NANDTweaks.Patches
{
    internal class PortOfficePatches
    {
        private static readonly GameObject refTrigger = new GameObject { name = "port interior trigger", layer = 2 };

        [HarmonyPatch(typeof(PortDude), "Awake")]
        private static class PortDudePatch
        {
            [HarmonyPostfix]
            public static void AddInteriorEffectsTrigger(PortDude __instance, Port ___port)
            {
                int portIndex = ___port.portIndex;
                // exclude outdoor offices
                if (portIndex != 5 && portIndex != 24)
                {
                    AddInteriorTrigger(__instance.transform, portIndex);

                    if (portIndex == 0) AddInteriorTrigger(__instance.transform, 38);
                    if (portIndex == 15) AddInteriorTrigger(__instance.transform, 39);
                    if (portIndex == 21) AddInteriorTrigger(__instance.transform, 40);
                    if (portIndex == 25) AddInteriorTrigger(__instance.transform, 41);
                    //if (portIndex == 13) AddInteriorTrigger(__instance.transform, 34);
                }
            }
        }

        public static void AddInteriorTrigger(Transform parent, int index)
        {
            if (ResourceRefs.colSizes[index] == Vector3.zero)
            {
                return;
            }
            GameObject interiorTrigger = UnityEngine.Object.Instantiate(refTrigger, parent.position, ResourceRefs.triggerRotations[index], parent);
            interiorTrigger.name = "port interior trigger " + index;
            var trigger = interiorTrigger.AddComponent<InteriorEffectsTrigger>();
            trigger.doors = new GPButtonTrapdoor[0];
            trigger.semiIndoor = true;
            interiorTrigger.transform.localPosition = ResourceRefs.triggerLocs[index];
            BoxCollider bcol = interiorTrigger.AddComponent<BoxCollider>();
            bcol.size = ResourceRefs.colSizes[index];
            bcol.isTrigger = true;
        }
    }
}
