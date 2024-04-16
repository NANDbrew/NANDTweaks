using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Patches
{
    internal static class StoreFoodPatches
    {
        private static readonly Dictionary<string, float> crateSizes = new Dictionary<string, float>()
        {
            {"firewood", 12f },
            {"fishing hooks", 20f },
            {"white tobacco", 6f },
            {"black tobacco", 6f },
            {"brown tobacco", 6f },
            {"green tobacco", 6f },
            {"blue tobacco", 6f },
            {"lantern candles", 15 },
        };

        [HarmonyPatch(typeof(ShipItem), "OnItemClick")]
        private static class OnItemClickPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ShipItem __instance, ShipItem heldItem)
            {
                if (!Plugin.storage.Value) return true;
                if (__instance is ShipItemCrate itemCrate)
                {
                    var thisPrefabIndex = heldItem.gameObject.GetComponent<SaveablePrefab>().prefabIndex;
                    var crateItemPrefabIndex = itemCrate.GetContainedPrefab().GetComponent<SaveablePrefab>().prefabIndex;

                    Good crateGood = itemCrate.GetPrivateField<Good>("goodC");
                    if (crateGood && crateGood.GetMissionIndex() > -1) return true;
                    if (thisPrefabIndex == crateItemPrefabIndex)
                    {
                        float maxAmount;
                        if (crateGood)
                        {
                            maxAmount = PrefabsDirectory.instance.directory[itemCrate.GetPrivateField<Good>("goodC").GetComponent<SaveablePrefab>().prefabIndex].GetComponent<ShipItemCrate>().amount;

                            if (itemCrate.smokedFood && (heldItem.amount < 1f || heldItem.amount > 1.5f)) return true;
                            if (!itemCrate.smokedFood && (heldItem.amount > 0.75f)) return true;
                        }
                        else crateSizes.TryGetValue(itemCrate.name, out maxAmount);

                        if (itemCrate.amount < maxAmount)
                        {
                            itemCrate.amount++;
                            heldItem.DestroyItem();
                            UISoundPlayer.instance.PlayUISound(UISounds.itemPickup, 0.8f, 0.5f);

                            return false;
                        }
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(ShipItemCrate), "UpdateLookText")]
        private static class UpdateLookTextPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(ShipItemCrate __instance, Good ___goodC)
            {
                if (!Plugin.storage.Value) return true;

                if (___goodC && ___goodC.GetMissionIndex() > -1) return true;
                if (__instance.sold)
                {
                    float maxAmount;
                    if (___goodC) maxAmount = PrefabsDirectory.instance.directory[___goodC.GetComponent<SaveablePrefab>().prefabIndex].GetComponent<ShipItemCrate>().amount;
                    else crateSizes.TryGetValue(__instance.name, out maxAmount);
                    string text = __instance.name;
                    if (__instance.smokedFood) text += " (smoked)";

                    text += "\n" + __instance.amount + " / " + maxAmount;
                    __instance.lookText = text;
                    return false;
                }
                return true;
            }
        }

 /*       [HarmonyPatch(typeof(PickupableItem), "OnAltActivate")]
        private static class OnAltActivatePatch
        {
            [HarmonyPrefix]
            public static bool Prefix(PickupableItem __instance, PickupableItem heldItem)
            {
                if (__instance is ShipItemBottle)
                {
                    ShipItemBottle instance = (ShipItemBottle) __instance;
                    ShipItemBottle component = heldItem.GetComponent<ShipItemBottle>();
                    if (component)
                    {
                        Good instanceGoodC = instance.GetPrivateField<Good>("goodC");
                        Good heldItemGoodC = component.GetPrivateField<Good>("goodC");

                        if (!instance.sold)
                        {
                            return false;
                        }
                        if (!component.sold)
                        {
                            return false;
                        }
                        if (instanceGoodC && instanceGoodC.GetMissionIndex() > -1)
                        {
                            return false;
                        }
                        if (heldItemGoodC && heldItemGoodC.GetMissionIndex() > -1)
                        {
                            return false;
                        }
                        if (component.GetPrivateField<float>("capacity") <= instance.GetPrivateField<float>("capacity"))
                        {
                            instance.health = component.FillBottle(instance.amount, instance.health);
                        }
                        else
                        {
                            component.health = instance.FillBottle(component.amount, component.health);
                        }
                        instance.UpdateLookText();
                        instance.itemRigidbodyC.UpdateMass();
                        Debug.Log("Poured liquid.");
                    }
                    else if (!heldItem.big && __instance.allowPlacingItems)
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            }
        }*/

    }
}
