using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ParrotCollider : MonoBehaviour
{
    public SoundPlayer SOUND;
    private bool hitted = false;

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "BALL") && !hitted)
        {
            SOUND.Play("hit_" + UnityEngine.Random.Range(0, 3));
            hitted = true;
        }
    }

    public bool WasHitted()
    {
        if (hitted)
        {
            hitted = false;
            return true;
        }
        else
            return false;
    }
}
