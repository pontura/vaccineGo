using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformGizmos : MonoBehaviour {

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
    #endif
}
