using UnityEngine;
using System.Collections;

public class Hit2 : MonoBehaviour
{
    [Header("Spawn Sound:")]
    public bool hasSound = false;
    [Range(0.0f, 1.0f)]
    public float volume = 1.0f;

    public AudioClip spawnSound = null;

    AudioSource myAudioSource = null;

    // Use this for initialization
    void Start()
    {
        AddAudioSourceAndPlaySoundOneShot();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddAudioSourceAndPlaySoundOneShot()
    {
        if (!hasSound)
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

        myAudioSource.PlayOneShot(myAudioSource.clip, volume);
    }
}
