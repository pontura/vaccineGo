using UnityEngine;
using System;
using System.Collections.Generic;
using GEX.io;

namespace GEX.graphics
{
	public class XButton
	{
        public int x;
		public int y;
		public int width;
		public int height;
		public String name;
		public int id;
		public Boolean visible;
        public float alpha;
		
        protected List<Spriteg> SPRITES;
        protected uint type;
        protected int FRAME;
        protected Boolean BLOCK;
        protected Boolean SELECTED;
        protected Boolean out_state;
        private bool SAVE_PRESSED;

        protected const int _UP_ = 0;
        protected const int _OVER_ = 1;
        protected const int _DOWN_ = 2;
		protected const int _DISABLE_= 	3;

		public XButton()
		{
            //SPRITES = new List<Spriteg>(4);
            SPRITES = new List<Spriteg>(4);
            SPRITES.Add(null);
            SPRITES.Add(null);
            SPRITES.Add(null);
            SPRITES.Add(null);
			//this.type =	type;
			x = 0; 		y = 0; 
			width = 0; 	height = 0;
            alpha = 1;
			FRAME = 	_UP_;
			BLOCK = 	false;
			SELECTED = 	false;
			name = 		"";
			visible = 	true;
			out_state =	true;

            SAVE_PRESSED = false;
		}
		
		public void setFrameUp(Spriteg sp)
		{
			SPRITES[_UP_] = null;
			SPRITES[_UP_] = sp.copy();
			width = (int)SPRITES[_UP_].width;
			height = (int)SPRITES[_UP_].height;
		}
		
		public void setFrameOver(Spriteg sp)
		{
			SPRITES[_OVER_] = null;
			SPRITES[_OVER_] = sp.copy();
		}
		
		public void setFrameDown(Spriteg sp)
		{
			SPRITES[_DOWN_] = null;
			SPRITES[_DOWN_] = sp.copy();
		}

        public void setFrameDisable(Spriteg sp)
        {
            SPRITES[_DISABLE_] = null;
            SPRITES[_DISABLE_] = sp.copy();
        }
		
		public Spriteg getFrameUp()
		{
			if (SPRITES[_UP_] == null)
			{
				SPRITES[_UP_] = new Spriteg();
				SPRITES[_UP_].createFrame();
			}	
			return SPRITES[_UP_];
		}
		
		public Spriteg getFrameOver()
		{
			if (SPRITES[_OVER_] == null)
			{
				SPRITES[_OVER_] = new Spriteg();
				SPRITES[_OVER_].createFrame();
			}	
			return SPRITES[_OVER_];
		}
		
		public Spriteg getFrameDown()
		{
			if (SPRITES[_DOWN_] == null)
			{
				SPRITES[_DOWN_] = new Spriteg();
				SPRITES[_DOWN_].createFrame();
			}
			return SPRITES[_DOWN_];
		}
		
		public Spriteg getFrameDisable()
		{
			if (SPRITES[_DISABLE_] == null)
			{
				makeDisable();
			}
			return SPRITES[_DISABLE_];
		}
		
		public void select()
		{
			SELECTED = true;
		}
		
		public void clear()
		{
			SELECTED = false;
		}
		
		public void makeDisable(int frame = 0, uint color = 0x000000, uint strength = 0x99)
		{
			Spriteg _sp_ = SPRITES[_UP_].copy();
			ImageFX.setSpriteColor(_sp_, color, strength);
            SPRITES[_DISABLE_] = _sp_;

            //Spriteg _sp_ = SPRITES[_OVER_];
            //ImageFX.setSpriteColor(_sp_, 0xFF0000, 0xAA);

            /*Spriteg _sp_ = new Spriteg();
            _sp_.createFrame(width, height, 0xFFFF0000);
            
            ImageFX.makeSpriteHDegrade(_sp_, 0x003333, 0xDD6666);
            ImageFX.setSpriteColor(_sp_, 0xFF0000, 0xAA);
            SPRITES[_DISABLE_] = _sp_;*/


		}
		
		public void setPos(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		
		//****************     events     ****************
		public Boolean isOver()
		{
			if (FRAME == _OVER_)
				return true;
			else 
				return false;
		}
		
		public Boolean wasOver()
		{
			if (out_state && (FRAME == _OVER_))
			{
				out_state = false;
				return true;
			}
			else
				return false;
		}
		
		public Boolean isDown()
		{
			if (FRAME == _DOWN_ && !BLOCK)
				return true;
			else 
				return false;
		}
		
		public Boolean wasDown()
		{
			if (FRAME == _DOWN_ && !BLOCK)
			{
				Mouse.CLICK = false;
				return true;
			}
			else
				return false;
		}

        public Boolean wasReleased()
        {
            if (FRAME == _DOWN_ && !BLOCK)
            {
                SAVE_PRESSED = true;
                return false;
            }
            else if (SAVE_PRESSED)
            {
                if (FRAME == _OVER_)
                {
                    SAVE_PRESSED = false;
                    return true;
                }
                else if (FRAME == _UP_)
                {
                    SAVE_PRESSED = false;
                }

                return false;
            }

            return false;
        }
		
		public void disable()
		{
			BLOCK = true;
		}
		
		public void enable()
		{
			BLOCK = false;
		}
		
		public Boolean isEnable()
		{
			return !BLOCK;
		}
		
		public Boolean isSelected()
		{
			return SELECTED;
		}

        public void drawInAll(Spriteg img = null, int x = 0, int y = 0)
        {
            Spriteg sp;

            for (int a = 0; a < SPRITES.Count; a++)
            {
                sp = SPRITES[a];
                sp.draw(img);
            }
        }

        public void drawCenteredInAll(Spriteg img)
        {
            Spriteg sp;

            for (int a = 0; a < SPRITES.Count; a++)
            {
                sp = SPRITES[a];
                sp.draw(img, ((int)sp.width >> 1) - ((int)img.width >> 1), ((int)sp.height >> 1) - ((int)img.height >> 1));
            }
        }

        public void cancelPress()
        {
            SAVE_PRESSED = false;
        }

		public void destroy()
		{
			for (int a = 0; a < SPRITES.Count; a++ )
			{
				if (SPRITES[a] != null)
				{
					SPRITES[a].destroy();
					SPRITES[a] = null;
				}
			}
			SPRITES = null;
			name = null;
		}
		
		//****************      update    ****************
		public void update()
		{

			width = (int)SPRITES[_UP_].width;
			height = (int)SPRITES[_UP_].height;
			
			if (visible)
			{
				if (BLOCK)
				{
					FRAME = _DISABLE_;
				}
				else if (SELECTED)
				{
					FRAME = _OVER_;
				}
                else if (Mouse.isOverRect((int)(x + SPRITES[_UP_].x), (int)(y + SPRITES[_UP_].y), (int)width, (int)height))
				{
					if (Mouse.CLICK)
						FRAME = _DOWN_;
					else
						FRAME = _OVER_;
				}
				else
				{
					out_state = true;
					FRAME = _UP_;
				}
			}
		}
		
		
		//****************      paint    *****************
		public void paint()
		{
            if (visible)
            {
                SPRITES[FRAME].alpha = alpha;
                SPRITES[FRAME].paint(x, y);
            }
		}
	}
}
