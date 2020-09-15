using System;
using UnityEngine;

namespace GEX.utils
{
	//[AddComponentMenu("UI/Spring Position", 112)]
	public class XSpringPosition : MonoBehaviour
	{
		public delegate void OnFinished(XSpringPosition spring);
        public Vector3 target = Vector3.zero;
		public float strength = 10f;
		public bool worldSpace;
		public bool ignoreTimeScale;
		public GameObject eventReceiver;
		public string callWhenFinished;
		public XSpringPosition.OnFinished onFinished;
		private Transform mTrans;
		private float mThreshold;

        //------- custom ---------------
        public object ctag;

        public delegate void Complete(object sender, object ctag);
        public event Complete onComplete;
        //------------------------------

		private void Start()
		{
			this.mTrans = base.transform;
		}
		private void Update()
		{
			float deltaTime = (!this.ignoreTimeScale) ? Time.deltaTime : Time.unscaledDeltaTime;
			if (this.worldSpace)
			{
				if (this.mThreshold == 0f)
				{
					this.mThreshold = (this.target - this.mTrans.position).magnitude * 0.001f;
				}
				this.mTrans.position = (XSpringPosition.SpringLerp(this.mTrans.position, this.target, this.strength, deltaTime));
				if (this.mThreshold >= (this.target - this.mTrans.position).magnitude)
				{
					this.mTrans.position = (this.target);
					if (this.onFinished != null)
					{
						this.onFinished(this);
					}
					if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
					{
						//this.eventReceiver.SendMessage(this.callWhenFinished, this, 1);
					}
					base.enabled = false;
				}
			}
			else
			{
				if (this.mThreshold == 0f)
				{
					this.mThreshold = (this.target - this.mTrans.localPosition).magnitude * 0.001f;
				}
				this.mTrans.localPosition = (XSpringPosition.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime));
				if (this.mThreshold >= (this.target - this.mTrans.localPosition).magnitude)
				{
					this.mTrans.localPosition = (this.target);
					if (this.onFinished != null)
					{
						this.onFinished(this);
					}
					if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
					{
						//this.eventReceiver.SendMessage(this.callWhenFinished, this, 1);
					}

                    if (onComplete != null)
                        onComplete(this, ctag);

					base.enabled = false;
				}
			}
		}
		public static XSpringPosition Begin(GameObject go, Vector3 pos, float strength)
		{
			XSpringPosition springPosition = go.GetComponent<XSpringPosition>();
			if (springPosition == null)
			{
				springPosition = go.AddComponent<XSpringPosition>();
			}
			springPosition.target = pos;
			springPosition.strength = strength;
			springPosition.onFinished = null;
			if (!springPosition.enabled)
			{
				springPosition.mThreshold = 0f;
				springPosition.enabled = true;
			}
			return springPosition;
		}
		public static float SpringLerp(float strength, float deltaTime)
		{
			if (deltaTime > 1f)
			{
				deltaTime = 1f;
			}
			int num = Mathf.RoundToInt(deltaTime * 1000f);
			deltaTime = 0.001f * strength;
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				num2 = Mathf.Lerp(num2, 1f, deltaTime);
			}
			return num2;
		}
		public static float SpringLerp(float from, float to, float strength, float deltaTime)
		{
			if (deltaTime > 1f)
			{
				deltaTime = 1f;
			}
			int num = Mathf.RoundToInt(deltaTime * 1000f);
			deltaTime = 0.001f * strength;
			for (int i = 0; i < num; i++)
			{
				from = Mathf.Lerp(from, to, deltaTime);
			}
			return from;
		}
		public static Vector2 SpringLerp(Vector2 from, Vector2 to, float strength, float deltaTime)
		{
			return Vector2.Lerp(from, to, XSpringPosition.SpringLerp(strength, deltaTime));
		}
		public static Vector3 SpringLerp(Vector3 from, Vector3 to, float strength, float deltaTime)
		{
			return Vector3.Lerp(from, to, XSpringPosition.SpringLerp(strength, deltaTime));
		}
		public static Quaternion SpringLerp(Quaternion from, Quaternion to, float strength, float deltaTime)
		{
			return Quaternion.Slerp(from, to, XSpringPosition.SpringLerp(strength, deltaTime));
		}
	}
}
