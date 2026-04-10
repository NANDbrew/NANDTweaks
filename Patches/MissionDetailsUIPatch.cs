using HarmonyLib;
using UnityEngine;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(MissionDetailsUI))]
    internal class MissionDetailsUIPatch
    {
        [HarmonyPatch("UpdateMap")]
        [HarmonyPostfix]
        public static void Postfix(TextMesh ___locationText, Texture ___oceanMap, Mission ___currentMission, Transform ___destinationMarker, LineRenderer ___routeLine, Renderer ___mapRenderer)
        {
            if (!Plugin.offMapMissionPatch.Value) return;
            if (___currentMission.UseOceanMap() && !___currentMission.originPort.oceanMapLocation && ___currentMission.destinationPort.oceanMapLocation)
            {
                ___mapRenderer.gameObject.SetActive(value: true);
                ___mapRenderer.material.SetTexture("_EmissionMap", ___oceanMap);
                ___mapRenderer.material.SetTexture("_MainTex", ___oceanMap);
                ___routeLine.gameObject.SetActive(false);//.SetPosition(0, ___currentMission.originPort.localMapLocation.localPosition);
                ___locationText.text = "";                            //___routeLine.SetPosition(1, ___currentMission.destinationPort.localMapLocation.localPosition);
                ___destinationMarker.localPosition = ___currentMission.destinationPort.oceanMapLocation.localPosition;
            }
            else
            {
                ___routeLine.gameObject.SetActive(true);
            }
        }

    }
}
