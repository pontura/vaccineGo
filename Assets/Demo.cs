using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    public screens screen;
    public enum screens
    {
        Settings,
        Adults,
        Kids
    }
    void Start()
    {
        if (screen == screens.Settings)
        {
            Events.LoadScene(screen.ToString());
            return;
        }
        else if (screen == screens.Adults)
            PersistentData.Instance.gameSettings = PersistentData.GameSettings.Adults;
        else
            PersistentData.Instance.gameSettings = PersistentData.GameSettings.Kids;

        Events.LoadScene("LangSelector");
    }
}
