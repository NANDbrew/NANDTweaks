using HarmonyLib;
using NANDTweaks.Scripts;
using NANDTweaks;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks
{
    internal class ShipItemEmbarkPatches
    {

        [HarmonyPatch(typeof(ShipItem))]
        private static class EmbarkPatches
        {

            [HarmonyPatch("Awake")]
            [HarmonyPrefix]
            public static void Awake(ShipItem __instance)
            {
                __instance.gameObject.AddComponent<EmbarkTracker>();
            }

            [HarmonyPatch("OnTriggerEnter")]
            [HarmonyPrefix]
            public static bool OnTriggerEnter(Collider other, ShipItem __instance, Collider ___currentBoatCollider)
            {
                if (!Plugin.stickyFix.Value) return true;

                if (other.CompareTag("EmbarkCol"))
                {
                    List<Collider> colliders = __instance.GetComponent<EmbarkTracker>().embarkColliders;
                    if (!colliders.Contains(other))
                    {
                        colliders.Add(other);
                    }
                    if (___currentBoatCollider != null)
                    {
                        return false;
                    }
                }
                return true;
            }

            [HarmonyPatch("OnTriggerExit")]
            [HarmonyPostfix]
            public static void OnTriggerExit(ShipItem __instance, Collider other, ref Collider ___currentlyStayedEmbarkCol)
            {
                if (!Plugin.stickyFix.Value) return;

                List<Collider> colliders = __instance.GetComponent<EmbarkTracker>().embarkColliders;
                colliders.Remove(other);

                if (colliders.Count >= 1)
                {
                    ___currentlyStayedEmbarkCol = colliders[0];
                }
            }


            [HarmonyPatch("EnterBoat")]
            [HarmonyPrefix]
            public static void EnterBoat(ShipItem __instance, Collider ___currentBoatCollider, Collider embarkCol)
            {
                if (!Plugin.stickyFix.Value) return;
                if (__instance.GetComponent<HangableItem>() is HangableItem item)
                {
                    if (item.IsHanging())
                    {
                        return;
                    }
                }

                 __instance.InvokePrivateMethod("ExitBoat");

            }

            [HarmonyPatch("ExitBoat")]
            [HarmonyPrefix]
            public static bool ExitBoat(ShipItem __instance, ItemRigidbody ___itemRigidbodyC, Collider ___currentBoatCollider)
            {
                if (!Plugin.stickyFix.Value) return true;

                if (Plugin.hook_shelf.Value)
                {
                    if (__instance.GetComponent<HangableItem>() is HangableItem item)
                    {
                        if (item.IsHanging())
                        {
                            return false;
                        }

                    }

                    if (___itemRigidbodyC is ItemRigidbody body)
                    {
                        if (body.attached)
                        {
                            return false;

                        }

                    }
                
                }
                List<Collider> colliders = __instance.GetComponent<EmbarkTracker>().embarkColliders;
                colliders.Remove(___currentBoatCollider);

                return true;
            }

            [HarmonyPatch("InsertIntoCargoCarrier")]
            [HarmonyPostfix]
            public static void CarrierPatch(ShipItem __instance)
            {
                __instance.GetComponent<EmbarkTracker>().embarkColliders.Clear();
            }
        }
    }
}
