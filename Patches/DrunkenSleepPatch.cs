using System;
using HarmonyLib;
using UnityEngine;

namespace RahasQOL
{
    // Token: 0x02000009 RID: 9
    [HarmonyPatch(typeof(PlayerNeeds), "LateUpdate")]
    internal static class DrunkSleepPatch
    {
        // Token: 0x06000010 RID: 16 RVA: 0x00002340 File Offset: 0x00000540
        private static void Postfix(bool ___godMode)
        {
            if (NANDTweaks.Plugin.drunkenSleep.Value == false || ___godMode) return;
            if (GameState.sleeping)
            {
                PlayerNeeds.sleep -= Time.deltaTime * Sun.sun.timescale * 15f * (PlayerNeeds.alcohol / 100f);
                if (PlayerNeeds.sleep > 100f)
                {
                    PlayerNeeds.sleep = 100f;
                }
                if (PlayerNeeds.sleep <= 0f)
                {
                    PlayerNeeds.sleepDebt += PlayerNeeds.sleep * 0.5f;
                    PlayerNeeds.sleep = 0f;
                }
                if (PlayerNeeds.sleepDebt < 0f)
                {
                    PlayerNeeds.sleepDebt = 0f;
                }
                if (PlayerNeeds.sleepDebt > 100f)
                {
                    PlayerNeeds.sleepDebt = 100f;
                }
            }
        }
    }
}
