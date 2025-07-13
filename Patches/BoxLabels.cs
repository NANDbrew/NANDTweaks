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
        [HarmonyPatch(typeof(ShipItemCrate), "OnLoad")]
        private static class BoxLabelAdder2
        {
            [HarmonyPostfix]
            public static void Postfix(ShipItemCrate __instance)
            {
                if (!Plugin.boxLabels.Value) return;
                int index = __instance.GetPrefabIndex();
                if (index == 131) { AddLabel(__instance.gameObject, 0); } // candles
                else if (index == 313) { AddLabel(__instance.gameObject, 2); } // green tobacco
                else if (index == 319) { AddLabel(__instance.gameObject, 5); } // blue tobacco
                else if (index == 315) { AddLabel(__instance.gameObject, 3); } // black tobacco
                else if (index == 317) { AddLabel(__instance.gameObject, 4); } // brown tobacco
                else if (index == 311) { AddLabel(__instance.gameObject, 1); } // white tobacco
            }
            
        }
        
        private static void AddLabel(GameObject target, int index)
        {
            GameObject stamp = GameObject.CreatePrimitive(PrimitiveType.Quad);
            stamp.transform.SetParent(target.transform, false);
            stamp.transform.localPosition = new Vector3(0f, 0.1252f, 0f);
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
            // candle
            labels[0] = MatLoader.CreateMaterial(source, MatLoader.labelsTex, new Vector2(0.077f, 0.573f), new Vector2(0.35f, 0.35f));
            labels[0].color = Color.white;
            labels[0].name = "candles";

            // white
            labels[1] = MatLoader.CreateMaterial(source, MatLoader.labelsTex, new Vector2(0.577f, 0.573f), new Vector2(0.35f, 0.35f));
            labels[1].color = new Color(0.685f, 0.62f, 0.612f);
            labels[1].name = "whiteTobacco";

            // green
            labels[2] = MatLoader.CreateMaterial(source, MatLoader.labelsTex, new Vector2(0.577f, 0.573f), new Vector2(0.35f, 0.35f));
            labels[2].color = new Color(0.482f, 0.651f, 0.451f);
            labels[2].name = "greenTobacco";

            // black
            labels[3] = MatLoader.CreateMaterial(source, MatLoader.labelsTex, new Vector2(0.577f, 0.573f), new Vector2(0.35f, 0.35f));
            labels[3].color = new Color(0.129f, 0.141f, 0.129f);
            labels[3].name = "blackTobacco";

            // brown
            labels[4] = MatLoader.CreateMaterial(source, MatLoader.labelsTex, new Vector2(0.577f, 0.573f), new Vector2(0.35f, 0.35f));
            labels[4].color = new Color(0.322f, 0.271f, 0.192f);
            labels[4].name = "brownTobacco";

            // blue
            labels[5] = MatLoader.CreateMaterial(source, MatLoader.labelsTex, new Vector2(0.577f, 0.573f), new Vector2(0.35f, 0.35f));
            labels[5].color = new Color(0.441f, 0.588f, 0.647f);
            labels[5].name = "blueTobacco";
            return labels;
        }
    }
}
