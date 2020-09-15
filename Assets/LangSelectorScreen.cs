using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LangSelectorScreen : MonoBehaviour
{
    public Text field;
    InputManagerPontura inputManagerPontura;
    void Start()
    {
        Events.SceneLoaded();
        print("VoicesManager.Instance.GetTotalAvailableLangs(): " + VoicesManager.Instance.GetTotalAvailableLangs()); 
        if (VoicesManager.Instance.GetTotalAvailableLangs() < 2)
            GoToGame();
        else
        {
            inputManagerPontura = GetComponent<InputManagerPontura>();
            inputManagerPontura.OnInput += OnInput;
            VoicesManager.Instance.PlayAudio(0, "selectalanguage");
        }
    }
    private void OnDestroy()
    {
        if(inputManagerPontura != null)
            inputManagerPontura.OnInput -= OnInput;
    }
    void OnInput(InputManagerPontura.types type)
    {
        switch(type)
        {
            case InputManagerPontura.types.SWIPE_LEFT:
                ChangeLang(true);
                break;
            case InputManagerPontura.types.SWIPE_RIGHT:
                ChangeLang(false);
                break;
            case InputManagerPontura.types.GATILLO_DOWN:
                VoicesManager.Instance.PlayAudio(0, "ok");
                GoToGame();
                Destroy(this);
                break;
        }
    }
    int langID = 0;
    void ChangeLang(bool next)
    {
        if (next)
            langID++;
        else
            langID--;

        if (langID < 0)
            langID = 0;
        else if (langID >= VoicesManager.Instance.GetTotalAvailableLangs() - 1)
            langID = VoicesManager.Instance.GetTotalAvailableLangs() - 1;
        SetNewLang();
    }
    void SetNewLang()
    {
        print(langID);
        PersistentData.Instance.SetLang(langID);
        VoicesManager.Instance.PlayAudio(0, VoicesManager.Instance.lang.ToString().ToLower());
    }
    void GoToGame()
    {
        if(PersistentData.Instance.gameSettings == PersistentData.GameSettings.Adults)
            Events.LoadScene("Adults");
        else
            Events.LoadScene("Kids");
    }
}
