using HarmonyLib;
using NANDTweaks.Scripts;
using UnityEngine;

namespace NANDTweaks
{
    internal class MissionGoodsPatches
    {

        [HarmonyPatch(typeof(Good))]
        private static class TextureReplacer
        {
            [HarmonyPatch("RegisterToMission")]
            [HarmonyPostfix]
            private static void Postfix(Good __instance)
            {
                if (Plugin.cargoDecal.Value == DecalType.None) return;
                //if (MatLoader.bannerTex == null) return;
                try
                {
                    MatLoader.UpdateColor();
                    var label = GameObject.Instantiate(AssetTools.prefab.transform.Find(__instance.sizeDescription), __instance.transform, false);
                    int labelIndex = (int)PortRegion.none;
                    if (Plugin.cargoDecal.Value == DecalType.Origin)
                    {
                        PortRegion reg = __instance.GetAssignedMission().originPort.region;
                        Port port = __instance.GetAssignedMission().originPort;
                        labelIndex = (int)reg;
                        if (reg == PortRegion.medi && port.portIndex == 21) labelIndex = 5;
                        else if (reg == PortRegion.emerald && port.localMap == LocalMap.lagoon) labelIndex = 4;
                    }
                    label.GetComponent<Renderer>().material = Scripts.MatLoader.missionLabels[labelIndex];
                }
                catch { Debug.LogError("NANDTweaks: failed to add mission decal to " + __instance.name); }

            }
        }
    }
}
