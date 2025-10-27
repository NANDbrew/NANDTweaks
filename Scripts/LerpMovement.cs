using NANDTweaks.Patches;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Scripts
{
    public static class LerpMovement
    {
        public static IEnumerator HackPlayerPos(Transform player, Transform target, Vector3 targetOffset, float lerpSpeed)
        {
            LadderPatch.animating = true;
            Vector3 start = player.position;
            for (float t = 0f; t < 1f; t += Time.deltaTime * lerpSpeed)
            {
                player.position = Vector3.Lerp(start, target.position + targetOffset, t);
                yield return new WaitForEndOfFrame();
            }
            Plugin.logSource.Log(BepInEx.Logging.LogLevel.Debug, "Player ending position = " + target.position);
            LadderPatch.animating = false;
        }
        public static IEnumerator HackPlayerPosLocal(Transform player, Vector3 targetOffset, float lerpSpeed)
        {
            LadderPatch.animating = true;
            Vector3 start = player.localPosition;
            for (float t = 0f; t < 1f; t += Time.deltaTime * lerpSpeed)
            {
                player.localPosition = Vector3.Lerp(start, targetOffset, t);
                yield return new WaitForEndOfFrame();
            }
            Plugin.logSource.Log(BepInEx.Logging.LogLevel.Debug, "Player ending position = " + player.localPosition);
            LadderPatch.animating = false;
        }

    }
}
