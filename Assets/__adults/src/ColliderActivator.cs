using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ColliderActivator : MonoBehaviour
{
    public bool triggered = false;
    public ColorType type = ColorType.A;
    public BeachBall ball = null; //null if never collides

    public void Start()
    { 
    
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("enter:" + other.gameObject.tag);

        if (other.gameObject.tag == "BALL")
        {
            ball = other.gameObject.GetComponent<BeachBall>();
            type = ball.type;
            triggered = true;
        }
    }

    public void ResetTrigger()
    {
        triggered = false;
    }

    public void Update()
    { 
    
    }
}
