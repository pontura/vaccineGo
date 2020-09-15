using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : Singleton<InputManager>
{
	public types type;

	public bool button1Clicked;
	public bool button2Clicked;

	public enum types
	{
		GATILLO_DOWN,
		PAD_DOWN,
		TWO_BUTTONS_DOWN,
		SWIPE_LEFT,
		SWIPPING,
		SWIPE_RIGHT
    }

	float timerPad;
	bool padDown;
	bool gatilloDown;
	float axis;

	void Update () {



        ////////////////////////////////
        //  editor:
        if (Input.GetKeyDown (KeyCode.Space)) {
			SetNewGesto (types.GATILLO_DOWN);
			return;
		} else if (Input.GetKeyDown (KeyCode.KeypadEnter)) {
			SetNewGesto (types.PAD_DOWN);
			return;
		} else if (Input.GetKeyDown (KeyCode.LeftControl)) {
			SetNewGesto (types.TWO_BUTTONS_DOWN);
			return;
		}
        else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			SetNewGesto (types.SWIPE_LEFT);
			return;
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			SetNewGesto (types.SWIPE_RIGHT);
			return;
		}

		



        ////////////////////////////////
        //  QUEST:
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            SetNewGesto(types.GATILLO_DOWN);
        else if(OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
            SetNewGesto(types.TWO_BUTTONS_DOWN);
        else if (OVRInput.Get(OVRInput.Touch.One) && OVRInput.Get(OVRInput.Touch.Two))
            SetNewGesto(types.PAD_DOWN);
        else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft) && type != types.SWIPE_LEFT)
            SetNewGesto(types.SWIPE_LEFT);
        else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight) && type != types.SWIPE_RIGHT)
            SetNewGesto(types.SWIPE_RIGHT);
        return;
        ////////////////////////////////
        ///





        if (OVRInput.GetUp(OVRInput.Touch.Any))
        {
            button1Clicked = false;
        }

        if (OVRInput.Get(OVRInput.Axis1D.Any)>0.9f){
			if (!button2Clicked) {

				gatilloDown = true;				
				button2Clicked = true;

                if (!CheckDoubleInput())
                {
                    SetNewGesto(types.GATILLO_DOWN);
                    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "002_Main")
                        UnityEngine.XR.InputTracking.Recenter();
                }
            }

		}
		else if(OVRInput.Get(OVRInput.Axis1D.Any)<0.2f){
			button2Clicked = false;

		//	UnityEngine.XR.InputTracking.Recenter();

			//SetNewGesto(types.GATILLO_DOWN);
		} 

		if(OVRInput.GetDown(OVRInput.Button.One) && padDown == false){
			button1Clicked = true;
			padDown = true;

			if (CheckDoubleInput ())
				return;
			this.type = types.PAD_DOWN;

		}  else  if(OVRInput.GetUp(OVRInput.Button.One) && padDown == true){

            if (CheckDoubleInput())
                return;

            button1Clicked = false;
			padDown = false;
            
            SetNewGesto(types.PAD_DOWN);
		} else if( OVRInput.Get(OVRInput.Touch.Any) && padDown == false ){          
            type = types.SWIPPING;
			axis = OVRInput.Get (OVRInput.Axis2D.Any).x;		
		} else if( OVRInput.GetUp(OVRInput.Touch.Any) && type == types.SWIPPING ){

            if( OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft) && type != types.SWIPE_LEFT)
                SetNewGesto(types.SWIPE_LEFT);
            else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight) && type != types.SWIPE_RIGHT)
                SetNewGesto(types.SWIPE_RIGHT);

            //float endAxis = OVRInput.Get(OVRInput.Axis2D.Any).x;
            //if (axis < -0.4f)
            //    SetNewGesto(types.SWIPE_LEFT);
            //else if (axis > 0.4f)
            //    SetNewGesto(types.SWIPE_RIGHT);

		} 
		
//		if( OVRInput.GetDown(OVRInput.Touch.Any)){
//			button2Clicked = true;
//			if (CheckDoubleInput ())
//				return;
//			UnityEngine.XR.InputTracking.Recenter();
//			SetNewGesto (types.GATILLO_HOLD);
//			SetNewGesto(types.GATILLO_DOWN);
//
//		}


	}
    float lastTime_two_Buttons = 0;
	bool CheckDoubleInput()
	{
        if (button2Clicked && button1Clicked) {
			SetNewGesto (types.TWO_BUTTONS_DOWN);
			button1Clicked = false;
			button2Clicked = false;
			return true;
		}
		return false;
	}
	void SetNewGesto(types type)
	{
		this.type = type;
		OnInput (type);
	}
	public System.Action<types> OnInput = delegate { };
}
