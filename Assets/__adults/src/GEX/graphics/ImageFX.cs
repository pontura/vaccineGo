using UnityEngine;
using System;
using System.Collections.Generic;
using GEX.debug;
using GEX.define;

namespace GEX.graphics
{
	public class ImageFX
	{
        //************************************
		//******       TRANSFORMS   **********
		//************************************
		public static uint H_MIRROR =		0;
		public static uint V_MIRROR = 	    1;

        //************************************
		//******       STROKES      **********
		//************************************
		public static uint STROKE_UPSIDE= 	    1;
		public static uint STROKE_DOWNSIDE= 	1 << 1;
		public static uint STROKE_LEFTSIDE= 	1 << 2;
		public static uint STROKE_RIGHTSIDE= 	1 << 3;
		public static uint STROKE_ALL= 		    1 << 4;


        public static uint lerp(uint backgroundColor, uint foregroundColor, uint xblendType, uint value)
        {
            byte A1 = 0, R1 = 0, G1 = 0, B1 = 0;
            byte A2 = 0, R2 = 0, G2 = 0, B2 = 0;
            uint A = 0, R = 0, G = 0, B = 0;
            uint COLOR = 0;

            A1 = (byte)((backgroundColor >> 24) & 0xFF);
            R1 = (byte)((backgroundColor >> 16) & 0xFF);
            G1 = (byte)((backgroundColor >> 8) & 0xFF);
            B1 = (byte)(backgroundColor & 0xFF);

            A2 = (byte)((foregroundColor >> 24) & 0xFF);
            R2 = (byte)((foregroundColor >> 16) & 0xFF);
            G2 = (byte)((foregroundColor >> 8) & 0xFF);
            B2 = (byte)(foregroundColor & 0xFF);

            if (A2 == 0xFF)
                return foregroundColor;

            if (xblendType == XBlendType.GREATER_THAN)
            {
                if (A2 < value)
                    return backgroundColor;
            }

            R = (uint)((R2 * A2) + (R1 * (0x100 - A2)) >> 8);    // >> 8 = / 256
            G = (uint)((G2 * A2) + (G1 * (0x100 - A2)) >> 8);
            B = (uint)((B2 * A2) + (B1 * (0x100 - A2)) >> 8);
            //A = 0xFF;
            A = (uint)((A2 * A2) + (A1 * (0x100 - A2)) >> 8);


            COLOR = (A << 24) | (R << 16) | (G << 8) | B;

            return COLOR;
        }

        public static uint lerp(uint backgroundColor, uint foregroundColor)
        {
            byte A1 = 0, R1 = 0, G1 = 0, B1 = 0;
            byte A2 = 0, R2 = 0, G2 = 0, B2 = 0;
            uint A = 0, R = 0, G = 0, B = 0;
            uint COLOR = 0; 

            A1 = (byte)((backgroundColor >> 24) & 0xFF);
            R1 = (byte)((backgroundColor >> 16) & 0xFF);
            G1 = (byte)((backgroundColor >> 8) & 0xFF);
            B1 = (byte)(backgroundColor & 0xFF);

            A2 = (byte)((foregroundColor >> 24) & 0xFF);
            R2 = (byte)((foregroundColor >> 16) & 0xFF);
            G2 = (byte)((foregroundColor >> 8) & 0xFF);
            B2 = (byte)(foregroundColor & 0xFF);

            if (A2 == 0xFF)
                return foregroundColor;

            R = (uint)((R2 * A2) + (R1 * (0x100 - A2)) >> 8);    // >> 8 = / 256
            G = (uint)((G2 * A2) + (G1 * (0x100 - A2)) >> 8);
            B = (uint)((B2 * A2) + (B1 * (0x100 - A2)) >> 8);
            //A = 0xFF;
            A = (uint)((A2 * A2) + (A1 * (0x100 - A2)) >> 8);
           

            COLOR = (A << 24) | (R << 16) | (G << 8) | B;

            return COLOR;
        }
        //#######################################################################
        //###                     Interpolate Colors                          ###
        //#######################################################################
        //##                                                                  ###
        //##     color: the color used for start degrading                    ###
        //##     color2: the color where ends degrading                       ###
        //##                                                                  ###
        //#######################################################################
        public static uint interpolateColors(uint colorA, uint colorB, uint amount)
        {
            byte A1 = 0, R1 = 0, G1 = 0, B1 = 0;
            byte A2 = 0, R2 = 0, G2 = 0, B2 = 0;
            uint A = 0, R = 0, G = 0, B = 0;
            uint COLOR = 0;

            A1 = (byte)((colorA >> 24) & 0xFF);
            R1 = (byte)((colorA >> 16) & 0xFF);
            G1 = (byte)((colorA >> 8) & 0xFF);
            B1 = (byte)(colorA & 0xFF);

            A2 = (byte)((colorB >> 24) & 0xFF);
            R2 = (byte)((colorB >> 16) & 0xFF);
            G2 = (byte)((colorB >> 8) & 0xFF);
            B2 = (byte)(colorB & 0xFF);

            R = (uint)(R1 + ((R2 - R1) * amount) / 256);
            G = (uint)(G1 + ((G2 - G1) * amount) / 256);
            B = (uint)(B1 + ((B2 - B1) * amount) / 256);
            A = (uint)(A1 + ((A2 - A1) * amount) / 256);

            /*R = R & 0xFF;
            G = G & 0xFF;
            B = B & 0xFF;
            A = A & 0xFF;*/
  
            COLOR = (A << 24) | (R << 16) | (G << 8) | B;

            return COLOR;
        }

