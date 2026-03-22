using System.IO;
using UnityEngine;

namespace NANDTweaks.Scripts
{
    internal class AssetTools
    {
        public static AssetBundle bundle;
        public static AssetBundle bundle2;
        public static GameObject prefab;
        const string assetDir = "NANDTweaks";
        const string assetFile = "logos";
        const string assetFile2 = "effects_regions";
        public static void LoadAssetBundles()    //Load the bundle
        {
            string dataPath = Directory.GetParent(Plugin.instance.Info.Location).FullName;
            string firstTry = Path.Combine(dataPath, assetDir, assetFile);
            string secondTry = Path.Combine(dataPath, assetFile);

            bundle = AssetBundle.LoadFromFile(File.Exists(firstTry) ? firstTry : secondTry);
            if (bundle == null)
            {
                Plugin.logSource.LogError("NANDTweaks: Bundle 1 not loaded! Did you place it in the correct folder?");
            }
            else 
            {
                Plugin.logSource.Log(BepInEx.Logging.LogLevel.Info, "loaded bundle " + bundle.ToString());
                prefab = bundle.LoadAsset<GameObject>("Labels.prefab");
            }

            string firstTry2 = Path.Combine(dataPath, assetDir, assetFile2);
            string secondTry2 = Path.Combine(dataPath, assetFile2);

            bundle2 = AssetBundle.LoadFromFile(File.Exists(firstTry2) ? firstTry2 : secondTry2);
            if (bundle2 == null)
            {
                Plugin.logSource.LogError("NANDtweaks: Bundle2 not loaded! Did you place it in the correct folder?");
            }
            else
            {
                Plugin.logSource.Log(BepInEx.Logging.LogLevel.Info, "loaded bundle " + bundle2.ToString());
                //prefab2 = bundle.LoadAsset<GameObject>("Labels.prefab");
            }
        }

    }
}
