using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//kzlukos@gmail.com
//
public abstract class VRLookAt : MonoBehaviour
{
    public virtual void LookAtStart() { }
    public virtual void LookAtStop() { }
    public virtual void LookAtUpdate() { }
}

