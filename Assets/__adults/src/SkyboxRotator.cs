using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
        public float speedMultiplier;
         
     // Update is called once per frame
     void Update ()
     {
         //Sets the float value of "_Rotation", adjust it by Time.time and a multiplier.
         RenderSettings.skybox.SetFloat("_Rotation", Time.time * speedMultiplier);     
     }
}
