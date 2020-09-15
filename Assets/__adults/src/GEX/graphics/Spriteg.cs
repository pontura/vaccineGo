using UnityEngine;
using System;
using System.Collections.Generic;
using GEX.debug;
using GEX.io;
using GEX.fonts;
using GEX.define;

namespace GEX.graphics
{
    public class Spriteg
    {

        //private Texture2D texture;
        private int frames;
        public int frame;
        public string name;

        public float x;
        public float y;
        public float width;
        public float height;
        public float _scaleX;
        public float _scaleY;
        public float alpha;
        public bool autoIncrement = false;
        public bool isCentered = false;
        public float angle;

        private List<Texture2D> img_bk;
        private List<Texture2D> img;
        //private var filters:Vector.<Filter>;
        //private var point:Point;
        //private var matrix:Matrix;
        //private var pointTemp:Point;
        //private var _last_alpha:Number;
        //private var colorT:ColorTransform;
        //private var rect:Rectangle;
        //private var clip:Rectangle;
        //private var _clipped:Boolean;
        private uint nxtFrCount;
        private Color32 color;
        private bool colorize;

        private uint FRAME;

        private const uint _UP_ = 0;
        private const uint _OVER_ = 1;
        private const uint _DOWN_ = 2;
        

        //*********************************************************
        //**                                                     **
        //*********************************************************
        //**                                                     **
        //**                   CONSTRUCTOR                       **
        //**                                                     **
        //*********************************************************
        /*public Spriteg()
        {
            __Spriteg__(null, -1, -1);
        }

        public Spriteg(Texture2D tex)
        {
            __Spriteg__(tex, -1, -1);
        }

        public Spriteg(Texture2D tex, )*/

        public Spriteg(Texture2D tex = null, float width = -1, float height = -1)
        {
            //texture = tex;

            name = "";
            _scaleX = 1;
            _scaleY = 1;

            frames = 0;
            frame = 0;
            x = 0;
            y = 0;
            alpha = 1;
            //_last_alpha = 	1;
            FRAME = _UP_;
            nxtFrCount = 0;

            angle = 0;

            color = new Color32();
            colorize = false;

            //point = 		new Point();
            //matrix = 		new Matrix();
            //pointTemp = 	new Point();
            //rect = 			new Rectangle();
            //_clipped = 		false;
            //clip = 			new Rectangle();
            //colorT = 		new ColorTransform(1, 1, 1, 1, 0, 0, 0, 0)
            img = new List<Texture2D>();
            img_bk = new List<Texture2D>();

            if (tex != null)
            {
                frames = 1;

                img.Add(tex);
                img_bk.Add(ImageFX.cloneTexture(tex));

                if ((width == -1) && (height == -1))
                {
                    this.width = tex.width * _scaleX;
                    this.height = tex.height * _scaleY;
                }
                else if ((width != -1) && (height == -1))
                {
                    this.width = width;
                    this.height = (width / tex.width) * tex.height;
                }
                else if ((width == -1) && (height != -1))
                {
                    this.height = height;
                    this.width = (height / tex.height) * tex.width;
                }
                else
                {
                    this.width = width;
                    this.height = height;
                }
            }

            //XConsole.printTitle("CREATE SPRITEG");
            //XConsole.println("Size: (" + texture.width + "x" + texture.height + ")");
            //XConsole.printEndLine();
        }

        //*********************************************************
        //**                                                     **
        //*********************************************************
        //**                                                     **
        //**                    ADD FRAME                        **
        //**                                                     **
        //*********************************************************
        public void addFrame(Texture2D texRef)
        {
            img.Add(ImageFX.cloneTexture(texRef));
            img_bk.Add(ImageFX.cloneTexture(texRef));

            width = texRef.width * _scaleX;
            height = texRef.height * _scaleY;
            frames = img.Count;
            if (autoIncrement)
                frame++;
        }

        //*********************************************************
        //**                                                     **
        //*********************************************************
        //**                                                     **
        //**                  CREATE FRAME                       **
        //**                                                     **
        //*********************************************************
        /*public void createFrame()
        {
            createFrame(1, 1, 0x00000000);
        }*/

        public void createFrame(int width = 1, int height = 1, uint color = 0x00000000)
        {
            Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
            Color32[] pixels = new Color32[width * height];
            Color32 _color_ = ImageFX.ARGBToColor32(color);

            for (int a = 0; a < pixels.Length; a++)
                pixels[a] = _color_;

            tex.SetPixels32(pixels);
            tex.Apply();

            img.Add(tex);
            img_bk.Add(ImageFX.cloneTexture(tex));

            this.width = width;
            this.height = height;
            frames = img.Count;
            if (autoIncrement)
                frame++;
        }

