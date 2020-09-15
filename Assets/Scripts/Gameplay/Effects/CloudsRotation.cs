using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// kzlukos@gmail.com
// Rotates cloud parent object
public class CloudsRotation : MonoBehaviour {

	[SerializeField]
	private float rotationSpeed = 1f;

	//
	void Update () {
		transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * rotationSpeed);

	}
}

