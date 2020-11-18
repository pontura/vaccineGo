using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PersistentData : MonoBehaviour {

    public int version;
	public bool DEBBUGER;
	public int num_of_vaccines;
	public bool langSelected;

	public ServerLogin serverLogin;

	public static PersistentData Instance { get; protected set; }
    public GameObject loadingMask;

    public AssetsBundleLoader assetsBundleLoader;

    public GameSettings gameSettings;
    public enum GameSettings
    {
        Adults,
        Kids
    }


    void Awake()
	{
#if UNITY_EDITOR
        if (DEBBUGER)
            PlayerPrefs.DeleteAll();
#endif
        loadingMask.SetActive(false);
        Instance = this;
		DontDestroyOnLoad (this.gameObject);
		serverLogin = GetComponent<ServerLogin> ();
        
        Events.SceneLoaded += SceneLoaded;
        Events.LoadScene += LoadScene;

        assetsBundleLoader = GetComponent<AssetsBundleLoader>();

        // lo hace assetsBundle: serverLogin.Init();
    }
   
    bool loading;    
    void LoadScene(string sceneName)
    {        
        if (loading)
            return;
        loading = true;
        loadingMask.SetActive(true);
        StartCoroutine( LoadCoroutine(sceneName) );
    }
    IEnumerator LoadCoroutine(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        yield return null;
    }
    void SceneLoaded()
    {
        loading = false;
        loadingMask.SetActive(false);
    }
    
    public void AddAvailableLangByID(int _id, bool isAvailable)
    {
        VoicesManager.Instance.ChangeAvailableLang(_id, isAvailable);
    }
    public void SetLang(int availableLangID)
    {
        int id = 0;
        foreach (VoicesManager.AvailableLang al in VoicesManager.Instance.availableLangs)
        {
            if (al.available)
            {
                if (id == availableLangID)
                {
                    VoicesManager.Instance.SetLang(al.lang);
                    return;
                }
                id++;
            }
        } 
    }
    
    
    public string GetFirstAvailableLang()
    {
        foreach (VoicesManager.AvailableLang al in VoicesManager.Instance.availableLangs)
            if (al.available)
                return al.lang.ToString();
        return null;
    }
}
