using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BoardLevelOnOff : MonoBehaviour
{

    public GameObject complete, incomplete;

    public void SetComplete(bool isComplete)
    {
        complete.SetActive(isComplete);
        incomplete.SetActive(!isComplete);
    }

}
