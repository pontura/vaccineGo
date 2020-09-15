using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace GEX.components
{
	public class UInputField : MonoBehaviour
	{
        [HideInInspector]
        public InputField inputField;
        private bool _focused;
        private Func<GameObject, string, int> _func_submit;
        private Func<GameObject, string, int> _func_text_changed;

        [NonSerialized]
        public bool autoSubmit = false;
        private bool queueFocus = false;
        private string lastText;

        public void Awake()
        {
            inputField = GetComponent<InputField>();
            _focused = inputField.isFocused;
            lastText = inputField.text;
        }

        public String text{
            get { return inputField.text; }
            set { inputField.text = value; }
        }

        public int characterLimit
        {
            get { return inputField.characterLimit; }
            set { inputField.characterLimit = value; }
        }

        public void addEventOnSubmit(Func<GameObject, string, int> returnedFunction)
        {
            _func_submit = returnedFunction;
        }

        public void addEventOnTextChanged(Func<GameObject, string, int> returnedFunction)
        {
            lastText = inputField.text;
            _func_text_changed = returnedFunction;
        }

        public void removeEventOnSubmit()
        {
            _func_submit = null;
        }

        public void removeEventOnTextChanged()
        {
            _func_text_changed = null;
        }

        public void focus()
        {
            queueFocus = true;
        }

        public bool isFocused()
        {
            return inputField.isFocused || queueFocus;
        }

        private void OnGUI()
        {
            _focused = inputField.isFocused;
        }

        public void Update()
        {
            String str;

            if (queueFocus)
            {
                inputField.ActivateInputField();
                inputField.Select();
                queueFocus = false;
            }

            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (_focused)
                {
                    str = inputField.text;
                    if (autoSubmit)
                    {
                        inputField.text = "";
                        inputField.ActivateInputField();
                        inputField.Select();
                    }

                    if (_func_submit != null)
                        _func_submit.DynamicInvoke(gameObject, str);
                }
            }


            if (lastText != inputField.text)
            {
                lastText = inputField.text;
                if (_func_text_changed != null)
                    _func_text_changed.DynamicInvoke(gameObject, inputField.text);
            }

        }


	}
}
