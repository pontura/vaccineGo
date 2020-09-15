using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// kzlukos@gmail.com
// GameState observer responsible for playing narration audio clips
public class NarrationController : GameStateObserver {
    
	AudioClipCollection _audioCollection;
	[SerializeField] AudioSource narratorAudioSource;

	public bool IsPlaying { 
		get {
			return narratorAudioSource!= null ? narratorAudioSource.isPlaying : false;
		}
	}
		
	[System.Serializable]
	private class NarrationDelay
	{
		public GameState state;
		public float time;
	}

	[SerializeField] private NarrationDelay[] narrationDelays;
    //
    public void UpdateLang() 
	{

        //pontura: 
        print("FindAudioCollection: " + VoicesManager.Instance.lang);
        _audioCollection = FindAudioCollection(VoicesManager.Instance.lang);

      //  if (PersistentData.Instance.lang == PersistentData.languages.EN)
		//	_audioCollection = FindAudioCollection(Lang.ENG);
		//else if(PersistentData.Instance.lang == PersistentData.languages.ES)
		//	_audioCollection = FindAudioCollection(Lang.ES);
       // _audioCollection = FindAudioCollection(Settings.Instance.Language);
		//narratorAudioSource.mute = (CustomNetworkManager.Instance.NetworkMode == NetworkMode.Server);
	}

	//
	override protected void OnStateChangeHandler(GameState prev, GameState current) 
	{

		if (narratorAudioSource == null)
			return;
		
		StopAllCoroutines ();
		narratorAudioSource.Stop ();


        if(_audioCollection != null)
        {
            AudioClipCollection.Clip clipObject = System.Array.Find(_audioCollection.audioClips, clip => clip.state == current); //Performance
            

            if (clipObject != null)
            {
                narratorAudioSource.clip = clipObject.clip;
                // Get delay
                float delay = 0f;
                foreach (NarrationDelay narrationDelay in narrationDelays)
                    if (narrationDelay.state == current)
                        delay = narrationDelay.time;

                StartCoroutine(StartNarrationClipCoroutine(delay));
            }
        }
			
    }
		
	//
	private IEnumerator StartNarrationClipCoroutine(float delay)
	{
        print("narratorAudioSource:   " + narratorAudioSource);
        print("clip:   " + narratorAudioSource.clip);
        if (narratorAudioSource.clip != null) 
		{
			yield return new WaitForSeconds (delay);
            print("_____dice:   " + narratorAudioSource.clip);
			narratorAudioSource.Play ();
		}
	}


	//
    
	private AudioClipCollection FindAudioCollection(string lang)
	{
        print ("___________FindAudioCollection(Lang lang)" + lang);
		foreach (AudioClipCollection collection in transform.GetComponentsInChildren<AudioClipCollection>())
		{
           // if (collection.lang == lang)
           // {
                foreach (AudioClipCollection.Clip clip in collection.audioClips)
                {
                    if (clip.clip != null)
                    {
                        string audioNameFiltered = clip.clip.name;
                        audioNameFiltered = audioNameFiltered.Substring(3, audioNameFiltered.Length-3);

                        clip.clip = VoicesManager.Instance.GetAudio(1, audioNameFiltered, lang);
                    //string loadedClip = "Audios/" + lang.ToString() + "/" + lang.ToString() + "_" + audioNameFiltered;
                    // clip.clip = Resources.Load<AudioClip>(loadedClip);

                    print("loadedClip: " + audioNameFiltered + "         lang: " + lang);
                }
                }
                return collection;
           // }
		}
		return null;
	}

    //
   // private void OnLanguageChangeHandler(Lang lang)
   // {
   //     _audioCollection = FindAudioCollection(lang);
  //  }

}
