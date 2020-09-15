using System;
using System.Collections.Generic;

public enum AgeMode
{
    KIDS,
    ADULTS
}


public class GAME
{

    public static Level LEVEL
    {
        get 
        {
            return Level.LEVELS[LEVEL_NUMBER];
        }
    }

    public static Level OLD_SPECIAL_LEVEL
    {
        get { return Level.OLD_SPECIAL_LEVEL; }
    }

    public static Level SPECIAL_LEVEL
    {
        get { return Level.SPECIAL_LEVEL; }
    }

    public static int LEVEL_NUMBER = 0;
    private static bool _init = false;
    public static AgeMode ageMode = AgeMode.ADULTS;
    public static int vaccines = 1;

    public static void Initialize()
    {
        if (_init)
            return;

        LEVEL_NUMBER = 0;
        Level.Initialize();

        _init = true;
    }

    public static void SetSpecialLevel(Level lvl)
    {
        Level.SPECIAL_LEVEL = lvl;
    }

    public static bool AllLevelsComplete()
    { 
        return LEVEL_NUMBER >= Level.LEVELS.Count;
    }

    public static void ResetLevels()
    {
        LEVEL_NUMBER = 0;
    }

    public static void RemoveLevel(Level lvl)
    {
        Level.LEVELS.Remove(lvl);
    }
}
