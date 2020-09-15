using UnityEngine;
using System;
//using com.define;

namespace GEX.debug
{
	class XConsole
	{
        private static Vector2 scrollPosition;
        private static String output =          "";
        private static bool internal_output=    false;
        private static bool ENABLE =            true;
        private static bool error = false;

        public static void setActive(bool active)
        {
            if (active)
            {
                ENABLE = true;
                internal_output = true;
                //Config.DBG_SHOW_XCONSOLE = true;
            }
            else
            {
                ENABLE = false;
                //Config.DBG_SHOW_XCONSOLE = false;
            }
        }

        //#####################################
        //###########    PRINT     ############
        //#####################################
        public static void print(String str)
        {
            if (ENABLE)
            {
                error = false;
                if (internal_output)
                    output += str;
                else
                    Debug.Log(str);
            }
        }

        //#####################################
        //###########    PRINTERROR     ##########
        //#####################################
        public static void printError(String str)
        {
            if (ENABLE)
            {
                error = true;
                if (internal_output)
                    output += str + "\n";
                else
                    Debug.Log(str);
            }
        }

        //#####################################
        //###########    PRINTLN     ##########
        //#####################################
        public static void println(String str)
        {
            if (ENABLE)
            {
                error = false;
                if (internal_output)
                    output += str + "\n";
                else
                    Debug.Log(str);
            }
        }

        //#####################################
        //########    PRINT TITLE     #########
        //#####################################
        public static void printTitle(String str)
        {
            if (ENABLE)
            {
                error = false;
                String title = "--------------------------\n---     " + str + "\n--------------------------";

                if (internal_output)
                    output += title + "\n";
                else
                    Debug.Log(title);
            }
        }

        //#####################################
        //########  PRINT END LINE    #########
        //#####################################
        public static void printEndLine()
        {
            if (ENABLE)
            {
                error = false;
                String end = "--------------------------";

                if (internal_output)
                    output += end + "\n";
                else
                    Debug.Log(end);
            }
        }

        //#####################################
        //###########     CLEAR    ############
        //#####################################
        public static void clear()
        { 
            output = "";
        }

        //#####################################
        //###########    ENABLE    ############
        //#####################################
        public static void enable()
        {
            ENABLE = true;
        }

        //#####################################
        //###########    DISABLE    ###########
        //#####################################
        public static void disable()
        {
            ENABLE = false;
        }

        //#####################################
        //#########    SET OUTPUT    ##########
        //#####################################
        public static void setOutputAs(bool _internal)
        {
            internal_output = _internal;
        }

        //#####################################
        //###########    UPDATE    ############
        //#####################################
        public static void update()
        {
            Color savedColor = Color.white;

            if (internal_output)
            {
                //if (scrollPosition == null)
                //  scrollPosition = new Vector2();

                GUI.Box(new Rect(25, 25, Screen.width - 50, Screen.height - 50), "Debugging console");
                GUILayout.BeginArea(new Rect(50, 50, Screen.width - 50, Screen.height - 50));
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width - 50), GUILayout.Height(Screen.height - 100));

                GUI.skin.box.wordWrap = true; // set the wordwrap on for box only.
                if (error)
                {
                    savedColor = GUI.contentColor;
                    GUI.contentColor = Color.red;
                }
                
                GUILayout.Box(output);

                if (error)
                {
                    GUI.contentColor = savedColor;
                }

                GUILayout.EndScrollView();

                GUILayout.EndArea();
            }
        }

	}
}
