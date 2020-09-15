using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//kzlukos@gmail.com
//
public class VRLookCamera : MonoBehaviour
{
    public Ray GazeRay
    {
        get { return new Ray(transform.position, transform.forward); }
    }
		
    private VRLookAt _prevLookAt = null;


    //
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500, (1 << 20)))
        {
           
            VRLookAt lookAtComponent = hit.transform.GetComponent<VRLookAt>();
            if (lookAtComponent != null && lookAtComponent.enabled) {

                if (lookAtComponent != _prevLookAt)
                {
                    if (_prevLookAt != null)
                        _prevLookAt.LookAtStop();
                    _prevLookAt = lookAtComponent;
                    lookAtComponent.LookAtStart();
                }
                lookAtComponent.LookAtUpdate();

            } else
            {
                ResetLookAt();
            }

        }
        else
        {
            ResetLookAt();
        }
    }

    //
    private void ResetLookAt()
    {
        if (_prevLookAt != null)
            _prevLookAt.LookAtStop();
        _prevLookAt = null;
    }

}