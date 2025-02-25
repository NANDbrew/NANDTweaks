﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Patches
{
    internal class SaveLoadPatches
    {
        [HarmonyPatch(typeof(SaveLoadManager))]
        private static class SaveLoadManagerPatches
        {
            [HarmonyPatch("Awake")]
            [HarmonyPostfix]
            public static void AwakePatch(SaveLoadManager __instance)
            {
                __instance.gameObject.AddComponent<Shotter3>();
            }

            [HarmonyPatch("DoSaveGame")]
            [HarmonyPostfix]
            public static void SavePatch(SaveLoadManager __instance)
            {
                if (Plugin.saveLoadThumbs.Value)
                {
                    try 
                    {
                        __instance.GetComponent<Shotter3>().SaveThumbnail(SaveSlots.GetCurrentSavePath());
                    }
                    catch 
                    { 
                        Debug.LogError("NANDTweaks couldn't save thumbnail"); 
                    }
                }
            }

            // Save/load ship state
            [HarmonyPatch("LoadModData")]
            [HarmonyPostfix]
            public static void LoadModDataPatch(SaveLoadManager __instance)
            {
                if (!Plugin.saveLoadState.Value) return;
                __instance.StartCoroutine(Scripts.SaveLoader.LoadAfterDelay());
            }
            [HarmonyPatch("SaveModData")]
            [HarmonyPrefix]
            public static void SaveModDataPatch(SaveLoadManager __instance)
            {
                if (!Plugin.saveLoadState.Value) return;
                foreach (BoatRefs gameObject in BoatListCreator.boatList)
                {
                    if (gameObject.GetComponent<PurchasableBoat>().isPurchased())
                    {
                        Scripts.SaveLoader.SaveSailConfig(gameObject);
                    }
                }
            }
        }
    }
}
