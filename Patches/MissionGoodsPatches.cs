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
                if (Plugin.cargoDecal.Value == Plugin.DecalType.None) return;
                //if (MatLoader.bannerTex == null) return;
                MatLoader.UpdateColor();

                #region DestinationStamp
                if (Plugin.cargoDecal.Value == Plugin.DecalType.Origin)
                {
                    GameObject stampA = new GameObject{ name = "stamp A" }; 
                    stampA.transform.parent = __instance.transform;
                    GameObject stampB = new GameObject{ name = "stamp B" };
                    stampB.transform.parent = __instance.transform;
                    

                    GameObject stamp = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    stamp.transform.SetParent(stampA.transform, false);
                    stamp.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    stamp.GetComponent<MeshCollider>().enabled = false;
                    stamp.name = "stamp";

                    stamp.GetComponent<MeshRenderer>().material = MatLoader.localMats[(int)__instance.GetAssignedMission().originPort.region];
                    if (__instance.GetAssignedMission().originPort.portIndex == 21) stamp.GetComponent<MeshRenderer>().material = MatLoader.localMats[3];

                    GameObject stamp2 = UnityEngine.Object.Instantiate(stamp, stampB.transform, false);

/*
                    GameObject origStamp = UnityEngine.Object.Instantiate(stamp, stampA.transform, false);
                    origStamp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    origStamp.transform.localPosition = new Vector3(-0.5f, -0.5f, 0);
                    origStamp.name = "origin stamp";
                    if (__instance.GetAssignedMission().originPort.region == PortRegion.alankh)
                    {
                        origStamp.GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(0.0f, 0.5f);
                    }
                    else if (__instance.GetAssignedMission().originPort.region == PortRegion.medi)
                    {
                        origStamp.GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(0.5f, 0.5f);
                    }
                    else if (__instance.GetAssignedMission().originPort.region == PortRegion.emerald)
                    {
                        origStamp.GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(0.0f, 0.0f);
                    }
                    GameObject origStamp2 = UnityEngine.Object.Instantiate(origStamp, stampB.transform, false);
*/

                    if (__instance.sizeDescription == "very large crate")
                    {
                        //stampA.transform.localEulerAngles = new Vector3()
                        stampA.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
                        stampA.transform.localPosition = new Vector3(0f, 0f, -0.7f);

                        stampB.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
                        stampB.transform.localPosition = new Vector3(0f, 0.0f, 0.69f);
                        stampB.transform.localEulerAngles = new Vector3(0, 180, 0);
                    }
                    if (__instance.sizeDescription == "large crate")
                    {
                        //stampA.transform.localEulerAngles = new Vector3()
                        stampA.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
                        stampA.transform.localPosition = new Vector3(0f, 0.35f, -0.59f);

                        stampB.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
                        stampB.transform.localPosition = new Vector3(0f, 0.35f, 0.56f);
                        stampB.transform.localEulerAngles = new Vector3(0, 180, 0);
                    }
                    else if (__instance.sizeDescription == "standard crate")
                    {
                        //stampA.transform.localEulerAngles = new Vector3()
                        stampA.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
                        stampA.transform.localPosition = new Vector3(0f, 0.7f, 0.0f);
                        stampA.transform.localEulerAngles = new Vector3(90, 0, 0);

                        stampB.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
                        stampB.transform.localPosition = new Vector3(0f, 0.005f, 0.0f);
                        stampB.transform.localEulerAngles = new Vector3(270, 0, 0);
                    }
                    else if (__instance.sizeDescription == "small crate")
                    {
                        stampA.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
                        stampA.transform.localPosition = new Vector3(0.0f, 0.404f, 0.0f);
                        stampA.transform.localEulerAngles = new Vector3(90, 0, 0);

                        stampB.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
                        stampB.transform.localPosition = new Vector3(0.0f, -0.055f, 0.0f);
                        stampB.transform.localEulerAngles = new Vector3(270, 0, 0);
                    }
                    else if (__instance.sizeDescription == "large package")
                    {
                        stampA.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
                        stampA.transform.localPosition = new Vector3(0.31f, 0.32f, 0.453f);
                        stampA.transform.localEulerAngles = new Vector3(0, 180, 40);

                        stampB.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
                        stampB.transform.localPosition = new Vector3(-0.31f, -0.32f, -0.453f);
                        stampB.transform.localEulerAngles = new Vector3(0, 0, 120);
                    }
                    else if (__instance.sizeDescription == "standard package")
                    {
                        stampA.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                        stampA.transform.localPosition = new Vector3(0.4f, 0.23f, 0.264f);
                        stampA.transform.localEulerAngles = new Vector3(0, 180, 40);

                        stampB.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                        stampB.transform.localPosition = new Vector3(-0.4f, -0.23f, -0.263f);
                        stampB.transform.localEulerAngles = new Vector3(0, 0, 120);
                    }
                    else if (__instance.sizeDescription == "medium package")
                    {
                        stampA.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
                        stampA.transform.localPosition = new Vector3(0.22f, 0.3f, 0.32f);
                        stampA.transform.localEulerAngles = new Vector3(0, 180, 40);

                        stampB.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
                        stampB.transform.localPosition = new Vector3(-0.22f, -0.3f, -0.32f);
                        stampB.transform.localEulerAngles = new Vector3(0, 0, 120);
                    }
                    else if (__instance.sizeDescription == "small package")
                    {
                        stampA.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
                        stampA.transform.localPosition = new Vector3(-0.4f, 0.23f, 0.162f);
                        stampA.transform.localEulerAngles = new Vector3(359, 180, 300);

                        stampB.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
                        stampB.transform.localPosition = new Vector3(0.39f, 0.23f, -0.165f);
                        stampB.transform.localEulerAngles = new Vector3(0, 0, 300);
                    }
                    else if (__instance.sizeDescription == "standard barrel")
                    {
                        stampA.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
                        stampA.transform.localPosition = new Vector3(0.0f, 0.99f, 0.0f);
                        stampA.transform.localEulerAngles = new Vector3(90, 0, 0);
                        //origStamp.transform.localPosition = new Vector3(-0.4f, -0.4f);

                        stampB.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
                        stampB.transform.localPosition = new Vector3(0.0f, 0.049f, 0.0f);
                        stampB.transform.localEulerAngles = new Vector3(270, 0, 0);
                        //origStamp2.transform.localPosition = new Vector3(-0.4f, -0.4f);

                    }
                    else
                    {
                        UnityEngine.Object.Destroy(stampA);
                        UnityEngine.Object.Destroy(stampB);
                    }
                }

                #endregion

                #region BigLogo
                if (Plugin.cargoDecal.Value == Plugin.DecalType.CompanyLogo)
                {
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
                
                #endregion
            }
        }
    }
}
