using UnityEngine;
using System;
using System.Collections.Generic;
using GEX.graphics;
using GEX.io;


namespace GEX.components
{
	public class GameStick
	{

        public float axisX, axisY;//from -1 to 1

        private bool ENABLE, VISIBLE;
        private Spriteg backStick, stick;
        private int _startX, _startY;
        private int _mode;

        public const int MODE_COMMON =              0;
        public const int MODE_HORIZONTAL_FOLLOW =   1;

        public GameStick(Texture2D imageBack, Texture2D imageStick, int size, int mode)
        {
            backStick = new Spriteg(imageBack,  size);
            stick =     new Spriteg(imageStick, size / 4);

            backStick.isCentered =  true;
            stick.isCentered =      true;

            _mode = mode;

            _resetGraphics();

            axisX = axisY = 0;

            VISIBLE = true;
            ENABLE = false;
        }

        private void _resetGraphics()
        {
            backStick.x = -backStick.width;
            stick.x = -stick.width;
        }


        public void setVisibility(bool visible)
        {
            VISIBLE = visible;
        }

        public void enable()
        {

            _startX = Mouse.x;
            _startY = Mouse.y;

            backStick.setPos(_startX, _startY);
            ENABLE = true;
        }

        public void disable()
        {
            ENABLE = false;
            _resetGraphics();
        }

        public void update()
        {
            float _sin, _cos;
            int dx, dy;
            float _angle;
            float _hipo_;
            int half_back = (int)(backStick.width / 2);
            float mov_x, mov_y;

            if (ENABLE)
            {

                dx = Mouse.x - _startX;
                dy = Mouse.y - _startY;

                mov_x = dx / (backStick.width / 2);
                mov_y = dy / (backStick.height / 2);

                _angle = (Mathf.Atan2(mov_y, mov_x));


                if (mov_x > 1)
                    mov_x = 1;
                else if (mov_x < -1)
                    mov_x = -1;
                if (mov_y > 1)
                    mov_y = 1;
                else if (mov_y < -1)
                    mov_y = -1;

                axisX = mov_x;
                axisY = mov_y;

                _cos = Mathf.Cos(_angle);
                _sin = Mathf.Sin(_angle);


                _hipo_ = Mathf.Sqrt((dx * dx) + (dy * dy));
                if (_hipo_ > half_back)
                    _hipo_ = half_back;

                if (_mode == MODE_HORIZONTAL_FOLLOW)
                {
                    if (_hipo_ == half_back)
                    {
                        if (dx > 0)
                            _startX = Mouse.x - half_back;
                        else
                            _startX = Mouse.x + half_back;
                        _startY = Mouse.y;

                        backStick.setPos(_startX, _startY);
                    }
                }

                //Debug.Log(_cos + "     " + _sin);
                //Debug.Log(mov_x + "   " + mov_y);

                stick.x = backStick.x + (_cos * _hipo_);
                stick.y = backStick.y + (_sin * _hipo_);

            }
        }

        public void destroy()
        {
            backStick.destroy();
            backStick = null;
        }

        public void paint()
        {
            if (ENABLE && VISIBLE)
            {
                backStick.paint();
                stick.paint();
            }
        }

	}
}
