using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//kzlukos@gmail.ocm
//
public partial class UIController : GameStateObserver
{
	
    //
    override protected void OnStateChangeHandler(GameState prev, GameState current)
    {
		//return;
        HideViews();

        switch (prev)
        {
		case GameState.Init:
			
                commonView.resetButton.gameObject.SetActive(true);
                commonView.TimerRunning = true;
                break;
		case GameState.WaitingForClient:
				commonView.gameObject.SetActive(true);
				waitView.gameObject.SetActive (false);
				break;
        }

        switch (current)
        {
			case GameState.Init:
				TimeManager.Instance.Pause = true;
                commonView.TimerRunning = false;
                startView.gameObject.SetActive(true);
                commonView.resetButton.gameObject.SetActive(false);
                break;

			case GameState.Introduction:
				break;

            case GameState.Approach:
                pauseView.gameObject.SetActive(true);
                break;

            case GameState.ApproachPause:
                pauseView.gameObject.SetActive(true);
                break;

            case GameState.Badge:
				countdownView.gameObject.SetActive (true);
				countdownView.StartCountDown (13);
                break;

            case GameState.BadgeDone:
                pauseView.gameObject.SetActive(true);
                break;
			case GameState.NewVaccinePause:
            case GameState.BadgePause:
			case GameState.LoopLargo1:
                pauseView.gameObject.SetActive(true);
                break;
			case GameState.Transfer2Lights:
            case GameState.Transfer:
                countdownView.gameObject.SetActive(true);
                countdownView.StartCountDown(4);
                break;
            case GameState.Repeat:
                countdownView.gameObject.SetActive(true);
                countdownView.StartCountDown(6);
                break;
            case GameState.RepeatLast:
                countdownView.gameObject.SetActive(true);
                countdownView.StartCountDown(7);
                break;
			case GameState.RepeatLast2:
				countdownView.gameObject.SetActive(true);
				countdownView.StartCountDown(7);
				break;

        }

    }
}
