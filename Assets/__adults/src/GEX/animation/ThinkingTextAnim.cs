using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using GEX.utils;

namespace GEX.animation
{
	public class ThinkingTextAnim : MonoBehaviour
	{
        public String textInput;

        private XTimer timer;
        private String dots = ".";
        private Text textComponent;

        private void Start()
        {
            textComponent = GetComponent<Text>();

            timer = new XTimer();
            timer.setDelay(1.0f);
            timer.start();
        }

        private void Update()
        {
            if (timer.update())
            {
                timer.start();
                updateInfo();
            }
        }

        private void updateInfo()
        {
            textComponent.text = textInput + dots;

            dots += ".";
            if (dots.Length > 3)
                dots = ".";
        }
	}
}
