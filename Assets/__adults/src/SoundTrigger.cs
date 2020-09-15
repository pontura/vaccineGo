using System;
using System.Collections.Generic;
using UnityEngine;


public class SoundTrigger : MonoBehaviour
{

    public bool haveSound = false;
    [Range(0.0f, 1.0f)]
    public float volume = 1.0f;
    public bool playOnEnable = false;

    public AudioClip spawnSound = null;

    AudioSource myAudioSource = null;

    // Use this for initialization
    void Start()
    {
        if(!playOnEnable)
            AddAudioSourceAndPlaySoundOneShot();
    }

    void OnEnable()
    {
        if(playOnEnable)
            AddAudioSourceAndPlaySoundOneShot();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddAudioSourceAndPlaySoundOneShot()
    {
        if (!haveSound)
            return;

        AddAudioSource();

        PlaySoundOneShot(spawnSound);
    }

    void AddAudioSource()
    {
        if (spawnSound != null)
        {
            myAudioSource = gameObject.GetComponent<AudioSource>();

            if (myAudioSource == null)
                myAudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void PlaySoundOneShot(AudioClip ac)
    {
        if (myAudioSource == null)
            return;

        if (ac == null)
            return;

        myAudioSource.Stop();

        myAudioSource.playOnAwake = false;

        myAudioSource.loop = false;

        myAudioSource.clip = ac;

        myAudioSource.volume = volume;

        myAudioSource.PlayOneShot(myAudioSource.clip, 1f);


    }

}

