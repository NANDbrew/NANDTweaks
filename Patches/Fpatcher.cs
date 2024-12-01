using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(StartMenu))]
    internal static class FPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("LoadGame")]
        public static void LoadPatch(StartMenu __instance, ref bool ___fPressed)
        {
            if (Plugin.skipDisclaimer.Value)
            {
                ___fPressed = true;
            }
            if (Plugin.hideLoading.Value)
            {
                __instance.StartCoroutine(FPatchWaiter(__instance));
            }
        }

        public static IEnumerator FPatchWaiter(MonoBehaviour source)
        {
            OVRScreenFade fade = Camera.main.GetComponent<OVRScreenFade>();
            yield return new WaitForSeconds(1);
            fade.SetFadeLevel(1f);
 /*           while (GameState.currentlyLoading)
            {
                yield return new WaitForEndOfFrame();
            }*/

            Debug.Log("waiting for trigger");
            yield return new WaitUntil(() => GameState.justStarted);
            while (GameState.justStarted)
            {
                fade.SetFadeLevel(1f);
                if (Sleep.instance.recoveryText.text.Length == 0) Sleep.instance.recoveryText.text = "\n\n\nsettling...";
                yield return new WaitForEndOfFrame();
            }
            Sleep.instance.recoveryText.text = "";
            for (float t = 0f; t < 1f; t += Time.deltaTime)
            {
                float fadeLevel = Mathf.Lerp(1f, 0, t / 1f);
                fade.SetFadeLevel(fadeLevel);
                yield return new WaitForEndOfFrame();
            }

            fade.SetFadeLevel(0f);
            Debug.Log("did the wazooo");
        }

    }
}
