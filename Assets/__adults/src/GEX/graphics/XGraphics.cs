using UnityEngine;
using System;
using System.Collections.Generic;

namespace GEX.graphics
{
	class XGraphics
	{
        public const uint HCENTER = 0x01;
        public const uint VCENTER = 0x02;
        public const uint LEFT =    0x04;
        public const uint RIGHT =   0x08;
        public const uint TOP =     0x10;
        public const uint BOTTOM =  0x20;
        public const uint BASELINE= 0x40;


        public static int getPosFromLeft(float percent)
        {
            return (int)((Screen.width * percent) / 100f);
        }

        public static int getPosFromRight(float percent)
        {
            return Screen.width - (int)((Screen.width * percent) / 100f);
        }

        public static int getPosFromTop(float percent)
        {
            return (int)((Screen.height * percent) / 100f);
        }

        public static int getPosFromBottom(float percent)
        {
            return Screen.height - (int)((Screen.height * percent) / 100f);
        }

	}
}
