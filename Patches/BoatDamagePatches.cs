using HarmonyLib;
using System;
using UnityEngine;

namespace NANDTweaks
{
    [HarmonyPatch(typeof(BoatDamageWater), "Start")]
    internal static class BoatDamageWaterPatch
    {
        public static void Postfix(ref Vector3 ___anchor, ref float ___heightRange, ref float ___minScaleY, BoatDamage ___damage)
        {
            if (!Plugin.bailingTweaks.Value) return;
            int sceneIndex = ___damage.GetComponent<SaveableObject>().sceneIndex;
            switch (sceneIndex)
            {
                case 80:
                    ___heightRange = 1.85f;
                    ___anchor = new Vector3(___anchor.x, 1.8f, ___anchor.z);
                    ___minScaleY = 0.7f;
                    break;
                case 50:
                    ___heightRange = 2.2f;
                    ___anchor = new Vector3(___anchor.x, 2.05f, ___anchor.z);
                    //___minScaleY = 0.7f;
                    break;
                case 20:
                    ___heightRange = 2f;
                    ___anchor = new Vector3(___anchor.x, 1.6f, ___anchor.z);
                    ___minScaleY = 0.7f;
                    break;
                case 40:
                    //___heightRange = 0.9f;// this only makes sense with the sump/hatches
                    ___heightRange = 0.6f;
                    ___anchor = new Vector3(___anchor.x, 1.85f, ___anchor.z);
                    //___minScaleY = 0.7f;
                    break;
                case 10:
                    //___heightRange = 0.9f;
                    ___anchor = new Vector3(___anchor.x, 1.75f, ___anchor.z);
                    //___minScaleY = 0.7f;
                    break;
                case 90:
                    ___heightRange = 0.65f;
                    ___anchor = new Vector3(___anchor.x, 1.738f, ___anchor.z);
                    //___minScaleY = 0.7f;
                    break;
            }
        }
    }

    [HarmonyPatch(typeof(BoatDamageWaterButton), "OnItemClick")]
    internal static class BoatDamageWaterButtonPatch
    {
        public static bool Prefix(PickupableItem heldItem, ref float ___fillCooldown, BoatDamage ___damage, ref bool __result)
        {
            if (!Plugin.bailingTweaks.Value) return true;
            __result = false;
            if (___fillCooldown > 0f)
            {
                return false;
            }

            if (heldItem.GetType() != typeof(ShipItemBottle))
            {
                Plugin.logSource.Log(BepInEx.Logging.LogLevel.Debug, "bailing failed: is not bottle");
                return false;
            }

            ShipItemBottle shipItemBottle = (ShipItemBottle)heldItem;
            if (shipItemBottle.amount != 9f && shipItemBottle.health > 0f)
            {
                Plugin.logSource.Log(BepInEx.Logging.LogLevel.Debug, "bailing failed: bottle has other liquid");
                return false;
            }

            float num = shipItemBottle.GetRemainingCapacity();
            if (num > 5f && shipItemBottle.GetCapacity() != 9f)
            {
                num = 5f;
            }

            float unitsToFill = ___damage.waterLevel * ___damage.waterUnitsCapacity;
            if (num > unitsToFill)
            {
                num = unitsToFill;
            }

            shipItemBottle.amount = 9f;
            shipItemBottle.health += Mathf.Round(num);


            ___damage.waterLevel -= num * (1f / ___damage.waterUnitsCapacity);
            ___fillCooldown = 0.66f;
            if (num > 0f)
            {
                UISoundPlayer.instance.PlayLiquidPourSound();
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(BoatDamageWaterButton), "ExtraLateUpdate")]
    internal static class BoatDamageWaterButtonPatch2
    {
        public static bool Prefix(GoPointer ___pointedAtBy, ref GoPointer ___lastPointer, Collider ___col, ref float ___fillCooldown, BoatDamage ___damage)
        {
            if (!Plugin.bailingTweaks.Value) return true;
            if ((bool)___pointedAtBy)
            {
                ___lastPointer = ___pointedAtBy;
            }

            if ((bool)___lastPointer)
            {
                if (___lastPointer.GetHeldItem() != null && ___lastPointer.GetHeldItem().GetType() == typeof(ShipItemBottle))
                {
                    ___col.enabled = true;
                }
                else
                {
                    ___col.enabled = false;
                }

                if (___damage.waterLevel * ___damage.waterUnitsCapacity < 1.0f && !___pointedAtBy)
                {
                    ___col.enabled = false;
                }
            }

            if (___fillCooldown > 0f)
            {
                ___fillCooldown -= Time.deltaTime;
            }
            return false;
        }
        public static void Postfix(BoatDamageWaterButton __instance,  Collider ___col, BoatDamage ___damage)
        {
            if (___col != null && ___col.enabled)
            {
                if (Plugin.waterText.Value == WaterText.Units)
                {
                    __instance.lookText = Mathf.RoundToInt(___damage.waterLevel * ___damage.waterUnitsCapacity).ToString() + " / " + ___damage.waterUnitsCapacity;
                }
                else if (Plugin.waterText.Value == WaterText.Percent)
                {
                    __instance.lookText = Mathf.RoundToInt(___damage.waterLevel * 100).ToString() + "%";
                }
            }
            else
            {
                __instance.lookText = "";
            }
        }

    }

}
