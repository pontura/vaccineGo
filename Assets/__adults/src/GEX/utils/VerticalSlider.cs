using UnityEngine;
using System;
using System.Collections.Generic;


namespace GEX.utils
{
	public class VerticalSlider
	{

        private float start_y, end_y;
        public float deltaY, saved_deltaY;
        //private float deltaYInverted;
        private float limit_top, limit_down;
        private bool TOUCHED, RELEASED, MOVED;
        private int hard_code_move_count;
        private int moved_count;

        public VerticalSlider(float limitTop, float limitDown)
        {
            deltaY =        0;
            saved_deltaY =  0;
            //deltaYInverted = 0;
            limit_top = limitTop;
            limit_down = limitDown;
            TOUCHED =   false;
            RELEASED =  false;
            MOVED =     false;
            moved_count = 0;
        }

        public void updateLimits(float limitTop, float limitDown)
        {
            limit_top = limitTop;
            limit_down = limitDown;
        }

        public bool wasTouched()
        {
            if (TOUCHED)
            {
                TOUCHED = false;
                return true;
            }
            else
                return false;
        }

        public bool wasReleased()
        {
            if (RELEASED)
            {
                RELEASED = false;
                return true;
            }
            else
                return false;
        }

        public bool wasMoved()
        { 
            return MOVED;
        }

        public void update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("mouse DOWN");

                start_y = Screen.height - Input.mousePosition.y + deltaY;
                TOUCHED = true;
                MOVED =     false;
                moved_count = 0;
            }
            else if (Input.GetMouseButton(0))
            {
                //Debug.Log("mouse");

                end_y = Screen.height - Input.mousePosition.y;

                deltaY = start_y - end_y;

                if (deltaY != saved_deltaY)
                {
                    if (moved_count++ > 7)
                        MOVED = true;
                    //Debug.Log("dif: "+ moved_count);
                }

                saved_deltaY = deltaY;

                if (deltaY < limit_top)
                    deltaY = limit_top;

                if (deltaY > limit_down)
                    deltaY = limit_down;

                //deltaYInverted = -deltaY;
               //Debug.Log("y:" + deltaY);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //Debug.Log("mouse UP");
            
            }

        }




	}
}
