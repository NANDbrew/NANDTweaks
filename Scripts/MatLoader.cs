using BepInEx;
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
        public static Material[] mats;
        public static Material[] localMats;
        //public static Material logos;
        public static Material[] labels;
        public static Texture2D decalTex;
        public static Texture2D labelsTex;
        public static string firstTry;
        public static string secondTry;
        public static string maskPath;
        public static string maskPathsm;

        public static void Start()
        {
            firstTry = Directory.GetParent(Plugin.instance.Info.Location).FullName;
            secondTry = Path.Combine(firstTry, Plugin.PLUGIN_NAME);

            string path = File.Exists(Path.Combine(firstTry, "decal.png"))? Path.Combine(firstTry, "decal.png") : Path.Combine(secondTry, "decal.png");
            string path2 = File.Exists(Path.Combine(firstTry, "logos.png")) ? Path.Combine(firstTry, "logos.png") : Path.Combine(secondTry, "logos.png");
            string path3 = File.Exists(Path.Combine(firstTry, "labels3.png")) ? Path.Combine(firstTry, "labels3.png") : Path.Combine(secondTry, "labels3.png");
            decalTex = LoadTexture(path);
            labelsTex = LoadTexture(path3);
            Texture2D localLogo = LoadTexture(path2);


            localMats = new Material[4];
            localMats[0] = CreateMaterial(localLogo, new Vector2(0.0f, 0.5f), new Vector2(0.5f, 0.5f));
            localMats[0].name = "alankh";
            localMats[1] = CreateMaterial(localLogo, new Vector2(0.0f, 0.0f), new Vector2(0.5f, 0.5f));
            localMats[1].name = "emerald";
            localMats[2] = CreateMaterial(localLogo, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            localMats[2].name = "aestrin";
            localMats[3] = CreateMaterial(localLogo, new Vector2(0.5f, 0.0f), new Vector2(0.5f, 0.5f));
            localMats[3].name = "chronos";

            mats = new Material[6];
            mats[0] = CreateMaterial(decalTex, Vector2.zero, Vector2.one);
            mats[0].name = "decal crate large";
            mats[1] = CreateMaterial(decalTex, new Vector2(0.05f, 0f), new Vector2(0.86f, 1f));
            mats[1].name = "decal crate medium";
            mats[2] = CreateMaterial(decalTex, new Vector2(0.125f, 0f), new Vector2(0.75f, 1f));
            mats[2].name = "decal crate small";
            mats[3] = CreateMaterial(decalTex, new Vector2(-0.05f, 0f), new Vector2(1.13f, 1f));
            mats[3].name = "decal barrel";
            mats[4] = CreateMaterial(decalTex, new Vector2(0.2f, 0.28f), new Vector2(1f, 1f));
            mats[4].name = "decal package";
            mats[5] = CreateMaterial(decalTex, new Vector2(0.1f, 0f), new Vector2(0.8f, 1f));
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

        internal static Material CreateMaterial(Texture2D tex, Vector2 offset, Vector2 scale)
        {
            return CreateMaterial(null, tex, offset, scale);
        }

        internal static Material CreateMaterial(Material source, Texture2D tex, Vector2 offset, Vector2 scale)
        {
            Material mat;

            if (source) mat = new Material(source);
            else
            {
                mat = new Material(Shader.Find("Standard"));
                mat.SetFloat("_Glossiness", 0.1f);
            }
            mat.renderQueue = 2001;
            mat.mainTexture = tex;
            mat.mainTextureOffset = offset;
            mat.mainTextureScale = scale;

            mat.EnableKeyword("_ALPHATEST_ON");
            //mat.EnableKeyword("_ALPHABLEND_ON");
            mat.SetShaderPassEnabled("ShadowCaster", false);
            return mat;
        }

        public static void UpdateColor()
        {
            foreach (Material mat in mats)
            {
                mat.color = Plugin.decalColor.Value;
            }
            foreach (Material mat in localMats)
            {
                mat.color = Plugin.decalColor.Value;
            }
            //logos.color = Plugin.decalColor.Value;
            //labels.color = Plugin.decalColor.Value;
        }
    }

}
