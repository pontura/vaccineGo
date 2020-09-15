using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GEX.utils;


public class Dolphin : MonoBehaviour
{
    public Animator animator;
    private XTimer timer;
    public SoundPlayer SOUND;
    private bool hitted = false;


    public void Start()
    {
        timer = new XTimer();
        timer.setDelay(2f);
    }

    public void LaunchHitAnim()
    {
        animator.SetTrigger("hit");
        hitted = true;
        timer.start();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "BALL") && !hitted)
        {
            LaunchAnim("hit");
            SOUND.PlayRandom(new String[] { "fx_dolphin_01", "fx_dolphin_02", "fx_dolphin_03" });
        }
    }

    public void Update()
    {
        if (hitted)
        {
            if (timer.update())
                hitted = false;
        }
    }


    public void LaunchAnim(String str)
    {
        animator.SetTrigger(str);
    }

    public void SetAnimBool(String str, bool val)
    {
        animator.SetBool(str, val);
    }

    public void SetAnimOn(String animName)
    {
        animator.SetBool(animName, true);
    }

    public void SetAnimOff(String animName)
    {
        animator.SetBool(animName, false);
    }

}
