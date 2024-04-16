using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NANDTweaks.Scripts
{
    internal class MatLoader
    {
        public static Material[] mats = new Material[6];
        public static Texture2D bannerTex;
        public static void Start()
        {
            string path = Path.Combine(SailwindModdingHelper.Extensions.GetFolderLocation(Plugin.instance.Info), "decal.png");
            //mat = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));
            byte[] bytes = File.Exists(path) ? File.ReadAllBytes(path) : null;
            if (bytes != null)
            {
                bannerTex = new Texture2D(1,1);
                bannerTex.LoadImage(bytes);
                Debug.Log("MatLoader loaded texture from file");
            }
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = new Material(Shader.Find("Standard"))
                {
                    renderQueue = 2001
                };
                mats[i].EnableKeyword("_ALPHATEST_ON");
                mats[i].EnableKeyword("_ALPHABLEND_ON");
                mats[i].SetShaderPassEnabled("ShadowCaster", false);
                mats[i].SetFloat("_Glossiness", 0.2f);
                mats[i].mainTexture = bannerTex;

            }

            mats[0].name = "decal crate large";

            mats[1].mainTextureOffset = new Vector2(0.05f, 0f);
            mats[1].mainTextureScale = new Vector2(0.86f, 1f);
            mats[1].name = "decal crate medium";

            mats[2].mainTextureOffset = new Vector2(0.125f, 0f);
            mats[2].mainTextureScale = new Vector2(0.75f, 1f);
            mats[2].name = "decal crate small";

            mats[3].mainTextureOffset = new Vector2(-0.05f, 0f);
            mats[3].mainTextureScale = new Vector2(1.13f, 1f);
            mats[3].name = "decal barrel";

            mats[4].mainTextureOffset = new Vector2(0.2f, 0.28f);
            mats[4].mainTextureScale = new Vector2(1f, 1f);
            mats[4].name = "decal package";

            mats[5].mainTextureOffset = new Vector2(0.1f, 0f);
            mats[5].mainTextureScale = new Vector2(0.8f, 1f);
            mats[5].name = "decal crate very large";
            UpdateColor();
        }

        public static void UpdateColor()
        {
            foreach (Material mat in mats)
            {
                mat.color = Plugin.decalColor.Value;
            }
        }
    }

}
