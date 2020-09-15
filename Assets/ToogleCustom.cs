using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToogleCustom : MonoBehaviour
{
    public Text field;
    public void Init(string text, bool startActive)
    {
        field.text = text;
        GetComponent<UnityEngine.UI.Toggle>().isOn = startActive;
    }
}
