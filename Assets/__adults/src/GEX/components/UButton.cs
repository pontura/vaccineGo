using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
//using Holoville.HOTween;
using GEX.media.sound;
//using com.game.sounds;

namespace GEX.components
{
	public class UButton : MonoBehaviour, IPointerClickHandler, IMoveHandler, IDragHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
	{
        private Func<GameObject, int> _func_click, _func_drag, _func_released, _func_mouse_down, _func_enter, _func_exit;
        private Vector3 _savedPos;
        private bool _enable = true;
        //private bool firstTimeBackImageChanged = true;
        private static int globalSoundID = -1;

        public String nameId;
        public Image normalState;
        public Image overState;
        public Image downState;
        public Image disableState;
        public Button buttonComponent;
        public Text textComponent;

        private bool exit_from_button;
        private bool _allow_mouse_down_when_disabled;

        /*public void Awake()
        {
            exit_from_button = true;
            _allow_mouse_down_when_disabled = false;
            _enable = true;
        }*/

        public bool enable
        {
            get { return _enable; }
            set 
            { 
                _enable = value;
                if (_enable)
                    setEnable();
                else
                    setDisable();
            }
        }

        public string text
        {
            get 
            {
                if (textComponent == null)
                    return "";
                else
                    return textComponent.text;
            }

            set
            {
                if (textComponent != null)
                    textComponent.text = value;
            }
        }

        public void allowMouseDownWhenDisabled()
        {
            _allow_mouse_down_when_disabled = true;
        }

        public static void setGlobalSound(int soundID)
        {
            UButton.globalSoundID = soundID;
        }

        public void addEventOnReleased(Func<GameObject, int> returnedFunction)
        {
            _func_released = returnedFunction;
        }

        public void addEventOnDrag(Func<GameObject, int> returnedFunction)
        {
            _func_drag = returnedFunction;
           
        }
        
        public void addEventOnClick(Func<GameObject, int> returnedFunction)
        {
            _func_click = returnedFunction;
        }

        public void addEventOnMouseDown(Func<GameObject, int> returnedFunction)
        {
            _func_mouse_down = returnedFunction;
        }

        public void addEventOnEnter(Func<GameObject, int> returnedFunction)
        {
            _func_enter = returnedFunction;
        }

        public void addEventOnExit(Func<GameObject, int> returnedFunction)
        {
            _func_exit = returnedFunction;
        }

        public void OnMove(AxisEventData data)
        {

        }

        public void OnPointerClick(PointerEventData data)
        {
            if (!_enable)
                return;

            //if (globalSoundID != -1)
            //    GameSound.play(globalSoundID);

            if (_func_click != null)
                _func_click.DynamicInvoke(gameObject);

            setDown();
        }

        public void OnPointerEnter(PointerEventData data)
        {
            if (!_enable)
                return;

            if (_func_enter != null)
                _func_enter.DynamicInvoke(gameObject);

            
            exit_from_button = false;
            setOver();
        }

        public void OnPointerExit(PointerEventData data)
        {
            if (!_enable)
                return;

            if (_func_exit != null)
                _func_exit.DynamicInvoke(gameObject);

            exit_from_button = true;

            setUp();
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (!_enable  && !_allow_mouse_down_when_disabled)
                return;

            if (_func_mouse_down != null)
                _func_mouse_down.DynamicInvoke(gameObject);

            _savedPos = Input.mousePosition;
            setDown();
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (!_enable)
                return;
            
            //if (!exit_from_button)
            //    _OVER_();


            if ((_func_released != null) && computeAsClick(_savedPos, Input.mousePosition))
                _func_released.DynamicInvoke(gameObject);

            setUp();
        }

        private bool computeAsClick(Vector3 savedPos, Vector3 currPos)
        {
            int offset_x_ok = (Screen.width * 2) / 100; // 2%
            //int offset_y_ok = (Screen.height * 2) / 100; // 2%

            if (currPos.x >= (savedPos.x - offset_x_ok) &&
                currPos.x <= (savedPos.x + offset_x_ok) &&
                currPos.y >= (savedPos.y - offset_x_ok) &&
                currPos.y <= (savedPos.y + offset_x_ok))
                return true;
            else
                return false;
        }

        public void OnDrag(PointerEventData data)
        {
            /*if (!_enable)
                return;

            if (_func_drag != null)
                _func_drag.DynamicInvoke(gameObject);*/
        }


        private void setUp()
        {
            setImgs(true, false, false, false);
        }

        private void setOver()
        {
            setImgs(false, true, false, false);
            startAnim();
        }

        private void setDown()
        {
            setImgs(false, false, true, false);
            startAnim();
        }

        private void setEnable()
        {
            setImgs(true, false, false, false);
        }

        private void setDisable()
        {
            setImgs(false, false, false, true);
        }

        private void setImgs(bool up, bool over, bool down, bool disable)
        {
            hideAllImgs();

            if ((normalState != null) && (up))    normalState.gameObject.SetActive(true);
            if ((overState != null) && (over))      overState.gameObject.SetActive(true);
            if((downState != null) && (down))       downState.gameObject.SetActive(true);
            if ((disableState != null) && (disable))   disableState.gameObject.SetActive(true);
        }

        private void hideAllImgs()
        {
            if (normalState != null) normalState.gameObject.SetActive(false);
            if (overState != null) overState.gameObject.SetActive(false);
            if (downState != null) downState.gameObject.SetActive(false);
            if (disableState != null) disableState.gameObject.SetActive(false);
        }

        private float animCount = 0;
        private bool animActive = false;
        private bool animEnabled = true;

        private void startAnim()
        {
            if (animEnabled)
            {
                animCount = 0;
                animActive = true;
            }
        }

        public void setButtonAnim(bool anim)
        {
            animEnabled = anim;
        }

        private void Update()
        {
            float sin;

            if (animActive && animEnabled)
            {
                animCount += Time.deltaTime * 10;
                sin = Mathf.Sin(animCount);

                if (sin < 0)
                {
                    sin = 0;
                    animCount = 0;
                    animActive = false;
                }

                transform.localScale = new Vector3(transform.localScale.x, 1 + (sin / 10f), transform.localScale.z);
            }

        }

	}
}
