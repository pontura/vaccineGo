using UnityEngine;
using System;
using System.Collections.Generic;
using GEX.io;


namespace GEX.graphics
{
	public class Spritex
	{
        private int frames;
        public int frame;

        public float x;
        public float y;
        public float width;
        public float height;
        public float alpha;
        public bool autoIncrement = false;

        private float _original_width;
        private float _original_height;
        private float _scaleX, _scaleY;
        private float _w_by2, _h_by2;

        private List<Sprite> sprites;
        private SpriteRenderer spriteRenderer;
        private SpritexMono spriteMono;
        public GameObject gameObject;

        private uint FRAME;
        private const uint _UP_ = 0;
        private const uint _OVER_ = 1;
        private const uint _DOWN_ = 2;

        
        public Spritex(Sprite sprite = null, float width = -1, float height = -1)
        {

            x = y = 0;
            frames = 0;
            frame = 0;
            alpha = 1;

            _original_width =   sprite.rect.width;
            _original_height =  sprite.rect.height;

            sprites = new List<Sprite>();
            sprites.Add(sprite);

            //------- ADD FRAME ---------
            addFrame(sprite, width, height);
            //---------------------------

            gameObject =        new GameObject();
            gameObject.layer =  20;
            spriteRenderer =    gameObject.AddComponent<SpriteRenderer>();

            spriteMono = gameObject.AddComponent<SpritexMono>();


            spriteRenderer.sprite = sprite;


            SPXManager.addChild(this);
            gameObject.transform.localPosition = new Vector3(0, 0, 1);
            gameObject.transform.localScale = new Vector3(_scaleX, _scaleY);

            FRAME = _UP_;
        }


        public void addFrame(Sprite sprite, float width, float height)
        {
            sprites.Add(sprite);

            if (width == -1)
                this.width = _original_width;
            else
                this.width = width;
            if (height == -1)
                this.height = _original_height;
            else
                this.height = height;


            _w_by2 = this.width / 2;
            _h_by2 = this.width / 2;

            _scaleX = this.width / _original_width;
            _scaleY = this.height / _original_height;

            frames++;

            if (autoIncrement)
                frame++;
        }

        public void update()
        {
            float _x, _y;
      
            x = spriteMono.x - SPXManager.SCREEN_WIDTH_BY_2 + _w_by2;
            y = spriteMono.y - SPXManager.SCREEN_HEIGHT_BY_2 + _h_by2;


            _x = (x * SPXManager.X_MAX) / Screen.width;
            _y = -(y * SPXManager.Y_MAX) / Screen.height;


            gameObject.transform.localPosition = new Vector3(_x, _y, 1);

            if (alpha != 1)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            }


            //---------------------------------------------------------------
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

	}



    public class SpritexMono : MonoBehaviour
    {

        public float x= 0, y = 0;

    }

}
