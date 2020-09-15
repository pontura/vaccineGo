using System.Collections;
using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour {

	[SerializeField] Text titleField;
	[SerializeField] Text textField;
    ServerLogin serverlogin;


	void Start()
	{		
		//PersistentData.Instance.serverLogin.Init ();
		if (PersistentData.Instance.serverLogin.usernameInserted == "")
			titleField.text = "Insert your username";
		else
			titleField.text = "Insert password for " +  PersistentData.Instance.serverLogin.usernameInserted;
		
		Events.OnKeyboardText +=OnKeyboardText;
		Events.OnKeyboardTitle +=OnKeyboardTitle;
	}
	void OnDestroy()
	{
		Events.OnKeyboardText -=OnKeyboardText;
		Events.OnKeyboardTitle -=OnKeyboardTitle;
	}

	bool submited;
	public void Submit()
	{
		if (submited)
			return;

		submited = true;
		
		Events.OnKeyboardFieldEntered ( textField.text);
        titleField.text = "";
    }
	void OnKeyboardTitle(string text)
	{
		titleField.text = text;
	}
	void OnKeyboardText(string text)
	{
		textField.text = text;
	}

}
