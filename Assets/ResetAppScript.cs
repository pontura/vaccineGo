using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAppScript : MonoBehaviour {

	void Update()
	{
		if(OVRInput.Get(OVRInput.Axis1D.Any)>0.9f){
			//PlayerPrefs.DeleteAll ();
			//UnityEngine.SceneManagement.SceneManager.LoadScene ("ResetApp");
		}
	}
}
