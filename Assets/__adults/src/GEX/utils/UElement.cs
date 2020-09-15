using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace GEX.utils
{
	public class UElement
    {
        public float _x, _y, _width, _height;
        public GameObject gameObject;
        private RectTransform rt;

        public UElement(GameObject gobj, bool centerPivot = true, bool resetAnchor = false)
        {
            gameObject = gobj;

            rt = gobj.GetComponent<RectTransform>();

            if (!centerPivot)
                rt.pivot = new Vector2(0, 0);

            if (resetAnchor)
            {
                rt.anchorMin = new Vector2(0, 0);
                rt.anchorMax = new Vector2(0, 0);
            }
        }

        public void destroy()
        {
            rt = null;
        }

        public RectTransform getRectTransform()
        {
            return rt;
        }

        public float x
        {
            get { return rt.position.x; }
            set { rt.position = new Vector3(value, rt.position.y); }
        }

        public float y
        {
            get { return rt.position.y; }
            set { rt.position = new Vector3(rt.position.x, value); }
        }

        public float local_x
        {
            get { return rt.localPosition.x; }
            set { rt.localPosition = new Vector3(value, rt.localPosition.y); }
        }

        public float local_y
        {
            get { return rt.localPosition.y; }
            set { rt.localPosition = new Vector3(rt.localPosition.x, value); }
        }

        public float width
        {
            get { return rt.rect.width; }
            set { rt.sizeDelta = new Vector2(value, rt.sizeDelta.y); }
        }

        public float height
        {
            get { return rt.rect.height; }
            set { rt.sizeDelta = new Vector2(rt.sizeDelta.x, value); }
        }


    }

}
