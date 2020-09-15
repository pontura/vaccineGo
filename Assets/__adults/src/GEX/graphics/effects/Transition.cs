using UnityEngine;
using System;
using System.Collections.Generic;
using GEX.graphics;
using GEX.utils;
//using Holoville.HOTween;

namespace GEX.graphics.effects
{
	public class Transition
	{

        //private static XTimer _timer01;
        private static bool _init = false, _paint = false;
        private static XTimer timer;
        private static Spriteg _vscreen;
        private static bool _complete;

        private static int currType;

        private const int type_start_dark = 0;
        private const int type_begin_paint = 1;

        private static void _createAll()
        {
            if (_init == false)
            {
                timer = new XTimer();
                _vscreen =          new Spriteg();
                _vscreen.createFrame(10, 10, 0xFF000000);
                _vscreen.width =    Screen.width;
                _vscreen.height =   Screen.height;
                _init = true;
            }
        }

        public static void beginPaint(float alpha)
        {
            _createAll();
            _paint =            true;
            _vscreen.alpha =    alpha;
        }

        public static void crossFade(float from, float to, float time = 1f, float extraTime = 0f)
        {
            _createAll();
            _complete = false;

            if (from == 0)
                from = 0.01f;
            _vscreen.alpha = from;
            _paint = true;
            timer.setDelay(extraTime);
            //HOTween.To(_vscreen, time, new TweenParms().Prop("alpha", to).Ease(EaseType.Linear).OnComplete(onEnterPanelComplete));

            currType = type_start_dark;
        }

        
        
        private static void onEnterPanelComplete()
        {
            _complete = true;
            timer.start();
        }



        public static bool update()
        {
            if (currType == type_start_dark)
            {
                if (_complete)
                {
                    if (timer.getTimeSet() > 0)
                        return timer.update();
                    else
                        return true;
                }
            }

            return false;
        }

        public static void stopPainting()
        { 
            _paint = false;
        }

        public static void paint()
        {
            if (_init && _paint)
            {
                if (currType == type_start_dark)
                {

                    _vscreen.paint();


                }
                else if (currType == type_begin_paint)
                {
                    _vscreen.paint();
                }
            }
        }



        /*
		
		private static int TYPE;
		
		private const int type_center_up_curtain=	0;
		private const int type_disolve=			1;
		private const int type_disolve_solid=		2;
		private const int type_cinematic_in=		3;
		private const int type_cinematic_out=		4;
		private const int type_dark_in=			5;
		private const int type_dark_out=			6;
		
		private static int _y0;
		private static int _y1;
		private static int _centerY;
		//private static var colorT:ColorTransform;
		private static float _alpha;
		private static int _alphaUINT;
		private static int _stop_at;
		private static int _steps;
		private static Spriteg vscreen;
		private static float _maxDark;
		
		
		public static void startCenterUpCurtain(int steps = 3)
		{
			TYPE = type_center_up_curtain;
			
			_steps = steps;
			_centerY = (int)Screen.height >> 1;
			_y0 = _centerY;
			_y1 = _centerY;
		}
		
		public static void startDarkIn(float maxDark)
		{
			TYPE = type_dark_in;
			
			_maxDark = maxDark;
			
			if (vscreen != null)
			{
				vscreen.destroy();
				vscreen = null;
			}
			
			//if (vscreen == null)
			//{
				vscreen = new Spriteg();
				vscreen.createFrame(Screen.width, Screen.height, 0xFF000000);
				vscreen.alpha = 0;
			//}
			
			_alpha = 0;
		}
		
		public static void startDarkOut(float maxDark)
		{
			TYPE = type_dark_out;
			
			_maxDark = maxDark;
			
			if (vscreen != null)
			{
				vscreen.destroy();
				vscreen = null;
			}
		
			vscreen = new Spriteg();
			vscreen.createFrame(Screen.width, Screen.height, 0xFF000000);
			vscreen.alpha = maxDark;
			
			_alpha = maxDark;
		}
		
		public static void startDisolve()
		{
			TYPE = type_disolve;
			_alpha = 1;
			//if(colorT == null)
			//	colorT = new ColorTransform(1, 1, 1, 1, 0, 0, 0, 0);
		}
		
		public static void startDisolveSolid()
		{
			TYPE = type_disolve_solid;
			_alphaUINT = 0x00;
		}
		
		public static void startCinematicIn(int stop_at)
		{
			TYPE = type_cinematic_in;
			_y0 = 0;
			_y1 = Screen.height;
			_stop_at = stop_at;
		}
		
		public static void paint()
		{
			switch(TYPE)
			{
				case type_dark_in:
					vscreen.paint();
					break;
					
				case type_dark_out:
					vscreen.paint();
					break;
					
				case type_center_up_curtain:
					//g.fillRect(new Rectangle(0, 0, Screen.width, _y0), 0xFF000000);
					//g.fillRect(new Rectangle(0, _y1, Screen.width, _centerY + _centerY - _y1), 0xFF000000);
					break;
										
				case type_disolve:
					//g.colorTransform(g.rect, colorT);
					break;
					
				case type_disolve_solid:
					//g.fillRect(g.rect, _alphaUINT << 24);
					break;
					
				case type_cinematic_in:
					//g.fillRect(new Rectangle(0, 0, Screen.width, _y0), 0xFF000000);
					//g.fillRect(new Rectangle(0, _y1, Screen.width, Screen.height - _y1), 0xFF000000);
					break;
			}
		}
		
		public static bool update()
		{
			switch(TYPE)
			{
				case type_dark_in:
					_alpha += 0.04f;
					if (_alpha >= _maxDark)
					{
						_alpha = _maxDark;
						vscreen.alpha = _alpha;
						return false;
					}
					vscreen.alpha = _alpha;
					break;
					
				case type_dark_out:
					_alpha -= 0.04f;
					if (_alpha <= 0)
					{
						_alpha = 0;
						vscreen.alpha = _alpha;
						return false;
					}
					vscreen.alpha = _alpha;
					break;
				
				case type_center_up_curtain:
					if (_y0 > 0)
					{
						_y0 -= _steps;
						_y1 += _steps;
						return false;
					}
					break;
					
				case type_disolve:
					_alpha -= 0.05f;
					//colorT.alphaMultiplier = _alpha;
					return(_alpha <= 0);
					//break;
					
				case type_disolve_solid:
					_alphaUINT += 0x0A;
					return(_alphaUINT >= 0xFF);
					//break;
					
				case type_cinematic_in:
					if (_y0 < _stop_at)
					{
						_y0 += 2;
						_y1 -= 2;
						return false;
					}
					break;
			}
			
			return true;
		}*/

	}
	
}
