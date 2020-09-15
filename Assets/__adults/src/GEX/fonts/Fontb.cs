#define DBG_XCONSOLE

using UnityEngine;
using System;
using System.Collections.Generic;
using GEX.debug;
using GEX.graphics;



namespace GEX.fonts
{
	public class Fontb
	{
        private String FONT_NAME;
		private int FONT_SIZE;
		private int	FONT_HEIGHT;
		private int SYMBOLS_NUM;
		private int MODE;
		private Boolean BOLD;
		private Boolean ITALIC;
		private int EXTRA_DATA;
		private int GRID_WIDTH;
		private int GRID_HEIGHT;
		private int	PIXEL_COMPONENTS;
		
		private Texture2D [] SBMP;
		private uint [][] CHAR;
		private uint [] CHAR_AVAILABLE;
		
		private const uint _y= 			    0;
		private const uint _width= 		    1;
		private const uint _height= 		2;
		private const uint _sbmp= 		    3;
		private const uint _max_grid_x=     10;	
		
		private uint spaceCharWidth = 		5;
		private int spaceX= 				0;
		private int spaceY= 				0;
		//private var rect:Rectangle;
        private Rect rect;
		//private var point:Point;
        private float scale=                1;
        private Color32 color,      colorSaved;
        private bool colorize;
		
		//SX functions
		//private static List<uint> str_parts_st;
		//private static List<uint> str_parts_end;
		//private static List<uint> str_color;
		//private static List<uint> str_col_strength;
		
		private const String sx_color_0 = "[$color=";
		private const String sx_color_1 = "[$$color]";


        //##########################################################
		//######                                              ######
		//######              Fontb (constructor)             ######
		//######                                              ######
		//##########################################################
        private Fontb()
        { 
            
        }

        public Fontb(byte[] DATA):this(DATA, 0xFFFFFFF1)
        {}

