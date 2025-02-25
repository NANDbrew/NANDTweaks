﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Patches
{
    internal class PortOfficePatches
    {
        private static readonly GameObject refTrigger = new GameObject { name = "port interior trigger" };

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

                    if (portIndex == 0) AddInteriorTrigger(__instance.transform, 30);
                    if (portIndex == 15) AddInteriorTrigger(__instance.transform, 31);
                    if (portIndex == 21) AddInteriorTrigger(__instance.transform, 32);
                    if (portIndex == 25) AddInteriorTrigger(__instance.transform, 33);
                    //if (portIndex == 13) AddInteriorTrigger(__instance.transform, 34);
                }
            }
        }

        public static void AddInteriorTrigger(Transform parent, int index)
        {
            GameObject interiorTrigger = UnityEngine.Object.Instantiate(refTrigger, parent.position, ResourceRefs.triggerRotations[index], parent);
            interiorTrigger.name += " " + index;
            interiorTrigger.AddComponent<InteriorEffectsTrigger>().doors = new GPButtonTrapdoor[0];
            interiorTrigger.transform.localPosition = ResourceRefs.triggerLocs[index];
            BoxCollider bcol = interiorTrigger.AddComponent<BoxCollider>();
            bcol.size = ResourceRefs.colSizes[index];
            bcol.isTrigger = true;
        }
    }
}
