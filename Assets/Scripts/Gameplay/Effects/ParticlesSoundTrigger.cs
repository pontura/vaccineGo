using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// kzlukos@gmail.com
// 
[RequireComponent(typeof(AudioSource))]
public class ParticlesSoundTrigger : MonoBehaviour {

    AudioSource _audioSource;
    ParticleSystem _particleSystem;
    bool prevPlaying = false;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _particleSystem = GetComponent<ParticleSystem>();
        prevPlaying = _particleSystem.isPlaying;
    }

    //
    void Update()
    {
        if (!prevPlaying && _particleSystem.isPlaying)
            _audioSource.Play();

        if(!_particleSystem.isPlaying)
            _audioSource.Stop();

        prevPlaying = _particleSystem.isPlaying;
    }

}
