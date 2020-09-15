using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GEX.animation
{
	public class ScaleAnim : MonoBehaviour
	{
        public float speed = 5f;
        public float maxScale = 0.2f;

        private Vector3 originalScale;
        private bool init = false;
        private float count = 0;

        public static void Apply(GameObject gobj, float speed = 5f, float maxScale = 0.2f)
        {
            if (gobj.GetComponent<ScaleAnim>() == null)
            {
                ScaleAnim an =  gobj.AddComponent<ScaleAnim>();
                an.speed =      speed;
                an.maxScale =   maxScale;
            }
        }

        public static void Remove(GameObject gobj)
        {
            ScaleAnim an = gobj.GetComponent<ScaleAnim>();
            if (an != null)
            {
                GameObject.Destroy(an);
            }
        }

        private void Start()
        {
            if (!init)
            {
                originalScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                count = 0;
                init = true;
            }
        }

        private void Update()
        {
            float scaleAnim;
            float _scale_;
            count += Time.deltaTime * speed;
            scaleAnim = 0.5f + (Mathf.Sin(count) / 2f);

            _scale_ = maxScale * scaleAnim;

            transform.localScale = new Vector3(originalScale.x + _scale_, originalScale.y + _scale_, originalScale.z + _scale_);

        }

	}
}
