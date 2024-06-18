using HarmonyLib;
using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Patches
{
    internal static class ShipyardInfoPatches
    {
        private static int category;

/*        private static Font alankh;
        private static Font emerald;
        private static Font medi;

        [HarmonyPatch(typeof(ReputationUI), "Awake")]
        private static class FontGetter
        {
            [HarmonyPostfix]
            private static void Postfix(ReputationUI __instance)
            {
                alankh = __instance.transform.GetChild(0).GetComponent<TextMesh>().font;
                emerald = __instance.transform.GetChild(1).GetComponent<TextMesh>().font;
                medi = __instance.transform.GetChild(2).GetComponent<TextMesh>().font;
            }
        }*/


        private class SailInfo : MonoBehaviour
        {
            public float GetSailMass(Sail sail)
            {
                float num = sail.GetRealSailPower() * 20f;
                float num2;
                if (sail.category == SailCategory.junk || sail.category == SailCategory.gaff)
                {
                    num2 = sail.GetRealSailPower() * 20f;
                }
                else if (sail.category == SailCategory.staysail)
                {
                    num2 = 0f;
                }
                else
                {
                    num2 = sail.GetRealSailPower() * 10f;
                }
                return num + num2;
            }
        }

        [HarmonyPatch(typeof(ShipyardUI), "ChangeMenuCategory")]
        private static class ShipyardChangeMenu
        {
            [HarmonyPostfix]
            private static void Postfix(ShipyardUI __instance, int newCategory)
            {
                category = newCategory;

                __instance.UpdateDescriptionText();
            }
        }

/*        [HarmonyPatch(typeof(ShipyardUI), "ShowUI")]
        private static class ShipyardChangeFont
        {
            [HarmonyPostfix]
            private static void Postfix(ShipyardUI __instance, GameObject ___ui)
            {
                Font currentFont;
                if (GameState.currentShipyard.region == 0)
                {
                    currentFont = alankh;
                }
                else if (GameState.currentShipyard.region == 1)
                {
                    currentFont = emerald;
                }
                else
                {
                    currentFont = medi;
                }

                foreach (TextMesh textMesh in ___ui.GetComponentsInChildren<TextMesh>()) 
                {
                    textMesh.font = currentFont;
                }
            }
        }*/

        [HarmonyPatch(typeof(ShipyardUI), "UpdateDescriptionText")]
        private static class ShipyardUIStartPatch
        {
            [HarmonyPostfix]
            public static void Postfix(TextMesh ___descText)
            {
                Sail currentSail = GameState.currentShipyard.sailInstaller.GetCurrentSail();

                if ((bool)currentSail)
                {
                    SailInfo sailInfo = new SailInfo();
                    string mass = Mathf.RoundToInt(sailInfo.GetSailMass(currentSail)).ToString();

                    List<string> texts = new List<string>(___descText.text.Split('\n'));
                    texts.Insert(2, "weight: " + mass);
                    ___descText.text = string.Join("\n", texts);
                }

                if (category > -1)
                {
                    BoatPartsOrder currentOrder = GameState.currentShipyard.partsInstaller.GetCurrentOrder();
                    BoatCustomParts currentParts = GameState.currentShipyard.GetCurrentBoat().GetComponent<BoatCustomParts>();
                    int numLines = 0;
                    string text = "";

                    for (int i = 0; i < currentParts.availableParts.Count; i++)
                    {
                        int thisPartMass = Mathf.RoundToInt((float)currentParts.availableParts[i].partOptions[currentOrder.orderedOptions[i]].mass);
                        if (currentParts.availableParts[i].category == category && thisPartMass > 0)
                        {
                            text += currentParts.availableParts[i].partOptions[currentOrder.orderedOptions[i]].optionName + " :: weight: " + thisPartMass;
                            if (currentParts.availableParts[i].partOptions[currentOrder.orderedOptions[i]].GetComponent<Mast>() is Mast mast)
                            {
                                text += " | height: " + mast.mastHeight;
                            }
                            text += "\n";
                            numLines++;
                        }

                        if (currentParts.availableParts[i].partOptions[currentOrder.orderedOptions[i]].optionName.Contains("stay"))
                        {
                            currentParts.availableParts[i].category = 2;
                        }                    }
                    if (numLines > 0)
                    {
                        ___descText.GetComponent<TextMesh>().characterSize = numLines > 5 ? 0.2f - (0.015f * (numLines % 5)) : 0.2f;

                        ___descText.text = text;
                    }
                }
            }
        }
    }
}
