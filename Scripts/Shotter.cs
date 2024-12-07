﻿using System;
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
        string maskPath = Path.Combine(Plugin.dataPath, "mask.png");
        string maskPathSm = Path.Combine(Plugin.dataPath, "mask_sm.png");
        Texture2D mask;
#if DEBUG
        Texture2D image;
        Texture2D output;
#endif
        private void Awake()
        {
            string mpath = Screen.height > 1024 ? maskPath : maskPathSm;
            if (File.Exists(mpath))
            {
                byte[] bytes = File.ReadAllBytes(mpath);
                mask = new Texture2D(1, 1);
                mask.LoadImage(bytes);
#if DEBUG
                Debug.Log("loaded mask from file");
                Debug.Log("mask dimensions = " + mask.texelSize.ToString());
#endif
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
            path += ".png";
            if (Plugin.compatMode.Value || mask == null || mask.width == 1)
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
            Debug.Log("read pixels");
#endif
            screenImage = ApplyMask(screenImage, mask);
#if DEBUG
            output = new Texture2D(screenImage.width, screenImage.height);
            output.SetPixels(screenImage.GetPixels());
            output.Apply();
            Debug.Log("applied mask");
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
