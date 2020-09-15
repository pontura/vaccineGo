using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericHit : MonoBehaviour
{
    [Header("Object to be spawned")]
    public GameObject obj;
    [Header("Object rotation fix")]
    public Vector3 rotationFix;
    [Header("Object translation fix")]
    public Vector3 translationFix; // will be added
    [Header("if empty -> always triggered")]
    public String triggerOnTag = ""; // if empty = any
    [Header("if empty -> this position used")]
    public Transform spawnPos;
    [Header("overwrite XZ from collision.")]
    public bool overwriteXZ = false;
    [Header("TRIGGER BALL EVENT.")]
    public bool TRIGGER_BALL_EVENT = false;
    /*[Header("spawn pos for overwrite Y")]
    public Transform spawnPosForY;*/


    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnTag != "")
        {
            if (other.gameObject.tag == triggerOnTag)
            {
                //triggered = true;
                CreateHit(other);

                // Hardcode
                if (TRIGGER_BALL_EVENT && (triggerOnTag == "BALL"))
                    other.GetComponent<BeachBall>().ExternalHit();
            }
        }
        else
        {
            CreateHit(other);
        }
        
        
        
    }

    private void CreateHit(Collider other)
    {
        Vector3 pos;

        if (spawnPos != null)
            pos = spawnPos.position;
        else
            pos = this.transform.position;

        if (overwriteXZ)
        {
            pos.x = other.transform.position.x;
            pos.z = other.transform.position.z;
        }


        pos += translationFix;

        /*if(spawnPosForY != null)
            pos.y = spawnPosForY.position.y;*/

        GameObject newObj = GameObject.Instantiate(obj, pos, Quaternion.Euler(rotationFix));
        newObj.SetActive(true);
    }

}
