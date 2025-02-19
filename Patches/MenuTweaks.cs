using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;

namespace NANDTweaks.Patches
{
    internal class MenuTweaks
    {
        private static GameObject saveSlotUILabel;
        private static TextMesh saveSlotUILabelText;

        [HarmonyPatch(typeof(BackupSavesListUI))]
        internal class BackupSavesListUIPatches
        {
            [HarmonyPatch("Update")]
            [HarmonyPostfix] private static void Postfix(BackupSavesListUI __instance, int ___showingListFor)
            {
                if (___showingListFor == 1 && __instance.list.gameObject.activeInHierarchy)
                {
                    saveSlotUILabel.SetActive(value: false);
                }
                else
                {
                    saveSlotUILabel.SetActive(value: true);
                }
            }
        }
        // saves menu labels: "new game" / "continue"
        [HarmonyPatch(typeof(StartMenu))]
        internal class StartMenuPatches
        {
            [HarmonyPatch("Awake")]
            [HarmonyPostfix]
            public static void Postfix(StartMenu __instance, GameObject ___saveSlotUI)
            {

                // Create UI label
                var saveSlotUIText = ___saveSlotUI.transform.Find("text").gameObject;
                if (saveSlotUIText)
                {
                    saveSlotUILabel = GameObject.Instantiate(saveSlotUIText, saveSlotUIText.transform.position, saveSlotUIText.transform.rotation, ___saveSlotUI.transform);
                    saveSlotUILabel.transform.localPosition = new Vector3(0f, 2.25f, 0f);
                    saveSlotUILabel.name = "label text";
                    saveSlotUILabelText = saveSlotUILabel.GetComponent<TextMesh>();
                    saveSlotUILabelText.fontSize = 80;
                    saveSlotUILabelText.fontStyle = FontStyle.Bold;
                    //saveSlotUILabelText.anchor = TextAnchor.LowerCenter;

                    //saveSlotUIText.GetComponent<TextMesh>().text = "choose save slot";
                    saveSlotUIText.transform.localPosition = new Vector3(0f, 2.125f, 0f);
                    saveSlotUIText.GetComponent<TextMesh>().lineSpacing = 0.8f;
                }

            }

            [HarmonyPatch("EnableSlotMenu")]
            [HarmonyPostfix]
            public static void SlotMenuPatch(StartMenu __instance, bool ___selectedContinue)
            {
                //saveSlotUILabelText.text =
                if (___selectedContinue == true)
                {
                    saveSlotUILabelText.text = "Continue";
                }
                else
                {
                    saveSlotUILabelText.text = "New Game";
                }
            }
        }
    }
}
