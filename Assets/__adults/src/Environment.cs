using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

public class Environment : MonoBehaviour
{
    private static Environment _me = null;

    public static Environment me
    {
        get
        {
            if (_me == null)
                _me = GameObject.FindObjectOfType<Environment>();

            return _me;
        }
    }

    public SoundPlayer SOUND;
    public SoundPlayer SOUND_FX;
    public SoundPlayer MUSIC;
    public SoundPlayer AMBIENTAL;
    public GameObject SPECIAL_MODE_FX;
    public GameObject NEW_SPECIAL_FX;
    public GameObject MOON;
    public GameObject FIREWORKS;
    private bool specialMode = false;
    private bool changeToNight = false;
    private const float originalExposure = 0.78f;
    private const float originalMoonY = -256.76f;
    private const float toMoonY = 48f;
    

    private void Start()
    {
        ResetExposure();
    }

    private void ResetExposure()
    {
        RenderSettings.skybox.SetFloat("_Exposure", originalExposure);
    }

    public void PLAY_ONE_MUSIC_ALL_APP()
    {
        MUSIC.Play("music_funny_incidental", true, 0.15f);
        AMBIENTAL.Play("ambient_01", true, 0.20f);
    }

    public void PlayAmbientIntroSound()
    {
        if (!Config.SAME_MUSIC_ALL_APP)
            SOUND.Play("ambient_intro", false, 0.35f);
    }

    public void PlayAmbientSound()
    {
        //SOUND.Play("ambient_01", true, 0.35f);
        if (!Config.SAME_MUSIC_ALL_APP)
            MUSIC.Play("music_funny_incidental", true, 0.15f);
    }

    public void PlayAmbientEndGame()
    {
        if (!Config.SAME_MUSIC_ALL_APP)
            SOUND.Play("ambient_intro", false, 0.35f);
    }

    public void StopAmbienSound()
    {
        //SOUND.Stop();
        if (!Config.SAME_MUSIC_ALL_APP)
            MUSIC.Stop();
    }

    public void StopSound()
    {
        SOUND.Stop();
    }

    public void PlayWinFx()
    {
        SOUND_FX.Play("fx_win_01", false, 0.5f);
    }

    public void StartNewSpecialMode()
    {
        //SOUND.Play("ambient_special_01", true, 0.25f);
        if (!Config.SAME_MUSIC_ALL_APP)
            MUSIC.Play("music_funny_incidental", true, 0.15f);
        
        NEW_SPECIAL_FX.SetActive(true);
    }

    public void StopNewSpecialMode()
    {
        if (!Config.SAME_MUSIC_ALL_APP)
            MUSIC.Stop();
        
        NEW_SPECIAL_FX.SetActive(false);
    }

    public void Fireworks()
    {
        FIREWORKS.SetActive(true);
        FIREWORKS.GetComponent<ParticleSystem>().Play();
    }

    public void StartSpecialMode()
    {
        if (!Config.SAME_MUSIC_ALL_APP)
            SOUND.Play("ambient_special_01", true);
        
        SPECIAL_MODE_FX.SetActive(true);
        changeToNight = true;
        specialMode = true;

        MOON.transform.DOKill();
        MOON.transform.position = new Vector3(MOON.transform.position.x, originalMoonY, MOON.transform.position.z);
        MOON.SetActive(true);
        MOON.transform.DOMoveY(toMoonY, 10f).SetEase(Ease.InOutFlash);
    }

    public void StopSpecialMode()
    {
        if (!Config.SAME_MUSIC_ALL_APP)
            SOUND.Stop();
        
        SPECIAL_MODE_FX.SetActive(false);
        changeToNight = false;
        specialMode = false;

        MOON.transform.DOKill();
        MOON.transform.position = new Vector3(MOON.transform.position.x, originalMoonY, MOON.transform.position.z);
        MOON.SetActive(false);

        ResetExposure();
    }


    private void Update()
    {
        float val;
        if (changeToNight)
        {
            val = RenderSettings.skybox.GetFloat("_Exposure");
            val -= Time.deltaTime * 0.1f;

            if (val <= 0.13f)
            {
                val = 0.13f;
                changeToNight = false;
            }

            RenderSettings.skybox.SetFloat("_Exposure", val);
        }
  
    }

}
