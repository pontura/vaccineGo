using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum ColorMode
{
    ONLY_A,
    ONLY_B,
    ONLY_C,
    DIFFERENT_COLOR,
    DIFFERENT_COLOR_NO_REPEAT
}

public enum KillMode
{
    ONE_SHOT,
    TWO_SHOTS,
    MULTIPLE_SHOTS
}

public enum RingPosition
{ 
    ANY,
    MIDDLE,
    TWO_MIDDLE,
    ONE_RANDOM
}

public enum RingDistance
{ 
    NEAR,
    FAR
}

public enum ExtraCondition
{ 
    NONE,
    FAIL_WILL_RESET,
    FAIL_WILL_GOTO_NEXT_GAME
}

public enum CountMode
{
    COUNT_SCORE,
    COUNT_HOLE_IN
}

public enum LevelKind
{ 
    NORMAL,
    SPECIAL
}

public enum RingNumberMode
{ 
    NOTHING,
    DECREMENTAL,
    INCREMENTAL
}

public class Level
{
    public static List<Level> LEVELS;
    public static Level OLD_SPECIAL_LEVEL;
    public static Level SPECIAL_LEVEL;
    private static bool _init = false;

    public GMode GAME_MODE;
    public ColorMode COLOR_MODE;
    public KillMode KILL_MODE;
    public int FIXED_SHOTS; // amount of shots to kill ring
    public bool RESPAWN_AFTER_COMPLETE_ALL;
    public int TARGET;
    public RingPosition RING_POSITION;
    public RingDistance RING_DISTANCE;
    public bool SPECIAL_END_MODE;
    public int GLOW_IN_SECONDS = -1;// = -1; // -1 no glow
    public ExtraCondition EXTRA_CONDITION = ExtraCondition.NONE;
    public CountMode COUNT_MODE = CountMode.COUNT_SCORE;
    public LevelKind LEVEL_KIND = LevelKind.NORMAL;
    public RingNumberMode RING_NUMBERS = RingNumberMode.DECREMENTAL;

    public static void Initialize(bool reset = false)
    {
        if (reset)
        { }
        else
        {
            if (_init)
                return;
        }

        LEVELS = new List<Level>();

        //training
        AddLevel(GMode.STATIC_MIDDLE, ColorMode.ONLY_A, KillMode.MULTIPLE_SHOTS, 5, true, 50, RingPosition.MIDDLE, RingDistance.NEAR, false, -1, ExtraCondition.NONE, CountMode.COUNT_SCORE, LevelKind.NORMAL, RingNumberMode.NOTHING);

        AddLevel(GMode.STATIC_MIDDLE, ColorMode.DIFFERENT_COLOR_NO_REPEAT, KillMode.MULTIPLE_SHOTS, 10, true, 150, RingPosition.TWO_MIDDLE, RingDistance.NEAR, false, 7, ExtraCondition.NONE, CountMode.COUNT_SCORE, LevelKind.NORMAL, RingNumberMode.NOTHING);
        AddLevel(GMode.STATIC_MIDDLE, ColorMode.ONLY_A, KillMode.MULTIPLE_SHOTS, 20, true, 100, RingPosition.MIDDLE, RingDistance.FAR, false, 8, ExtraCondition.NONE, CountMode.COUNT_SCORE, LevelKind.NORMAL, RingNumberMode.NOTHING);

        AddSpecial();

        AddLevel(GMode.STATIC_MIDDLE, ColorMode.DIFFERENT_COLOR_NO_REPEAT, KillMode.MULTIPLE_SHOTS, 10, true, 150, RingPosition.TWO_MIDDLE, RingDistance.FAR, false, 8, ExtraCondition.NONE, CountMode.COUNT_SCORE, LevelKind.NORMAL, RingNumberMode.NOTHING);
        AddLevel(GMode.STATIC_MIDDLE, ColorMode.ONLY_C, KillMode.MULTIPLE_SHOTS, 20, true, 300, RingPosition.MIDDLE, RingDistance.FAR, false, 6, ExtraCondition.NONE, CountMode.COUNT_SCORE, LevelKind.NORMAL, RingNumberMode.NOTHING);

        AddSpecial();


        AddLevel(GMode.STATIC_MIDDLE, ColorMode.ONLY_B, KillMode.MULTIPLE_SHOTS, 20, true, 10, RingPosition.MIDDLE, RingDistance.NEAR, false, 6, ExtraCondition.FAIL_WILL_GOTO_NEXT_GAME, CountMode.COUNT_HOLE_IN, LevelKind.NORMAL, RingNumberMode.INCREMENTAL);
        AddLevel(GMode.STATIC_MIDDLE, ColorMode.DIFFERENT_COLOR, KillMode.MULTIPLE_SHOTS, 25, true, 15, RingPosition.ONE_RANDOM, RingDistance.NEAR, false, 12, ExtraCondition.FAIL_WILL_RESET, CountMode.COUNT_HOLE_IN, LevelKind.NORMAL, RingNumberMode.INCREMENTAL);



        /*AddLevel(GMode.STATIC_MIDDLE, ColorMode.ONLY_A, KillMode.MULTIPLE_SHOTS, 20, true, 200, RingPosition.MIDDLE, RingDistance.NEAR, false);
        AddLevel(GMode.STATIC_MIDDLE, ColorMode.DIFFERENT_COLOR_NO_REPEAT, KillMode.MULTIPLE_SHOTS, 10, true, 300, RingPosition.TWO_MIDDLE, RingDistance.NEAR, false);

        AddLevel(GMode.STATIC_MIDDLE, ColorMode.ONLY_A, KillMode.MULTIPLE_SHOTS, 20, true, 200, RingPosition.MIDDLE, RingDistance.FAR, false);
        AddLevel(GMode.STATIC_MIDDLE, ColorMode.DIFFERENT_COLOR_NO_REPEAT, KillMode.MULTIPLE_SHOTS, 10, true, 300, RingPosition.TWO_MIDDLE, RingDistance.FAR, false);*/


        /*AddLevel(GMode.STATIC_DOWN,     ColorMode.ONLY_A,           KillMode.ONE_SHOT, -1, true, 30, RingPosition.ANY, RingDistance.NEAR, false);
        AddLevel(GMode.STATIC_MIDDLE,   ColorMode.ONLY_B,           KillMode.TWO_SHOTS, -1, true, 120, RingPosition.ANY, RingDistance.NEAR, false);
        AddLevel(GMode.STATIC_MIDDLE,   ColorMode.DIFFERENT_COLOR,  KillMode.ONE_SHOT, -1, false, 220, RingPosition.ANY, RingDistance.NEAR, false);*/
        //AddLevel(GMode.MOVE,            ColorMode.DIFFERENT_COLOR,  KillMode.MULTIPLE_SHOTS, -1, false, 320, RingPosition.ANY, RingDistance.NEAR, false);
        //AddLevel(GMode.MOVE,            ColorMode.DIFFERENT_COLOR,  KillMode.MULTIPLE_SHOTS, -1, false, 800, RingPosition.ANY, RingDistance.NEAR, false);

        OLD_SPECIAL_LEVEL = Create(GMode.SPECIAL, ColorMode.ONLY_A, KillMode.TWO_SHOTS, -1, true, 1000, RingPosition.ANY, RingDistance.NEAR, true, -1, ExtraCondition.NONE, CountMode.COUNT_SCORE, LevelKind.SPECIAL, RingNumberMode.DECREMENTAL);

        SPECIAL_LEVEL = Create(GMode.STATIC_MIDDLE, ColorMode.DIFFERENT_COLOR, KillMode.MULTIPLE_SHOTS, 20, false, 10000, RingPosition.MIDDLE, RingDistance.NEAR, false, -1, ExtraCondition.NONE, CountMode.COUNT_SCORE, LevelKind.SPECIAL, RingNumberMode.DECREMENTAL);

        _init = true;
    }