        public static uint color32ToARGB(Color32 color)
        {
            uint A = 0, R = 0, G = 0, B = 0;
            uint COLOR;
            
            A = (uint)(color.a);
            R = (uint)(color.r);
            G = (uint)(color.g);
            B = (uint)(color.b);
            COLOR = (A << 24) | (R << 16) | (G << 8) | B;

            return COLOR;
        }

        public static Color32 ARGBToColor32(uint color)
        {
            byte A = 0, R = 0, G = 0, B = 0;

            A = (byte)((color >> 24) & 0xFF);
            R = (byte)((color >> 16) & 0xFF);
            G = (byte)((color >> 8) & 0xFF);
            B = (byte)(color & 0xFF);

            return new Color32(R, G, B, A);
        }


        public static Texture2D cloneTexture(Texture2D tex)
        {
            Texture2D newTex = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
            newTex.SetPixels32(tex.GetPixels32());
            newTex.Apply();
            return newTex;
        }


        //-------------------------------------------------------------------------------------------------------------------------------------
        //-- The colors array is a flatterned 2D array, where pixels are laid out left to right, bottom to top (i.e. row after row).         --
        //-- Array size must be at least width by height of the mip level used. The default mip level is zero (the base texture)             --
        //-- in which case the size is just the size of the texture. In general case, mip level size is mipWidth=max(1,width>>miplevel) and  --
        //-- similarly for height.                                                                                                           --
        //-------------------------------------------------------------------------------------------------------------------------------------
        public static Texture2D copyPixels(Texture2D src, int x, int y, int width, int height)
        {
            Color32[] pixels = src.GetPixels32();
            Color32[] destPixels = new Color32[width * height];
            Color32 pix;
            int index;
            Texture2D newTex = new Texture2D(width, height);
            int a = 0;
          
          
            int _y_ = src.height - y - height;
            
            for (int i = 0; i < height; i++)
            {
                index = (_y_ * src.width) + x - 1;

                for (int j = 0; j < width; j++)
                {
                    pix = pixels[++index];
                    destPixels[a++] = pix;
                }

                _y_++;
            }

            newTex.SetPixels32(destPixels);
            newTex.Apply();

            return newTex;
        }




        //#######################################################################
		//###                        DEGRADE MAKER                            ###
		//#######################################################################
		//##                                                                  ###
		//##     color: the color used for start degrading                    ###
		//##     color2: the color where ends degrading                       ###
		//##                                                                  ###
		//#######################################################################
		public static void makeSpriteHDegrade(Spriteg sprite, uint color, uint color2)
		{
			makeHDegrade(sprite.getImage(), color, color2);
		}
		
		public static void makeSpriteHDegradeAll(Spriteg sprite, uint color, uint color2)
		{
			/*var IMAGES:Vector.<BitmapData> = sprite.getImages();
			
			for (var a:uint = 0; a < IMAGES.length; a++ )
			{
				if(IMAGES[a]!= null)
					makeHDegrade(IMAGES[a], color, color2);
			}*/
		}
		
		public static void makeHDegrade(Texture2D src, uint color, uint color2)
		{
			_makeDegrade_(src, color, color2, 0);// 0 = horizontal
		}
		
		public static void makeSpriteVDegrade(Spriteg sprite, uint color, uint color2)
		{
			makeVDegrade(sprite.getImage(), color, color2);
		}
		
		/*public static void makeSpriteVDegradeAll(sprite:Spriteg, color:uint, color2:uint)
		{
			var IMAGES:Vector.<BitmapData> = sprite.getImages();
			
			for (var a:uint = 0; a < IMAGES.length; a++ )
			{
				if(IMAGES[a]!= null)
					makeVDegrade(IMAGES[a], color, color2);
			}
		}*/
		
		public static void makeVDegrade(Texture2D src, uint color, uint color2)
		{
			_makeDegrade_(src, color, color2, 1);// 0 = vertical
		}



