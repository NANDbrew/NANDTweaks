using System.IO;
using UnityEngine;

namespace NANDTweaks.Scripts
{
    internal class MatLoader
    {
        public static Material[] missionLabels;
        public static Texture2D decalTex;
        public static Texture2D labelsTex;
        public static string firstTry;
        public static string secondTry;
        public static string maskPath;
        public static string maskPathsm;

        public static void Start()
        {
            //firstTry = Directory.GetParent(Plugin.instance.Info.Location).FullName;
            //secondTry = Path.Combine(firstTry, Plugin.PLUGIN_NAME);

            Material refMat = AssetTools.bundle.LoadAsset<Material>("mission_label.mat");
            refMat.renderQueue = 2001;
            missionLabels = new Material[6];

            missionLabels[0] = CreateMaterial(refMat, new Vector2(0.0f, 0.75f)); // al'ankh
            missionLabels[1] = CreateMaterial(refMat, new Vector2(0.0f, 0.5f)); // emerald
            missionLabels[2] = CreateMaterial(refMat, new Vector2(0.25f, 0.75f)); // aestrin
            missionLabels[3] = CreateMaterial(refMat, new Vector2(0.75f, 0f)); // anchor
            missionLabels[4] = CreateMaterial(refMat, new Vector2(0.0f, 0.25f)); // fire fish
            missionLabels[5] = CreateMaterial(refMat, new Vector2(0.25f, 0.5f)); // chronos
            
            UpdateColor();
        }


        static Material CreateMaterial(Material source, Vector2 offset)
        {
            Material mat = new Material(source)
            {
                mainTextureOffset = offset
            };
            return mat;
        }

        public static void UpdateColor()
        {
            foreach (Material mat in missionLabels)
            {
                mat.color = Plugin.decalColor.Value;
            }

        }


        #region obsolete
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
        #endregion
    }

}
