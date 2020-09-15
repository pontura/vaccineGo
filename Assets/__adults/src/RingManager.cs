using System;
using System.Collections.Generic;
using UnityEngine;
using GEX.utils;

public class RingManager : MonoBehaviour
{
    public static RingManager me
    {
        get
        {
            if (_me == null)
                _me = GameObject.FindObjectOfType<RingManager>();

            return _me;
        }
    }

    public RingColumn[] columns;
    public GameObject ring;
    private static RingManager _me;
    private bool[] holed;
    private bool _ACTIVE = false;
    //private GMode GAME_MODE;
    private bool _init = false;
    private Level LEVEL;
    private bool spawnAll = true;
    private Vector3 originalPos;
    private int ringsCompletedCount = 0;
    private XTimer glowTimer = null;
    private int glowTimerState = 0;
    
    public void Start()
    {
        originalPos = transform.position;

 

        //CreateRing(RingType.A, columns[0].spawns[0].transform.position);
    }

    public void Begin(Level level)
    {
        int a;

        this.LEVEL = level;

        //if (!_init)
        //{
            for (a = 0; a < columns.Length; a++)
                columns[a].Init();
        
           /* _init = true;
        }*/

        holed = new bool[columns.Length];
        for (a = 0; a < holed.Length; a++)
            holed[a] = true;


        if (level.RING_DISTANCE == RingDistance.NEAR)
        {
            transform.position = originalPos;
        }
        else if (level.RING_DISTANCE == RingDistance.FAR)
        {
            transform.position = new Vector3(originalPos.x, originalPos.y, originalPos.z + 7f);            
        }

        ringsCompletedCount = 0;

        print("LEVEL.GLOW_IN_SECONDS " + LEVEL.GLOW_IN_SECONDS);

        if (LEVEL.GLOW_IN_SECONDS != -1)
        {
            glowTimer = new XTimer();
            glowTimer.setDelay((float)LEVEL.GLOW_IN_SECONDS);
            glowTimer.start();
            glowTimerState = 0;
        }
        else
            glowTimer = null;

        _ACTIVE = true;
        spawnAll = true;
    }

    public void AddRingAsComplete()
    {
        ringsCompletedCount++;
    }

