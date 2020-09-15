using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// kzlukos@gmail.com
// Parent class for all singleton objects derived form MonoBehaviour
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    public static int test;
    public static T Instance { get; protected set; }

    //
	void Awake()
	{
		Instance = GetComponent<T>();
	}
    public virtual void OnEnable()
    {
        Instance = GetComponent<T>();
    }
}
