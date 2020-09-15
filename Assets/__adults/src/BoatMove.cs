using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BoatMove : MonoBehaviour
{

    public Transform tr;

    public void Start()
    { 
        tr = this.GetComponent<Transform>();
    }

    public void Update()
    {
        tr.transform.position += new Vector3(3f * Time.deltaTime, 0, 0);
    }

}