using System.Collections;
using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashUI : MonoBehaviour {

    public Image progressBar;
	[SerializeField] Text textField;
    bool onLoading = false;
    void Awake()
	{		
		Events.OnKeyboardText +=OnKeyboardText;
	}
    private void Start()
    {
        onLoading = true;
        string url = PersistentData.Instance.serverLogin.url + "bundles/";
        Debug.Log("url: " + url);
        StartCoroutine( PersistentData.Instance.assetsBundleLoader.DownloadAll(url, OnSuccess) );
    }
    void OnSuccess(string result)
    {
        onLoading = false;
        progressBar.fillAmount = 1;
        Debug.Log("OnSuccess " + result);
        if (result == "ok")
        {
           // DestroyImmediate(VoicesManager.Instance.gameObject);
            PersistentData.Instance.assetsBundleLoader.GetAsset("voicesmanager.all", "voicesmanager");
        }
        Debug.Log("AssetsBundle Loaded " + result);
        PersistentData.Instance.serverLogin.Init();
    }
    private void Update()
    {
        if(onLoading)
        {
            float p =  PersistentData.Instance.assetsBundleLoader.Progress;
            Debug.Log(p);
            progressBar.fillAmount = p;
        }
    }
    void OnDestroy()
	{
		Events.OnKeyboardText -=OnKeyboardText;
	}
	void OnKeyboardText(string text)
	{
		textField.text = text;
	}

}
