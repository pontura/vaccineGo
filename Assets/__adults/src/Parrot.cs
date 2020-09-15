using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GEX.utils;

public class Parrot : MonoBehaviour
{
    public Animator animator;
    //public GameObject particleSpawnPos;
    public GameObject particles;
    private XTimer timer;
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

        GameObject obj = GameObject.Instantiate(particles, particles.transform.position, Quaternion.identity);
        obj.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "BALL") && !hitted)
        {
            LaunchHitAnim();
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

}
