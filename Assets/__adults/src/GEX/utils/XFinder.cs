using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace GEX.utils
{
	public class XFinder
	{

        private static List<GameObject> saved_childs = new List<GameObject>();


        public static List<GameObject> getChildsAtRootLevel(GameObject root)
        {
            int total = root.transform.childCount;
            List<GameObject> GOBJS = new List<GameObject>();

            for (int a = 0; a < total; a++ )
                GOBJS.Add(root.transform.GetChild(a).gameObject);

            return GOBJS;
        }


        public static List<GameObject> getAllChilds(GameObject root)
        { 
            if(saved_childs == null)
                saved_childs = new List<GameObject>();
            else
                saved_childs.Clear();

            _getAllChilds(root);

            return saved_childs;
        }


        private static void _getAllChilds(GameObject root)
        {

            foreach (Transform trans in root.transform)
            {
                //Debug.Log (trans.name);
                saved_childs.Add(trans.gameObject);
                _getAllChilds(trans.gameObject);
            }
        }

        public static GameObject [] getAllHierarchyGameObjects(String [] doNotInclude = null)
        {
            object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
            List<GameObject> GOBJS_AT_0 = new List<GameObject>();
            List<GameObject> NOT_REPEATED = new List<GameObject>();
            GameObject _root_;
            int a;
            foreach (object o in obj)
            {
                GameObject g = (GameObject)o;

                if (g.name == g.transform.root.name)
                    GOBJS_AT_0.Add(g);
            }


            for (a = 0; a < GOBJS_AT_0.Count; a++)
            {
                _root_ = GOBJS_AT_0[a].transform.root.gameObject;

                if (!NOT_REPEATED.Contains(_root_))
                {
                    if (doNotInclude == null)
                        NOT_REPEATED.Add(_root_);
                    else
                    {
                        if (!existInList(_root_.name, doNotInclude))
                            NOT_REPEATED.Add(_root_);
                    }
                }
            }

            return NOT_REPEATED.ToArray();
        }

        private static bool existInList(String name, String [] list)
        {
            for (int a = 0; a < list.Length; a++)
            {
                if (name == list[a])
                    return true;
            }

            return false;
        }





        public static GameObject findAt(GameObject root, String name)
        {
            List<GameObject> GOBJS = getAllChilds(root);

            for (int a = 0; a < GOBJS.Count; a++)
            {
                if (GOBJS[a].name == name)
                    return GOBJS[a];
            }

            return null;
        }

        public static GameObject findRecursively(String name)
        {
            GameObject[] gobjs = getAllHierarchyGameObjects();

            for (int a = 0; a < gobjs.Length; a++)
            {
                if (gobjs[a].name == name)
                    return gobjs[a];
            }

            return null;
        }



	}
}
