using HarmonyLib;
using NANDTweaks.Scripts;
using SailwindModdingHelper;
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
        private static class UpdateLookTextPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ShipItem __instance)
            {
                if (!Plugin.boxLabels.Value) return;
                if (__instance is ShipItemCrate)
                {
                    if (__instance.category != TransactionCategory.otherItems && __instance.category != TransactionCategory.toolsAndSupplies) return;
                    if (__instance.name == "lantern candles")
                    {
                        Material mat = AddLabel(__instance.gameObject);
                        mat.mainTextureOffset = new Vector2(0.077f, 0.573f);
                    }
                    if (__instance.name.Contains("tobacco"))
                    {
                        Material mat = AddLabel(__instance.gameObject);
                        mat.mainTextureOffset = new Vector2(0.577f, 0.573f);

                        if (__instance.name.Contains("green")) mat.color = new Color(0.482f, 0.651f, 0.451f);
                        else if (__instance.name.Contains("blue")) mat.color = new Color(0.441f, 0.588f, 0.647f);
                        else if (__instance.name.Contains("brown")) mat.color = new Color(0.322f, 0.271f, 0.192f);
                        else if (__instance.name.Contains("black")) mat.color = new Color(0.129f, 0.141f, 0.129f);
                        else if (__instance.name.Contains("green")) mat.color = new Color(0.482f, 0.651f, 0.451f);
                        else if (__instance.name.Contains("white")) mat.color = new Color(0.685f, 0.62f, 0.612f);
                    }

                }
            }
        }

        private static Material AddLabel(GameObject target)
        {
            GameObject stamp = GameObject.CreatePrimitive(PrimitiveType.Quad);
            stamp.transform.SetParent(target.transform, false);
            stamp.transform.localPosition = new Vector3(0f, 0.125f, 0f);
            stamp.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
            stamp.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            stamp.GetComponent<MeshCollider>().enabled = false;
            stamp.name = "label";

            var renderer = stamp.GetComponent<MeshRenderer>();

            renderer.material = new Material(target.GetComponent<MeshRenderer>().material)
            {
                mainTexture = MatLoader.labelsTex,
                mainTextureScale = new Vector2(0.35f, 0.35f),
                renderQueue = 2001,
            };
            renderer.material.EnableKeyword("_ALPHATEST_ON");
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            return renderer.material;
        }
    }
}
