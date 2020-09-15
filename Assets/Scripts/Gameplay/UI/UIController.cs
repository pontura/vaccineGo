using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//kzlukos@gmail.ocm
// Assigns UI events and observes and changes UI elements display according to game state (partial .Handlers)
public partial class UIController : GameStateObserver 
{

    [SerializeField] StartView startView;
    [SerializeField] CommonView commonView;
    [SerializeField] PauseView pauseView;
    [SerializeField] CountdownView countdownView;
	[SerializeField] WaitView waitView;


    //Initialization

	void OnDestroy()
	{
		TimeManager.Instance.onRepetitionsNumberChange -= startView.OnRepetitionsNumberChangeHandler;
		TimeManager.Instance.onPauseToggle -= pauseView.OnPauseToggleHander;
	}
    void Start()
    {

       // if(CustomNetworkManager.Instance.NetworkMode != NetworkMode.Server)
      //  {
       //     gameObject.SetActive(false);
       //     return;
      //  }

		TimeManager.Instance.Pause = true;
		waitView.gameObject.SetActive (true);


        TimeManager.Instance.onRepetitionsNumberChange += startView.OnRepetitionsNumberChangeHandler;
        TimeManager.Instance.onPauseToggle += pauseView.OnPauseToggleHander;

        //StartView
        startView.resetViewButton.onClick.AddListener(delegate {
            Settings.Instance.ResetView();
        });

        startView.startButton.onClick.AddListener(delegate {
            commonView.ResetTimer();
            TimeManager.Instance.Pause = false;
        });
			
		startView.languageDropdown.value = PlayerPrefs.GetInt(PlayerPrefKeys.Language);
		startView.languageDropdown.RefreshShownValue();
	//	Settings.Instance.Language = (Lang)startView.languageDropdown.value;

        //startView.languageDropdown.onValueChanged.AddListener(delegate (int value) {
        //    Settings.Instance.Language = (Lang)value;
        //});



        //CommonView
        if (!PlayerPrefs.HasKey(PlayerPrefKeys.AudioVolume))
            PlayerPrefs.SetFloat(PlayerPrefKeys.AudioVolume, 1f);

        commonView.volumeControl.value = PlayerPrefs.GetFloat(PlayerPrefKeys.AudioVolume);
		Settings.Instance.AudioVolume = commonView.volumeControl.value;

		commonView.volumeControl.onValueChanged.AddListener (delegate (float value) {
			Settings.Instance.AudioVolume = value;
		});

        commonView.gameObject.SetActive(false);
        commonView.resetButton.onClick.AddListener(delegate {
          //  GameController.Instance.CurrentState = GameState.Init;
           // TimeManager.Instance.Repetitions = 1;
        });

        pauseView.pauseButton.onClick.AddListener(delegate {
            TimeManager.Instance.Pause = !TimeManager.Instance.Pause;
        });

    }

    //
    void HideViews()
    {
        startView.gameObject.SetActive(false);
        pauseView.gameObject.SetActive(false);
        countdownView.gameObject.SetActive(false);
    }


}
