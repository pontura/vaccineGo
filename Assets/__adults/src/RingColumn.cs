using System;
using System.Collections.Generic;
using UnityEngine;


public struct RingInfo
{
    public Vector3 pos;
    public int colIndex;

    public RingInfo(Vector3 pos, int colIndex)
    {
        this.pos = pos; this.colIndex = colIndex;
    }
}

public class RingColumn : MonoBehaviour
{
    public RingObj[] rings { get { return _rings; } }

    public Transform [] spawns;
    private RingObj [] _rings;


    public void Init()
    {
        _rings = new RingObj[spawns.Length];
        for (int a = 0; a < _rings.Length; a++)
        {
            _rings[a] = null;
        }
    }

    public bool AreSlotsEmpty()
    {
        for (int a = 0; a < _rings.Length; a++)
        {
            if (_rings[a] != null)
                return false;
        }

        return true;
    }

    public void SetRing(RingObj ringObj, int colIndex)
    {

        _rings[colIndex] = ringObj;

    }

    public RingInfo GetRandomSpawnInfo()
    {
        int rndIndex =  UnityEngine.Random.Range(0, spawns.Length);
        Vector3 pos =   spawns[rndIndex].position;
        RingInfo info = new RingInfo(pos, rndIndex);
        return info;
    }

}
