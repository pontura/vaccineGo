using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SubState
{
    // ON GAME LOAD
    public const int SetupAll =         0;
    public const int SetupExtras =      1;
    public const int ShowSplash =       2;

    // ON LOGIN
    public const int OnLoginInit =  0;

    // ON MAIN MENU
    public const int MMenuSelect =  0;
    public const int MMenuOptions=  1;
    public const int MMenuCredits = 2;
    public const int MMenuShop =    3;
    
    // ON GAME
    public const int OnMenu =       0;
    public const int OnPlay =       1;
    

}
