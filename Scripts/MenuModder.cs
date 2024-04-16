using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace NANDTweaks
{
    internal class MenuModder
    {
        public static void Setup()
        {

            StartMenu startMenu = UnityEngine.Object.FindObjectOfType<StartMenu>();
            if (startMenu)
            {
                var saveSlotUI = startMenu.GetPrivateField<GameObject>("saveSlotUI");
                if (saveSlotUI)
                {
                    Debug.Log("found saveSlotUI");
                    foreach (var button in saveSlotUI.GetComponentsInChildren<StartMenuButton>())
                    {
                        int slot = button.GetPrivateField<int>("saveSlot");
                        StartMenuButtonType type = button.GetPrivateField<StartMenuButtonType>("type");

                        if (type == StartMenuButtonType.Slot)
                        {
                            TextMesh textMesh = button.transform.parent.GetComponentInChildren<TextMesh>();
                            Renderer renderer = button.GetComponent<MeshRenderer>();

                            if (!File.Exists(SaveSlots.GetSlotSavePath(slot))) continue; // skip slot if empty

                            string path = SaveSlots.GetSlotSavePath(slot) + ".png";
                            Texture2D tex = new Texture2D(1, 1);
                            byte[] bytes = File.Exists(path) ? File.ReadAllBytes(path) : null;
                            if (bytes != null)
                            {

                                Debug.Log("Success! loaded file");
                                tex.LoadImage(bytes);

                                //Debug.Log("tex=" + tex.width + "x" + tex.height);
                                Material mat = new Material(Shader.Find("UI/Default"));
                                mat.mainTexture = tex;
                                renderer.material = mat;

                                renderer.material.mainTexture = tex;

                                float ratio = (float)tex.height / (float)tex.width;
                                float offset = (1f - ratio) / 2;
                                //Debug.Log("ratio=" + ratio);
                                //Debug.Log("offset=" + offset);
                                renderer.material.mainTextureScale = new Vector2(ratio, 1);
                                renderer.material.mainTextureOffset = new Vector2(offset, 0);

                                //renderer.material.DisableKeyword("_EMISSION");

                                textMesh.color = new Color(1f, 0.8f, 0.6f);
                                textMesh.fontSize = 45;
                                Vector3 textPos = new Vector3(0f, -0.1f, 0.04f);
                                textMesh.transform.localPosition = textPos;
                            }

                        }
                    }
                }
                else Debug.Log("no ui found");
            }
        }
    }
}