        //*************** drawString ********************
        public void drawString(Fontb font = null, String str = "", int x = 0, int y = 0, uint anchor = 0)
        {
            if (anchor == 0)
            {
                int fontW = font.getTextWidth(str);
                int fontH = font.getTextHeight(str);
                if ((img[frame].width < (x + fontW)) || (img[frame].height < (y + fontH)))
                {
                    /*img[frame].dispose();
                    img[frame] = null;
                    img[frame] = new BitmapData(x + fontW, y + fontH, true, 0x00000000);
                    width = img[frame].width;
                    height = img[frame].height;*/
                }
                font.drawString(img[frame], str, x, y);
                font.drawString(img_bk[frame], str, x, y);
            }
            else
            {	// WARNING!!! if anchor is setted there will not be resize for bmp
                font.drawString(img[frame], str, x, y, anchor);
                font.drawString(img_bk[frame], str, x, y, anchor);
            }
        }

        //*************** drawCenterString ********************
        public void drawCenterString(Fontb font, String str)
        {
            font.drawString(img[frame], str, (int)width >> 1, (int)height >> 1, XGraphics.HCENTER | XGraphics.VCENTER);
        }

        //*********************************************************
        //**                                                     **
        //*********************************************************
        //**                                                     **
        //**                   SET FRAME                         **
        //**                                                     **
        //*********************************************************
        public void setFrame(int frame)
        {
            this.frame = frame;
            width = img[frame].width * _scaleX;
            height = img[frame].height * _scaleY;
        }

        //*********************************************************
        //**                                                     **
        //*********************************************************
        //**                                                     **
        //**                  NEXT FRAME                         **
        //**                                                     **
        //*********************************************************
        public void nextFrame()
        {
            if (++frame > (frames - 1))
                frame = 0;

            width = img[frame].width * _scaleX;
            height = img[frame].height * _scaleY;
        }

        //*********************************************************
        //**                                                     **
        //*********************************************************
        //**                                                     **
        //**                  NEXT FRAME X                       **
        //**                                                     **
        //*********************************************************
        public void nextFrameX(int times)
        {
            if (nxtFrCount++ > times)
            {
                nxtFrCount = 0;

                if (++frame > (frames - 1))
                    frame = 0;

                width = img[frame].width * _scaleX;
                height = img[frame].height * _scaleY;
            }
        }

