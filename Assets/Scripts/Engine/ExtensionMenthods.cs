using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMenthods {

	public static float GetSignedAngle(this Vector2 self, Vector2 other)
	{
		return Vector2.Angle (self, other) * Mathf.Sign (other.x * self.y - other.y * self.x);
	}

}
