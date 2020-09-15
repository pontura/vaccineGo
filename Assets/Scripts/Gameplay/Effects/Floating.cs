using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//kzlukos@gmail.com
//Oscilates local y position over time and optionally rotates the object
public class Floating : MonoBehaviour {

	[SerializeField] private Transform floatingTransform;

	[Header("Rotation")]
	[SerializeField] float rotationSpeed = 20f;
	[SerializeField] private bool x;
	[SerializeField] private bool y;
	[SerializeField] private bool z;

	[Header("Position")]
	[SerializeField] float oscilationAmplitude = 0.5f;
	[SerializeField] float oscilationSpeed = 1f;

    private float _startTime;

	//
	void Awake() {
		ResetTime ();
	}

    //
    void Update()
    {
		float osiclationTime = Time.time - _startTime;
		floatingTransform.localPosition = Vector3.up * Mathf.Sin(osiclationTime * oscilationSpeed) * oscilationAmplitude;

		if(x)
			floatingTransform.Rotate(Vector3.right, Time.deltaTime * rotationSpeed);
		if(y)
			floatingTransform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
		if(z)
			floatingTransform.Rotate(Vector3.forward, Time.deltaTime * rotationSpeed);

    }

	//
	public void ResetTime()
	{
		_startTime = Time.time;
	}


}


