using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NANDTweaks.Scripts;
using UnityEngine;

namespace NANDTweaks.Patches
{
    internal static class ShipItemMiscPatches
    {
        [HarmonyPatch(typeof(ShipItem), "Awake")]
        private static class AwakePatch
        {
            [HarmonyPrefix]
            public static void Prefix(ShipItem __instance)
            {

                //__instance.gameObject.AddComponent<ShipItemInventory>();

                if (__instance.name.ToLower().Contains("map"))
                {
                    __instance.gameObject.AddComponent<ShipItemMoveOnAltActivate>().targetDistance = 1.15f;
                    __instance.gameObject.GetComponent<ShipItemMoveOnAltActivate>().targetHeight = -0.25f;
                    __instance.gameObject.AddComponent<ShipItemRotateOnAltActivate>().targetAngle = 75f;
                }
                else if (__instance is ShipItemScroll)
                {
                    __instance.gameObject.AddComponent<ShipItemMoveOnAltActivate>().targetDistance = 1.15f;
                    __instance.InvokePrivateMethod("HideArrows");
                }

                
            }
        }

        [HarmonyPatch(typeof(ShipItem), "OnDrop")]
        private static class OnDropPatch
        {
            [HarmonyPrefix]
            public static void Prefix(ShipItem __instance)
            {
                ResetMove(__instance);
                ResetRotate(__instance);
            }
        }

        [HarmonyPatch(typeof(ShipItem), "OnEnterInventory")]
        private static class OnEnterInventoryPatch
        {
            [HarmonyPrefix]
            public static void Prefix(ShipItem __instance)
            {

                ResetMove(__instance);
                ResetRotate(__instance);

            }
        }

        [HarmonyPatch(typeof(ShipItem), "OnAltActivate")]
        private static class OnAltActivatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ShipItem __instance)
            {

                Rotate(__instance);
                Move(__instance);
                if (__instance is ShipItemScroll instance)
                {
                    OpenClose(instance);
                }

                return true;
            }

            private static void OpenClose(ShipItemScroll instance)
            {
                MeshFilter filter = instance.GetComponent<MeshFilter>();
                Mesh closedMesh = instance.GetPrivateField<Mesh>("closedMesh");
                Mesh openMesh = instance.GetPrivateField<Mesh>("openMesh");
                if (filter.sharedMesh == closedMesh)
                {
                    instance.GetPrivateField<Renderer>("page").enabled = true;
                    UISoundPlayer.instance.PlayOpenSound();
                    instance.GetComponent<MeshFilter>().sharedMesh = openMesh;
                    instance.InvokePrivateMethod("UpdateArrows");
                }
                else if (filter.sharedMesh == openMesh)
                {
                    instance.GetPrivateField<Renderer>("page").enabled = false;
                    UISoundPlayer.instance.PlayCloseSound();
                    instance.GetComponent<MeshFilter>().sharedMesh = closedMesh;
                    instance.InvokePrivateMethod("HideArrows");
                }
            }

            private static void Rotate(ShipItem instance)
            {
                ShipItemRotateOnAltActivate shipRotateOnAltActivate = instance.GetComponent<ShipItemRotateOnAltActivate>();
                if (shipRotateOnAltActivate)
                {
                    if (!instance.sold || shipRotateOnAltActivate.rotating)
                    {
                        return;
                    }
                    shipRotateOnAltActivate.rotating = true;
                    if (instance.heldRotationOffset != 0)
                    {
                        instance.StartCoroutine(RotateShipItem(shipRotateOnAltActivate, 0f));
                        return;
                    }
                    instance.StartCoroutine(RotateShipItem(shipRotateOnAltActivate, shipRotateOnAltActivate.targetAngle));
                }
            }

            private static void Move(ShipItem instance)
            {
                ShipItemMoveOnAltActivate shipMoveOnAltActivate = instance.GetComponent<ShipItemMoveOnAltActivate>();
                if (shipMoveOnAltActivate)
                {
                    if (!instance.sold || shipMoveOnAltActivate.moving)
                    {
                        return;
                    }
                    shipMoveOnAltActivate.moving = true;
                    if (instance.holdDistance != shipMoveOnAltActivate.defaultDistance)
                    {
                        instance.StartCoroutine(MoveShipItem(shipMoveOnAltActivate, shipMoveOnAltActivate.defaultDistance));
                    }
                    else instance.StartCoroutine(MoveShipItem(shipMoveOnAltActivate, shipMoveOnAltActivate.targetDistance));
                    if (instance.holdHeight != shipMoveOnAltActivate.defaultHeight)
                    {
                        instance.StartCoroutine(MoveShipItem2(shipMoveOnAltActivate, shipMoveOnAltActivate.defaultHeight));
                    }
                    else instance.StartCoroutine(MoveShipItem2(shipMoveOnAltActivate, shipMoveOnAltActivate.targetHeight));
                    return;
                }
            }

            public static IEnumerator RotateShipItem(ShipItemRotateOnAltActivate shipRotateOnAltActivate, float target)
            {
                float start = shipRotateOnAltActivate.shipItem.heldRotationOffset;
                for (float t = 0f; t < 1f; t += Time.deltaTime * 3.22f)
                {
                    shipRotateOnAltActivate.shipItem.heldRotationOffset = Mathf.Lerp(start, target, t);
                    yield return new WaitForEndOfFrame();
                }
                shipRotateOnAltActivate.shipItem.heldRotationOffset = target;
                shipRotateOnAltActivate.rotating = false;
                Debug.Log("rotated.");
                yield break;
            }

            public static IEnumerator MoveShipItem(ShipItemMoveOnAltActivate shipMoveOnAltActivate, float target)
            {
                float start = shipMoveOnAltActivate.shipItem.holdDistance;
                for (float t = 0f; t < 1f; t += Time.deltaTime * 3.22f)
                {
                    shipMoveOnAltActivate.shipItem.holdDistance = Mathf.Lerp(start, target, t);
                    yield return new WaitForEndOfFrame();
                }
                shipMoveOnAltActivate.shipItem.holdDistance = target;
                shipMoveOnAltActivate.moving = false;
                Debug.Log("moved.");
                yield break;
            }

            public static IEnumerator MoveShipItem2(ShipItemMoveOnAltActivate shipMoveOnAltActivate, float target)
            {
                float start = shipMoveOnAltActivate.shipItem.holdHeight;
                for (float t = 0f; t < 1f; t += Time.deltaTime * 3.22f)
                {
                    shipMoveOnAltActivate.shipItem.holdHeight= Mathf.Lerp(start, target, t);
                    yield return new WaitForEndOfFrame();
                }
                shipMoveOnAltActivate.shipItem.holdHeight= target;
                shipMoveOnAltActivate.moving = false;
                Debug.Log("moved.");
                yield break;
            }
        }


    public static void ResetMove(ShipItem instance)
        {
            ShipItemMoveOnAltActivate shipMoveOnAltActivate = instance.GetComponent<ShipItemMoveOnAltActivate>();
            if (shipMoveOnAltActivate)
            {
                instance.holdDistance = shipMoveOnAltActivate.defaultDistance;
                instance.holdHeight = shipMoveOnAltActivate.defaultHeight;
            }
        }

        public static void ResetRotate(ShipItem instance)
        {
            ShipItemRotateOnAltActivate shipRotateOnAltActivate = instance.GetComponent<ShipItemRotateOnAltActivate>();
            if (shipRotateOnAltActivate)
            {
                instance.heldRotationOffset = 0f;
            }
        }

    }
}
