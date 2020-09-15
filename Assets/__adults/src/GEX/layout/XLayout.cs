using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using GEX.utils;

namespace GEX.layout
{

    [ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
    public class XLayout : UIBehaviour
    {
        [HideInInspector]
        public float _width = 100, _height = 100;
        [HideInInspector]
        public float _left = 0, _right = 0, _top = 0, _bottom = 0;
        [HideInInspector]
        public int _width_type = NONE, _height_type = NONE;
        [HideInInspector]
        public int _left_type = NONE, _right_type = NONE, _top_type = NONE, _bottom_type = NONE;
        [HideInInspector]
        public int _anchor_x = ANCHOR_LEFT;
        [HideInInspector]
        public int _anchor_y = ANCHOR_TOP;

        public const int NONE = 0;
        public const int PIXELS = 1;
        public const int PERCENT = 2;
        public const int ASPECT = 3;


        public const int ANCHOR_LEFT = 0;
        public const int ANCHOR_RIGHT = 1;
        public const int ANCHOR_TOP = 0;
        public const int ANCHOR_BOTTOM = 1;
        public const int ANCHOR_CENTER = 2;


        public float width
        {
            set { _width = value; OnParentResized(); }
            get { return _width; }
        }

        public float height
        {
            set { _height = value; OnParentResized(); }
            get { return _height; }
        }

        public float left
        {
            set { _left = value; OnParentResized(); }
            get { return _left; }
        }

        public float right
        {
            set { _right = value; OnParentResized(); }
            get { return _right; }
        }

        public float top
        {
            set { _top = value; OnParentResized(); }
            get { return _top; }
        }

        public float bottom
        {
            set { _bottom = value; OnParentResized(); }
            get { return _bottom; }
        }

        public int widthType
        {
            set { _width_type = value; OnParentResized(); }
            get { return _width_type; }
        }

        public int heightType
        {
            set { _height_type = value; OnParentResized(); }
            get { return _height_type; }
        }





        public int leftType
        {
            set { _left_type = value; OnParentResized(); }
            get { return _left_type; }
        }

        public int rightType
        {
            set { _right_type = value; OnParentResized(); }
            get { return _right_type; }
        }

        public int topType
        {
            set { _top_type = value; OnParentResized(); }
            get { return _top_type; }
        }

        public int bottomType
        {
            set { _bottom_type = value; OnParentResized(); }
            get { return _bottom_type; }
        }

        public int anchorX
        {
            set { _anchor_x = value; OnParentResized(); }
            get { return _anchor_x; }
        }

        public int anchorY
        {
            set { _anchor_y = value; OnParentResized(); }
            get { return _anchor_y; }
        }



        public void Awake()
        {
            XLayout parent_layout = null;

            try
            {
                parent_layout = transform.parent.GetComponent<XLayout>();
            }
            catch (Exception e)
            { }

            if (parent_layout != null)
                OnParentResized(transform.parent.GetComponent<RectTransform>());
        }

        //###########################################################
        //###              WILL CREATE A CHILD EVENT              ###
        //###########################################################
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            XLayout[] childs = this.GetComponentsInChildren<XLayout>();

            foreach (XLayout child in childs)
            {
                if (child != this)
                    child.OnParentResized(this.GetComponent<RectTransform>());
            }

        }


        private void resizeWidth(UElement elem, RectTransform parent)
        {
            if (_width_type == NONE)
                elem.width = parent.rect.width;
            if (_width_type == PERCENT)
                elem.width = parent.rect.width * (_width / 100f);
            else if (_width_type == PIXELS)
                elem.width = _width;
            else if (_width_type == ASPECT)
                elem.width = _width * elem.height;
        }

        private void resizeHeight(UElement elem, RectTransform parent)
        {
            if (_height_type == NONE)
                elem.height = parent.rect.height;
            else if (_height_type == PERCENT)
                elem.height = parent.rect.height * (_height / 100f);
            else if (_height_type == PIXELS)
                elem.height = _height;
            else if (_height_type == ASPECT)
                elem.height = _height * elem.width;
        }

        private RectTransform getParent()
        {
            return transform.parent.GetComponent<RectTransform>();
        }

        public void OnParentResized(RectTransform parent = null)
        {

            parent = getParent();

            RectTransform rt = this.GetComponent<RectTransform>();
            UElement elem = new UElement(rt.gameObject, false, false);
            float percent;

            if (_width_type == ASPECT)
            {
                resizeHeight(elem, parent);
                resizeWidth(elem, parent);
            }
            else
            {
                resizeWidth(elem, parent);
                resizeHeight(elem, parent);
            }


            elem.local_x = (-parent.rect.width / 2f);
            elem.local_y = (-parent.rect.height / 2f);

            if (_width_type == NONE)
            {
                //---------------------
                //--       LEFT      --
                //---------------------
                if (_left_type == PIXELS)
                {
                    elem.local_x += _left;
                    elem.width -= _left;
                }
                else if (_left_type == PERCENT)
                {
                    percent = (parent.rect.width * _left) / 100f;
                    elem.local_x += percent;
                    elem.width -= percent;
                }

                //---------------------
                //--      RIGHT      --
                //---------------------
                if (_right_type == PIXELS)
                {
                    elem.width -= _right;
                }
                else if (_right_type == PERCENT)
                {
                    percent = (parent.rect.width * _right) / 100f;
                    elem.width -= percent;
                }
            }


            if (_height_type == NONE)
            {

                //---------------------
                //--       TOP       --
                //---------------------
                if (_top_type == PIXELS)
                {
                    elem.height -= _top;
                }
                else if (_top_type == PERCENT)
                {
                    percent = (parent.rect.height * _top) / 100f;
                    elem.height -= percent;
                }

                //---------------------
                //--      BOTTOM     --
                //---------------------
                if (_bottom_type == PIXELS)
                {
                    elem.local_y += _bottom;
                    elem.height -= _bottom;
                }
                else if (_bottom_type == PERCENT)
                {
                    percent = (parent.rect.height * _bottom) / 100f;
                    elem.local_y += percent;
                    elem.height -= percent;
                }

            }




            //-----------------------------------------------------------------------------
            if ((_width_type == PIXELS) || (_width_type == PERCENT) || (_width_type == ASPECT))
            {
                //=====================
                //======= LEFT ========
                //=====================
                if (_left_type == PIXELS)
                {
                    elem.local_x += _left;
                }
                else if (_left_type == PERCENT)
                {
                    percent = (parent.rect.width * _left) / 100f;
                    elem.local_x += percent;
                }
                //=====================
                //======= RIGHT =======
                //=====================
                else if (_right_type == PIXELS)
                {
                    if ((_width_type == PIXELS) || (_width_type == PERCENT) || (_width_type == ASPECT))
                        elem.local_x = (parent.rect.width / 2f) - elem.width;
                    elem.local_x -= _right;
                }
                else if (_right_type == PERCENT)
                {
                    if ((_width_type == PIXELS) || (_width_type == PERCENT) || (_width_type == ASPECT))
                        elem.local_x = (parent.rect.width / 2f) - elem.width;
                    percent = (parent.rect.width * _right) / 100f;
                    elem.local_x -= percent;

                }
            }


            if ((_height_type == PIXELS) || (_height_type == PERCENT) || (_height_type == ASPECT))
            {
                //=====================
                //======= TOP  ========
                //=====================
                if (_top_type == PIXELS)
                {
                    if ((_height_type == PIXELS) || (_height_type == PERCENT) || (_height_type == ASPECT))
                        elem.local_y = (parent.rect.height / 2f) - elem.height;
                    elem.local_y -= _top;
                }
                else if (_top_type == PERCENT)
                {
                    if ((_height_type == PIXELS) || (_height_type == PERCENT) || (_height_type == ASPECT))
                        elem.local_y = (parent.rect.height / 2f) - elem.height;
                    percent = (parent.rect.height * _top) / 100f;
                    elem.local_y -= percent;
                }
                //=====================
                //======= BOTTOM ======
                //=====================
                else if (_bottom_type == PIXELS)
                {
                    elem.local_y += _bottom;
                }
                else if (_bottom_type == PERCENT)
                {
                    percent = (parent.rect.height * _bottom) / 100f;
                    elem.local_y += percent;
                }
            }
            //-------------------------------------------------------------------------------


            //=====================
            //====== ANCHOR X =====
            //=====================
            if (_anchor_x == ANCHOR_RIGHT)
                elem.local_x -= elem.width;
            else if (_anchor_x == ANCHOR_CENTER)
                elem.local_x -= elem.width / 2;

            //=====================
            //====== ANCHOR Y =====
            //=====================
            if (_anchor_y == ANCHOR_BOTTOM)
                elem.local_y += elem.height;
            else if (_anchor_y == ANCHOR_CENTER)
                elem.local_y += elem.height / 2;


        }

    }
}
