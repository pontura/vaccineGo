using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PauseView : View {

    public Button pauseButton;
    private bool _paused;

    public void OnPauseToggleHander(bool paused)
    {
        pauseButton.GetComponentInChildren<Text>().text = paused ? "Resume" : "Pause";
    }

}
