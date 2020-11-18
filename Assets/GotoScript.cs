using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoScript : MonoBehaviour
{
    void Start()
    {
        if(PersistentData.Instance.gameSettings == PersistentData.GameSettings.Adults)
            Events.LoadScene("Adults");
        else
            Events.LoadScene("Kids");
    }
}