        public Fontb(byte [] DATA, uint color) 
		{
			//Initialize
			initialize();
			
			FONT_NAME = "";
			BOLD= false;
			ITALIC = false;
			
			//rect = new Rectangle();
            rect = new Rect();
            //point = new Point();
			
#if DBG_XCONSOLE
			XConsole.disable();
			
			XConsole.printTitle("Creating bitmap font");
#endif
		
			//00-01:  [MN] It contains the magic number for the file. MNL= B, MNH= F (0x42 - 0x46)
			if(DATA[0]== 0x42 && DATA[1]== 0x46)
			{
#if DBG_XCONSOLE
				XConsole.println("sbf check OK");
#endif
			}
			else 
			{
#if DBG_XCONSOLE
				XConsole.println("This is not a simple bitmap font.");
#endif
                return;
			}
			
			
			//10-2F:  [FN] Font Name 
			byte C;
			//DATA.position = 0x10;
			for(uint fn= 0; fn < 32; fn++)
			{
				C = DATA[0x10 + fn];
				if(C == 0)
					break;

                //FONT_NAME += DATA.readUTFBytes(1);
                FONT_NAME += (char)C;
			}
#if DBG_XCONSOLE
			XConsole.println("Font Name: " + FONT_NAME);
#endif
			
			
			//30-30:  [NS] Number of symbols on the file. Max value 255
			SYMBOLS_NUM= DATA[0x30];
#if DBG_XCONSOLE
            XConsole.println("Symbols: " + SYMBOLS_NUM);
#endif
			
			//31-31:  [MD] Mode 
			//Alpha:    ARGB_8888 [0xC0]    ARGB_4444 [0x30]	
			//ColorKey: RGB_888 [0x80]      RGB_565 [0x37]     RGB_332 [0x12]
			MODE = DATA[0x31];
			
			if(MODE == 0xC0)
			{
				PIXEL_COMPONENTS = 4;
#if DBG_XCONSOLE
                XConsole.println("ARGB_8888");
#endif
			}
			else if(MODE == 0x30)
			{
				PIXEL_COMPONENTS = 2;
#if DBG_XCONSOLE
                XConsole.println("ARGB_4444");
#endif
			}
			else if(MODE == 0x80)
			{
				PIXEL_COMPONENTS = 3;
#if DBG_XCONSOLE
                XConsole.println("RGB_888");
#endif
			}
			else if(MODE == 0x37)
			{
				PIXEL_COMPONENTS = 2;
#if DBG_XCONSOLE
                XConsole.println("RGB_565");
#endif
			}
			else if(MODE == 0x12)
			{
				PIXEL_COMPONENTS = 1;
#if DBG_XCONSOLE
                XConsole.println("RGB_332");
#endif
			}
			
			
			//32-32: [SZ] Font Size
			FONT_SIZE = DATA[0x32];
#if DBG_XCONSOLE
			XConsole.println("Font Size: " + FONT_SIZE);
#endif
			
			//33-33: [BI] Bold, Italic
			//     0000 00BI
			if((DATA[0x33] & 0x01)== 1)
				ITALIC= true;
			else if((DATA[0x33] * 0x02) == 1)
				BOLD= true;
#if DBG_XCONSOLE
			XConsole.println("Bold: " + BOLD + "\nItalic: " + ITALIC);
#endif
			
			//34-34: [EXTRA] Extra Data
			EXTRA_DATA= DATA[0x34];
			
			//35-35: [GW] Grid Width Max Value = 255
			GRID_WIDTH= DATA[0x35];
			
			//36-36: [GH] Grid Height Max Value = 255
			GRID_HEIGHT= DATA[0x36];
			FONT_HEIGHT = GRID_HEIGHT;
#if DBG_XCONSOLE
			XConsole.println("Grid: " + GRID_WIDTH + "x" + GRID_HEIGHT);
#endif			
			
			//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
			//^^^^^   GET SYMBOL DATA   ^^^^^
			//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
			uint address= 0x90;//int address= 0x00;
			uint _sname_;
			uint _swidth_;
			uint _sheight_;
			uint _syoffset_;
			uint  _total_data_;
			uint A, R, G, B;
			uint D0, D1; //, D2, D3;
			uint _color_;
            XImage X_SBMP;
			
			//SBMP= new int[SYMBOLS_NUM][];
			//SBMP = new Vector.<BitmapData>(SYMBOLS_NUM);
			SBMP = new Texture2D[SYMBOLS_NUM];
			//CHAR = new char[0xFF][];
			//CHAR = new Vector.<Vector.<uint>>(0xFF);
			CHAR = new uint[0xFF][];
			//CHAR_AVAILABLE = new char[SYMBOLS_NUM];
			//CHAR_AVAILABLE = new Vector.<uint>(SYMBOLS_NUM);
            CHAR_AVAILABLE = new uint[SYMBOLS_NUM];
			
			
			for(uint _c_ = 0 ; _c_ < 0xFF; _c_++)
			{
				//CHAR[_c_] = new char[4];
				CHAR[_c_] = new uint[4];
				
				CHAR[_c_][_y]=      0;
				CHAR[_c_][_width]=  0;
				CHAR[_c_][_height]= 0;
				CHAR[_c_][_sbmp]=   0;
			}
			
			//Setting Space Character
			CHAR[32][_width]= spaceCharWidth; //' ' = 32
			
			
			for(int a= 0; a < SYMBOLS_NUM; a++)
			{
				//[SN1] Symbol Name
				_sname_= DATA[address++];
				
				//[SW] Symbol Width
				_swidth_= (uint)(DATA[address++] + 1);
				
				//[SH] Symbol Height
				_sheight_= (uint)(DATA[address++] + 1);
				
				//[SY] Symbol Y offset
				_syoffset_= DATA[address++];
				
				//[SBMP] Symbol Bitmap
				_total_data_ = (_swidth_ * _sheight_);
				
				CHAR[_sname_][_width]=  _swidth_;
				CHAR[_sname_][_height]= _sheight_;
				CHAR[_sname_][_y]=      _syoffset_;
				CHAR[_sname_][_sbmp]=   (uint)a;

				CHAR_AVAILABLE[a]= _sname_;
				//var sname:ByteArray = new ByteArray();
				//sname.writeByte(_sname_);
				//sname.position = 0;
#if DBG_XCONSOLE
				XConsole.println("<<==================>>");
				//Console.println("_sname_: " + sname.readUTFBytes(1) + " c:" + _sname_);
				XConsole.println("_sname_: " + (char)((byte)_sname_) + " c:" + _sname_);
				XConsole.println("_swidth_: " + _swidth_);
				XConsole.println("_sheight_: " + _sheight_);
				XConsole.println("_syoffset_: " + _syoffset_);
				XConsole.println("_total_data_: " + _total_data_);
#endif

				
				//SBMP[a] = new int[_total_data_];
				//SBMP[a] = new Vector.<uint>(_total_data_);
				//SBMP[a] = new BitmapData(_swidth_, _sheight_);
				SBMP[a] = new Texture2D((int)_swidth_, (int)_sheight_, TextureFormat.RGBA32, false);
                X_SBMP = new XImage((int)_swidth_, (int)_sheight_);
				int f = 0;
				int _x_off_ = 0, _y_off_ = 0;
				
				//while(f < SBMP[a].length)
				while(f < _total_data_)
				{
					// ^^^^^ ARGB_8888 ^^^^^
					if(MODE == 0xC0)
					{
						A= DATA[address++];
						R= DATA[address++];
						G= DATA[address++];
						B= DATA[address++];
						
						_color_= (A << 24) | (R << 16) | (G << 8) | B;
						
						//SBMP[a].setPixel32(_x_off_, _y_off_, _color_);
                        X_SBMP.setPixel32(_x_off_, _y_off_, _color_);
					}
					// ^^^^^ ARGB_4444 ^^^^^
					if(MODE == 0x30)
					{
						D0 = DATA[address++];
						D1 = DATA[address++];
						
						A= D0 >> 4;
						R= D0 & 0x0F;
						G= D1 >> 4;
						B= D1 & 0x0F;
						
						A = (A * 0xFF) / 15;
						R = (R * 0xFF) / 15;
						G = (G * 0xFF) / 15;
						B = (B * 0xFF) / 15;
						_color_= (A << 24) | (R << 16) | (G << 8) | B;
						
						//SBMP[a].setPixel32(_x_off_, _y_off_, _color_);
                        X_SBMP.setPixel32(_x_off_, _y_off_, _color_);
					}
					// ^^^^^ RGB_888 ^^^^^
					if(MODE == 0x80)
					{
						//A= 0x55;
						R= DATA[address++];
						G= DATA[address++];
						B= DATA[address++];
						
						_color_= 0xFF000000 | (R << 16) | (G << 8) | B;
						
						if(_color_ == 0xFFFF00FF)
							_color_ = 0x00000000;
						
						//SBMP[a].setPixel32(_x_off_, _y_off_, _color_);
                        X_SBMP.setPixel32(_x_off_, _y_off_, _color_);
					}
					// ^^^^^ RGB_565 ^^^^^
					if(MODE == 0x37)
					{
						D0 = DATA[address++];
						D1 = DATA[address++];
						
						//  7  6  5  4  3  2  1  0     7  6  5  4  3  2  1  0
						// [R][R][R][R][R][G][G][G]   [G][G][G][B][B][B][B][B]
						//  --------------_________    ________---------------
						
						R = D0 >> 3;
						G = ((D0 & 0x07) << 3) | (D1 >> 5);
						B = D1 & 0x1F;
						
						R = (R * 0xFF) / 31;
						G = (G * 0xFF) / 63;
						B = (B * 0xFF) / 31;
						
						_color_= 0xFF000000 | (R << 16) | (G << 8) | B;
						
						if(_color_ == 0xFFFF00FF)
							_color_ = 0x00000000;
						
						//SBMP[a].setPixel32(_x_off_, _y_off_, _color_);
                        X_SBMP.setPixel32(_x_off_, _y_off_, _color_);
					}
					// ^^^^^ RGB_332 ^^^^^
					if(MODE == 0x12)
					{
						D0 = DATA[address++];
						
						//  7  6  5  4  3  2  1  0 
						// [R][R][R][G][G][G][B][B]
						//  --------_________------
						
						R = D0 >> 5;
						G = (D0 & 0x1F) >> 2;
						B = D0 & 0x03;
						
						R = (R * 0xFF) / 7;
						G = (G * 0xFF) / 7;
						B = (B * 0xFF) / 3;
						
						_color_= 0xFF000000 | (R << 16) | (G << 8) | B;
						
						if(_color_ == 0xFFFF00FF)
							_color_ = 0x00000000;
						
						//SBMP[a].setPixel32(_x_off_, _y_off_, _color_);
                        X_SBMP.setPixel32(_x_off_, _y_off_, _color_);
					}
					
					//Console.println("DATA "+ f + " -> " + _color_);
				
					
					if (++_x_off_> (_swidth_ - 1))
					{
						_x_off_ = 0;
						_y_off_++;
					}


                    SBMP[a].SetPixels32(X_SBMP.getUnityColors32());
                    SBMP[a].Apply();

					f++;
				}

                X_SBMP.destroy();
                X_SBMP = null;
				
			}
			/*
			if (color != 0xFFFFFFF1)
			{
				for (var q:uint = 0; q < SBMP.length; q++ )
					ImageFX.setColor(SBMP[q], color, 100);
			}
			*/
#if DBG_XCONSOLE
			XConsole.enable();
#endif
		}


