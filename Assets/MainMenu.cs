using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text textField;
    
    public void OnClicked()
    {
        textField.text = "Clicked";
    }

}
