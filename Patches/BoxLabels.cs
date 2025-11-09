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
        [HarmonyPatch(typeof(PrefabsDirectory), "PopulateShipItems")]
        private static class BoxLabelAdder
        {
            [HarmonyPostfix]
            public static void Postfix2(PrefabsDirectory __instance)
            {
                if (!Plugin.boxLabels.Value) return;
                AddLabel(__instance.directory[131], "small box", Pictogram.candle, Color.white); // candles
                AddLabel(__instance.directory[313], "small box", Pictogram.pipe, LabelColor.green); // green tobacco
                AddLabel(__instance.directory[319], "small box", Pictogram.pipe, LabelColor.blue); // blue tobacco
                AddLabel(__instance.directory[315], "small box", Pictogram.pipe, LabelColor.black); // black tobacco
                AddLabel(__instance.directory[317], "small box", Pictogram.pipe, LabelColor.brown); // brown tobacco
                AddLabel(__instance.directory[311], "small box", Pictogram.pipe, LabelColor.white); // white tobacco
                AddLabel(__instance.directory[387], "small box", Pictogram.tea, LabelColor.white); // white tea
                AddLabel(__instance.directory[389], "small box", Pictogram.tea, LabelColor.green); // green tea
                AddLabel(__instance.directory[388], "small box", Pictogram.tea, LabelColor.black); // black tea
                AddLabel(__instance.directory[373], "small box", Pictogram.coffee, LabelColor.brown); // coffee
                AddLabel(__instance.directory[386], "small keg", Pictogram.coffee, LabelColor.brown); // coffee keg
                AddLabel(__instance.directory[385], "small keg", Pictogram.salt, LabelColor.white); // salt keg

            }
        }

        private static void AddLabel(GameObject target, string shape, Vector2 picture, Color color)
        {
            try
            {
                var label = GameObject.Instantiate(AssetTools.prefab.transform.Find(shape));
                label.SetParent(target.transform, false);
                label.localRotation = Quaternion.identity;
                var mat = label.GetComponent<MeshRenderer>().material;
                mat.mainTextureOffset = picture;
                mat.color = color;
                mat.renderQueue = 2001;

                if (MatLoader.labelsTex != null)
                {
                    mat.mainTexture = MatLoader.labelsTex;
                }
            }
            catch { Debug.LogError("NANDTweaks: failed to add label to " + target.name); }
        }

        private struct LabelColor
        {
            public static Color green => new Color(0.482f, 0.651f, 0.451f);
            public static Color blue => new Color(0.441f, 0.588f, 0.647f);
            public static Color white => new Color(0.685f, 0.62f, 0.612f);
            public static Color black => new Color(0.129f, 0.141f, 0.129f);
            public static Color brown => new Color(0.322f, 0.271f, 0.192f);
        }

        private struct Pictogram
        {
            public static Vector2 tea => new Vector2(0.33f, 0.33f);
            public static Vector2 pipe => new Vector2(0.33f, 0.66f);
            public static Vector2 candle => new Vector2(0f, 0.66f);
            public static Vector2 salt => new Vector2(0f, 0.33f);
            public static Vector2 coffee => new Vector2(0.66f, 0.33f);

        }

    }
}
