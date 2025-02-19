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
            if (!Plugin.camPatches.Value) return;
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
    [HarmonyPatch(typeof(BoatCamera), "UpdateZoom")]
    internal class ZoomPatch
    {
        static float maxHeight = 15f;
        static float minHeight = 5f;
        static float currentLimit;
        const float minZoom = -8f;
        public static bool Prefix(BoatCamera __instance, ref float ___camHeight, ref float ___zoomLevel, ref float ___zoomSpeed)
        {
            if (!Plugin.camPatches.Value) return true;
            if (GameState.currentShipyard) return true;
            float boatMass = (float)Traverse.Create(GameState.currentBoat.parent.GetComponent<BoatMass>()).Field("selfMass").GetValue();
            // get current boat's position in mass range, and smash mass range down to between 0 and 1
            currentLimit = Mathf.InverseLerp(800, 15000, boatMass);
            // set height range of cam target based on mass's position within range
            minHeight = Mathf.Lerp(3f, 6, currentLimit);
            maxHeight = Mathf.Lerp(6f, 15f, currentLimit);
            // stretch mass range out to zoom limits (-30 is max out for small boats, -60 for large)
            currentLimit = Mathf.Lerp(-30, -60, currentLimit);
            ___zoomSpeed = GameInput.GetKey(InputName.Run) ? 20f : 10f;
            ___zoomLevel = Mathf.Clamp(___zoomLevel + GameInput.GetScrollAxis() * ___zoomSpeed, currentLimit, minZoom);

            float zoom = Mathf.InverseLerp(minZoom, currentLimit, ___zoomLevel);
            ___camHeight = Mathf.Lerp(minHeight, maxHeight, zoom);

            return false;
        }
    }
}
