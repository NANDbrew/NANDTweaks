using System.IO;
using UnityEngine;

namespace NANDTweaks.Scripts
{
    internal class AssetTools
    {
        public static AssetBundle bundle;
        public static GameObject prefab;
        const string assetDir = "NANDTweaks";
        const string assetFile = "logos";
        public static void LoadAssetBundles()    //Load the bundle
        {
            string dataPath = Directory.GetParent(Plugin.instance.Info.Location).FullName;
            string firstTry = Path.Combine(dataPath, assetDir, assetFile);
            string secondTry = Path.Combine(dataPath, assetFile);

            bundle = AssetBundle.LoadFromFile(File.Exists(firstTry) ? firstTry : secondTry);
            if (bundle == null)
            {
                Plugin.logSource.LogError("Bundle not loaded! Did you place it in the correct folder?");
            }
            else 
            {
                Plugin.logSource.Log(BepInEx.Logging.LogLevel.Info, "loaded bundle " + bundle.ToString());
                prefab = bundle.LoadAsset<GameObject>("Labels.prefab");
            }
        }

    }
}
