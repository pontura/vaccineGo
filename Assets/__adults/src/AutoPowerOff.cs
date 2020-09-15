using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GEX.utils;

public class AutoPowerOff : MonoBehaviour
{
    public float time = 5f;
    private XTimer timer;
    
    void OnEnable()
    {
        timer = new XTimer();
        timer.setDelay(time);
        timer.start();
    }

    void Update()
    {
        if (timer != null)
        {
            if (timer.update())
            {
                this.gameObject.SetActive(false);
                timer = null;
            }
        }
    }

}
