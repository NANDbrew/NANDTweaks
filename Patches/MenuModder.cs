//using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using HarmonyLib;

namespace NANDTweaks.Patches
{
    [HarmonyPatch(typeof(StartMenu), "Start")]
    internal static class MenuModder
    {
        public static void Postfix(StartMenu __instance, GameObject ___saveSlotUI)
        {
            if (!Plugin.saveLoadThumbs.Value) return;
            foreach (var button in ___saveSlotUI.GetComponentsInChildren<StartMenuButton>())
            {
                StartMenuButtonType type = (StartMenuButtonType)Traverse.Create(button).Field("type").GetValue();

                if (type != StartMenuButtonType.Slot) continue;

                if (!File.Exists(SaveSlots.GetSlotSavePath(button.saveSlot))) continue; // skip slot if empty

                string path = SaveSlots.GetSlotSavePath(button.saveSlot) + ".png";
                byte[] bytes = File.Exists(path) ? File.ReadAllBytes(path) : null;
                if (bytes != null)
                {
                    Texture2D tex = new Texture2D(1, 1);
                    TextMesh textMesh = button.transform.parent.GetComponentInChildren<TextMesh>();
                    Renderer renderer = button.GetComponent<MeshRenderer>();

                    Debug.Log("Success! loaded file");
                    tex.LoadImage(bytes);
                    float ratio = (float)tex.height / (float)tex.width;
                    float offset = (1f - ratio) / 2;

                    //Debug.Log("tex=" + tex.width + "x" + tex.height);
                    renderer.material = new Material(Shader.Find("UI/Default"))
                    {
                        mainTexture = tex,
                        mainTextureScale = new Vector2(ratio, 1),
                        mainTextureOffset = new Vector2(offset, 0)
                    };

                    textMesh.color = new Color(1f, 0.8f, 0.6f);
                    textMesh.fontSize = 45;
                    textMesh.transform.localPosition = new Vector3(0f, -0.1f, 0.04f);
                }

            }

        }
    }
}
