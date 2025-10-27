using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using NANDTweaks;
using UnityEngine;

namespace SailwindTestbed
{
    [HarmonyPatch(typeof(OceanFishes), "Awake")]
    internal static class FishCenterAdder
    {
        internal static GameObject fishesRegion;
        internal static void Postfix(ref LocalFishesRegion[] ___localFishesRegions)
        {
            if (!Plugin.albacoreArea.Value) return;
            fishesRegion = new GameObject(name: "albacore_center");
            fishesRegion.transform.position = new Vector3(-36000f, 0f, -50000f);
            LocalFishesRegion fishes = fishesRegion.AddComponent<LocalFishesRegion>();
            fishes.overrideInfluence = 0.2f;
            fishes.outerRadius = 7000;
            fishes.innerRadius = 2000;
            ___localFishesRegions = ___localFishesRegions.AddToArray(fishes);
        }
    }
    [HarmonyPatch(typeof(PrefabsDirectory), "Start")]
    internal static class FishPatch
    {
        internal static void Postfix(GameObject[] ___directory)
        {
            if (!Plugin.albacoreArea.Value) return;
            FishCenterAdder.fishesRegion.transform.parent = FloatingOriginManager.instance.transform;
            FishCenterAdder.fishesRegion.GetComponent<LocalFishesRegion>().localFishPrefabs = new GameObject[] { ___directory[140] };
            //FishCenterAdder.fishesRegion.transform.localPosition = FloatingOriginManager.instance.RealPosToShiftingPos(new Vector3(-36000f, 0f, -50000f));
            Plugin.logSource.Log(BepInEx.Logging.LogLevel.Debug, "fishCenter pos = " + FloatingOriginManager.instance.GetGlobeCoords(FishCenterAdder.fishesRegion.transform));

        }
    }
}