        //*********************************************************
        //**                                                     **
        //*********************************************************
        //**                                                     **
        //**                    SET POS                          **
        //**                                                     **
        //*********************************************************
        public void setPos(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        //*********************************************************
        //**                                                     **
        //*********************************************************
        //**                                                     **
        //**                    CENTER                           **
        //**                                                     **
        //*********************************************************
        /*public function center(widthIn:int, heightIn:int):void
		{
			this.x = (widthIn >> 1) - (width >> 1);
			this.y = (heightIn >> 1) - (height >> 1);
		}*/

        //**************** getFrame **********************
        public int getFrame()
        {
            return frame;
        }

        //**************** getFrames *********************
        public int getFrames()
        {
            return frames;
        }

        //**************** getWidth *********************
        public float getWidth(int frame)
        {
            return img[frame].width * _scaleX;
        }

        //**************** getHeight *********************
        public float getHeight(int frame)
        {
            return img[frame].height * _scaleY;
        }

        //***************** getImages ********************
        public List<Texture2D> getImages()
        {
            return img;
        }

        //***************** getImage  ********************
        public Texture2D getImage()
        {
            return img[frame];
        }

        //***************** getXImage  ********************
        public XImage getXImage()
        {
            return new XImage(img[frame]);
        }

        
        //*****************    draw   ********************
        public void draw(Spriteg src = null, int x = 0, int y = 0, uint blendType = XBlendType.NONE, uint value = 0)
        {
            XImage _dest_ = new XImage(img[frame]);
            XImage _src_ = new XImage(src.getImage());

            
            _dest_.draw(_src_, x, y, _src_.width, _src_.height, blendType, value);

            img[frame].SetPixels32(_dest_.getUnityColors32());
            img[frame].Apply();

            _dest_.destroy();
            _src_.destroy();
            _dest_ = null;
            _src_ = null;
        }

        //*****************    drawBmp   ******************
        /*public function drawBmp(src:BitmapData = null, x:int = 0, y:int = 0):void
        {
            img[frame].copyPixels(src, src.rect, new Point(x, y), null, null, true);
        }
		
        //*****************    clear   *******************
        public function clear(color:uint):void
        {
            img[frame].fillRect(img[frame].rect, color);
        }*/

        //****************    drawRect   *****************
        public void drawRect(int x, int y, int width, int height, uint color)
        {
            //rect.x = x; 		rect.y = y;
            //rect.width = width; rect.height = height;
            //img[frame].fillRect(rect, color);
            //img_bk[frame].fillRect(rect, color);

            XImage _img_ = new XImage(img[frame]);
            XImage _img_bk_ = new XImage(img_bk[frame]);

            _img_.fillRect(x, y, width, height, color);
            _img_bk_.fillRect(x, y, width, height, color);

            img[frame].SetPixels32(_img_.getUnityColors32());
            img_bk[frame].SetPixels32(_img_.getUnityColors32());

            img[frame].Apply();
            img_bk[frame].Apply();

            _img_.destroy();
            _img_bk_.destroy();

            _img_ = null;
            _img_bk_ = null;
        }

        //****************      COPY       ***************
        public Spriteg copy()
        {
            Spriteg sp = new Spriteg();

            for (int a = 0; a < img.Count; a++)
                sp.addFrame(img[a]);

            sp.width = width;
            sp.height = height;

            return sp;
        }

        public float scaleX
        {
            set { this._scaleX = value; width = img[frame].width * _scaleX; }
            get { return this._scaleX; }
        }

        public float scaleY
        {
            set { this._scaleY = value; height = img[frame].height * _scaleY; }
            get { return this._scaleY; }
        }

        //*********************************************************
        //**                                                     **
        //*********************************************************
        //**                                                     **
        //**                    DESTROY                          **
        //**                                                     **
        //*********************************************************
        public void destroy()
        {
            /*if(filters!= null)
            {
                for(var a:uint= 0; a < filters.length; a++)
                    filters[a].destroy();
                filters= null;
            }*/

            for (int b = 0; b < img.Count; b++)
            {
                img[b] = null;
                img_bk[b] = null;
            }

            img = null;
            img_bk = null;
            /*point= 		null;
            matrix= 	null;
            pointTemp= 	null;
            colorT = 	null;
            rect = 		null;
            clip = 		null;*/

        }

        //*********************************************************
        //**                                                     **
        //*********************************************************
        //**                                                     **
        //**                      UPDATE                         **
        //**                                                     **
        //*********************************************************
        public void update()
        {

            if (Mouse.isOverRect((int)x, (int)y, (int)width, (int)height))
            {
                if (Mouse.CLICK)
                    FRAME = _DOWN_;
                else
                    FRAME = _OVER_;
            }
            else
                FRAME = _UP_;

        }

        //****************     events     ****************
        public bool isOver()
        {
            if (FRAME == _OVER_)
                return true;
            else
                return false;
        }

        public bool isDown()
        {
            if (FRAME == _DOWN_)
                return true;
            else
                return false;
        }

        public bool wasDown()
        {
            if (FRAME == _DOWN_)
            {
                Mouse.CLICK = false;
                return true;
            }
            else
                return false;
        }

        public void setColor(uint color)
        {
            /*this.color.r = (byte)((color >> 24) & 0xFF);
            this.color.g = (byte)((color >> 16) & 0xFF);
            this.color.b = (byte)((color >> 8) & 0xFF);
            this.color.a = (byte)(color & 0xFF);*/
            this.color = ImageFX.ARGBToColor32(color);
            colorize = true;
        }


        public void setColor(Color32 color)
        {
            this.color = color;
            colorize = true;
        }

        public void resetColor()
        {
            colorize = false;
        }

        //*****************  setClip *********************
        /*public function setClip(x:uint, y:uint, width:uint, height:uint):void
        {
            _clipped = true;
            clip.x = x;			clip.y = y;
            clip.width = width;	clip.height = height;
        }*/

        //*********************************************************
        //**                                                     **
        //*********************************************************
        //**                                                     **
        //**                      PAINT                          **
        //**                                                     **
        //*********************************************************
        public void paint()
        {

            paint(0, 0);

        }

        public void paint(int offsetX, int offsetY)
        {
            Color saveColor;
            int _w_center, _h_center;
            Matrix4x4 matrixBackup = GUI.matrix;

            Vector2 pos = new Vector2(x, y);
            Rect rect = new Rect(pos.x - width * 0.5f, pos.y - height * 0.5f, width, height);
            Vector2 pivot = new Vector2(rect.xMin + rect.width * 0.5f, rect.yMin + rect.height * 0.5f);

            if (angle != 0)
            {
                //matrixBackup = GUI.matrix;
                GUIUtility.RotateAroundPivot(angle, pivot);
            }

            if (isCentered)
            {
                _w_center = (int)width >> 1;
                _h_center = (int)height >> 1;
            }
            else
            {
                _w_center = 0;
                _h_center = 0;
            }

            if ((alpha != 1) && colorize)
            {
                saveColor = GUI.color;
                GUI.color = new Color(color.r, color.g, color.b, alpha);
                GUI.DrawTexture(new Rect(x + offsetX - _w_center, y + offsetY - _h_center, width, height), img[frame]);
                GUI.color = saveColor;
            }
            else if (alpha != 1)
            {
                saveColor = GUI.color;
                GUI.color = new Color(1, 1, 1, alpha);
                GUI.DrawTexture(new Rect(x + offsetX - _w_center, y + offsetY - _h_center, width, height), img[frame]);
                GUI.color = saveColor;
            }
            else
            {
                GUI.DrawTexture(new Rect(x + offsetX - _w_center, y + offsetY - _h_center, width, height), img[frame]);
            }


            if (angle != 0)
            {
                GUI.matrix = matrixBackup;
            }

        }





    }
}
