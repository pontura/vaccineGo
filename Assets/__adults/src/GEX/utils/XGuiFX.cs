#pragma warning disable 414

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace GEX.utils
{
	public class XGuiFX
	{

        //private static List<Graphic> _GRAPHICS_;
        private static float reachAlpha;
        private static bool debugEnabled = false;

        public static void debug(bool active)
        {
            debugEnabled = active;
        }

        public static void setAlpha(GameObject mainContainer, float alpha, bool includeMe = true) 
        {
            setAlpha(mainContainer, alpha, 0, includeMe);
        }

        public static float getAlphaOf(GameObject currObj)
        {
            Image img;
            Text txt;

            img = currObj.GetComponent<Image>();
            txt = currObj.GetComponent<Text>();

            if (img != null)
                return _getAlpha(img);
            else if (txt != null)
                return _getAlpha(txt);

            return -1;
        }

        private static float _getAlpha(Graphic g)
        {
            return g.canvasRenderer.GetColor().a;
        }


        public static void setAlpha(GameObject mainContainer, float alpha, float duration, bool includeMe = true)
        {
            Image img;
            Text txt;
            List<GameObject> OBJECTS = XFinder.getAllChilds(mainContainer);

            if (includeMe)
            {
                img = mainContainer.GetComponent<Image>();
                txt = mainContainer.GetComponent<Text>();

                if (img != null)
                    _setAlpha_(img, alpha, duration);
                else if (txt != null)
                    _setAlpha_(txt, alpha, duration);
            }


            for (int a = 0; a < OBJECTS.Count; a++)
            {
                img = OBJECTS[a].GetComponent<Image>();
                txt = OBJECTS[a].GetComponent<Text>();

                if (img != null)
                    _setAlpha_(img, alpha, duration);
                else if (txt != null)
                    _setAlpha_(txt, alpha, duration);
            }

        }

        private static void _setAlpha_(Graphic g, float alpha, float duration)
        {
            if (debugEnabled)
            {
                Debug.Log("<color=blue>set alpha -> " + g.name + "</color>");
            }

            g.CrossFadeColor(new Color(g.color.r, g.color.g, g.color.b, alpha), duration, false, true);
            reachAlpha = alpha;
        }



	}
}
