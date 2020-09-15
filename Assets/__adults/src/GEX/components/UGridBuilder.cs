using UnityEngine;
using System.Collections;
using GEX.utils;
using System.Collections.Generic;
using GEX.graphics;

namespace GEX.components
{

    
    public class UGridBuilder : MonoBehaviour
	{
        public int totalX;
        [Header("static if <= 0")]
        public int totalY = 0;
        public XPadding padding;
        public bool showGridGuide = false;
        public bool inverseOrder = false;
        public bool _AUTO_ARRANGE_HARD_CODE = false;

        public GameObject[] ITEMS
        {
            get 
            {
                List<GameObject> gobjs = new List<GameObject>();
                foreach (Transform child in transform)
                    gobjs.Add(child.gameObject);

                return gobjs.ToArray();
            } 
        }

        public int totalItems
        {
            get { return ITEMS.Length; }
        }

        // -------------------------
        // ONLY works if totalY > 0
        // -------------------------
        public int maxItems
        {
            get 
            {
                if (totalY > 0)
                    return (totalX * totalY);
                else
                    return totalItems;
            }
        }

        public void BuildObject()
        {

        }

        public void setSize(int x, int y)
        {
            totalX = x;
            totalY = y;
        }

        // -------------------------
        // ONLY works if totalY > 0
        // -------------------------
        public bool isFull()
        {
            return (totalItems >= maxItems);
        }

        public bool isEmpty()
        { 
            return (totalItems <= 0);
        }

        private List<UElement> createUElements()
        {

            List<GameObject> GOBJS = XFinder.getChildsAtRootLevel(this.gameObject);
            List<UElement> uelements = new List<UElement>();

            for (int a = 0; a < GOBJS.Count; a++)
            {
                uelements.Add(new UElement(GOBJS[a], false, true));
            }

            return uelements;
        }


        public void arrange()
        {
            int _totalY;
            UElement ROOT = new UElement(this.gameObject);
            int root_width, root_height;
            int x_count, y_count;
            RectTransform ROOT_RT = this.gameObject.GetComponent<RectTransform>();
            root_width = (int)ROOT_RT.rect.width;
            root_height = (int)ROOT_RT.rect.height;
            List<UElement> ITEMS;
            UElement item;
            int cont_width, cont_height;


            if (showGridGuide)
                XGUICreator.removeAllRects(this.gameObject);

            if (padding == null)
                padding = new XPadding();
            

            ITEMS = createUElements();


            // --------------------
            // STATIC (pre-defined)
            // --------------------
            if (totalY > 0)
                _totalY = totalY;
            else
            // --------------------
            // DYNAMIC
            // --------------------
                _totalY = (int)Mathf.Ceil(ITEMS.Count / (float)totalX);



            if ((_totalY == 0) || (ITEMS.Count == 0))
                return;

            cont_width =    root_width / totalX;
            cont_height = root_height / _totalY;
            
            //Debug.Log("ROOT(" + root_width + "," + root_height + ")");
            //Debug.Log("Totals:" + totalX + " & " + totalY);


            float offset_x = ROOT.x - (ROOT.width / 2);
            float offset_y = ROOT.y - (ROOT.height / 2);
            float left_padding_offset;
            float right_padding_offset;
            float top_padding_offset;
            float bottom_padding_offset;

            left_padding_offset = cont_width - (cont_width * (1 - padding.left));
            right_padding_offset = cont_width - (cont_width * (1 - padding.right));

            top_padding_offset = cont_height - (cont_height * (1 - padding.top));
            bottom_padding_offset = cont_height - (cont_height * (1 - padding.bottom));

            x_count = y_count = 0;

            for (int a = 0; a < ITEMS.Count; a++)
            {
                item = ITEMS[a];
                
                item.width = cont_width - left_padding_offset - right_padding_offset;
                item.height = cont_height - top_padding_offset - bottom_padding_offset;

                item.x = offset_x + (x_count * cont_width) + left_padding_offset;

                if (inverseOrder)
                    item.y = offset_y + (y_count * cont_height) + bottom_padding_offset;
                else
                    item.y = offset_y + ((_totalY - y_count - 1) * cont_height) + bottom_padding_offset;

                if (++x_count >= totalX)
                {
                    x_count = 0;
                    y_count++;
                }
            }


            if (showGridGuide)
            {
                for (int _y = 0; _y < _totalY + 1; _y++)
                    XGUICreator.drawRect(this.gameObject, offset_x + 0, offset_y + (_y * cont_height), root_width, 2, Color.white);

                for (int _x = 0; _x < totalX + 1; _x++)
                    XGUICreator.drawRect(this.gameObject, offset_x + (_x * cont_width), offset_y + 0, 2, root_height, Color.white);
            }




            item = ITEMS[0];


        }

        public void addItem(GameObject gobj)
        {
            gobj.transform.SetParent(this.transform);
        }

        public void clear()
        {
            foreach (Transform child in this.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public void Start()
        {

            arrange();


        }

        public void Update()
        {
            if (_AUTO_ARRANGE_HARD_CODE)
            {
                arrange();
            }
        }

	}
}
