using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class VRCamEmulator : MonoBehaviour
{
#if UNITY_EDITOR
    private Transform tr;

    public float speedH = 4.0f;
    public float speedV = 4.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public void Start()
    { 
        tr = this.GetComponent<Transform>();

        this.GetComponent<OVRCameraRig>().enabled = false;
    }

    public void Update()
    {
        if (Input.GetMouseButton(1))
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            tr.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
    }
#endif

}

