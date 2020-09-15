using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GEX.utils;
using DG.Tweening;


public class Ring : MonoBehaviour
{

    private XTimer timer;

    public void Awake()
    { 
        
    }

    public void Start()
    {
        timer = new XTimer();
        timer.setDelay(4f);
        timer.start();
    }

    public void Update()
    {
        if (timer != null)
        {
            if (timer.update())
            {

                //GameObject.Destroy(this.gameObject);

                this.transform.GetComponent<Rigidbody>().useGravity = false;
                this.transform.GetComponent<Rigidbody>().detectCollisions = false;
                this.gameObject.transform.DOScale(0f, 1f).OnComplete(OnComplete);

                timer = null;
            }
        }
    }

    private void OnComplete()
    {
        GameObject.Destroy(this.gameObject);
    }

}