    public static Level CreateSpecialLvl(int ammountOfShots)
    {
        return Create(GMode.STATIC_MIDDLE, ColorMode.DIFFERENT_COLOR, KillMode.MULTIPLE_SHOTS, ammountOfShots, false, 10000, RingPosition.MIDDLE, RingDistance.NEAR, false, -1, ExtraCondition.NONE, CountMode.COUNT_HOLE_IN, LevelKind.SPECIAL, RingNumberMode.NOTHING);
    }

    private static void AddLevel(GMode mode, ColorMode colorMode, KillMode killMode, int fixedShots, bool respawnAfterCompleteAll, int target, RingPosition ringPosition, RingDistance ringDistance, bool specialEndMode, int glowInSeconds, ExtraCondition extraCondition, CountMode countMode, LevelKind levelKind, RingNumberMode ringNumbers)
    {
        LEVELS.Add(Create(mode, colorMode, killMode, fixedShots, respawnAfterCompleteAll, target, ringPosition, ringDistance, specialEndMode, glowInSeconds, extraCondition, countMode, levelKind, ringNumbers));
    }

    private static void AddSpecial()
    {
        LEVELS.Add(CreateSpecialLvl(10000));
    }

    private static Level Create(GMode mode, ColorMode colorMode, KillMode killMode, int fixedShots, bool respawnAfterCompleteAll, int target, RingPosition ringPosition, RingDistance ringDistance, bool specialEndMode, int glowInSeconds, ExtraCondition extraCondition, CountMode countMode, LevelKind levelKind, RingNumberMode ringNumbers)
    {
        Level lvl = new Level();
        lvl.GAME_MODE =                     mode;
        lvl.COLOR_MODE =                    colorMode;
        lvl.KILL_MODE =                     killMode;
        lvl.FIXED_SHOTS=                    fixedShots;
        lvl.TARGET =                        target;
        lvl.RESPAWN_AFTER_COMPLETE_ALL =    respawnAfterCompleteAll;
        lvl.RING_POSITION =                 ringPosition;
        lvl.RING_DISTANCE =                 ringDistance;
        lvl.SPECIAL_END_MODE =              specialEndMode;
        lvl.GLOW_IN_SECONDS =               glowInSeconds;
        lvl.EXTRA_CONDITION =               extraCondition;
        lvl.COUNT_MODE =                    countMode;
        lvl.LEVEL_KIND =                    levelKind;
        lvl.RING_NUMBERS =                  ringNumbers;
        return lvl;
    }
}
