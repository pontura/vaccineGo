using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GEX.utils;
using DG.Tweening;


public class BeachBall : MonoBehaviour
{
    public ColorType type { get { return _type; } }

    public GameObject ballA, ballB, ballC;
    public GameObject hitParticle;
    public SoundPlayer SOUND;
    private XTimer timer;
    private bool hit = false;
    private ColorType _type;
    private bool holedIn = false;
    Transform worldParent;

    public void Awake()
    {

    }

    public void Init(Transform container)
    {
        worldParent = transform.parent;
        transform.SetParent(container);
        transform.localPosition += transform.forward * 0.5f;
    }

    public void SetType(ColorType type)
    {
        GameObject b = null;
        this._type = type;

        ballA.SetActive(false);
        ballB.SetActive(false);
        ballC.SetActive(false);

        if (type == ColorType.A) b = ballA;
        else if (type == ColorType.B) b = ballB;
        else if (type == ColorType.C) b = ballC;

        b.SetActive(true);
    }

    public void Launch()
    {
        transform.SetParent(worldParent);
        timer = new XTimer();   
        timer.setDelay(4f);
        timer.start();

        this.transform.GetComponent<Rigidbody>().useGravity = true;
        this.transform.GetComponent<Rigidbody>().detectCollisions = true;
    }

    private void Hit()
    {
        var gobj = GameObject.Instantiate(hitParticle, this.transform.position, Quaternion.identity);
        gobj.SetActive(true);
    }

    public void ExternalHit()
    {
        hit = true;

        if (!holedIn)
            InGameScore.me.AddFailed();
    }

    public bool CanRelaunch()
    {
        return hit;
    }

    private void OnCollisionEnter(Collision collision)
    {
        String otherTag;
        bool isRingWood = false;
        /*if ((collision.gameObject.tag == "BALL") && !hitted)
        {
            LaunchAnim("hit");
        }*/

        //Debug.Log("SALE: " + collision.gameObject.name + "  TAG:  " + collision.gameObject.tag);

        float speed = collision.relativeVelocity.magnitude;
        otherTag = collision.gameObject.tag;

        if (otherTag != "Player")
        {
            if (speed > 10)
            {
                Hit();

                if (otherTag == "wood")
                {
                    if (UnityEngine.Random.Range(0, 2) == 0)
                        SOUND.Play("fx_wood_hit_01");
                    else
                        SOUND.Play("fx_wood_hit_02");
                }
                else if (otherTag == "ring_wood")
                {
                    if (UnityEngine.Random.Range(0, 2) == 0)
                        SOUND.Play("fx_wood_hit_01");
                    else
                        SOUND.Play("fx_wood_hit_02");

                    isRingWood = true;
                }
                else
                {
                    SOUND.Play("fx_sand_hit_01");
                }
            }

            hit = true;

            if (!isRingWood)
            {
                InGameScore.me.AddFailed();
            }
        }
    }

    public void SetAsHoledIn()
    {
        holedIn = true;
    }

    public bool WasHoledIn()
    {
        return holedIn;
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
        hit = true;
    }


}
