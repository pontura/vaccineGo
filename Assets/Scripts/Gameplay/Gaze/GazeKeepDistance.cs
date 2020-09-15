using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//kzlukos@gmail.com
//Follows the gaze and keeps fixed distance from the camera
public class GazeKeepDistance : MonoBehaviour {

    public float distance = 6f;

    //
    private void Update()
    {
        if (Camera.main == null)
            return;

        transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance;
        transform.LookAt(Camera.main.transform.position);
    }

}
