using System;
using System.Collections.Generic;


public enum ColorType
{ 
    A = 1,
    B = 2,
    C = 3,
    CANT_CREATE_NOW = 0xFF
}

public class ColorTypeList
{
    public static ColorType[] colorTypes = new ColorType[]{ColorType.A, ColorType.B, ColorType.C};

    public static ColorType GetAnyRandom()
    {
        return colorTypes[UnityEngine.Random.Range(0, colorTypes.Length)];
    }
}