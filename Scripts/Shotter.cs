using System.Collections;
using UnityEngine;
using HarmonyLib;
using NANDTweaks.Scripts;

namespace NANDTweaks
{
    public class Shotter3 : MonoBehaviour
    {
        Texture2D[] masks;
#if DEBUG
        Texture2D image;
        Texture2D output;
#endif
        private void Awake()
        {
            string[] maskPaths = { "mask.png", "mask_ss.png", "mask_be_ss.png", "mask_sm.png", "mask_sm_ss.png", "mask_sm_be_ss.png" };

            masks = new Texture2D[maskPaths.Length];
            for (int i = 0; i < maskPaths.Length; i++)
            {
                masks[i] = AssetTools.bundle.LoadAsset<Texture2D>(maskPaths[i]);
            }

        }

        public void SaveThumbnail(string path)
        {
            //path += ".png";
            if (Plugin.startMenu is StartMenu startMenu)
            {
                GameObject logo = Traverse.Create(startMenu).Field("logo").GetValue<GameObject>();
                logo.SetActive(false);
                var quitUi = Traverse.Create(startMenu).Field("confirmQuitUI").GetValue<GameObject>();
                quitUi.SetActive(false);
            }

            StartCoroutine(RecordFrame(path));
        }
        IEnumerator RecordFrame(string path)
        {
            path += ".png";
            int index = Screen.height >= 1024 ? 0 : 3;
            if (GameState.modData.TryGetValue("ScrambledSeas", out string ss))
            {
                index += ss.IndexOf("borderExpander>1") > -1 ? 2 : 1;
            }
            Texture2D mask = masks[index];

            if (Plugin.compatMode.Value)
            {
                ScreenCapture.CaptureScreenshot(path);
                yield break;
            }

            Texture2D screenImage = new Texture2D(mask.width, mask.height);

            //Get Image from screen
            yield return new WaitForEndOfFrame();
            screenImage.ReadPixels(new Rect(Screen.width / 2 - mask.width / 2, Screen.height / 2 - mask.height / 2, mask.width, mask.height), 0, 0);
#if DEBUG
            image = new Texture2D(screenImage.width, screenImage.height);
            image.SetPixels(screenImage.GetPixels());
            image.Apply();
            Plugin.logSource.Log(BepInEx.Logging.LogLevel.Debug, "read pixels");
#endif
            screenImage = ApplyMask(screenImage, mask);
#if DEBUG
            output = new Texture2D(screenImage.width, screenImage.height);
            output.SetPixels(screenImage.GetPixels());
            output.Apply();
            Plugin.logSource.Log(BepInEx.Logging.LogLevel.Debug, "applied mask");
#endif
            byte[] imageBytes = screenImage.EncodeToPNG();
            //Save image to file
            System.IO.File.WriteAllBytes(path, imageBytes);
            // clean up
            UnityEngine.Object.Destroy(screenImage);
        }

        private Texture2D ApplyMask(Texture2D img, Texture2D mask)
        {

            Color[] dest = img.GetPixels();
            Color[] src = mask.GetPixels();

            for (int i = 0; i < dest.Length; ++i)
            {
                dest[i].a = src[i].a;
            }
            img.SetPixels(dest);
            return img;

        }
    }
}
