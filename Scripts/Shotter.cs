using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
//using SailwindModdingHelper;
using MonoMod.Utils;
using HarmonyLib;
using BepInEx;
using System.Text.RegularExpressions;

namespace NANDTweaks
{
    public class Shotter3 : MonoBehaviour
    {
        static string maskPath = Path.Combine(Plugin.dataPath, "mask.png");
        static string maskPathSm = Path.Combine(Plugin.dataPath, "mask_sm.png");
        Texture2D output;
        static Texture2D mask;
        //RenderTexture output;

        private void Awake()
        {
            string mpath = Screen.height > 1024 ? maskPath : maskPathSm;
            if (File.Exists(mpath))
            {
                byte[] bytes = File.ReadAllBytes(mpath);
                mask = new Texture2D(1, 1);
                mask.LoadImage(bytes);
                Debug.Log("loaded mask from file");
            }
        }

        public void SaveThumbnail(string path)
        {
            //path += ".png";

            if (UnityEngine.Object.FindObjectOfType<StartMenu>() is StartMenu startMenu)
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
            yield return new WaitForEndOfFrame();
            path += ".png";
            if (Plugin.compatMode.Value || mask == null || mask.width == 1)
            {
                ScreenCapture.CaptureScreenshot(path);
                yield break;
            }

            int targetWidth = 1024;
            int targetHeight = 1024;

            Texture2D screenImage = new Texture2D(targetWidth, targetHeight);

            //Get Image from screen
            screenImage.ReadPixels(new Rect(Screen.width / 2 - mask.width / 2, Screen.height / 2 - mask.height / 2, mask.width, mask.height), 0, 0);
            output = screenImage;
            Debug.Log("read pixels");
            screenImage = ApplyMask(screenImage, mask);
            Debug.Log("applied mask");
            byte[] imageBytes = screenImage.EncodeToPNG();
            //Save image to file
            System.IO.File.WriteAllBytes(path, imageBytes);
            // cleanup
            output = screenImage;
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
