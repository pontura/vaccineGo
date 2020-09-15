using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.IO;

public class VoicesManager : MonoBehaviour
{
    public string lang;

    public string[] languages;
    public AvailableLang[] availableLangs;

    [Serializable]
    public class AvailableLang
    {
        public string lang;
        public bool available; 
    }
  //  public Games games;
    public AudioSource audioSource;
    
    public Games all;

    [Serializable]
    public class Games
    {
        public AudioClipsByLang[] ui;
        public AudioClipsByLang[] kids;
        public AudioClipsByLang[] adults;
    }
    [Serializable]
    public class AudioClipsByLang
    {
        public string lang;
        public List<AudioClip> clips;
    }
    public static VoicesManager Instance { get; protected set; }

    public void SetLang(string lang)
    {
        this.lang = lang.ToLower();
    }
    void Awake()
    {
        Instance = this;
        SetSavedLangs();
    }
    private void Start()
    {
        if (lang == "") lang = "en";
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(this);
        ////#if UNITI_EDITOR
        //if (loadOnInit)
        //{
        //    int langID = 0;
        //    foreach (string lang in languages)
        //    {
        //        AudioClipsByLang audioLanguages = new AudioClipsByLang();
        //        audioLanguages.lang = lang;
        //        audioLanguages.clips = GetClipsFrom("UI", audioLanguages, lang);
        //        langID++;
        //    }
        //}
        ////#endif
    }
    //List<AudioClip> GetClipsFrom(string folder, AudioClipsByLang arr, string lang)
    //{
    //    List<AudioClip> arrNew = new List<AudioClip>();
    //    string path = Application.dataPath + "/Voices/" + folder + "/" + lang;
    //    print("AddClipsTo " + path);
    //    DirectoryInfo dir = new DirectoryInfo(path);
    //    FileInfo[] info = dir.GetFiles("*.mp3");
    //    foreach (FileInfo f in info)
    //    {           
    //        AudioClip ac = (AudioClip)AssetDatabase.LoadAssetAtPath(f.FullName, typeof(AudioClip));
    //        arrNew.Add(ac);
    //        print(f.FullName + " " + ac);
    //    }
    //    return arrNew;
    //}
    string GetLangNameByID(int _id)
    {
        int id = 0;
        foreach (string langName in languages)
        {
            if (id == _id)
                return langName;

        }
        return languages[0];
    }

    public int GetTotalAvailableLangs()
    {
        int total = 0;
        foreach (VoicesManager.AvailableLang al in VoicesManager.Instance.availableLangs)
            if (al.available)
            {
                total++;
            }
        return total;
    }
    void SetSavedLangs()
    {
        SetAvailableLang(0, PlayerPrefs.GetInt("lang_en"));
        SetAvailableLang(1, PlayerPrefs.GetInt("lang_es"));
        SetAvailableLang(2, PlayerPrefs.GetInt("lang_ar"));
        SetAvailableLang(3, PlayerPrefs.GetInt("lang_se"));
    }
    public void ChangeAvailableLang(int arrayID, bool available)
    {
        string langName = "";
        switch (arrayID)
        {
            case 3: langName = "lang_se"; break;
            case 1: langName = "lang_es"; break;
            case 2: langName = "lang_ar"; break;
            default: langName = "lang_en"; break;
        }
        int value = 0;
        if (available) value = 1;

        PlayerPrefs.SetInt(langName, value);
        SetAvailableLang(arrayID, value);
    }
    void SetAvailableLang(int arrayID, int value)
    {
        // availableLangs[arrayID].available = true; return; //////////comentaR:!!!

        if (arrayID >= availableLangs.Length - 1)
            return;
        if (value == 1)
            availableLangs[arrayID].available = true;
        else
            availableLangs[arrayID].available = false;
    }
    public void SetDefaultLang()
    {
        int lang_en = PlayerPrefs.GetInt("lang_en");
        int lang_es = PlayerPrefs.GetInt("lang_es");
        int lang_ar = PlayerPrefs.GetInt("lang_ar");
        int lang_se = PlayerPrefs.GetInt("lang_se");

        if (lang_en == 0)
            VoicesManager.Instance.availableLangs[0].available = false;
        else
            VoicesManager.Instance.availableLangs[0].available = true;

        if (lang_es == 0)
            VoicesManager.Instance.availableLangs[1].available = false;
        else
            VoicesManager.Instance.availableLangs[1].available = true;

        if (lang_ar == 0)
            VoicesManager.Instance.availableLangs[2].available = false;
        else
            VoicesManager.Instance.availableLangs[2].available = true;

        foreach (VoicesManager.AvailableLang al in VoicesManager.Instance.availableLangs)
        {
            if (al.available)
            {
                VoicesManager.Instance.lang = al.lang;
            }
        }
    }

    // 0 = ui;
    // 1 = kids;
    // 2 = adults;
    public void PlayAudio(int gameID, string audioName)
    {
        audioSource.Stop();
        print("gameID " + gameID + "    }audioName " + audioName + "   lang " + lang);
        audioSource.clip = GetAudio(gameID, audioName, lang);
        audioSource.Play();
    }
    public AudioClip GetAudio(int gameID, string audioName, string langName)
    {
        Debug.Log("GetAudio " + gameID + " audioName: " + audioName + " lang: " + lang);
        AudioClipsByLang[] audioLanguages = GetALInGame(gameID);
        AudioClip audioClip = GetClip(audioLanguages, langName, audioName);
        return audioClip;
    }
    AudioClip GetClip(AudioClipsByLang[] audioClipsByLangs, string lang, string audioName)
    {
        Debug.Log("audioName " + audioName + " lalangIDg: " + lang);
        foreach (AudioClipsByLang audioClipsByLang in audioClipsByLangs)
        {
            if (audioClipsByLang.lang == lang)
            {
                foreach (AudioClip ac in audioClipsByLang.clips)
                {
                    print(ac.name);
                    if(ac.name == audioName || ac.name.ToUpper() == audioName.ToUpper())
                    {
                        return ac;                       
                    }
                    string[] nameArr = ac.name.Split("_"[0]);
                    string audioNameReal = ac.name.ToString().ToLower();

                    if (nameArr.Length > 1)
                    {
                        audioNameReal = ac.name.Remove(0, 3);
                    }
                    int num1 = 0;
                    int.TryParse(audioName, out num1);
                    int num2 = -1;
                    int.TryParse(audioNameReal, out num2);
                    if (num1 > 0 && num2 > 0 && num1 == num2)
                        return ac;
                    //Debug.Log(lang + " audioName " + audioName + " audioNameReal " + audioNameReal + " compare: " + string.Compare(audioName.ToString(), audioNameReal.ToString()));
                    if (string.Compare(audioName.ToLower().ToString(), audioNameReal.ToLower().ToString()) == 0)
                        return ac;
                }
            }
        }
        Debug.Log("No hay audio para " + audioName);
        return null;
    }
    AudioClipsByLang[] GetALInGame(int gameID)
    {
        switch (gameID)
        {
            case 0:
                return all.ui;
            case 1:
                return all.kids;
            default:
                return all.adults;
        }
    }
}
