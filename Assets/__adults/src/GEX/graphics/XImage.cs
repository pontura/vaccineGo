using UnityEngine;
using System;
using System.Collections.Generic;
using GEX.define;

namespace GEX.graphics
{
	public class XImage
	{
        public int width, height;
        public uint[][] data;
        public XRect rect;


        public XImage(int width, int height)
        {
            this.width =    width;
            this.height =   height;

            rect = new XRect(0, 0, (int)width, (int)height);

            data = new uint[height][];

            for(int a = 0; a < data.Length; a++)
                data[a] = new uint[width];
        }


        public XImage(Texture2D tex)
        {
            this.width =    tex.width;
            this.height =   tex.height;

            rect = new XRect(0, 0, (int)width, (int)height);

            Color32 [] pixels = tex.GetPixels32();

            data = new uint[height][];

            for (int _y = 0; _y < data.Length; _y++)
            {
                data[_y] = new uint[width];

                for (int _x = 0; _x < width; _x++)
                    data[_y][_x] = ImageFX.color32ToARGB(pixels[((height - 1 - _y) * width) + _x]);

            }
        }


        public void setPixel32(int x, int y, uint color)
        {
            if ((x < width) && (y < height))
                data[y][x] = color;
        }



        public uint getPixel32(int x, int y)
        {
            if ((x >= width) || (y >= height))
                return 0x00000000;
            else
                return data[y][x];
        }

        public uint[] getPixels32()
        {
            uint [] pixels = new uint[width * height];
            int a = 0;

            for (int _y_ = 0; _y_ < height; _y_++)
            {
                for (int _x_ = 0; _x_ < width; _x_++)
                {
                    pixels[a++] = data[_y_][_x_];
                }
            }

            return pixels;
        }

        public void setPixels32(uint [] pixels)
        {
            int a = 0;

            for (int _y_ = 0; _y_ < height; _y_++)
            {
                for (int _x_ = 0; _x_ < width; _x_++)
                {
                    data[_y_][_x_] = pixels[a++];
                }
            }
        }

        public void fillRect(int x, int y, int width, int height, uint color)
        {
            width +=    x;
            height+=    y;

            for (int _y_ = y; _y_ < height; _y_++)
            {
                for (int _x_ = x; _x_ < width; _x_++)
                {
                    setPixel32(_x_, _y_, color);
                }
            }
        }

        public void fillRect(XRect rect, uint color)
        {
            rect.width +=   rect.x;
            rect.height +=  rect.y;
            
            for (int _y_ = rect.y; _y_ < rect.height; _y_++)
            {
                for (int _x_ = rect.x; _x_ < rect.width; _x_++)
                {
                    setPixel32(_x_, _y_, color);
                }
            }

            rect.width -=   rect.x;
            rect.height -=  rect.y;
        }


        public void draw(XImage src, int x, int y, int width, int height, uint blendType = XBlendType.NONE, uint value = 0)
        {
            uint color;
            uint color2;
            int _y_, _x_;

            for (_y_ = 0; _y_ < height; _y_++)
            {
                for (_x_ = 0; _x_ < width; _x_++)
                {
                    color = getPixel32(x + _x_, y + _y_);
                    color2 = src.getPixel32(_x_, _y_);
                    setPixel32(x + _x_, y + _y_, ImageFX.lerp(color, color2, blendType, value));
                }
            }
            
        }


        public void draw(XImage src, int x, int y, int width, int height, bool alpha = false)
        {
            uint color;
            uint color2;
            int _y_, _x_;
            
            if (alpha)
            {

                for (_y_ = 0; _y_ < height; _y_++)
                {
                    for (_x_ = 0; _x_ < width; _x_++)
                    {
                        color = getPixel32(x + _x_, y + _y_);
                        color2 = src.getPixel32(_x_, _y_);
                        setPixel32(x + _x_, y + _y_, ImageFX.lerp(color, color2));
                    }
                }
            }
            else
            {
                for (_y_ = 0; _y_ < height; _y_++)
                {
                    for (_x_ = 0; _x_ < width; _x_++)
                    {
                        color = src.getPixel32(_x_, _y_);
                        setPixel32(x + _x_, y + _y_, color);
                    }
                }
            }
        }



        public Color32[] getUnityColors32()
        {
            Color32[] colors = new Color32[width * height];
            int a = 0;

            for (int _y_ = (int)height-1; _y_ >= 0; _y_--)
            //for (int _y_ = 0; _y_ < height; _y_++)
            {
                for (int _x_ = 0; _x_ < width; _x_++)
                { 
                    colors[a++] = ImageFX.ARGBToColor32(data[_y_][_x_]);
                }
            }

            return colors;
        }


        public void destroy()
        {
            data = null;
        }
	}
}
