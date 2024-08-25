using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using SailwindModdingHelper;
using MonoMod.Utils;

namespace NANDTweaks
{
    public class Shotter3 : MonoBehaviour
    {
        //static string imagePath = Path.Combine(SailwindModdingHelper.Extensions.GetFolderLocation(Plugin.instance.Info), "image.png");
        static string maskPath = Path.Combine(SailwindModdingHelper.Extensions.GetFolderLocation(Plugin.instance.Info), "mask.png");
        static string maskPathSm = Path.Combine(SailwindModdingHelper.Extensions.GetFolderLocation(Plugin.instance.Info), "mask_sm.png");

        static Texture2D mask;
        //RenderTexture output;
        public void SaveThumbnail(string path)
        {
            //path += ".png";

            StartMenu startMenu = UnityEngine.Object.FindObjectOfType<StartMenu>();
            if (startMenu)
            {
                var logo = startMenu.GetPrivateField<GameObject>("logo");
                logo.SetActive(false);
                var quitUi = startMenu.GetPrivateField<GameObject>("confirmQuitUI");
                quitUi.SetActive(false);
            }

            StartCoroutine(RecordFrame(path));
        }
        IEnumerator RecordFrame(string path)
        {
            yield return new WaitForEndOfFrame();

            if (Plugin.compatMode.Value)
            {
                ScreenCapture.CaptureScreenshot(path);
                yield break;
            }

            var mPath = maskPath;
            int targetWidth = 1024;
            int targetHeight = 1024;
            if (Screen.height < targetHeight)
            {
                mPath = maskPathSm;
                targetWidth = 720;
                targetHeight = 720;
            }
            if (mask == null)
            {
                mask = new Texture2D(targetWidth, targetHeight);
                byte[] bytes = File.Exists(mPath) ? File.ReadAllBytes(mPath) : null;
                mask.LoadImage(bytes);
                Debug.Log("loaded mask from file");
            }

            Texture2D screenImage = new Texture2D(targetWidth, targetHeight);
            //Get Image from screen
            screenImage.ReadPixels(new Rect(Screen.width / 2 - targetWidth / 2, Screen.height / 2 - targetHeight / 2, targetWidth, targetHeight), 0, 0);

            screenImage = ApplyMask(screenImage, mask);
            byte[] imageBytes = screenImage.EncodeToPNG();
            //Save image to file
            System.IO.File.WriteAllBytes(path + ".png", imageBytes);
            // cleanup
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
