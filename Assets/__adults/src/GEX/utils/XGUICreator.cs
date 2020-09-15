using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;


namespace GEX.utils
{
	public class XGUICreator
	{

        public static void removeAllRects(GameObject root)
        {
            List<GameObject> gobjs = XFinder.getAllChilds(root);

            foreach (GameObject gobj in gobjs)
            { 
                if(gobj.name == "xgui@draw:line")
                    GameObject.DestroyImmediate(gobj);
            }
        }


        public static void drawRect(GameObject root, float x, float y, float width, float height, Color color)
        {
            GameObject gobj = new GameObject();
            gobj.name = "xgui@draw:line";
            
            gobj.transform.parent = root.transform;

            Image img = gobj.AddComponent<Image>();
            img.color = color;

            UElement uelement = new UElement(gobj, false);

            uelement.x = x;
            uelement.y = y;
            uelement.width = width;
            uelement.height = height;

            uelement.destroy();
            uelement = null;
        }




	}
}
