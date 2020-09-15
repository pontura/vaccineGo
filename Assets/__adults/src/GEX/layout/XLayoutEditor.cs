#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace GEX.layout
{
    [CustomEditor(typeof(XLayout))]
    public class XLayoutEditor : Editor
    {

        private GameObject _gobj = null;

        struct Prop
        {
            public float value;
            public int type;
            public float newValue;
            public int newType;
        }

        private float width_value;
        private int width_type;
        private float width_new_value;
        private int width_new_type;

        private float height_value;
        private int height_type;
        private float height_new_value;
        private int height_new_type;

        private Prop p_width, p_height, p_left, p_right, p_top, p_bottom;
        private Prop p_anchor_x, p_anchor_y;

        private String[] WIDTH_OPTIONS = new String[] { "none", "px", "%", "* height" };
        private String[] WIDTH_OPTIONS_BLOCKED_ASPECT = new String[] { "none", "px", "%" };
        private String[] HEIGHT_OPTIONS = new String[] { "none", "px", "%", "* width" };
        private String[] HEIGHT_OPTIONS_BLOCKED_ASPECT = new String[] { "none", "px", "%" };

        private const int NONE = 0;
        private const int PIXELS = 1;
        private const int PERCENT = 2;
        private const int ASPECT = 3;

        private String[] ANCHOR_X = new String[] { "left  *", "right", "center" };
        private String[] ANCHOR_Y = new String[] { "top  *", "bottom", "center" };

        private const int ANCHOR_LEFT = 0;
        private const int ANCHOR_RIGHT = 1;
        private const int ANCHOR_TOP = 0;
        private const int ANCHOR_BOTTOM = 1;
        private const int ANCHOR_CENTER = 2;

        private void init()
        {
            RectTransform r;

            if (_gobj == null)
            {
                XLayout layout = (XLayout)target;
                _gobj = layout.gameObject;
                r = _gobj.GetComponent<RectTransform>();

                p_width = new Prop();
                p_height = new Prop();
                p_left = new Prop();
                p_right = new Prop();
                p_top = new Prop();
                p_bottom = new Prop();

                p_anchor_x = new Prop();
                p_anchor_y = new Prop();

                p_width.value = p_width.newValue = layout.width;
                p_width.type = p_width.newType = layout.widthType;

                p_height.value = p_height.newValue = layout.height;
                p_height.type = p_height.newType = layout.heightType;

                p_left.value = p_left.newValue = layout.left;
                p_left.type = p_left.newType = layout.leftType;

                p_right.value = p_right.newValue = layout.right;
                p_right.type = p_right.newType = layout.rightType;

                p_top.value = p_top.newValue = layout.top;
                p_top.type = p_top.newType = layout.topType;

                p_bottom.value = p_bottom.newValue = layout.bottom;
                p_bottom.type = p_bottom.newType = layout.bottomType;


                p_anchor_x.type = p_anchor_x.newType = layout.anchorX;
                p_anchor_y.type = p_anchor_y.newType = layout.anchorX;
            }

        }


        public override void OnInspectorGUI()
        {

            bool any_value_changed;
            string[] options;

            init();




            DrawDefaultInspector();

            XLayout layout = (XLayout)target;


            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            //It's the layout base. There is no parent before this
            if (layout.gameObject.transform.parent.GetComponent<XLayout>() == null)
            {
                EditorGUILayout.HelpBox("This layout component is the base for the other childs.", MessageType.Info);
                EditorGUILayout.HelpBox("It can't be modified.", MessageType.Info);
                return;
            }

            //----------------------------------
            //--             WIDTH            --
            //----------------------------------
            EditorGUILayout.BeginHorizontal();
            if (p_width.type == NONE)
                GUI.enabled = false;
            GUILayout.Label("width:", GUILayout.Width(70));
            p_width.newValue = EditorGUILayout.FloatField(p_width.value, GUILayout.Width(100));
            GUI.enabled = true;
            if (p_height.type == ASPECT)
                options = WIDTH_OPTIONS_BLOCKED_ASPECT;
            else
                options = WIDTH_OPTIONS;
            p_width.newType = EditorGUILayout.Popup(p_width.type, options, GUILayout.Width(80));
            EditorGUILayout.EndHorizontal();

            //----------------------------------
            //--             HEIGHT           --
            //----------------------------------
            EditorGUILayout.BeginHorizontal();
            if (p_height.type == NONE)
                GUI.enabled = false;
            GUILayout.Label("height:", GUILayout.Width(70));
            p_height.newValue = EditorGUILayout.FloatField(p_height.value, GUILayout.Width(100));
            GUI.enabled = true;
            if (p_width.type == ASPECT)
                options = HEIGHT_OPTIONS_BLOCKED_ASPECT;
            else
                options = HEIGHT_OPTIONS;
            p_height.newType = EditorGUILayout.Popup(p_height.type, options, GUILayout.Width(80));
            EditorGUILayout.EndHorizontal();

            //----------------------------------
            //--             LEFT             --
            //----------------------------------
            EditorGUILayout.BeginHorizontal();
            if (p_left.type == NONE)
                GUI.enabled = false;
            GUILayout.Label("left:", GUILayout.Width(70));
            p_left.newValue = EditorGUILayout.FloatField(p_left.value, GUILayout.Width(100));
            GUI.enabled = true;
            p_left.newType = EditorGUILayout.Popup(p_left.type, new String[] { "none", "px", "%" }, GUILayout.Width(80));
            EditorGUILayout.EndHorizontal();

            //----------------------------------
            //--             RIGHT            --
            //----------------------------------
            EditorGUILayout.BeginHorizontal();
            if ((p_width.type != NONE) && (p_left.type != NONE)) GUI.enabled = false;
            if (p_right.type == NONE)
                GUI.enabled = false;
            GUILayout.Label("right:", GUILayout.Width(70));
            p_right.newValue = EditorGUILayout.FloatField(p_right.value, GUILayout.Width(100));
            GUI.enabled = true;
            if ((p_width.type != NONE) && (p_left.type != NONE)) GUI.enabled = false;
            p_right.newType = EditorGUILayout.Popup(p_right.type, new String[] { "none", "px", "%" }, GUILayout.Width(80));
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            //----------------------------------
            //--             TOP              --
            //----------------------------------
            EditorGUILayout.BeginHorizontal();
            if (p_top.type == NONE)
                GUI.enabled = false;
            GUILayout.Label("top:", GUILayout.Width(70));
            p_top.newValue = EditorGUILayout.FloatField(p_top.value, GUILayout.Width(100));
            GUI.enabled = true;
            p_top.newType = EditorGUILayout.Popup(p_top.type, new String[] { "none", "px", "%" }, GUILayout.Width(80));
            EditorGUILayout.EndHorizontal();

            //----------------------------------
            //--             BOTTOM           --
            //----------------------------------
            if ((p_height.type != NONE) && (p_top.type != NONE)) GUI.enabled = false;
            EditorGUILayout.BeginHorizontal();
            if (p_bottom.type == NONE)
                GUI.enabled = false;
            GUILayout.Label("bottom:", GUILayout.Width(70));
            p_bottom.newValue = EditorGUILayout.FloatField(p_bottom.value, GUILayout.Width(100));
            GUI.enabled = true;
            if ((p_height.type != NONE) && (p_top.type != NONE)) GUI.enabled = false;
            p_bottom.newType = EditorGUILayout.Popup(p_bottom.type, new String[] { "none", "px", "%" }, GUILayout.Width(80));
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.Space();
            EditorGUILayout.Space();



            //----------------------------------
            //--         ANCHOR X             --
            //----------------------------------
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("anchor X:", GUILayout.Width(70));
            p_anchor_x.newType = EditorGUILayout.Popup(p_anchor_x.type, ANCHOR_X, GUILayout.Width(80));
            EditorGUILayout.EndHorizontal();

            //----------------------------------
            //--         ANCHOR Y             --
            //----------------------------------
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("anchor Y:", GUILayout.Width(70));
            p_anchor_y.newType = EditorGUILayout.Popup(p_anchor_y.type, ANCHOR_Y, GUILayout.Width(80));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("* Leave anchor X as left and anchor Y as top if you want to use the padding mode.", MessageType.Info);



            //##################################
            //###        VALUE CHANGED       ###
            //##################################
            any_value_changed = false;


            //----------------------------------
            //--             WIDTH            --
            //----------------------------------
            if (p_width.value != p_width.newValue)
            {
                p_width.value = p_width.newValue;
                layout.width = p_width.value;
                any_value_changed = true;
            }

            if (p_width.type != p_width.newType)
            {
                p_width.type = p_width.newType;
                layout.widthType = p_width.type;
                any_value_changed = true;
            }

            //----------------------------------
            //--            HEIGHT            --
            //----------------------------------
            if (p_height.value != p_height.newValue)
            {
                p_height.value = p_height.newValue;
                layout.height = p_height.value;
                any_value_changed = true;
            }

            if (p_height.type != p_height.newType)
            {
                p_height.type = p_height.newType;
                layout.heightType = p_height.type;
                any_value_changed = true;
            }

            //----------------------------------
            //--            LEFT              --
            //----------------------------------
            if (p_left.value != p_left.newValue)
            {
                p_left.value = p_left.newValue;
                layout.left = p_left.value;
                any_value_changed = true;
            }

            if (p_left.type != p_left.newType)
            {
                p_left.type = p_left.newType;
                layout.leftType = p_left.type;
                any_value_changed = true;
            }

            //----------------------------------
            //--            RIGHT             --
            //----------------------------------
            if (p_right.value != p_right.newValue)
            {
                p_right.value = p_right.newValue;
                layout.right = p_right.value;
                any_value_changed = true;
            }

            if (p_right.type != p_right.newType)
            {
                p_right.type = p_right.newType;
                layout.rightType = p_right.type;
                any_value_changed = true;
            }

            //----------------------------------
            //--            TOP               --
            //----------------------------------
            if (p_top.value != p_top.newValue)
            {
                p_top.value = p_top.newValue;
                layout.top = p_top.value;
                any_value_changed = true;
            }

            if (p_top.type != p_top.newType)
            {
                p_top.type = p_top.newType;
                layout.topType = p_top.type;
                any_value_changed = true;
            }

            //----------------------------------
            //--            BOTTOM            --
            //----------------------------------
            if (p_bottom.value != p_bottom.newValue)
            {
                p_bottom.value = p_bottom.newValue;
                layout.bottom = p_bottom.value;
                any_value_changed = true;
            }

            if (p_bottom.type != p_bottom.newType)
            {
                p_bottom.type = p_bottom.newType;
                layout.bottomType = p_bottom.type;
                any_value_changed = true;
            }

            //----------------------------------
            //--          ANCHOR X            --
            //----------------------------------
            if (p_anchor_x.type != p_anchor_x.newType)
            {
                p_anchor_x.type = p_anchor_x.newType;
                layout.anchorX = p_anchor_x.type;
                any_value_changed = true;
            }

            //----------------------------------
            //--          ANCHOR Y            --
            //----------------------------------
            if (p_anchor_y.type != p_anchor_y.newType)
            {
                p_anchor_y.type = p_anchor_y.newType;
                layout.anchorY = p_anchor_y.type;
                any_value_changed = true;
            }

            /*if (any_value_changed)
                SEND_VALUES();*/


        }

        /*private void SEND_VALUES()
        {
            XLayout layout = (XLayout)target;
            XLayout parent_layout = layout.gameObject.transform.parent.GetComponent<XLayout>();
            if (parent_layout != null)
                layout.OnParentResized(layout.gameObject.transform.parent.GetComponent<RectTransform>());
        }*/

    }
}
#endif