using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(BoatCamera), "Update")]
    internal class CameraPatch
    {
        public static void Postfix(BoatCamera __instance, ref float ___camHeight, ref Vector3 ___currentPosOffset)
        {
            if (!GameState.currentShipyard) return;
            if (Input.GetMouseButton(1))
            {
                ___currentPosOffset += __instance.transform.up * Input.GetAxis("Mouse Y");
            }

            if (___currentPosOffset.y > 15f)
            {
                ___currentPosOffset = new Vector3(___currentPosOffset.x, 15f, ___currentPosOffset.z);
            }

            if (___currentPosOffset.y < -5f)
            {
                ___currentPosOffset = new Vector3(___currentPosOffset.x, -5f, ___currentPosOffset.z);
            }
        }
    }
}
