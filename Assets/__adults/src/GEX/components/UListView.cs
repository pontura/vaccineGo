using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using GEX.utils;
//using com.data;

namespace GEX.components
{
    public class UListView : MonoBehaviour
	{
    
        public RectTransform listArea;
        public Scrollbar scrollbar;
        public int itemSize = 20;
        public Boolean autoHideScrollBar =  true;
        public Boolean autoResize =         false;
        public Boolean topWhenNewItem =     false;
        public Boolean bottomWhenNewItem =  false;
        public Boolean enableClickOnSelectedItems = true;
        public Boolean autoInit =           true;

        protected UXElement root;
        protected RectTransform scrollViewRect;
        protected UXElement LIST;
        protected UElement CONTENT;
        protected Vector2 saved_anchored_position;
        protected Vector2 saved_size_delta;

        protected List<UXElement> ELEMENTS;
        protected List<System.Object> CUSTOM_OBJECTS;

        protected float ORIGINAL_HEIGHT;
        protected int _selectedIndex;

        protected Func<GameObject, int> _func_selected_item_changed;
        protected Func<int, int> _func_selected_index_changed;
        protected Func<GameObject, int, int> _func_selected_item_and_index_changed;

        public void Awake()
        {
            if (autoInit)
                _init();
        }

        public void _init()
        {
            root = new UXElement(this.gameObject, false, false);

            LIST = new UXElement(listArea.gameObject, true, false);

            //Debug.Log(LIST.width + ", " + LIST.height);
            ORIGINAL_HEIGHT = LIST.height;
            //Debug.Log("ORIGINAL_HEIGHT: " + ORIGINAL_HEIGHT);

            

            LIST.anchorMinY = 1;
            LIST.height = ORIGINAL_HEIGHT;
            ELEMENTS = new List<UXElement>();
            CUSTOM_OBJECTS = new List<object>();

            scrollViewRect = listArea.parent.GetComponent<RectTransform>();


            //=========== save original values ==========
            saved_anchored_position =   new Vector2(scrollViewRect.anchoredPosition.x, scrollViewRect.anchoredPosition.y);
            saved_size_delta =          new Vector2(scrollViewRect.sizeDelta.x, scrollViewRect.sizeDelta.y);
            //============================================

            if (autoHideScrollBar)
                _HIDE_SCROLL_BAR();


            _selectedIndex = -1;
        }

        public void setScrollBarValue(float value)
        {
            scrollbar.value = value;
        }

        private void setOriginalLimitHeight()
        {
            root.height = ORIGINAL_HEIGHT;
            root.y = 0 - (ORIGINAL_HEIGHT / 2);
        }

        private void resizeToFit(float total_height)
        { 
            root.height = total_height;
            root.y = 0 - (total_height / 2);

            LIST.height = total_height;
            LIST.y = 0 - (total_height / 2);
        }


        public int Count
        {
            get { return ELEMENTS.Count; }
        }


        private void _HIDE_SCROLL_BAR()
        {
            scrollViewRect.anchoredPosition =   new Vector3(0, scrollViewRect.anchoredPosition.y);
            scrollViewRect.sizeDelta =          new Vector2(0, scrollViewRect.sizeDelta.y);
            
            scrollbar.gameObject.SetActive(false);
        }

        private void _SHOW_SCROLL_BAR()
        {
            scrollViewRect.anchoredPosition3D = saved_anchored_position;
            scrollViewRect.sizeDelta = saved_size_delta;
            scrollbar.gameObject.SetActive(true);
        }

