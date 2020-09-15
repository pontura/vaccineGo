using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : Singleton<SoundFXManager> {

	public AudioClip pause;
	public AudioClip unpause;
	public AudioClip reset;
	public AudioClip start;

	public AudioSource audioSource;

	public void OnSoundFX (string audioCipName) {

		AudioClip ac = null;

		switch (audioCipName) {

		case "pause": ac = pause; break;
		case "unpause": ac = unpause; break;
		case "reset": ac = reset; break;
		case "start": ac = start; break;
			
		}
		if (ac != null) {
			audioSource.clip = ac;
			audioSource.Play ();
		}
	}

}
