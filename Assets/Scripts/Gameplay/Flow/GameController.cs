using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,
    Init,
    Introduction,
    Approach,
    ApproachPause,
    Badge,
    BadgeDone,
    BadgePause,
    Explanation,
    Vaccine,
    Transfer,
    Repeat,
    Medal,
    Conclusion,
    Farewell,
    Curtain,
	WaitingForClient,
    RepeatLast,
    TransferSoundsFading,
	RepeatNewVaccine,
	NewVaccineDone,
	Restart,
	NewVaccinePause,
	Transfer2,
	Transfer2Lights,
	RepeatNewVaccine2,
	NewVaccineDone2,
	RepeatNewVaccine3,
	NewVaccineDone3,
	RepeatNewVaccine4,
	NewVaccineDone4,
	RepeatLast2,
	YaCasi,
	LoopLargo1,
	LoopLargo_tomate_un_momento,
	LoopLargo_luces2,
	PausaCorta,
	Loop_largo_cascada
}


// kzlukos@gmail.com
// Controlls game state and synchronizes it over network
public partial class GameController : Singleton<GameController>
{

	public AudioSourceFadeVolume audioSourceFadeVolume;
    //
    public System.Action<GameState, GameState> OnStateChange = delegate { };
    public GameState _currentState;
    public GameState CurrentState
    {
        get { return _currentState; } 
        set
        {
            GameState prevState = _currentState;
            _currentState = value;
            OnStateChange(prevState, _currentState);
            
        }
    }

	void ReloadAll()
	{
		//UnityEngine.SceneManagement.SceneManager.LoadScene("040_Main");
	}

    void OnServerStateChangeHandler(GameState state)
    {
		GameState prevState = _currentState;
        _currentState = state;
		OnStateChange(prevState, _currentState);
    }



}