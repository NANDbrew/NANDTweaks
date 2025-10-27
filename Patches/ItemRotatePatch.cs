using HarmonyLib;
using UnityEngine;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(PickupableItem), "OnScroll")]
    public static class OnScrollPatch
    {
        public static bool Prefix(float input, PickupableItem __instance)
        {
            if (Plugin.rotateItemKey.Value.IsPressed() && __instance.big)
            {
                __instance.transform.Rotate(__instance.held.transform.forward, input * 4f, Space.World);

                var rot = Quaternion.Inverse(__instance.held.transform.rotation) * __instance.transform.rotation;
                AccessTools.Field(typeof(GoPointer), "bigItemLocalRot").SetValue(__instance.held, rot);
                __instance.heldRotationOffset = 0f;
                return false;
            }
            else if (Plugin.pushItemKey.Value.IsPressed())
            {
                input /= 40;
                __instance.holdDistance = Mathf.Clamp(__instance.holdDistance + input, 0.5f, 2f);

                var pos = (Vector3)AccessTools.Field(typeof(GoPointer), "bigItemLocalPos").GetValue(__instance.held);
                pos = new Vector3(pos.x, pos.y, Mathf.Clamp(pos.z + input, 1f, 3.0f));
                AccessTools.Field(typeof(GoPointer), "bigItemLocalPos").SetValue(__instance.held, pos);
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(GoPointer))]
    public static class GoPointerPatch
    {
        static float baseHoldDist = 0f;
        [HarmonyPatch("PickUpItem")]
        [HarmonyPostfix]
        public static void Postfix(PickupableItem item)
        {
            baseHoldDist = item.holdDistance;
        }
        [HarmonyPatch("DropItem")]
        [HarmonyPrefix]
        public static void Prefix(PickupableItem ___heldItem)
        {
            if (___heldItem != null)
            {
                ___heldItem.holdDistance = baseHoldDist;
            }
        }
    }
}
