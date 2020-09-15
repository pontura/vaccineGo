using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/*[Serializable]
public class SoundPack
{
    public String langCode;
    public AudioClip[] clips;
}*/

public class SoundPlayer : MonoBehaviour
{

    public AudioClip[] clips;
    //public SoundPack[] packs;
    private AudioSource audioSrc;
    private String queAudio1 = "", queAudio2 = "";
    private bool queLang1 = false, queLang2 = false;
    private bool useNativeLang = false;
    private bool _init = false;

    public void PlayUsingNativeLangIfExists(String filename1, bool lang1, String filename2, bool lang2, float volume = 1.0f)
    {

        //if (Language.HasNativeLangCode())
        //    useNativeLang = true;
        //else
        //    useNativeLang = false;

        Play(filename1, lang1, filename2, lang2, volume);
    }

    public void Play(String filename1, bool lang1, String filename2, bool lang2, float volume =1.0f)
    {
        this.queAudio1 = filename1;
        this.queAudio2 = filename2;
        this.queLang1 = lang1;
        this.queLang2 = lang2;

        //if (useNativeLang)
        //{
        //    PlayLang(queAudio1, Language.nativeLangCode, false, volume);
        //}
        //else
        //{
            if (queLang1)
                PlayLang(queAudio1, false, volume);
            else
                Play(queAudio1, false, volume);
       // }

        queAudio1 = "";
        queLang1 = false;
    }

    /*public void Play(String filename1, String filename2, float volume = 1.0f)
    {
        this.queAudio1 = filename1;
        this.queAudio2 = filename2;

        Play(queAudio1, false, volume);
        queAudio1 = "";
    }*/

    public void Play(String name, bool loop = false, float volume = 1.0f)
    {
        Setup();

        //name = PersistentData.Instance.lang.ToString().ToLower() + "_" + name;
        //VoicesManager.Instance.PlayAudio(2, name, PersistentData.Instance.lang);
        //Debug.Log("VoicesManager FILE NAME NOW:" + name);

        audioSrc.Stop();
        audioSrc.clip = GetByName(name);
        audioSrc.loop = loop;
        audioSrc.volume = volume;
        audioSrc.Play();
    }

    public void PlayLang(string name, bool loop = false, float volume = 1.0f)
    {
       // name = PersistentData.Instance.lang.ToString().ToLower() + "_" + name;
        VoicesManager.Instance.PlayAudio(2, name);
        Debug.Log("VoicesManager FILE NAME NOW:" + name);
       // Play(Language.ApplyConvention(name), loop, volume);
    }

    public void PlayLangUsingNativeLangIfExists(String name, bool loop = false, float volume = 1.0f)
    {
        //if (Language.HasNativeLangCode())
        //    Play(Language.ApplyConvention(name, Language.nativeLangCode), loop, volume);
        //else
        // Play(Language.ApplyConvention(name), loop, volume);
        Play(VoicesManager.Instance.lang.ToString().ToLower() + "_" +  name, loop, volume);
    }

    public void PlayLang(String name, String langCode, bool loop = false, float volume = 1.0f)
    {
        //Debug.Log("PlayLang 2:(" + queAudio2 + ") FILE NAME NOW:" + name);
       // Play(Language.ApplyConvention(name, langCode), loop, volume);
        VoicesManager.Instance.PlayAudio(0, name);
        Debug.Log("PlayLang VoicesManager FILE NAME NOW:" + name);
    }


    public void PlayRandom(String [] soundNames)
    {
        int rnd = UnityEngine.Random.Range(0, soundNames.Length);
        Play(soundNames[rnd]);
    }

    public void Stop()
    {
        if(audioSrc != null)
            audioSrc.Stop();  
    }

    public bool HasFinished()
    {
       // print("HasFinished " + VoicesManager.Instance.audioSource.clip.name + " " +  VoicesManager.Instance.audioSource.isPlaying);
        return !VoicesManager.Instance.audioSource.isPlaying;// false;// !audioSrc.isPlaying;
    }

    public void KillAll()
    {
        Stop();
        queAudio1 = ""; queAudio2 = "";
        queLang1 = false; queLang2 = false;
        useNativeLang = false;
    }

    private AudioClip GetByName(String name)
    {
        print("name " + name);
        for (int a = 0; a < clips.Length; a++)
        {
            if (clips[a].name == name)
                return clips[a];
        }
        return null;
    }


    private void Setup()
    {
        if (!_init)
        {
            audioSrc = gameObject.GetComponent<AudioSource>();
            

            if (audioSrc == null)
                audioSrc = gameObject.AddComponent<AudioSource>();

            audioSrc.playOnAwake = false;

            _init = true;
        }
    }

    private void Update()
    {
        if (queAudio2 != "")
        {
            //Debug.Log(queAudio2);
        
            if (!audioSrc.isPlaying)
            {
                //if (useNativeLang)
                //{
                //    PlayLang(queAudio2, Language.nativeLangCode, false, audioSrc.volume);
                //    useNativeLang = false;
                //}
                //else
                //{
                    if (queLang2)
                        PlayLang(queAudio2, false, audioSrc.volume);
                    else
                        Play(queAudio2, false, audioSrc.volume);
              //  }

                queAudio2 = "";
                queLang2 = false;
            }
        }
    }
}
