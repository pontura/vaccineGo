using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// kzlukos@gmail.com
// Language and audio volume settings
public class Settings : MonoBehaviour {

	//
	private static Settings _instance;
	public static Settings Instance
	{
		get { return _instance; }
	}

	//[SerializeField]
	//private SettingsNetworkSynchronizer _networkSynchronizer;

	//
	//public System.Action<Lang> OnLanguageChange = delegate { };
//	[SerializeField] public Lang _selectedLang = Lang.ES;

   // public Lang Language
   // {
   //     get { return _selectedLang; }
   //     set
   //     {
			////if(PersistentData.Instance.lang == PersistentData.languages.EN)
			////	_selectedLang = Lang.ENG;
			////else
			////	_selectedLang = Lang.ES;
			//PlayerPrefs.SetInt(PlayerPrefKeys.Language, 1);
   //         //OnLanguageChange(_selectedLang);
			//NetworkPushSettings ();
   //     }
   // }

	//
	public System.Action<float> OnAudioVolumeChange = delegate { };

    float _audioVolume;
    public float AudioVolume
	{
		get { return AudioListener.volume; }
		set
		{
            _audioVolume = value;
			PlayerPrefs.SetFloat(PlayerPrefKeys.AudioVolume, _audioVolume);
			OnAudioVolumeChange(_audioVolume);

            NetworkPushSettings();

			//pontura:
			//if (CustomNetworkManager.Instance == null)
			//	return;
			
   //         if (CustomNetworkManager.Instance.NetworkMode == NetworkMode.Client || CustomNetworkManager.Instance.NetworkMode == NetworkMode.StandAlone)
   //             AudioListener.volume = _audioVolume;

        }
	}

	//
	public void NetworkPushSettings()
	{
		//pontura:
		//if (CustomNetworkManager.Instance == null)
		//	return;
		
		//if (CustomNetworkManager.Instance.NetworkMode == NetworkMode.Server)
         //   _networkSynchronizer.RpcUpdateClientSettings(_selectedLang, _audioVolume);
	}


    //
	//void OnServerSettingsChangeHandler(Lang lang, float volume)
 //   {
 //       Language = lang;
	//	AudioVolume = volume;
 //   }

    public void ResetView()
    {
       // _networkSynchronizer.RpcResetView();
    }

    //
    private void Awake()
    {
		_instance = this;

		//pontura:
		//if (CustomNetworkManager.Instance == null)
		//	return;
  //      if (CustomNetworkManager.Instance.NetworkMode == NetworkMode.Server)
  //          AudioListener.volume = 0f;

        
		//_networkSynchronizer.OnServerChangeSettings += OnServerSettingsChangeHandler;
    }
		

}
