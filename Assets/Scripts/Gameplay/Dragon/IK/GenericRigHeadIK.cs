using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// kzlukos@gmail.com
// Head bone IK for generic Rig (looking at target)
public class GenericRigHeadIK : MonoBehaviour {

	public Transform targetTransform;
	[SerializeField]
	protected Transform headBone;
	[SerializeField]
	protected float weightLerpFactor = 0.05f;

	public float IKWeight = 0f;
	protected float _smoothIKWeigh = 0f;
	private Quaternion _defaultRotation;


	void Start()
	{
		StartCoroutine (ResetRotationCoroutine ());
	}

	//
	protected virtual void LateUpdate()
	{
		IKWeight = Mathf.Clamp (IKWeight, 0f, 1f);
		_smoothIKWeigh = Mathf.Lerp (_smoothIKWeigh, IKWeight, weightLerpFactor);

		_defaultRotation = headBone.transform.rotation;
		Vector3 targetDirection = (targetTransform.position - headBone.position);

		Vector2 v1 = new Vector2 (targetDirection.x, targetDirection.z);
		Vector2 v2 = new Vector2 (headBone.up.x, headBone.up.z);
		float horisontalAxisAngle = v1.GetSignedAngle(v2);

		v1 = new Vector2 (Mathf.Sqrt (Mathf.Pow (targetDirection.magnitude, 2)) - Mathf.Pow (targetDirection.y, 2), targetDirection.y);
		v2 = new Vector2 (Mathf.Sqrt (1f - Mathf.Pow (headBone.up.y, 2)), headBone.up.y); // normalized
		float verticalAngle = v1.GetSignedAngle(v2);


		headBone.Rotate (Mathf.Clamp(horisontalAxisAngle, -50f, 50f), 0f, Mathf.Clamp(verticalAngle, -30f, 30f));
		headBone.rotation = Quaternion.Lerp (_defaultRotation, headBone.rotation, _smoothIKWeigh);
	}


	//
	private IEnumerator ResetRotationCoroutine()
	{
		while (true) {
			yield return new WaitForEndOfFrame ();
			headBone.transform.rotation = _defaultRotation;
		}
	}
		
}
