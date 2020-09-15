using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

//kzlukos@gmail.com
//Makes the screen black when experience is finished
public class Curtain : GameStateObserver {

    [SerializeField] private ScreenOverlay overlayScript;
    [SerializeField] private float transitiontime = 2f;

    private float _targetAlpha = 0f;
    private float _transitionStartTime;

    //
    protected override void OnStateChangeHandler(GameState prev, GameState current)
    {
        _targetAlpha = (current == GameState.Curtain) ? 1f : 0f;
        _transitionStartTime = Time.time;
    }

	void Update () {

		if (overlayScript == null) 
		{
			if (Camera.main != null) 
			{
				overlayScript = Camera.main.GetComponent<ScreenOverlay> ();
			}
			return;
		}

        if (Time.time - _transitionStartTime <= transitiontime)
        { 
            float transitionPhase = (Time.time - _transitionStartTime) / transitiontime;
			float newAlpha = Mathf.Lerp(overlayScript.intensity, _targetAlpha, transitionPhase);

			if (newAlpha == 0f) {
				overlayScript.enabled = false;
			} else {
				overlayScript.enabled = true;
				overlayScript.intensity = newAlpha;
			}

        }
    }
}