    public bool CompleteRing(bool autoReset = false)
    {
        if (ringsCompletedCount > 0)
        {
            if (autoReset)
                ringsCompletedCount = 0;
            
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetGlowActive()
    {
        RingObj[] _rings_ = GetAllActiveRings();

        for (int a = 0; a < _rings_.Length; a++)
        {
            _rings_[a].SetGlowActive();
        }

        Environment.me.Fireworks();
    }

    private void SetGlowPreMessage(bool active)
    {
        RingObj[] _rings_ = GetAllActiveRings();

        for (int a = 0; a < _rings_.Length; a++)
            _rings_[a].SetGlowPreMessage(active);
    }

    private RingObj [] GetAllActiveRings()
    {
        List<RingObj> _rings_ = new List<RingObj>();
        RingObj [] r;

        for (int a = 0; a < columns.Length; a++)
        {
            if (columns[a] != null)
            { 
                r = columns[a].rings;
                if(r != null)
                {
                    for (int b = 0; b < r.Length; b++)
                    {
                        if (r[b] != null)
                            _rings_.Add(r[b]);
                    }
                }
            }
        }

        return _rings_.ToArray();
    }

    public void Update()
    {
        if (!_ACTIVE)
            return;

        //
        // GLOW if timer passed
        //
        if (glowTimer != null)
        {
            if (glowTimerState == 0)
            {
                if (glowTimer.update())
                {
                    InGameBoard.InGameBoardModule.me.TalkSpecialBeQuiet(1);
                    SetGlowPreMessage(true);
                    glowTimerState = 1;
                }
            }
            else if (glowTimerState == 1)
            {
                if (InGameBoard.InGameBoardModule.me.TalkFinished())
                {
                    glowTimer = null;
                    SetGlowPreMessage(false);
                    SetGlowActive();
                    glowTimerState = -1;
                }
            }
        }

        if (LEVEL.SPECIAL_END_MODE)
        {
            ComputeCol(columns[1]);
            return;
        }

        if(LEVEL.RESPAWN_AFTER_COMPLETE_ALL)
        {
            if (!AreAllHoled())
                return;
        }

        if (LEVEL.RING_POSITION == RingPosition.MIDDLE)
        {
            ComputeCol(columns[1]);
            return;
        }
        else if (LEVEL.RING_POSITION == RingPosition.ONE_RANDOM)
        {
            ComputeCol(columns[UnityEngine.Random.Range(0, 2)]);
            return;
        }
        else if (LEVEL.RING_POSITION == RingPosition.TWO_MIDDLE)
        {
            ComputeCol(columns[0], ColorType.A);
            ComputeCol(columns[1], ColorType.B);
            return;
        }


        if (spawnAll)
        {
            for (int a = 0; a < columns.Length; a++)
                ComputeCol(columns[a]);

            spawnAll = false;
            return;
        }


        for (int a = 0; a < columns.Length; a++)
        {
            if (LEVEL.RESPAWN_AFTER_COMPLETE_ALL)
            {
                if (!holed[a])
                {
                    holed[a] = columns[a].AreSlotsEmpty();
                }
                else if (AreAllHoled())
                {
                    spawnAll = true;
                }
                
            }
            else
            {
                ComputeCol(columns[a]);
            }
        }

    }

    public ColorType GetNextColorType()
    {
        RingObj r = null;
        ColorType colType = ColorType.A;
        //Debug.Log("GET NEXT COLOR!!!!!!!!!");

        if (LEVEL.COLOR_MODE == ColorMode.ONLY_A)       return ColorType.A;
        else if (LEVEL.COLOR_MODE == ColorMode.ONLY_B)  return ColorType.B;
        else if (LEVEL.COLOR_MODE == ColorMode.ONLY_C)  return ColorType.C;


        int startAt = UnityEngine.Random.Range(0, ColorTypeList.colorTypes.Length);

        for (int a = startAt; a < me.columns.Length; a++)
        {
            r = me.GetRingObjIfAny(me.columns[a]);
            if (r != null)
            {
                //Debug.Log("ENTER A");
                return r.type;
            }
        }

        // If Cant find any just start from left
        for (int a = 0; a < me.columns.Length; a++)
        {
            r = me.GetRingObjIfAny(me.columns[a]);
            if (r != null)
            {
                //Debug.Log("ENTER B");
                return r.type;
            }
        }
        // ------------------------------------------

        if (r == null)
        {
            //Debug.Log("ENTER C null");
            colType = ColorType.CANT_CREATE_NOW;//ColorTypeList.GetAnyRandom();
        }
        return colType;
    }

    private RingObj GetRingObjIfAny(RingColumn cols)
    {
        for (int a = 0; a < cols.rings.Length; a++)
        {
            if (cols.rings[a] != null)
                return cols.rings[a];
        }

        return null;
    }

    private bool IsAnyHoled()
    {
        for (int a = 0; a < holed.Length; a++)
        {
            if (holed[a])
                return true;
        }

        return false;
    }

    private bool AreAllHoled()
    {
        for (int a = 0; a < holed.Length; a++)
        {
            if (!holed[a])
                return false;
        }

        return true;
    }

    public void KillGame()
    {
        for (int a = 0; a < columns.Length; a++)
        {
            RingObj [] rings = columns[a].rings;
            if (rings != null)
            {
                for (int b = 0; b < rings.Length; b++)
                {
                    if (rings[b] != null)
                        rings[b].ExternalKill();
                }
            }
        }

        _ACTIVE = false;
    }
    
    private bool ComputeCol(RingColumn cols, ColorType specificColorType = ColorType.A)
    {
        RingObj ring;
        RingInfo info;
        ColorType colorType = ColorType.A;

        if (cols.AreSlotsEmpty())
        {
            if (LEVEL.COLOR_MODE == ColorMode.ONLY_A)               colorType = ColorType.A;
            else if (LEVEL.COLOR_MODE == ColorMode.ONLY_B)          colorType = ColorType.B;
            else if (LEVEL.COLOR_MODE == ColorMode.ONLY_C)          colorType = ColorType.C;
            else if (LEVEL.COLOR_MODE == ColorMode.DIFFERENT_COLOR) colorType = ColorTypeList.GetAnyRandom();
            else if (LEVEL.COLOR_MODE == ColorMode.DIFFERENT_COLOR_NO_REPEAT)
                colorType = specificColorType;
            

            info = cols.GetRandomSpawnInfo();
            ring = CreateRing(colorType, info.pos);
            cols.SetRing(ring, info.colIndex);

            for (int a = 0; a < columns.Length; a++)
            {
                if (columns[a] == cols)
                    holed[a] = false;
            }

            return true;
        }
        else
        {
            return false;
        }

    }


    private RingObj CreateRing(ColorType type, Vector3 pos)
    {
        GameObject obj =    GameObject.Instantiate(ring, pos, Quaternion.identity);
        RingObj r =         obj.GetComponent<RingObj>();

        int goal = 1;

        if (LEVEL.FIXED_SHOTS != -1)
        {
            goal = LEVEL.FIXED_SHOTS;
        }
        else if (LEVEL.KILL_MODE == KillMode.ONE_SHOT)
            goal = 1;
        else if (LEVEL.KILL_MODE == KillMode.TWO_SHOTS)
            goal = 2;
        else if(LEVEL.KILL_MODE == KillMode.MULTIPLE_SHOTS)
            goal = UnityEngine.Random.Range(1, 4);

        r.SetType(type);
        r.SetNumbersMode(LEVEL.RING_NUMBERS);

        if (LEVEL.GAME_MODE == GMode.STATIC_DOWN)
        {
            r.Show(RingAnim.None, goal);
        }
        else if (LEVEL.GAME_MODE == GMode.STATIC_MIDDLE)
        {
            r.Show(RingAnim.NoneButMiddle, goal);
        }
        else if (LEVEL.GAME_MODE == GMode.MOVE)
        {
            r.Show(RingAnim.UpDown, goal);
        }
        else if (LEVEL.GAME_MODE == GMode.SPECIAL)
        {
            r.Show(RingAnim.Special, goal);
        }

        return r;
    }

}

