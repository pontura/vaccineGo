using UnityEngine;
using System;
using System.Collections.Generic;
using GEX.graphics;

namespace GEX.transition
{
    public class LoadingObjectTransition : MonoBehaviour
    {
        private static GameObject GOBJ = null;
        private static Spriteg loading = null;
        private static Spriteg background = null;
        private static String st = "";
        private static String loaing_img_path = "2D/gui/others/loading";

        public static void show()
        {
            GOBJ = new GameObject("loading_transition");
            GOBJ.AddComponent<LoadingObjectTransition>();
        }

        public static void kill()
        {
            if (loading != null)
                loading.destroy();
            if (background != null)
                background.destroy();

            loading = null;
            background = null;

            if (GOBJ != null)
            {
                Destroy(GOBJ);
                GOBJ = null;
            }
        }

        public void Awake()
        {
          //  DontDestroyOnLoad(this);

            loading = new Spriteg(Resources.Load(loaing_img_path) as Texture2D, Screen.width / 10);
            loading.isCentered = true;
            loading.setPos(Screen.width >> 1, Screen.height >> 1);
            loading.alpha = 0;

            background = new Spriteg();
            background.createFrame(10, 10, 0xFF000000);
            background.width = Screen.width;
            background.height = Screen.height;
            background.alpha = 0;

            st = "fade_in";
        }

        public void Update()
        {
            if (st == "fade_in")
            {
                background.alpha += 0.03f;
                loading.alpha += 0.03f;

                if (loading.alpha >= 1)
                {
                    st = "wait";
                    loading.alpha = 1;
                    background.alpha = 1;
                }
            }
            else if (st == "fade_out")
            {
                loading.alpha -= 0.03f;
                background.alpha -= 0.03f;
                if (loading.alpha <= 0)
                {
                    st = "wait";
                    loading.alpha = 0;
                    background.alpha = 0;
                }
            }


            loading.angle += 0.9f;
        }

        public void OnGUI()
        {
            background.paint();
            loading.paint();
        }
    }
}
