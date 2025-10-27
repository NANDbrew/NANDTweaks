using System.Linq;
using System.Security.Cryptography;
using HarmonyLib;
using UnityEngine;

namespace NANDTweaks
{
    [HarmonyPatch(typeof(ShipyardUI))]
    internal class ShipyardUITweaks
    {
        internal static Material offMat;
        internal static Material onMat;
        private const float fourByThree = 1.3333333333333333333333333333333f;
        //private const float sixteenByNine = 1.7777777777777777777777777777778f;
        private const float ratio = 0.44444444444444444444444444444448f; // difference between 16:9 and 4:3
        internal static int currentCategory = -1;

        private static Transform infoPanel;
        private static Transform[] elements;
        private static ShipyardButton[] categoryButtons;
        private static Vector3[] startPositions;
        private static readonly Vector3[] newPositions = 
        {
            new Vector3(3.5f, 0.0f, 0f), // sails menu
            new Vector3(4.35f, -2.17f, 1.6f), // parts menu
            new Vector3(-3.5f, 0.4f, 0f), // "select mast" tip
            new Vector3(11f, 6.9f, 9.48f), // sail type menu
            new Vector3(4.85f, -0.5f, -0.31f), // add new sail
            new Vector3(12.41f, 1.44f, 10.36f), // masts button
            new Vector3(15.77f, 1.44f, 9.8f), // 'other' button
            new Vector3(14.09f, 1.44f, 10.08f), // stays button
            new Vector3(10.74f, 1.44f, 10.65f), // sails button
            new Vector3(-4.8f, -0.3f, -0.18f), // current order panel
            //new Vector3(-12f, -5.44f, 9.23f),
            //new Vector3(-7.6f, -5.35f, 9.96f),
            //new Vector3(-9.5f, -5.40f, 9.68f),
            new Vector3(0f, -4.7f, 9.65f), // ship info textbox
            new Vector3(-15f, 10.8f, 10.15f), // exit button
            new Vector3(-15.8164f, -6.323f, 10.0311f), // money icon M
            new Vector3(-15.8164f, -6.323f, 10.0311f), // money icon A
            new Vector3(-15.8164f, -6.323f, 10.0311f), // money icon E
            new Vector3(-16.4264f, -6.42f, 8.6521f), // money light
            new Vector3(15.77f, 2.7f, 9.8f), // overflow button from ShipyardExpansion
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
            float w = Screen.width;
            float aspect = w / Screen.height;
            float newX;
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i] == null || newPositions == null || startPositions == null) { Debug.LogError("!!!"); break; }
                if (Plugin.wideShipyardUI.Value)
                {
                    newX = newPositions[i].x - startPositions[i].x;
                    newX *= (aspect - fourByThree) / ratio;

                    elements[i].localPosition = new Vector3(newX + startPositions[i].x, newPositions[i].y, newPositions[i].z);
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
            //var currentOrderPanel = firstChild.transform.Find("panel Current Order");
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
                //currentOrderPanel.Find("shipyard ui button clean hull"),
                //currentOrderPanel.Find("shipyard ui button confirm"),
                //currentOrderPanel.Find("shipyard ui button cancel purchase"),
                firstChild.transform.Find("shipyard ui text box ship info"),
                firstChild.transform.Find("shipyard ui button exit"),
                firstChild.transform.Find("money icon M"),
                firstChild.transform.Find("money icon A"),
                firstChild.transform.Find("money icon E"),
                firstChild.transform.Find("money light"),
                //newButton,
            };

            categoryButtons = new ShipyardButton[]
            {
                firstChild.transform.Find("mode button Sails").GetComponent<ShipyardButton>(),
                firstChild.transform.Find("mode button Parts Masts").GetComponent<ShipyardButton>(),
                firstChild.transform.Find("mode button Parts Other").GetComponent<ShipyardButton>(),
                firstChild.transform.Find("mode button Parts Stays").GetComponent<ShipyardButton>(),
            };

            if (elements.Contains(null) || categoryButtons.Contains(null))
            {
                Debug.LogError("nandtweaks.shipyard: missing ui element");
                return;
            }

            startPositions = new Vector3[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                startPositions[i] = elements[i].localPosition;

            }

            //var source0 = UnityEngine.Object.FindObjectOfType<GPButtonControlToggle>();

            //onMat = (Material)Traverse.Create(source0).Field("onMaterial").GetValue();
            onMat = categoryButtons.FirstOrDefault().GetComponent<Renderer>().material;
            //offMat = (Material)Traverse.Create(source0).Field("offMaterial").GetValue();
            offMat = __instance.darkParchmentMaterial;

        }

        [HarmonyPatch("ShowUI")]
        [HarmonyPostfix]
        public static void ShowUIPatch(ShipyardUI __instance)
        {
            if (elements == null || categoryButtons == null|| elements.Contains(null) || categoryButtons.Contains(null))
            {
                Debug.LogError("nandtweaks.shipyard: missing ui element");
                return;
            }

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
            RefreshCategoryButtons();
        }

        [HarmonyPatch("ChangeMenuCategory")]
        [HarmonyPostfix]
        public static void RefreshButtonsPatch(int newCategory)
        {
            currentCategory = newCategory;
            RefreshCategoryButtons();
        }
        internal static void RefreshCategoryButtons()
        {
            if (elements == null || categoryButtons == null) return;
            for (int i = 0; i < categoryButtons.Length; i++)
            {
                if (categoryButtons[i] == null) continue;

                Renderer renderer = categoryButtons[i].GetComponent<Renderer>();
                if (currentCategory + 1 == i) renderer.material = onMat;
                else renderer.material = offMat;
            }

        }
    }
}
