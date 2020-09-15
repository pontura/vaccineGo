using System;
using UnityEngine;

namespace GEX.graphics
{

	public class Pen
	{
		private static Texture2D _texture_;
        private static Texture2D [] _texture_colors_;

		public static void Initialize()
		{
            Color [] def_col = {Color.red, Color.green, Color.blue, Color.magenta, Color.black};
            Color curr_col;

            if (_texture_ == null)
            {
                _texture_ = new Texture2D(2, 2);

                _texture_colors_ = new Texture2D[def_col.Length];

                for (int a = 0; a < _texture_colors_.Length; a++)
                {
                    curr_col = def_col[a];
                    _texture_colors_[a] = new Texture2D(2, 2);
                    _texture_colors_[a].SetPixels(new Color[] { curr_col, curr_col, curr_col, curr_col });
                    _texture_colors_[a].Apply();
                }
            }
			
		}

        public static void setColor(Color color)
        {
            _texture_.SetPixels(new Color[]{color, color, color, color});
            _texture_.Apply();
        }

        public static void drawRectInverted(int x, int y, int width, int height, Color color)
        {


            drawRect(x, Screen.height - y, width, height, color);


        }

		public static void drawRect(int x, int y, int width, int height, Color color)
		{
            Texture2D tex = null;

            if(color == Color.red)
                tex = _texture_colors_[0];
            else if (color == Color.green)
                tex = _texture_colors_[1];
            else if (color == Color.blue)
                tex = _texture_colors_[2];
            else if (color == Color.magenta)
                tex = _texture_colors_[3];
            else if (color == Color.black)
                tex = _texture_colors_[4];

            GUI.DrawTexture(new Rect(x, y, width, height), tex);


			//Color colorTmp = GUI.color;
			//GUI.color = color;
			//GUI.DrawTexture(new Rect(x, y, width, height), _texture_);
			//GUI.color = colorTmp;
		}
		
		public static void drawLine(int x, int y, int x2, int y2, Color color)
		{
			
			int w = x2 - x ;
		    int h = y2 - y ;
		    int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0 ;
		    if (w<0) dx1 = -1 ; else if (w>0) dx1 = 1 ;
		    if (h<0) dy1 = -1 ; else if (h>0) dy1 = 1 ;
		    if (w<0) dx2 = -1 ; else if (w>0) dx2 = 1 ;
		    int longest = Mathf.Abs(w) ;
		    int shortest = Mathf.Abs(h) ;
		    if (!(longest>shortest)) {
		        longest = Mathf.Abs(h) ;
		        shortest = Mathf.Abs(w) ;
		        if (h<0) dy2 = -1 ; else if (h>0) dy2 = 1 ;
		        dx2 = 0 ;            
		    }
		    int numerator = longest >> 1 ;
		    for (int i=0;i<=longest;i++) {
		        putPixel(x,y,color) ;
		        numerator += shortest ;
		        if (!(numerator<longest)) {
		            numerator -= longest ;
		            x += dx1 ;
		            y += dy1 ;
		        } else {
		            x += dx2 ;
		            y += dy2 ;
		        }
			}
			
			
			
			
			/*int shortLen = y2-y;
			int longLen = x2-x;
			bool yLonger;
			int i;
			
			putPixel(x, y, color);
			putPixel(x2, y2, color);
			
			if((shortLen ^ (shortLen >> 31)) - (shortLen >> 31) > (longLen ^ (longLen >> 31)) - (longLen >> 31))
			{
				shortLen ^= longLen;
				longLen ^= shortLen;
				shortLen ^= longLen;

				yLonger = true;
			}
			else
				yLonger = false;
			
			
			
			

			int inc = longLen < 0 ? -1 : 1;

			float multDiff = longLen == 0 ? shortLen : shortLen / longLen;

			if (yLonger)
			{
				for (i = 0; i != longLen; i += inc)
					putPixel(x +(int)(i*multDiff), y+i, color);
			}
			else
			{
				for (i = 0; i != longLen; i += inc)
					putPixel(x+i, y+(int)(i*multDiff), color);
			}*/
		}
		
		public static void putPixel(int x, int y, Color color)
		{
			putPixel(x, y, color, -1, -1);
		}
		
		public static void putPixel(int x, int y, Color color, int type, int size)
		{
			drawRect(x, y, 2, 2, color);
		}
		
	}
}