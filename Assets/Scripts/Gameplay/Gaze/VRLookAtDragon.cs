using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//kzlukos@gmail.com
//
public class VRLookAtDragon : VRLookAt
{

    [SerializeField] AudioSource lightAudioSource;
    public float ContinouesLookingTime { get; private set; }

    [SerializeField]
    private ParticleSystem gazeActivatedParticleEffect;

    // To make sure that enabled could be set/readed
    private void Update() { }

    //
    private void Awake()
    {
        gazeActivatedParticleEffect.Stop();
        ContinouesLookingTime = 0f;
        enabled = false;
    }

    //
    override public void LookAtStart()
    {
        gazeActivatedParticleEffect.Play();
        lightAudioSource.Play();
        ContinouesLookingTime = 0f;
    }

    //
    override public void LookAtUpdate()
    {
        ContinouesLookingTime += Time.deltaTime;
    }

    //
    override public void LookAtStop()
    {
        gazeActivatedParticleEffect.Stop();
        //lightAudioSource.Stop();
        ContinouesLookingTime = 0f;
    }

}
