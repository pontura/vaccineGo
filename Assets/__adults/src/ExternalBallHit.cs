using System;
using System.Collections.Generic;
using UnityEngine;

public class ExternalBallHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BALL")
        {
            if (other != null)
                other.GetComponent<BeachBall>().ExternalHit();
        }
    }
}
