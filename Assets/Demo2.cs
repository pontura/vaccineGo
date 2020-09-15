using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo2 : MonoBehaviour
{
    public Text field;
    float init_x;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            field.text = "Two";
        }
        else if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            field.text = "PrimaryIndexTrigger";
        }
        else if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
        {
            field.text = "PrimaryTouchpad";
        }
        if (OVRInput.GetDown(OVRInput.Touch.Any))
        {
            init_x =  OVRInput.Get(OVRInput.Axis2D.Any).x;
        } else if (OVRInput.GetUp(OVRInput.Touch.Any))
        {
            if (init_x == 0)
                return;
            float ends = OVRInput.Get(OVRInput.Axis2D.Any).x;
            if (ends == init_x)
                print("nothing");
            else if (ends + 0.1f < init_x)
                field.text = "  Swipe Left";
            else if (ends - 0.1f > init_x)
                field.text = "  Swipe Right";
            init_x = 0;
        }
        //if (OVRInput.Get(OVRInput.Axis2D.Any).x != 0)
        //{
        //    field.text += "  axis_x: " + OVRInput.Get(OVRInput.Axis2D.Any).x;
        //}
    }
}
