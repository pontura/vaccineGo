using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using GEX.utils;


namespace GEX.transition
{
    public class TransitionFX
    {
        private static GameObject mcontainer;
        private static Image img;
        private static String mode = "";
        private static float startValue;
        private static float endValue;
        private static float incSteps = 0.05f;
        private static bool _finished;

        private static void _init()
        {
            RectTransform rt;
            if (mcontainer == null)
            {
                mcontainer = new GameObject("transition_fx");
                rt = mcontainer.AddComponent<RectTransform>();
                img = mcontainer.AddComponent<Image>();
                img.color = new Color32(0x00, 0x00, 0x00, 0x00);
                GameObject canvas = GameObject.Find("Canvas");

                rt.anchoredPosition = new Vector2(Screen.width >> 1, Screen.height >> 1);
                rt.sizeDelta = new Vector2(Screen.width, Screen.height);

                mcontainer.transform.SetParent(canvas.transform);

                _finished = false;
            }
        }

        public static void fadeIn(float startValue = 0, float endValue = 1)
        {
            TransitionFX.startValue = startValue;
            TransitionFX.endValue = endValue;

            _init();
            _setAlpha(startValue);

            _finished = false;
            mode = "fade_in";
        }

        public static void fadeOut(float startValue = 1, float endValue = 0)
        {
            TransitionFX.startValue = startValue;
            TransitionFX.endValue = endValue;

            _init();
            _setAlpha(startValue);

            _finished = false;
            mode = "fade_out";
        }


        //From 0 to 1
        private static void _setAlpha(float alpha)
        {
            byte _red_ = (byte)(0xFF * img.color.r);
            byte _green_ = (byte)(0xFF * img.color.g);
            byte _blue_ = (byte)(0xFF * img.color.b);
            byte _alpha_ = (byte)(0xFF * alpha);
            Color32 new_color = new Color32(_red_, _green_, _blue_, _alpha_);
            img.color = new_color;
        }

        private static float _getAlpha()
        {
            return img.color.a;
        }

        public static void bringToFront()
        {
            XGUI.setAtFront(mcontainer);
        }

        public static void destroy()
        {
            if (mcontainer != null)
            {
                _finished = false;
                img = null;
                mode = "";

                GameObject.Destroy(mcontainer);
                mcontainer = null;
            }
        }

        public static bool update()
        {
            if (_finished)
                return true;

            if (mode == "fade_in")
            {

                if ((_getAlpha() + incSteps) > endValue)
                {
                    _setAlpha(endValue);
                    _finished = true;
                    return true;
                }
                else
                {
                    _setAlpha(_getAlpha() + incSteps);
                    return false;
                }


            }
            else if (mode == "fade_out")
            {

                if ((_getAlpha() - incSteps) < endValue)
                {
                    _setAlpha(endValue);
                    _finished = true;
                    return true;
                }
                else
                {
                    _setAlpha(_getAlpha() - incSteps);
                    return false;
                }

            }


            return false;

        }

    }
}