        private void reorder()
        {
            UXElement uelement;
            for (int a = 0; a < ELEMENTS.Count; a++)
            {
                uelement = ELEMENTS[a];
                uelement.y = (-uelement.height / 2) - (a * uelement.height);
            }
        }

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@         remove item         @@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        public void removeItem(int index)
        {

            GameObject.Destroy(ELEMENTS[index].gameobject);
            ELEMENTS.Remove(ELEMENTS[index]);
            CUSTOM_OBJECTS.Remove(CUSTOM_OBJECTS[index]);

            reorder();

            float total_height = itemSize * ELEMENTS.Count;

            if (total_height < LIST.height)
            {
                if (total_height < ORIGINAL_HEIGHT)
                {
                    if (autoHideScrollBar)
                        _HIDE_SCROLL_BAR();

                    if (autoResize)
                        resizeToFit(total_height);
                    else
                        LIST.height = ORIGINAL_HEIGHT;
                }
                else
                    LIST.height = total_height;
            }

            //scrollbar.value = 0;
        }

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@          add item           @@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        public void addItem(GameObject gobj, System.Object customObject = null)
        {
            gobj.transform.parent = listArea.transform;

            if (gobj.GetComponent<UButton>() == null)
                gobj.AddComponent<UButton>();

            //gobj.GetComponent<UButton>().addEventOnClick(onElementClick);
            UButton ubutton = gobj.GetComponent<UButton>();
            ubutton.addEventOnMouseDown(onElementClick);
            if (enableClickOnSelectedItems)
                ubutton.allowMouseDownWhenDisabled();

            UXElement uelement = new UXElement(gobj, true, true);


            //uelement.width = LIST.width;
            uelement.height = itemSize;
            //uelement.x = uelement.width / 2;
            uelement.y = (-uelement.height / 2) - (ELEMENTS.Count * uelement.height);
            
            uelement.anchorMaxX = 1;
            uelement.width = 0;// it is like RIGHT = 0
            uelement.x = 0;


            ELEMENTS.Add(uelement);
            CUSTOM_OBJECTS.Add(customObject);

            float total_height = uelement.height * ELEMENTS.Count;

            if (total_height > ORIGINAL_HEIGHT)
            {
                LIST.height = total_height;

                if (autoResize)
                    setOriginalLimitHeight();

                if (autoHideScrollBar)
                {
                    _SHOW_SCROLL_BAR();
                }
            }
            else
            {
                if (autoResize)
                    resizeToFit(total_height);
            }

            //Debug.Log("List H:" + LIST.height + "  " + total_height);

            if(topWhenNewItem)
                scrollbar.value = 1;
            else if(bottomWhenNewItem)
                scrollbar.value = 0;


        }

        protected int getSelectedIndex(GameObject gobj)
        {
            for (int a = 0; a < ELEMENTS.Count; a++)
            {
                if (ELEMENTS[a].gameobject == gobj)
                    return a;
            }

            return -1;
        }

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@          get item           @@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        public GameObject getItemAt(int index)
        { 
            return ELEMENTS[index].gameobject;
        }

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@       get custom object     @@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        public System.Object getCustomObject(int index)
        {
            return CUSTOM_OBJECTS[index];
        }

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@            clear            @@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        public void clear()
        {
            for (int a = 0; a < ELEMENTS.Count; a++)
            { 
                GameObject.Destroy(ELEMENTS[a].gameobject);
            }
            ELEMENTS.Clear();
            CUSTOM_OBJECTS.Clear();
        }


        //#################################################################
        //#####                                                       #####
        //#################################################################
        //#####                                                       #####
        //#####                  EVENTS: triggers                     #####
        //#####                                                       #####
        //#################################################################
        protected virtual int onElementClick(GameObject sender)
        {
            
            int index = getSelectedIndex(sender);

            //Debug.Log("clicked: " + sender + " INDEX:" + index);
            //removeItem(index);
            
            if (_func_selected_item_changed != null)
                _func_selected_item_changed.DynamicInvoke(sender);

            if (_func_selected_index_changed != null)
                _func_selected_index_changed.DynamicInvoke(index);

            if (_func_selected_item_and_index_changed != null)
                _func_selected_item_and_index_changed.DynamicInvoke(sender, index);

            return 0;
        }


        //#################################################################
        //#####                                                       #####
        //#################################################################
        //#####                                                       #####
        //#####                  EVENTS: listeners                    #####
        //#####                                                       #####
        //#################################################################

        public void addEventOnSelectedItemChanged(Func<GameObject, int> returnedFunction)
        {
            _func_selected_item_changed = returnedFunction;
        }

        public void addEventOnSelectedItemChanged(Func<GameObject, int, int> returnedFunction)
        {
            _func_selected_item_and_index_changed = returnedFunction;
        }

        public void addEventOnSelectedIndexChanged(Func<int, int> returnedFunction)
        {
            _func_selected_index_changed = returnedFunction;
        }

        
        //#################################################################
        //#################################################################


	}
}
