using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManagerPontura : Singleton<InputManagerPontura>
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

	float timer;
	bool padDown;
	bool gatilloDown;
	float axis;
    float init_x;

    void Update () {


        ////////////////////////////////
        //  pico:
        //if (Pvr_ControllerManager.controllerlink.Controller0.Trigger.PressedDown)
        //{
        //    SetNewGesto(types.GATILLO_DOWN);
        //} else if (Pvr_ControllerManager.controllerlink.Controller0.Trigger.State)
        //{
        //    SetNewGesto(types.GATILLO_DOWN);
        //}
        //if (Pvr_ControllerManager.controllerlink.Controller0.App.State)
        //{
        //    SetNewGesto(types.TWO_BUTTONS_DOWN);
        //}
        //if (Pvr_ControllerManager.controllerlink.Controller0.Touch.State)
        //{
        //    SetNewGesto(types.PAD_DOWN);
        //}
        //if (Pvr_ControllerManager.controllerlink.Controller1.Trigger.State)
        //{
        //    SetNewGesto(types.GATILLO_DOWN);
        //}
        //if (Pvr_ControllerManager.controllerlink.Controller1.App.State)
        //{
        //    SetNewGesto(types.TWO_BUTTONS_DOWN);
        //}
        //if (Pvr_ControllerManager.controllerlink.Controller1.Touch.State)
        //{
        //    SetNewGesto(types.PAD_DOWN);
        //}
        //if (Pvr_ControllerManager.controllerlink.Controller0.SwipeDirection == Pvr_UnitySDKAPI.SwipeDirection.SwipeLeft)
        //{
        //    SetNewGesto(types.SWIPE_LEFT);
        //}
        //else if (Pvr_ControllerManager.controllerlink.Controller0.SwipeDirection == Pvr_UnitySDKAPI.SwipeDirection.SwipeRight)
        //{
        //    SetNewGesto(types.SWIPE_RIGHT);
        //}







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





        //////////////////////////////
        //  QUEST:
        //if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        //    SetNewGesto(types.GATILLO_DOWN);
        //else if(OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        //    SetNewGesto(types.TWO_BUTTONS_DOWN);
        //else if (OVRInput.Get(OVRInput.Touch.One) && OVRInput.Get(OVRInput.Touch.Two))
        //    SetNewGesto(types.PAD_DOWN);
        //else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft) && type != types.SWIPE_LEFT)
        //    SetNewGesto(types.SWIPE_LEFT);
        //else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight) && type != types.SWIPE_RIGHT)
        //    SetNewGesto(types.SWIPE_RIGHT);
        //return;
        //////////////////////////////

        //Doble click:
        if ( OVRInput.GetDown(OVRInput.Button.Two))
        {
            if(timer+0.75f > Time.time)
                SetNewGesto(types.TWO_BUTTONS_DOWN);
            timer = Time.time;
        }
        //else if ( OVRInput.Get(OVRInput.Button.Two))
        //{
        //    timer += Time.deltaTime;
        //    if (timer > 2)
        //    {
        //        SetNewGesto(types.TWO_BUTTONS_DOWN);
        //        timer = -1000;
        //    }
        //    return;
        //}
        else if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            SetNewGesto(types.GATILLO_DOWN);
        }
        else if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
        {
            SetNewGesto(types.PAD_DOWN);
        }
        if (OVRInput.GetDown(OVRInput.Touch.Any))
        {
            init_x = OVRInput.Get(OVRInput.Axis2D.Any).x;
        }
        else if (OVRInput.GetUp(OVRInput.Touch.Any))
        {
            float ends = OVRInput.Get(OVRInput.Axis2D.Any).x;
            if (ends == init_x)
                print("nothing");
            else if (ends < init_x)
                SetNewGesto(types.SWIPE_LEFT);
            else if (ends > init_x)
                SetNewGesto(types.SWIPE_RIGHT);
            init_x = 0;
        }

    }
   

	void SetNewGesto(types type)
	{
		this.type = type;
		OnInput (type);
	}
	public System.Action<types> OnInput = delegate { };
}
