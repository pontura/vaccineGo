using UnityEngine;
using System;
using System.Collections.Generic;

namespace GEX.utils
{
	public class XImageCapture
	{

        public const string FORMAT_RGB24 = "RGB24";
        public const string FORMAT_ARGB32 = "ARGB32";

        private static string ScreenShotName(int width, int height)
        {
            return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
            Application.dataPath,
            width, height,
            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        }


        public static Texture2D takeScreenshot(int width, int height, string format = "RGB24")
        {
            //takeHiResShot = false;
            Camera cam = Camera.main;

            RenderTexture rt = null;
            Texture2D screenShot = null;
            

            if (format == FORMAT_RGB24)
            {
                rt = new RenderTexture(width, height, 24);
                cam.targetTexture = rt;
                screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);

            }
            else if (format == FORMAT_ARGB32)
            {
                rt = new RenderTexture(width, height, 32);
                cam.targetTexture = rt;
                screenShot = new Texture2D(width, height, TextureFormat.ARGB32, false);

            }

            
            cam.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            cam.targetTexture = null;
            RenderTexture.active = null; //added to avoid errors
            GameObject.Destroy(rt);

            screenShot.Apply();

            return screenShot;
        }

        public static void takeScreenshotAndSaveIt(int width, int height)
        {
            Texture2D tex = takeScreenshot(width, height);
            saveFile(tex);
        }

        public static void takeScreenshotV2(int width, int height, String path, String filename)
        {
            Texture2D tex = takeScreenshot(width, height, FORMAT_ARGB32);
            
            byte[] bytes = tex.EncodeToPNG();
            string fullFilename = path + "/" + filename + ".png";//ScreenShotName(tex.width, tex.height);
            System.IO.Directory.CreateDirectory(path);
            System.IO.File.WriteAllBytes(fullFilename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", fullFilename));
        }

        private static void saveFile(Texture2D img)
        {
            byte[] bytes = img.EncodeToPNG();
            string filename = ScreenShotName(img.width, img.height);
            System.IO.Directory.CreateDirectory("screenshots");
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
        }


	}
}
