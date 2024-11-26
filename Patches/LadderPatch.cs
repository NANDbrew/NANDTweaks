﻿using HarmonyLib;
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
        public static bool Prefix(BoatLadder __instance)
        {
            if (!Plugin.ladderPatch.Value) return true;

            PlayerEmbarkDisembarkTrigger player = Refs.observerMirror.GetComponentInChildren<PlayerEmbarkDisembarkTrigger>();
            Transform transform = player.playerController.transform;
            BoatEmbarkCollider embarkCol = __instance.GetComponentInParent<BoatRefs>().GetComponentInChildren<BoatEmbarkCollider>();

            if (transform.parent == embarkCol.walkCollider) return false;

            float forwardNum = Plugin.embarkDist.Value;
            if (__instance.transform.localPosition.z > 0) forwardNum = -forwardNum;
            Vector3 offset = new Vector3(0, 1.25f, forwardNum);

            if (PlayerEmbarkDisembarkTrigger.embarked)
            {
                AccessTools.Method(player.GetType(), "ExitBoat").Invoke(player, null);
            }
            AccessTools.Method(player.GetType(), "EnterBoat").Invoke(player, new object[] { embarkCol.transform.parent, embarkCol.walkCollider } );
            Refs.charController.enabled = false;
            __instance.StartCoroutine(Scripts.LerpMovement.HackPlayerPosLocal(transform, __instance.transform, offset, 4f));
            Refs.charController.enabled = true;
            return false;
        }
    }
}
