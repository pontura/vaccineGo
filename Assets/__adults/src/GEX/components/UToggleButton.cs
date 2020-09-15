using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace GEX.components
{
    public class UToggleButton : MonoBehaviour, IPointerClickHandler
	{

        public Image imageON, imageOFF;
        public bool startChecked = false;

		private Func<GameObject, int> _func_click, _func_released;  //_func_drag
        private bool _checked;
        private bool _enable;
        private bool _refreshAtStartup;

        public void Start()
        {
            _enable = true;

            if (_refreshAtStartup)
            {
                _checked = startChecked;
                refreshImageState();
            }
        }

        /*public void Awake()
        {
            
        }*/

        public bool check
        {
            set { _checked = value; refreshImageState(); }
            get { return _checked; }
        }

        public void setRefreshAtStartup(bool active)
        {
            _refreshAtStartup = active;
        }

        private void refreshImageState()
        {
            //Debug.Log("refreshImageState: " + _checked + " at " + name);

            if (_checked)
            {
                imageON.enabled =   true;
                imageOFF.enabled =  false;
            }
            else
            {
                imageON.enabled =   false;
                imageOFF.enabled =  true;
            }

        }

        public void addEventOnClick(Func<GameObject, int> returnedFunction)
        {
            _func_click = returnedFunction;
        }

        public void OnPointerClick(PointerEventData data)
        {
            if (_enable)
            {
                _checked = !_checked;

                refreshImageState();

                if (_func_click != null)
                    _func_click.DynamicInvoke(gameObject);
            }
        }

        public void destroy()
        {
            _enable = false;
            //_func_click = null;
            //_func_drag = null;
            //_func_released = null;
        }
    }
}
