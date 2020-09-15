using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// kzlukos@gmail.com
// Playing wing clap sound
public class WingClaps : MonoBehaviour {

    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip[] audioClips;

    //
    public void OnClap()
    {
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
    }

}
