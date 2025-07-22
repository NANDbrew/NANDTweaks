using HarmonyLib;
using UnityEngine;

namespace NANDTweaks.Patches
{
    internal class PortOfficePatches
    {
        internal static readonly GameObject refTrigger = new GameObject { name = "interior trigger", layer = 2 };
        static bool initialized = false;
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
            if (!initialized)
            {
                var trigger = refTrigger.AddComponent<InteriorEffectsTrigger>();
                trigger.doors = new GPButtonTrapdoor[0];
                trigger.semiIndoor = true;
                refTrigger.AddComponent<BoxCollider>().isTrigger = true;
                initialized = true;
            }
            if (PortOfficeTriggers.colSizes[index] == Vector3.zero)
            {
                return;
            }
            GameObject interiorTrigger = UnityEngine.Object.Instantiate(refTrigger, parent.position, PortOfficeTriggers.triggerRotations[index], parent);
            interiorTrigger.name = "port interior trigger " + index;

            interiorTrigger.transform.localPosition = PortOfficeTriggers.triggerLocs[index];
            interiorTrigger.GetComponent<BoxCollider>().size = PortOfficeTriggers.colSizes[index];
        }
    }
}
