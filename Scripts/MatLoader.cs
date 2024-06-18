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
        public static Material logos;
        public static Material labels;
        public static Texture2D decalTex;
        //public static Texture2D logosTex;
        public static Texture2D labelsTex;
        public static void Start()
        {
            string path = Path.Combine(SailwindModdingHelper.Extensions.GetFolderLocation(Plugin.instance.Info), "decal.png");
            string path2 = Path.Combine(SailwindModdingHelper.Extensions.GetFolderLocation(Plugin.instance.Info), "logos.png");
            string path3 = Path.Combine(SailwindModdingHelper.Extensions.GetFolderLocation(Plugin.instance.Info), "labels3.png");
            //mat = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));
            decalTex = LoadTexture(path);
            labelsTex = LoadTexture(path3);
            logos = CreateMaterial(LoadTexture(path2));
            labels = CreateMaterial(LoadTexture(path3));


            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = CreateMaterial(decalTex);
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

        static Texture2D LoadTexture(string path)
        {
            byte[] bytes = File.Exists(path) ? File.ReadAllBytes(path) : null;
            var tex = new Texture2D(1, 1);
            if (bytes != null)
            {
                tex.LoadImage(bytes);
                Debug.Log("MatLoader loaded texture from file");
            }
            return tex;
        }
        static Material CreateMaterial(Texture2D tex)
        {
            var mat = new Material(Shader.Find("Standard"))
            {
                renderQueue = 2001,
                mainTexture = tex
            };
            mat.EnableKeyword("_ALPHATEST_ON");
            //mat.EnableKeyword("_ALPHABLEND_ON");
            mat.SetShaderPassEnabled("ShadowCaster", false);
            mat.SetFloat("_Glossiness", 0.1f);
            return mat;
        }
        public static void UpdateColor()
        {
            foreach (Material mat in mats)
            {
                mat.color = Plugin.decalColor.Value;
            }
            logos.color = Plugin.decalColor.Value;
            labels.color = Plugin.decalColor.Value;
        }
    }

}
