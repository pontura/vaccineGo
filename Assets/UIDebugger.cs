using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDebugger : Singleton<UIDebugger>
{
	public Text field;

	void Awake()
	{		
		Events.OnKeyboardText += OnKeyboardText;
	}
	void OnDestroy()
	{
		Events.OnKeyboardText -= OnKeyboardText;
	}
	void OnKeyboardText(string text)
	{
		//field.text = text;
		Invoke ("Reset", 5);
	}
	void Reset()
	{
		//field.text = "";
	}
}
