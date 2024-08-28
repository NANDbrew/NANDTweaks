using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace NANDTweaks
{


    [HarmonyPatch(typeof(ShipyardUI))]
    internal class ShipyardUITweaks
    {
        private static Transform infoPanel;
        private static Transform[] elements;
        private static ShipyardButton[] categoryButtons;
        private static Vector3[] startPositions;
        private static readonly Vector3[] newPositions = 
        {
            new Vector3(3.5f, -0.4f, 0f), // sails menu
            new Vector3(4.35f, -2.5f, 1.6f), // parts menu
            new Vector3(-3.5f, 0.5f, 0f), // masts menu
            new Vector3(11f, 6.5f, 9.48f), // sail type menu
            new Vector3(4.85f, -0.2f, -0.31f), // sail type menu
            new Vector3(12.41f, 1.24f, 10.36f), // masts button
            new Vector3(15.77f, 1.24f, 9.8f), // 'other' button
            new Vector3(14.09f, 1.24f, 10.08f), // stays button
            new Vector3(10.74f, 1.24f, 10.65f), // sails button
            new Vector3(-4.8f, 0f, -0.18f),
            new Vector3(-12f, -5.44f, 9.23f),
            new Vector3(-7.6f, -5.35f, 9.96f),
            new Vector3(-9.5f, -5.40f, 9.68f),
            new Vector3(0f, -4.7f, 9.65f),
            new Vector3(-15f, 10.8f, 10.15f),
            new Vector3(15.77f, 2.5f, 9.8f), // new, second 'other' button
        };
        public static void UpdatePositions()
        {
            if (newPositions == null || startPositions == null || elements == null || newPositions.Length < elements.Length || startPositions.Length < elements.Length)
            {
                Debug.LogError("Array issue!");
                return;
            }

            if (Plugin.wideShipyardUI.Value)
            {
                infoPanel.localScale = new Vector3(1f, 1.2f, 1.2f);
            }
            else
            {
                infoPanel.localScale = new Vector3(1f, 1.1f, 1.2f);

            }


            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i] == null || newPositions == null || startPositions == null) { Debug.LogError("Bullshit!"); break; }
                if (Plugin.wideShipyardUI.Value)
                {
                    elements[i].localPosition = newPositions[i];
                }
                else
                {
                    elements[i].localPosition = startPositions[i];
                }
            }

        }

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void AwakePatch(ShipyardUI __instance, GameObject ___sailMenu, GameObject ___newPartsMenu)
        {
            var firstChild = __instance.transform.GetChild(0);
            var currentOrderPanel = firstChild.transform.Find("panel Current Order");
            infoPanel = firstChild.transform.Find("shipyard ui text box ship info").Find("bg");


            elements = new Transform[]
            {
                ___sailMenu.transform,
                ___newPartsMenu.transform,
                ___sailMenu.transform.Find("panel masts"),
                ___sailMenu.transform.Find("panel SelectSailType"),
                ___sailMenu.transform.Find("panel Add New Sail"),
                firstChild.transform.Find("mode button Parts Masts"),
                firstChild.transform.Find("mode button Parts Other"),
                firstChild.transform.Find("mode button Parts Stays"),
                firstChild.transform.Find("mode button Sails"),
                firstChild.transform.Find("panel Current Order"),
                currentOrderPanel.Find("shipyard ui button clean hull"),
                currentOrderPanel.Find("shipyard ui button confirm"),
                currentOrderPanel.Find("shipyard ui button cancel purchase"),
                firstChild.transform.Find("shipyard ui text box ship info"),
                firstChild.transform.Find("shipyard ui button exit"),
                //newButton,
            };

            categoryButtons = new ShipyardButton[]
            {
                firstChild.transform.Find("mode button Sails").GetComponent<ShipyardButton>(),
                firstChild.transform.Find("mode button Parts Masts").GetComponent<ShipyardButton>(),
                firstChild.transform.Find("mode button Parts Other").GetComponent<ShipyardButton>(),
                firstChild.transform.Find("mode button Parts Stays").GetComponent<ShipyardButton>(),
            };

            startPositions = new Vector3[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                startPositions[i] = elements[i].localPosition;
            }


        }

        [HarmonyPatch("ShowUI")]
        [HarmonyPostfix]
        public static void ShowUIPatch(ShipyardUI __instance, GameObject ___sailMenu, GameObject ___newPartsMenu)
        {
            if (categoryButtons.Length == 4)
            {
                if (__instance.transform.GetChild(0).transform.Find("mode button Parts Extra") is Transform extraButton)
                {
                    categoryButtons = categoryButtons.AddToArray(extraButton.GetComponent<ShipyardButton>());
                    elements = elements.AddToArray(extraButton);
                    startPositions = startPositions.AddToArray(extraButton.localPosition);
                }
            }
            UpdatePositions();
        }

        [HarmonyPatch("ChangeMenuCategory")]
        [HarmonyPostfix]
        public static void RefreshButtonsPatch(ShipyardUI __instance, int newCategory)
        {
            for (int i = 0; i < categoryButtons.Length; i++) 
            {
                if (newCategory == i - 1) categoryButtons[i].enableRedOutline = true;
                else categoryButtons[i].enableRedOutline = false;
            }

        /*    switch (newCategory)
            {
                case -1: { elements[8].GetComponent<ShipyardButton>().enableRedOutline = true; elements[7].GetComponent<ShipyardButton>().enableRedOutline = false; elements[6].GetComponent<ShipyardButton>().enableRedOutline = false; elements[5].GetComponent<ShipyardButton>().enableRedOutline = false; } break;
                case 0: { elements[5].GetComponent<ShipyardButton>().enableRedOutline = true; } break;
                case 1: { elements[6].GetComponent<ShipyardButton>().enableRedOutline = true; } break;
                case 2: { elements[7].GetComponent<ShipyardButton>().enableRedOutline = true; } break;
            }*/
        }
    }
}
