using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using GEX.components;

namespace GEX.utils
{
	public class XGUI
	{
        public static T findIn<T>(String name, GameObject container) where T : Component
        {
            List<GameObject> GOBJS = XFinder.getAllChilds(container);
            T component;

            for (int a = 0; a < GOBJS.Count; a++)
            {
                if (GOBJS[a].name == name)
                {
                    component = GOBJS[a].GetComponent<T>();

                    if (component != null)
                        return component;
                }

            }

            return null;
        }

        public static GameObject findGameObjectIn(String name, GameObject gobj)
        {
            List<GameObject> GOBJS = XFinder.getAllChilds(gobj);

            for (int a = 0; a < GOBJS.Count; a++)
            {
                if (GOBJS[a].name == name)
                    return GOBJS[a];
            }

            return null;
        }

        public static Image findImageIn(String name, GameObject gobj)
        {
            List<GameObject> GOBJS = XFinder.getAllChilds(gobj);
            Image component;

            for (int a = 0; a < GOBJS.Count; a++)
            {
                if (GOBJS[a].name == name)
                {
                    component = GOBJS[a].GetComponent<Image>();

                    if (component != null)
                        return component;
                }

            }

            return null;
        }

        public static Text findTextIn(String name, GameObject gobj)
        {
            List<GameObject> GOBJS = XFinder.getAllChilds(gobj);
            Text component;

            for (int a = 0; a < GOBJS.Count; a++)
            {
                if (GOBJS[a].name == name)
                {
                    component = GOBJS[a].GetComponent<Text>();

                    if (component != null)
                        return component;
                }

            }

            return null;
        }

        public static UToggleButton findUToggleButtonIn(String name, GameObject gobj)
        {
            List<GameObject> GOBJS = XFinder.getAllChilds(gobj);
            UToggleButton component;

            for (int a = 0; a < GOBJS.Count; a++)
            {
                if (GOBJS[a].name == name)
                {
                    component = GOBJS[a].GetComponent<UToggleButton>();

                    if (component != null)
                        return component;
                }

            }

            return null;
        }

        public static List<UButton> findUButtonsIn(String containsInName, GameObject gobj)
        {
            List<UButton> UBUTTONS = new List<UButton>();
            List<GameObject> GOBJS = XFinder.getAllChilds(gobj);
            UButton component;

            for (int a = 0; a < GOBJS.Count; a++)
            {
                if (GOBJS[a].name != "")
                {
                    if (GOBJS[a].name.Contains(containsInName))
                    {
                        component = GOBJS[a].GetComponent<UButton>();

                        if (component != null)
                            UBUTTONS.Add(component);
                    }
                }
            }

            return UBUTTONS;
        }

        public static UButton findUButtonIn(String name, GameObject gobj)
        { 
            List<GameObject> GOBJS = XFinder.getAllChilds(gobj);
            UButton component;

            for (int a = 0; a < GOBJS.Count; a++)
            {
                if (GOBJS[a].name == name)
                {
                    component = GOBJS[a].GetComponent<UButton>();

                    if (component != null)
                        return component;
                }

            }

            return null;
        }

        public static UInputField findUInputField(String name, GameObject gobj)
        {
            List<GameObject> GOBJS = XFinder.getAllChilds(gobj);
            UInputField component;

            for (int a = 0; a < GOBJS.Count; a++)
            {
                if (GOBJS[a].name == name)
                {
                    component = GOBJS[a].GetComponent<UInputField>();

                    if (component != null)
                        return component;
                }

            }

            return null;
        }


        public static UButton findUButton(String name)
        {
            
            UButton [] ubuttons = GameObject.FindObjectsOfType<UButton>();

            foreach (UButton ubutton in ubuttons)
            {
                if (ubutton.name == name)
                    return ubutton;
            }
            

            return null;
        }


        public static void setIndex(GameObject gui, int index)
        {
            RectTransform RT = gui.GetComponent<RectTransform>();
            RT.SetSiblingIndex(index);
        }

        public static void setAtFront(GameObject gui)
        {
            RectTransform RT = gui.GetComponent<RectTransform>();
            RT.SetAsLastSibling();
        }

        public static void setAtBack(GameObject gui)
        {
            RectTransform RT = gui.GetComponent<RectTransform>();
            RT.SetAsLastSibling();
        }

        public static GameObject addGUI(String assetName, bool visible = true, String to = "Canvas")
        {
            GameObject gui = Resources.Load("GAME_GUI/" + assetName) as GameObject;
            return addGUI(gui, visible, to);
        }

        public static GameObject addGUI(GameObject gui, bool visible = true, String to = "Canvas")
        {
            GameObject canvas;
            GameObject mcontainer;

            gui = GameObject.Instantiate(gui) as GameObject;
            
            //fixSavedPrefabPositions(gui);

            canvas = GameObject.Find(to);

            mcontainer = gui.transform.GetChild(0).gameObject;
            mcontainer.SetActive(visible);
            mcontainer.transform.parent = canvas.transform;
            GameObject.Destroy(gui);

            return mcontainer;
        }


        public static GameObject addGUIXT(GameObject gui, bool visible = true, Transform to = null)
        {

            gui = GameObject.Instantiate(gui) as GameObject;
            gui.SetActive(visible);
            gui.transform.parent = to;

            return gui;
        }

        public static GameObject addGUIX(GameObject gui, bool visible = true, String to = "Canvas")
        {
            GameObject canvas;

            gui = GameObject.Instantiate(gui) as GameObject;
            canvas = GameObject.Find(to);

            gui.SetActive(visible);
            gui.transform.parent = canvas.transform;

            return gui;
        }

        public static GameObject moveToGUI(GameObject gui, bool visible = true, String to = "Canvas")
        {
            GameObject canvas;

            canvas = GameObject.Find(to);
            gui.SetActive(visible);
            gui.transform.parent = canvas.transform;

            return gui;
        }

        public static void fixSavedPrefabPositions(GameObject gobj)
        {
            GameObject curr_obj;
            RectTransform RT;

            List<GameObject> GOBJS = XFinder.getAllChilds(gobj);


            for (int a = 0; a < GOBJS.Count; a++)
            {

                curr_obj = GOBJS[a];
                RT = null;
                RT = curr_obj.GetComponent<RectTransform>();

                if (RT != null)
                {
                    RT.anchoredPosition = new Vector2(0, 0);
                }

            }


        }




	}
}
