using HarmonyLib;
using NANDTweaks.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Patches
{
    internal static class BoxLabels
    {
        [HarmonyPatch(typeof(ShipItem), "Awake")]
        private static class BoxLabelAdder
        {
            [HarmonyPostfix]
            public static void Postfix(ShipItem __instance)
            {
                if (!Plugin.boxLabels.Value) return;
                if (__instance.category != TransactionCategory.otherItems && __instance.category != TransactionCategory.toolsAndSupplies) return;
                if (__instance is ShipItemCrate)
                {
                    if (__instance.name == "lantern candles")
                    {
                        AddLabel(__instance.gameObject, 0);
                    }
                    else if (__instance.name.Contains("tobacco"))
                    {
                        if (__instance.name.Contains("green")) AddLabel(__instance.gameObject, 1);
                        else if (__instance.name.Contains("blue")) AddLabel(__instance.gameObject, 2);
                        else if (__instance.name.Contains("brown")) AddLabel(__instance.gameObject, 4);
                        else if (__instance.name.Contains("black")) AddLabel(__instance.gameObject, 3);
                        else if (__instance.name.Contains("white")) AddLabel(__instance.gameObject, 5);

                    }
                }
            }
        }
        
        private static void AddLabel(GameObject target, int index)
        {
            GameObject stamp = GameObject.CreatePrimitive(PrimitiveType.Quad);
            stamp.transform.SetParent(target.transform, false);
            stamp.transform.localPosition = new Vector3(0f, 0.125f, 0f);
            stamp.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
            stamp.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            stamp.GetComponent<MeshCollider>().enabled = false;
            stamp.name = "label";
            if (MatLoader.labels == null) MatLoader.labels = GenerateMats(target.GetComponent<Renderer>().material);
            stamp.GetComponent<MeshRenderer>().material = MatLoader.labels[index];
        }

        private static Material[] GenerateMats(Material source)
        {
            Material[] labels = new Material[6];
            labels[0] = MatLoader.CreateMaterial(source, MatLoader.labelsTex, new Vector2(0.077f, 0.573f), new Vector2(0.35f, 0.35f));
            labels[0].color = Color.white;
            labels[0].name = "candles";
            labels[1] = MatLoader.CreateMaterial(source, MatLoader.labelsTex, new Vector2(0.577f, 0.573f), new Vector2(0.35f, 0.35f));
            labels[1].color = new Color(0.685f, 0.62f, 0.612f);
            labels[1].name = "whiteTobacco";
            labels[2] = MatLoader.CreateMaterial(source, MatLoader.labelsTex, new Vector2(0.577f, 0.573f), new Vector2(0.35f, 0.35f));
            labels[2].color = new Color(0.482f, 0.651f, 0.451f);
            labels[2].name = "greenTobacco";
            labels[3] = MatLoader.CreateMaterial(source, MatLoader.labelsTex, new Vector2(0.577f, 0.573f), new Vector2(0.35f, 0.35f));
            labels[3].color = new Color(0.129f, 0.141f, 0.129f);
            labels[3].name = "blackTobacco";
            labels[4] = MatLoader.CreateMaterial(source, MatLoader.labelsTex, new Vector2(0.577f, 0.573f), new Vector2(0.35f, 0.35f));
            labels[4].color = new Color(0.322f, 0.271f, 0.192f);
            labels[4].name = "brownTobacco";
            labels[5] = MatLoader.CreateMaterial(source, MatLoader.labelsTex, new Vector2(0.577f, 0.573f), new Vector2(0.35f, 0.35f));
            labels[5].color = new Color(0.441f, 0.588f, 0.647f);
            labels[5].name = "blueTobacco";
            return labels;
        }
    }
}
