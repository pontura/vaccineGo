using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//kzlukos@gmail.ocm
// Groups UI elements which are displayed in any GameState
public class CommonView : View {

    public Text timer;
    public Button resetButton;
	public Slider volumeControl;
	private float _startTime;

    public bool TimerRunning { get; set; }

    //
    void Start()
    {
		//pontura
		return;



        ResetTimer();
    }

    //
    public void ResetTimer()
    {
        _startTime = Time.time;
        TimerRunning = false;
        UpdateTimerText();
    }

    //
    void Update()
    {
		//pontura
		return;

        if (TimerRunning) {
            UpdateTimerText();
        }
    }

    //
    private void UpdateTimerText()
    {
        float t = Time.time - _startTime;
		timer.text = ((int)t / 60).ToString("00") + ":" + (t % 60).ToString("00");
    }

}
