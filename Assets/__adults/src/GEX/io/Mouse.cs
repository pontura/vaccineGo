using UnityEngine;
using System;
using System.Collections.Generic;
using GEX.graphics;

namespace GEX.io
{
	class Mouse
	{
        //private static var screen:DisplayObject;
		private static bool ENABLE;
		public static int x;
		public static int y;
		public static bool CLICK;
		private static int LOCK_ID = -1;

        private static Boolean lock_until_move;
        private static int lock_until_move_x;
        private static int lock_until_move_y;
		
		public static void setUp()
		{
			//Mouse.screen = screen;
			x = -1; y = -1; CLICK = false;
            lock_until_move = false;
			//screen.addEventListener(MouseEvent.MOUSE_MOVE, onMouseMove);
			//screen.addEventListener(MouseEvent.MOUSE_DOWN, onMouseDown);
			//screen.addEventListener(MouseEvent.MOUSE_UP, onMouseUp);
			enable();
		}
		
		public static void reset()
		{
			x = -1; y = -1; CLICK = false;
			//screen.removeEventListener(MouseEvent.MOUSE_MOVE, onMouseMove);
			//screen.removeEventListener(MouseEvent.MOUSE_DOWN, onMouseDown);
			//screen.removeEventListener(MouseEvent.MOUSE_UP, onMouseUp);
			disable();
		}

        //-------------------------------------------
        //-- Lock x and y with -1 saving last
        //-- non -1 values. If user move mouse/input
        //-- to a different place, lock is released
        //-------------------------------------------
        public static void lockUntilMove()
        {
            lock_until_move = true;
            lock_until_move_x = x;
            lock_until_move_y = y;
            x = -1;
            y = -1;
        }

        public static void disable()
        {
            disable(-1);
        }

		public static void disable(int lock_id)
		{
			if(LOCK_ID == -1)
			{
				LOCK_ID = lock_id;
				ENABLE = false;
			}
		}
		
		public static void enable(int lock_id = -1)
		{
			if (lock_id == LOCK_ID)
			{
				LOCK_ID = -1;
				ENABLE = true;
			}
		}
		
		public static bool isOver(Spriteg sp)
		{
		
			if(x >= sp.x &&  x <= (sp.x + sp.width)  &&  y >= sp.y  &&  y <= (sp.y + sp.height) && ENABLE)
				return true;
			else
				return false;
			
		}
		
		public static bool isDown(Spriteg sp)
		{
		
			if(CLICK && x >= sp.x &&  x <= (sp.x + sp.width)  &&  y >= sp.y  &&  y <= (sp.y + sp.height) && ENABLE)
				return true;
			else
				return false;
			
		}
		
		public static bool wasDown(Spriteg sp)
		{
		
			if (CLICK && x >= sp.x &&  x <= (sp.x + sp.width)  &&  y >= sp.y  &&  y <= (sp.y + sp.height) && ENABLE)
			{
				CLICK = false;
				return true;
			}
			else
				return false;
			
		}
		
		public static bool isOverRect(int x2, int y2, int dx, int dy)
		{
		
			if(x >= x2  &&  x <= (x2 + dx)  &&  y >= y2  &&  y <= (y2 + dy) && ENABLE)
				return true;
			else
				return false;
			
		}
		
		//IMPORTANT! :: The difference is that instead of using <=, here I use < only.
		public static bool isOverRect2(int x2, int y2, int dx, int dy)
		{
		
			if(x >= x2  &&  x < (x2 + dx)  &&  y >= y2  &&  y < (y2 + dy) && ENABLE)
				return true;
			else
				return false;
			
		}
		
		public static bool isDownRect(int x2, int y2, int dx, int dy)
		{
		
			if(CLICK && x >= x2  &&  x <= (x2 + dx)  &&  y >= y2  &&  y <= (y2 + dy) && ENABLE)
				return true;
			else
				return false;
			
		}
		
		public static int getLocalX(Spriteg sprite)
		{
			return (x - (int)sprite.x);
		}
		
		public static int getLocalY(Spriteg sprite)
		{
			return (y - (int)sprite.y);
		}

        public static void update()
        {
            if (lock_until_move)
            {
                x = (int)Input.mousePosition.x;
                y = Screen.height - (int)Input.mousePosition.y;
                CLICK = Input.GetMouseButton(0);

                if ((x != lock_until_move_x) || (y != lock_until_move_y))
                {
                    lock_until_move = false;
                }
                else
                {
                    x = -1;
                    y = -1;
                }

            }
            else
            {
                x = (int)Input.mousePosition.x;
                y = Screen.height - (int)Input.mousePosition.y;
                CLICK = Input.GetMouseButton(0);
                //CLICK = Input.GetMouseButtonDown(0);
            }
            
        }

        public static String getDebugData()
        {
            return "Mouse " + x + "," + y + " click:" + CLICK + " lock_until_move:" + lock_until_move + " l_x:" + lock_until_move_x + " l_y:" + lock_until_move_y;
        }

		/*private static void onMouseMove(m:MouseEvent)
		{
			x = m.stageX; y = m.stageY;
		}
		
		private static void onMouseDown(m:MouseEvent)
		{
			x = m.stageX; y = m.stageY;
			CLICK = true;
		}
		
		private static void onMouseUp(m:MouseEvent)
		{
			x = m.stageX; y = m.stageY;
			CLICK = false;
		}*/
	}
}
