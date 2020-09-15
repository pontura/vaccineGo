using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    public Text versionCodeField;
    public Text deviceName;
    public Text expirationDate;
    public Text debbugField;
    public List<UnityEngine.UI.Toggle> buttons;
    public UnityEngine.UI.Toggle toggleButton;
    public Transform container;
    int gameID;

    void Start()
    {
        string version = Application.version;
        if(version != null)
             versionCodeField.text = "v " + version;

        deviceName.text = "Device name: " + PersistentData.Instance.serverLogin.deviceName;

        expirationDate.text = "Expiration date: " + PersistentData.Instance.serverLogin.expiredDay;
        expirationDate.text += "/" + PersistentData.Instance.serverLogin.expiredMonth;
        expirationDate.text += "/" + PersistentData.Instance.serverLogin.expiredDay;

        debbugField.text = "";

        foreach(VoicesManager.AvailableLang al in VoicesManager.Instance.availableLangs)
        { 
            UnityEngine.UI.Toggle t = Instantiate(toggleButton);
            t.GetComponent<ToogleCustom>().Init(al.lang, al.available);
            t.transform.SetParent(container);
            t.transform.localScale = Vector3.one;
            t.transform.localEulerAngles = Vector3.zero;
            t.transform.localPosition = Vector3.zero;
            buttons.Add(t);
        }
    }

    public void OnStartGame(int gameID)
    {
        this.gameID = gameID;

        for (int a = 0; a < buttons.Count; a++)
            PersistentData.Instance.AddAvailableLangByID(a, buttons[a].isOn);

        int totalLangs = VoicesManager.Instance.GetTotalAvailableLangs();
        VoicesManager.Instance.SetDefaultLang();

        if (totalLangs == 0)
        {
            debbugField.text = "You need at least one language!";
            return;
        }

        debbugField.text = "Loading...";
        Invoke("Delayed", 2);
    }
    void Delayed()
    {
        if (gameID == 0)
            PersistentData.Instance.gameSettings = PersistentData.GameSettings.Kids;
        else
            PersistentData.Instance.gameSettings = PersistentData.GameSettings.Adults;
           
        Events.LoadScene("LangSelector");
    }
    public void ResetApp()
    {
        PlayerPrefs.DeleteAll();
        UnityEngine.SceneManagement.SceneManager.LoadScene("ResetApp");
    }
}
