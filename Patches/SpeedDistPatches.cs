using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks
{
    internal class SpeedDistPatches
    {
        [HarmonyPatch(typeof(ShipItemChipLog), "OnAltActivate")]
        private static class ChipLogSwitcher
        {
            public static void Postfix(ShipItemChipLog __instance, Rigidbody ___bobberBody)
            {
                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    GameObject notches_A = __instance.transform.GetChild(0).GetChild(0).Find("notches_000").gameObject;
                    GameObject notches_M = __instance.transform.GetChild(0).GetChild(2).Find("notches_001").gameObject;
                    GameObject notches_E = __instance.transform.GetChild(0).GetChild(1).Find("notches_002").gameObject;
                    if (notches_M.activeSelf || notches_E.activeSelf)
                    {
                        notches_A.SetActive(false);
                        notches_E.SetActive(false);
                        notches_M.SetActive(false);
                        if (Plugin.milesPerDegree.Value == 60) ___bobberBody.GetComponent<ChipLogRopeEnd>().SetPrivateField("callibrationMult", 24);
                        else if (Plugin.milesPerDegree.Value == 90) ___bobberBody.GetComponent<ChipLogRopeEnd>().SetPrivateField("callibrationMult", 36);
                        else if (Plugin.milesPerDegree.Value == 140)  ___bobberBody.GetComponent<ChipLogRopeEnd>().SetPrivateField("callibrationMult", 28);
                         //36);
                    }
                    else
                    {
                        notches_A.SetActive(true);
                        notches_E.SetActive(true);
                        notches_M.SetActive(true);
                        ___bobberBody.GetComponent<ChipLogRopeEnd>().SetPrivateField("callibrationMult", 28);
                    }
                }
            }
        }
        [HarmonyPatch(typeof(MissionDetailsUI), "UpdateTexts")]
        private static class MissionMilesPatch
        {
            public static void Postfix(TextMesh ___distance, Mission ___currentMission, TextMesh ___goldPerMile)
            {
                if (Plugin.milesPerDegree.Value == 90) return;
                int dist;
                if (Plugin.milesPerDegree.Value == 60) dist = Mathf.RoundToInt(___currentMission.distance / 1.5f);
                else dist = Mathf.RoundToInt(___currentMission.distance * 1.555f);
                //else dist = Mathf.RoundToInt(___currentMission.distance);
                ___distance.text = "Distance: " + dist + " miles";
                ___goldPerMile.text = GetGoldPerMileRounded(___currentMission.pricePerKm) + " / mile";
            }
        }

        private static float GetGoldPerMileRounded(float pricePerKm)
        {
            if (Plugin.milesPerDegree.Value == 140) return (float)Math.Round(pricePerKm / 1.50f, 2);
            else if (Plugin.milesPerDegree.Value == 60) return (float)Math.Round(pricePerKm * 1.555f, 2);
            return  (float)Math.Round(pricePerKm, 2);           
        }
    }
}
