using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// kzlukos@gmail.com
//
[RequireComponent(typeof(AudioSource))]
public class AudioSourceFadeVolume : MonoBehaviour {

    bool _play = false;
    public bool Play
    {
        get { return _play; }
        set
        {
            _play = value;
            if (_play)
                _audioSource.Play();
            _time = 0f;
        }
    }

    [SerializeField] float fadeTime = 3f;

    AudioSource _audioSource;
    float _defaultVolume;
    float _time;

    //
    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _defaultVolume = _audioSource.volume;
    }

    //
    void Update()
    {
        _time += Time.deltaTime;
        if (_play)
            _audioSource.volume = Mathf.Lerp(0f, _defaultVolume, _time / fadeTime);
        if(!_play)
            _audioSource.volume = Mathf.Lerp(_defaultVolume, 0f, _time / fadeTime);

        if (_audioSource.volume == 0f)
            _audioSource.Stop();
    }
	public void StopSounds()
	{
		_play = false;
	}





}