        private void initialize()
		{
			//str_parts_st = 		new List<uint>();
			//str_parts_end = 	new List<uint>();
			//str_color = 		new List<uint>();
			//str_col_strength =	new List<uint>();
            color = new Color32();
            colorize = false;
		}


        //##########################################################
		//######                                              ######
		//######                    FIX                       ######
		//######                                              ######
		//##########################################################
		public void fixFont(String c, uint x, uint y, uint color)
		{
			//Used to set up pixels that were not filled in the editor.
			
			//var c:uint = char.charCodeAt(0);
            //SBMP[CHAR[c][_sbmp]].setPixel32(x, y, color);
		}


        //##########################################################
		//######                                              ######
		//######                 drawString                   ######
		//######                                              ######
		//##########################################################
        public void drawString(Texture2D dest = null, String str = "", int x = 0, int y = 0, uint anchor = 0xFF)
        {
            XImage _dest_ = new XImage(dest);
            XImage _char_;
            char[] str_chars;

            if (anchor != 0xFF)
            {
                int anchorX = getTextWidth(str);
                int anchorY = getTextHeight(str);

                if ((anchor & XGraphics.HCENTER) != 0)
                    x -= anchorX / 2;
                //else if((anchor & Graphics.LEFT) != 0)
                //	;
                else if ((anchor & XGraphics.RIGHT) != 0)
                    x -= anchorX;


                if ((anchor & XGraphics.VCENTER) != 0)
                    y -= (anchorY / 2);
                //else if((anchor & Graphics.TOP) != 0)
                //	;
                else if ((anchor & XGraphics.BOTTOM) != 0)
                    y -= anchorY;
                //if((anchor & Graphics.BASELINE) != 0)
                //	;//! to fix. Don´t know what does this
            }

            uint c;
            uint dx = (uint)x;
            uint dy = (uint)y;

            str_chars = str.ToCharArray();

            for (uint a = 0; a < str.Length; a++)
            {
                //c= str.charCodeAt(a);
                c = str_chars[a];

                if (c == 10) //10 = '\n'
                {
                    dx = (uint)x;
                    dy += (uint)(FONT_HEIGHT + spaceY);
                }
                else
                {
                    //rect.width = CHAR[c][_width];	rect.height = CHAR[c][_height];
                    rect.width = CHAR[c][_width]; rect.height = CHAR[c][_height];
                    //point.x = dx;					point.y = dy + CHAR[c][_y];
                    rect.x = dx; rect.y = dy + CHAR[c][_y];
                    //g.copyPixels(SBMP[CHAR[c][_sbmp]], rect, point, null, null, true); 


                    //GUI.DrawTexture(rect, SBMP[CHAR[c][_sbmp]]);
                    /*Color [] pixels = SBMP[CHAR[c][_sbmp]].GetPixels();
                    dest.SetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, pixels);*/

                    _char_ = new XImage(SBMP[CHAR[c][_sbmp]]);

                    _dest_.draw(_char_, (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, true);
                    
                    dest.SetPixels32(_dest_.getUnityColors32());
                    dest.Apply();

                    _char_.destroy();
                    _char_ = null;

                    dx += (uint)(CHAR[c][_width] + spaceX);
                }

            }

            _dest_.destroy();
            _dest_ = null;
        }

		public void drawString(String str = "", int x = 0, int y = 0, uint anchor= 0xFF)
		{
            char[] str_chars;

			if (anchor != 0xFF)
			{
				int anchorX= getTextWidth(str);
				int anchorY= getTextHeight(str);	
				
				if((anchor & XGraphics.HCENTER) != 0)
					x-= anchorX / 2;
				//else if((anchor & Graphics.LEFT) != 0)
				//	;
                else if ((anchor & XGraphics.RIGHT) != 0)
					x-= anchorX;


                if ((anchor & XGraphics.VCENTER) != 0)
					y-= (anchorY / 2);
				//else if((anchor & Graphics.TOP) != 0)
				//	;
                else if ((anchor & XGraphics.BOTTOM) != 0)
					y-= anchorY;
				//if((anchor & Graphics.BASELINE) != 0)
				//	;//! to fix. Don´t know what does this
			}

			uint c;
			uint dx = (uint)x;
            uint dy = (uint)y;

            str_chars = str.ToCharArray();

			for(uint a= 0; a < str.Length; a++)
			{
				//c= str.charCodeAt(a);
				c = str_chars[a];

				if(c == 10) //10 = '\n'
				{
					dx= (uint)x;
					dy+= (uint)(FONT_HEIGHT + spaceY);
				}
				else
				{
					//rect.width = CHAR[c][_width];	rect.height = CHAR[c][_height];
                    rect.width = CHAR[c][_width];   rect.height = CHAR[c][_height];
					//point.x = dx;					point.y = dy + CHAR[c][_y];
                    rect.x = dx;                    rect.y = dy + CHAR[c][_y];
					//g.copyPixels(SBMP[CHAR[c][_sbmp]], rect, point, null, null, true); 

                    if (colorize)
                    {
                        colorSaved = GUI.color;
                        GUI.color = color;
                    }
                    
                    GUI.DrawTexture(rect, SBMP[CHAR[c][_sbmp]]);
                    
                    if (colorize)
                        GUI.color = colorSaved;

					dx+= (uint)(CHAR[c][_width] + spaceX);
				}	
				
			}
			
		}




        public void setLineSpace(int space)
		{
			spaceY= space;
		}
		
		public void setSpaceX(int space)
		{
			spaceX= space;
		}
		
		public int getSpaceX()
		{
			return spaceX;
		}
		
		public void setSpaceChar(int space)
		{
			spaceCharWidth= (uint)space;
			CHAR[32][_width]= spaceCharWidth;//32 = ' '
		}
		
		public int getSpaceChar()
		{
			return (int)spaceCharWidth;
		}
		
		
		public int getTextWidth(String str)
		{
			uint c;
			int dx = 0;
			int dy = 0;
			int xSpaceMajor = 0;
			uint length = (uint)str.Length;
			char [] str_chars = str.ToCharArray();

			for(uint a = 0; a < length; a++)
			{
				//c= str.charCodeAt(a);
                c = str_chars[a];

				if(c == 10) //10 = '\n'
				{
					if(dx > xSpaceMajor)
						xSpaceMajor= dx;
					
					dx= 0;
					dy+= FONT_HEIGHT + spaceY;
					continue;
				}
				
				if(a < length - 1)
					dx+= (int)(CHAR[c][_width] + spaceX);
				else	
					dx+= (int)CHAR[c][_width];
				
				if(dx > xSpaceMajor)
					xSpaceMajor= dx;
			}
			
			return xSpaceMajor;	//The bigger row width
		}
		
		public int getTextHeight(String str)
		{
			uint c;
			int dy= FONT_HEIGHT;
            char [] str_chars = str.ToCharArray();
			
			for(uint a= 0; a < str.Length; a++)
			{
				//c= str.charCodeAt(a);
				c= str_chars[a];

				if(c == 10) //10 = '\n'
				{
					dy+= FONT_HEIGHT + spaceY;
					continue;
				}
			}
			
			return dy;
		}
		
		
		public int getTextSXWidth(String str)
		{
			uint c;
			int dx = 0;
			int dy = 0;
			int xSpaceMajor = 0;
			uint length;
			
			bool parse = true;
			int index;
			int index_2;
		    String str2;
            char[] str_chars;
			
			while (parse)
			{
				index = str.IndexOf(sx_color_1);
				
				if (index != -1)
					str = str.Replace(sx_color_1, "");
				else
					parse = false;
			}
			
			parse = true;
			
			while (parse)
			{
				index = str.IndexOf(sx_color_0);	
				index_2 = str.IndexOf("]", index);
				
				if (index != -1 && index_2 != -1)
				{
					//str2 = str.substr(0, index);
					str2 = str.Substring(0, index);
					//str = str.substr(index_2 + 1);
					str = str.Substring(index_2 + 1);
					str = str2 + str;
				}	
				else
					parse = false;	
			}
			
			length = (uint)str.Length;
			
            str_chars = str.ToCharArray();

			for(uint a= 0; a < length; a++)
			{
				//c= str.charCodeAt(a);
                c = str_chars[a];

				if(c == 10) //10 = '\n'
				{
					if(dx > xSpaceMajor)
						xSpaceMajor= dx;
					
					dx= 0;
					dy+= FONT_HEIGHT + spaceY;
					continue;
				}
				
				if(a < length - 1)
					dx+= (int)(CHAR[c][_width] + spaceX);
				else	
					dx+= (int)CHAR[c][_width];
				
				if(dx > xSpaceMajor)
					xSpaceMajor= dx;
			}
			
			return xSpaceMajor;	//The bigger row width
		}
		
		public int getFontHeight()
		{
			return FONT_HEIGHT;
		}

        public void setScale(float scale)
        {
            this.scale = scale;

            FONT_HEIGHT = (int)(FONT_HEIGHT * scale);

            for (int _c_ = 0; _c_ < CHAR.Length; _c_++)
            {
                CHAR[_c_][_y] =         (uint)(CHAR[_c_][_y] *      scale);
                CHAR[_c_][_width] =     (uint)(CHAR[_c_][_width] *  scale);
                CHAR[_c_][_height] =    (uint)(CHAR[_c_][_height] * scale);
            }

        }

        public void setColor(uint color)
        {
            /*this.color.a = (byte)((color >> 24) & 0xFF);
            this.color.r = (byte)((color >> 16) & 0xFF);
            this.color.g = (byte)((color >> 8) & 0xFF);
            this.color.b = (byte)(color & 0xFF);*/
            this.color = ImageFX.ARGBToColor32(color);
            colorize = true;
        }

        /*public void setColor(Color32 color)
        {
            this.color.r = color.r;
            this.color.g = color.g;
            this.color.b = color.b;
            this.color.a = color.a;
        }*/

        public void resetColor()
        {
            colorize = false;
        }

        public Fontb cloneAsReference()
        {
            Fontb font = new Fontb();


            font.FONT_NAME =        FONT_NAME;
            font.FONT_SIZE =        FONT_SIZE;
		    font.FONT_HEIGHT =      FONT_HEIGHT;
		    font.SYMBOLS_NUM =      SYMBOLS_NUM;
		    font.MODE=              MODE;
		    font.BOLD=              BOLD;
		    font.ITALIC=            ITALIC;
		    font.EXTRA_DATA =       EXTRA_DATA;
		    font.GRID_WIDTH=        GRID_WIDTH;
		    font.GRID_HEIGHT=       GRID_HEIGHT;
		    font.PIXEL_COMPONENTS=  PIXEL_COMPONENTS;
		
		    font.SBMP=              SBMP;
		    font.spaceX=            spaceX;
		    font.spaceY= 		    spaceY;
		
            font.rect=              rect;
		
            font.scale=             scale;
            font.color=             color;
            font.colorSaved=        colorSaved;
            font.colorize=          colorize;
		
		    /*font.str_parts_st = str_parts_st;
		    font.str_parts_end = str_parts_end;
		    font.str_color = str_color;
		    font.str_col_strength = str_col_strength;*/



            font.CHAR = new uint[0xFF][];
            //CHAR_AVAILABLE = new uint[SYMBOLS_NUM];


            for (uint _c_ = 0; _c_ < 0xFF; _c_++)
            {
                font.CHAR[_c_] = new uint[4];

                font.CHAR[_c_][_y] =        CHAR[_c_][_y];
                font.CHAR[_c_][_width] =    CHAR[_c_][_width];
                font.CHAR[_c_][_height] =   CHAR[_c_][_height];
                font.CHAR[_c_][_sbmp] =     CHAR[_c_][_sbmp];
            }

            //Setting Space Character
            font.CHAR[32][_width] = spaceCharWidth; //' ' = 32



            return font;
        }

        /*public void test()
        {
            GUI.DrawTexture(new Rect(200, 200, CHAR[56][_width], CHAR[56][_height]), SBMP[56]);
        }*/


	}
}
