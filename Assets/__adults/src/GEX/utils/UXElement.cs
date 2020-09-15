using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
namespace GEX.utils
{
    public class UXElement
    {
        public float _x, _y, _width, _height;
        public RectTransform rect;
        private Vector2 SAVED_ANCHOR_MIN;
        private Vector2 SAVED_ANCHOR_MAX;

        public UXElement(GameObject gobj, bool centerPivot = true, bool resetAnchor = false)
        {
            _init(gobj.GetComponent<RectTransform>(), centerPivot, resetAnchor);
        }

        public UXElement(RectTransform rect, bool centerPivot = true, bool resetAnchor = false)
        {
            _init(rect, centerPivot, resetAnchor);
        }

        private void _init(RectTransform rect, bool centerPivot, bool resetAnchor)
        {

            this.rect = rect;

            if (centerPivot)
                rect.pivot = new Vector2(0.5f, 0.5f);


            if (resetAnchor)
            {
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(0, 1);
            }

        }

        private void PUSH_ANCHOR()
        {
            SAVED_ANCHOR_MIN = new Vector2(rect.anchorMin.x, rect.anchorMin.y);
            SAVED_ANCHOR_MAX = new Vector2(rect.anchorMax.x, rect.anchorMax.y);
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
        }

        private void POP_ANCHOR()
        {
            rect.anchorMin = SAVED_ANCHOR_MIN;
            rect.anchorMax = SAVED_ANCHOR_MAX;
        }

        public float scaleX
        {
            get { return rect.localScale.x; }
            set { rect.localScale = new Vector3(value, rect.localScale.y, rect.localScale.z); }
        }

        public float scaleY
        {
            get { return rect.localScale.y; }
            set { rect.localScale = new Vector3(rect.localScale.x, value, rect.localScale.z); }
        }

        public float pivotX
        {
            get { return rect.pivot.x; }
            set { rect.pivot = new Vector2(value, rect.pivot.y); }
        }

        public float pivotY
        {
            get { return rect.pivot.y; }
            set { rect.pivot = new Vector2(rect.pivot.x, value); }
        }

        public float width
        {
            get { return rect.rect.width; }
            set { rect.sizeDelta = new Vector2(value, rect.sizeDelta.y); }
        }

        public float height
        {
            get { return rect.rect.height; }
            set { rect.sizeDelta = new Vector2(rect.sizeDelta.x, value); }
        }


        public float x
        {
            get { return rect.anchoredPosition3D.x; }
            set { rect.anchoredPosition3D = new Vector3(value, rect.anchoredPosition3D.y); }
        }

        public float y
        {
            get { return rect.anchoredPosition3D.y; }
            set { rect.anchoredPosition3D = new Vector3(rect.anchoredPosition3D.x, value); }
        }

        public float anchorMinX
        {
            get { return rect.anchorMin.x; }
            set { rect.anchorMin = new Vector2(value, rect.anchorMin.y); }
        }

        public float anchorMinY
        {
            get { return rect.anchorMin.y; }
            set { rect.anchorMin = new Vector2(rect.anchorMin.x, value); }
        }

        public float anchorMaxX
        {
            get { return rect.anchorMax.x; }
            set { rect.anchorMax = new Vector2(value, rect.anchorMax.y); }
        }

        public float anchorMaxY
        {
            get { return rect.anchorMax.y; }
            set { rect.anchorMax = new Vector2(rect.anchorMax.x, value); }
        }

        public GameObject gameobject
        {
            get { return rect.gameObject; }
        }

        public void setActive(bool active)
        {
            gameobject.SetActive(active);
        }

        public void resetOffsets()
        {
            rect.offsetMin = new Vector2(0, 0);
            rect.offsetMax = new Vector2(0, 0);
        }

    }
}