		private static void _makeDegrade_(Texture2D src, uint color, uint color2, uint type)
		{
			
			float steps = 1;
            XImage data;
			
			if(type == 0)		//horizontal
				steps= src.height;
			else if (type == 1)	//vertical
				steps= src.width;
			
			//src.fillRect(src.rect, 0x00000000);
            data = new XImage(src);
			
			uint COLOR;
			
			uint Red= 		    ((color & 0xFF0000) >> 16);
			uint Green= 	    ((color & 0x00FF00) >> 8);
			uint Blue= 		    (color & 0x0000FF);
		
			uint Red2= 		    ((color2 & 0xFF0000) >> 16);
			uint Green2= 	    ((color2 & 0x00FF00) >> 8);
			uint Blue2= 	    (color2 & 0x0000FF);
		
			//^^^^^^^^^^^^ COLORS DIFF ^^^^^^^^^^^^
			float DRed=         ((int)Red2 - (int)Red) / steps;
            float DGreen = ((int)Green2 - (int)Green) / steps;
            float DBlue = ((int)Blue2 - (int)Blue) / steps;
			float redCount=	    Red + DRed;
			float greenCount=   Green + DGreen;
			float blueCount=    Blue + DBlue;
			//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            //int NRED = ((int)Red2 - (int)Red);

            //XConsole.println("Red:" + Red + "  Red2:" + Red2);
            //XConsole.println("TEST----> " + NRED);


            //if (DBlue < 0)
              /*  XConsole.println("is:" + DRed);
                XConsole.println("steps:" + steps);
                XConsole.println("D:" + DRed + "    " + DGreen + "    " + DBlue);
                XConsole.println("Count:" + redCount + "    " + greenCount + "    " + blueCount);*/

			uint pix;
            //uint test;
			
			if (type == 0)		//HORIZONTAL
			{
				for(int i= 0; i < steps; i++)
				{
					COLOR= 	((uint)redCount << 16);
                    COLOR |= ((uint)greenCount << 8);
					COLOR|= (uint)blueCount;
                    //test = (uint)redCount;
                    //XConsole.println("Count:" + redCount + "    " + greenCount + "    " + blueCount);
                   // XConsole.println("->" + test);
                    //XConsole.println(COLOR.ToString("X"));
					pix= 0xFF000000 | COLOR;

					for (int j = 0; j < src.width; j++ )
					{
						//src.setPixel32(j, i, pix);
                        data.setPixel32(j, i, pix);
					}
					redCount+= DRed;
					greenCount+= DGreen;
                    blueCount += DBlue; 
				}
			}
			else if (type == 1)	//VERTICAL
			{
				for(int jj= 0; jj < steps; jj++)
				{
					COLOR= 	((uint)redCount) << 16;
					COLOR|= ((uint)greenCount) << 8;
					COLOR|= ((uint)blueCount);

					pix= 0xFF000000 | COLOR;
					
					for (int ii = 0; ii < src.height; ii++ )
					{
						//src.setPixel32(jj, ii, pix);
                        data.setPixel32(jj, ii, pix);
					}
					redCount+= DRed;
					greenCount+= DGreen;
					blueCount += DBlue;
				}
			}

            //XConsole.println("col count: " + data.getUnityColors32().Length);
            //XConsole.println("col tot: " + (src.width * src.height));

            src.SetPixels32(data.getUnityColors32());
            src.Apply();
            data.destroy();
            data = null;
		}



        //#######################################################################
		//###                        STROKE ADDER                             ###
		//#######################################################################
		//##                                                                  ###
		//##     color: the color used for the stroke                         ###
		//##     size: the stroke size                                        ###
		//##                                                                  ###
		//#######################################################################	
		public static void addSpriteStroke(Spriteg sprite = null, uint color = 0x000000, int size = 2, uint sides = 0x10)
		{
			addStroke(sprite.getImage(), color, size, sides);			
		}
		
		/*public static void addSpriteStrokeAll(Spriteg sprite = null, uint color = 0x000000, uint size = 2, uint sides = 0x10)
		{
			var IMAGES:Vector.<BitmapData> = sprite.getImages();
			
			for (var a:uint = 0; a < IMAGES.length; a++ )
			{
				if(IMAGES[a]!= null)
					addStroke(IMAGES[a], color, size, sides);
			}
		}*/
		
