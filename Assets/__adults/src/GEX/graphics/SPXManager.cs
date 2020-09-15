using UnityEngine;
using System;
using System.Collections.Generic;


namespace GEX.graphics
{
	public class SPXManager
	{

        private static Camera camera2D;
        private static List<Spritex> sprites;

        public static float PIXELS_TO_UNIT = 100;
        public static float X_MAX, Y_MAX;
        public static float SCREEN_WIDTH_BY_2, SCREEN_HEIGHT_BY_2;

        public static void initialize(Camera camera2D)
        { 
            SPXManager.camera2D = camera2D;
            SPXManager.camera2D.orthographicSize = (Screen.height / 2) / PIXELS_TO_UNIT;

            //X_MAX = (Screen.width / 2) / PIXELS_TO_UNIT;
            //Y_MAX = (Screen.height / 2) / PIXELS_TO_UNIT;
            X_MAX = (Screen.width / 1) / PIXELS_TO_UNIT;
            Y_MAX = (Screen.height / 1) / PIXELS_TO_UNIT;


            SCREEN_WIDTH_BY_2 =     Screen.width >> 1;
            SCREEN_HEIGHT_BY_2 =    Screen.height >> 1;

            sprites = new List<Spritex>();
        }

        public static void addChild(Spritex sp)
        {
            sp.gameObject.transform.parent = camera2D.transform;
            sprites.Add(sp);
        }

        public static void removeChild(Spritex sp)
        { 
        
        }

        public static void update()
        {
            if (camera2D == null)
                return;

            for (int a = 0; a < sprites.Count; a++)
            {
                sprites[a].update();
            }
        }

        

	}
}
