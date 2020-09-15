using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using GEX.utils;


namespace GEX.components
{
	public class UProgressBar : MonoBehaviour
	{
        public RectTransform resizableBar;
        public RectTransform graphicBar;
        
        private float _progress;
        private float total_width;

        public void Awake()
        {
            _progress = 0;
            total_width = resizableBar.anchorMax.x - resizableBar.anchorMin.x;
            if (graphicBar != null)
            {
                UElement rBar = new UElement(resizableBar.gameObject);
                UElement uelement = new UElement(graphicBar.gameObject);
                uelement.width = rBar.width;
                uelement.local_x = 0;
                _progress = 1;
                rBar.destroy();
                rBar = null;
                uelement.destroy();
                uelement = null;
            }

        }


        public void setProgress(float progress) // from 0 to 1.0
        {
            float dx;
            _progress = progress;
            dx = total_width * progress;

            resizableBar.anchorMax = new Vector2(resizableBar.anchorMin.x + dx, resizableBar.anchorMax.y);
        }

        public float getProgress()
        {
            return _progress;
        }

	}
}