		public static void addStroke(Texture2D src = null, uint color = 0x000000, int size = 2, uint sides = 0x10)
		{
			//^^^^^^^^^^^ if the stroke extends the rect ^^^^^^
			//^^^^^^^^^^^ fill it with the stroke color  ^^^^^^
            XImage data = new XImage(src);

			if((size >= src.width) || (size >= src.height))
			{
                data.fillRect(data.rect, color);
			}
			else
			{
				//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^  |****|
				//^^^^^^^^ UPSIDE ^^^^^^^^^^^^^  |    |
				//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^  |____|
				if((sides & (STROKE_UPSIDE | STROKE_ALL))!= 0)
				{
                    data.fillRect(0, 0, data.width, size, color);
				}
				//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^   |****|
				//^^^^^^^^ DOWNSIDE ^^^^^^^^^^^   |    |
				//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^   |****|
				if((sides & (STROKE_DOWNSIDE | STROKE_ALL))!= 0)
				{
                    data.fillRect(0, data.height - size, data.width, size, color);
				}
				//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^   ******
				//^^^^^^^^ LEFTSIDE ^^^^^^^^^^^   *    *
				//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^   *____*  
				if((sides & (STROKE_LEFTSIDE | STROKE_ALL))!= 0)
				{
                    data.fillRect(0, 0, size, data.height, color);
				}
				//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^   ******
				//^^^^^^^^ RIGHTSIDE ^^^^^^^^^^   *    *
				//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^   ******
				if((sides & (STROKE_RIGHTSIDE | STROKE_ALL))!= 0)
				{
                    data.fillRect(data.width - size, 0, size, data.height, color);
				}
			}

            src.SetPixels32(data.getUnityColors32());
            src.Apply();
            data.destroy();
            data = null;
		}


        //#######################################################################
		//###                          COLOR SETTER                           ###
		//#######################################################################
		//##                                                                  ###
		//##     alpha: the alpha amount the image array data will contain    ###
		//##                                                                  ###
		//#######################################################################
		public static void setSpriteColor(Spriteg sprite, uint color, uint strength)
		{
			setColor(sprite.getImage(), color, strength);
		}
		
		public static void setSpriteColorAll(Spriteg sprite, uint color, uint strength)
		{
			List<Texture2D> IMAGES = sprite.getImages();
			
			for (int a = 0; a < IMAGES.Count; a++ )
			{
				if(IMAGES[a]!= null)
					setColor(IMAGES[a], color, strength);
			}
		}


        /*public static void setColor(Texture2D bmp, uint color, uint strength)
        { 
            XImage img = new XImage(bmp);
            uint [] src = img.getPixels32();

            byte R= (byte)((color & 0xFF0000) >> 16);
			byte G= (byte)((color & 0x00FF00) >> 8);
			byte B= (byte)(color & 0x0000FF);
	    
			uint COLOR;
            uint Alpha;
			uint Red;
			uint Green;
			uint Blue;
			
	
			
			for(int i= 0; i < src.Length; i++)
			{
				Alpha=		(src[i] & 0xFF000000) >> 24;
				Red= 		(src[i] & 0xFF0000) >> 16;
				Green= 		(src[i] & 0x00FF00) >> 8;
				Blue= 		(src[i] & 0x0000FF);

				if(Alpha != 0)
				{
					//^^^^^^^^^^^^ COLORS DIFF ^^^^^^^^^^^^
					float dR= 	(R - Red)   / 	256f;
					float dG=	(G - Green) / 	256f;
					float dB= 	(B - Blue)  / 	256f;
					
					Red+= 	(uint)(dR * strength);
					Green+= (uint)(dG * strength);
					Blue+= 	(uint)(dB * strength);
				}
				
				COLOR= Alpha 	<< 	24;
				COLOR|= Red 	<< 	16;
				COLOR|= Green	<< 	8;
				COLOR|= Blue;

				src[i]= COLOR;
			}

            img.setPixels32(src);
            bmp.SetPixels32(img.getUnityColors32());
            img.destroy();
            img = null;
			
			src = null;
        }*/

		public static void setColor(Texture2D bmp, uint color, uint strength)
		{
			uint R= (color >> 16) & 0xFF;
			uint G= (color >> 8)  & 0xFF;
			uint B= (color)       & 0xFF;
			
			//int COLOR;
			byte Alpha;
			byte Red;
			byte Green;
			byte Blue;

			Color32 [] src = bmp.GetPixels32();

			for(int i= 0; i < src.Length; i++)
			{

                Alpha =     src[i].a;
                Red =       src[i].r;
                Green =     src[i].g;
                Blue =      src[i].b;

				if(Alpha != 0)
				{
					//^^^^^^^^^^^^ COLORS DIFF ^^^^^^^^^^^^
					/*float dR=  (R - Red)  / 256f;
                    float dG = (G - Green)/ 256f;
                    float dB = (B - Blue) / 256f;*/

                    float dR = ((int)R - (int)Red) / 256f;
                    float dG = ((int)G - (int)Green) / 256f;
                    float dB = ((int)B - (int)Blue) / 256f;
					
					Red+= 	(byte)(dR * strength);
					Green+= (byte)(dG * strength);
                    Blue += (byte)(dB * strength);
				}
				
                src[i].a = Alpha;
                src[i].r = Red;
                src[i].g = Green;
                src[i].b = Blue;
			}
			
            bmp.SetPixels32(src);
            bmp.Apply();
			
			src = null;
		}




	}
}
