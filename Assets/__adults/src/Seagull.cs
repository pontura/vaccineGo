using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

public class Seagull : MonoBehaviour
{
    private Transform tr;
    private Vector3 startPos;
    private bool _prepareToKill = false;
    private bool _canRemove = false;
    private const float MAX_DISTANCE_AND_KILL = 100f;

    public void Start()
    { 
        tr = GetComponent<Transform>();
        startPos = tr.position;
    }

    
    public void Update()
    {
        tr.transform.position += tr.forward * (3f * Time.deltaTime);//new Vector3(3f * Time.deltaTime, 0, 0);

        float distance = Vector3.Distance(startPos, tr.transform.position);

        if (!_prepareToKill)
        {
            if (distance > MAX_DISTANCE_AND_KILL)
            {
                tr.transform.DOScale(0f, 5f).OnComplete(OnDeathComplete);
                _prepareToKill = true;
            }
        }
    }

    private void OnDeathComplete()
    {
        _canRemove = true;
    }

    public bool CanRemove()
    {
        return _canRemove;
    }

    public void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }

}
