using HarmonyLib;
using NANDTweaks.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks
{
    internal class MissionGoodsPatches
    {

        [HarmonyPatch(typeof(Good))]
        private static class TextureReplacer
        {
            [HarmonyPatch("RegisterToMission")]
            [HarmonyPostfix]
            private static void Postfix(Good __instance)
            {
                if (!Plugin.cargoDecal.Value) return;
                if (MatLoader.bannerTex == null) return;
                MatLoader.UpdateColor();

                if (__instance.sizeDescription.Contains("crate") || __instance.sizeDescription.Contains("package"))
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    UnityEngine.Object.Destroy(cube.GetComponent<BoxCollider>());
                    cube.transform.SetParent(__instance.transform, false);
                    cube.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                    //huge crate
                    if (__instance.sizeDescription == "very large crate")
                    {
                        cube.transform.localScale = new Vector3(1.39f, 2.49f, 1.39f);
                        cube.transform.localPosition = new Vector3(0f, 0.35f, -0.006f);
                        cube.GetComponent<MeshRenderer>().material = MatLoader.mats[5];

                    }
                    // large crate
                    else if (__instance.sizeDescription == "large crate")
                    {
                        cube.transform.localScale = new Vector3(1.135f, 1.1f, 1.15f);
                        cube.transform.localPosition = new Vector3(0.0054f, 0.35f, -0.015f);
                        cube.GetComponent<MeshRenderer>().material = MatLoader.mats[0];

                    }
                    //standard crate
                    else if (__instance.sizeDescription == "standard crate")
                    {
                        cube.transform.localScale = new Vector3(0.7f, 1.13f, 0.97f);
                        cube.transform.localPosition = new Vector3(0.0054f, 0.35f, -0.002f);
                        cube.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                        cube.GetComponent<MeshRenderer>().material = MatLoader.mats[1];

                    }
                    //small crate
                    else if (__instance.sizeDescription == "small crate")
                    {
                        cube.transform.localScale = new Vector3(0.43f, 0.8f, 0.59f);
                        cube.transform.localPosition = new Vector3(0f, 0.175f, 0f);
                        cube.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                        cube.GetComponent<MeshRenderer>().material = MatLoader.mats[2];

                    }
                    //small package
                    else if (__instance.sizeDescription == "small package")
                    {
                        cube.transform.localScale = new Vector3(1f, 1f, 0.32f);
                        cube.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                        cube.GetComponent<MeshRenderer>().material = MatLoader.mats[0];

                    }
                    //medium package
                    else if (__instance.sizeDescription == "medium package")
                    {
                        cube.transform.localScale = new Vector3(1f, 1.19f, 0.64f);
                        cube.GetComponent<MeshRenderer>().material = MatLoader.mats[0];

                    }
                    //standard package
                    else if (__instance.sizeDescription == "standard package")
                    {
                        cube.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                        cube.transform.localScale = new Vector3(1f, 1.4f, 0.53f);
                        cube.GetComponent<MeshRenderer>().material = MatLoader.mats[4];

                    }
                    //large package
                    else if (__instance.sizeDescription == "large package")
                    {
                        cube.transform.localScale = new Vector3(1.41f, 1.43f, 0.904f);
                        cube.GetComponent<MeshRenderer>().material = MatLoader.mats[0];

                    }
                    else
                    {
                        UnityEngine.Object.Destroy(cube.gameObject);
                    }
                }

                else if (__instance.sizeDescription == "standard barrel")
                {
                    GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    UnityEngine.Object.Destroy(cylinder.GetComponent<CapsuleCollider>());
                    cylinder.transform.SetParent(__instance.transform, false);

                    cylinder.transform.localScale = new Vector3(1f, 0.48f, 1f);
                    cylinder.transform.localPosition = new Vector3(0f, 0.51f, 0f);
                    cylinder.transform.localEulerAngles = new Vector3(0f, 15f, 0f);

                    cylinder.GetComponent<MeshRenderer>().material = MatLoader.mats[3];
                }
            }
        }
    }
}
