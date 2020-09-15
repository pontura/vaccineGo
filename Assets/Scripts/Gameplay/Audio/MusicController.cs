using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// kzlukos@gmail.com
// 
public class MusicController : GameStateObserver {

	[SerializeField] AudioSource _musicAudioSource;

	//
	protected override void OnStateChangeHandler (GameState prev, GameState current)
	{
		if (current == GameState.Init)
			_musicAudioSource.Stop ();
		
		if (prev == GameState.Init)
			_musicAudioSource.Play ();
		
	}
}
