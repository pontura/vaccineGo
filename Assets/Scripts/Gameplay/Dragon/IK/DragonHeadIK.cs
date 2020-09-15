using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// kzlukos@gmail.com
// Allows aditional head twist controlled by curves
class DragonHeadIK : GenericRigHeadIK
{
	
	[SerializeField]
	private float cycleTime = 5f;
	[SerializeField]
	private float maxTwistAngle = 90f;

	[SerializeField]
	private AnimationCurve horisontalTwist;
	[SerializeField]
	private AnimationCurve clockwiseTwist;
	[SerializeField]

	//
	override protected void LateUpdate()
	{
		base.LateUpdate ();

		float horisontalTwistValue = horisontalTwist.Evaluate(Mathf.Repeat(Time.timeSinceLevelLoad, cycleTime) / cycleTime) * maxTwistAngle;
		float clockwiseTwistValue = clockwiseTwist.Evaluate(Mathf.Repeat(Time.timeSinceLevelLoad, cycleTime) / cycleTime) * maxTwistAngle;
		headBone.Rotate (horisontalTwistValue * _smoothIKWeigh, clockwiseTwistValue * _smoothIKWeigh, 0f);

	}

}