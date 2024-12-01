using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(PlayerEmbarkDisembarkTrigger), "Update")]
    internal static class PlayerEmbarkPatch
    {
        public static bool Prefix(bool __runOriginal)
        {
            if (Plugin.ladderPatch.Value && (LadderPatch.animating || !__runOriginal)) return false;
            return true;

        }

    }
    [HarmonyPatch(typeof(BoatLadder), "OnActivate")]
    internal static class LadderPatch
    {
        public static bool animating;
        public static bool Prefix(BoatLadder __instance, float ___upDistance)
        {
            if (!Plugin.ladderPatch.Value) return true;

            PlayerEmbarkDisembarkTrigger player = Refs.observerMirror.GetComponentInChildren<PlayerEmbarkDisembarkTrigger>();
            Transform transform = player.playerController.transform;
            BoatRefs boatRefs = __instance.GetComponentInParent<BoatRefs>();

            if (transform.parent == boatRefs.walkCol) return false;

            //float forwardNum = Plugin.embarkDist.Value;
            //if (__instance.transform.localPosition.z > 0) forwardNum = -forwardNum;
            //Vector3 offset = new Vector3(0, ___upDistance, forwardNum);

            float forwardNum = Vector3.Dot(__instance.transform.up, boatRefs.boatModel.transform.position - __instance.transform.position) < 0? -Plugin.embarkDist.Value : Plugin.embarkDist.Value;//__instance.transform.forward;
            //Debug.Log("forward num = " + forwardNum);
            
            Vector3 vector = __instance.transform.position + Vector3.up * Plugin.embarkHeight.Value + __instance.transform.up * forwardNum;
            Vector3 targetPos = boatRefs.boatModel.InverseTransformPoint(vector);

            if (PlayerEmbarkDisembarkTrigger.embarked)
            {
                AccessTools.Method(player.GetType(), "ExitBoat").Invoke(player, null);
            }
            AccessTools.Method(player.GetType(), "EnterBoat").Invoke(player, new object[] { boatRefs.boatModel, boatRefs.walkCol } );
            Refs.charController.enabled = false;
            __instance.StartCoroutine(Scripts.LerpMovement.HackPlayerPosLocal(transform, targetPos, 4f));
            Refs.charController.enabled = true;
            return false;
        }
    }
}
