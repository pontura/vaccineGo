using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using GEX.utils;

namespace GEX.components
{
	public class ULoadingBar : MonoBehaviour
	{

        public GameObject resizableBar;
        private RectTransform RT;
        private float total_width;

        public void Awake()
        {
            try
            {
                RT = resizableBar.GetComponent<RectTransform>();
                total_width = RT.anchorMax.x - RT.anchorMin.x;
            }
            catch (Exception ee)
            { }
        }


        public void setProgress(float progress) // from 0 to 1.0
        {
            float dx;
            dx = total_width * progress;

            try
            {
                RT.anchorMax = new Vector2(RT.anchorMin.x + dx, RT.anchorMax.y);
            }
            catch (Exception ee)
            { }
        }

	}
}
