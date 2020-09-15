using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum GMode
{ 
    STATIC_DOWN,
    STATIC_MIDDLE,
    MOVE,
    SPECIAL
}

public class GameMode
{
    public static GMode MODE = GMode.STATIC_DOWN; 
}