using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour {

    public NarrationController narrationController;

    public states state;
	public enum states
	{
		IDLE,
        LANG,
        CUANTAS,
		DONE
	}
	public TimeManager timeManager;

	bool isDone;

	int num_of_vaccines = 1;
    int lang_num = 0;
    float timer;

    int totalVacunas = 6;

	void Start()
	{
        InputManagerPontura.Instance.OnInput += OnInput;
		Restart ();
        Events.SceneLoaded();
	}
	void Update()
	{
		timer += Time.deltaTime;
	}
	public void Restart()
	{
		timer = 0;
		isDone = false;
        

        //if (!PersistentData.Instance.langSelected && VoicesManager.Instance.GetTotalAvailableLangs()>1)
        //{
        //    PersistentData.Instance.langSelected = true;
        //    StartCoroutine(LangSelect(true));
        //    state = states.LANG;
        //}
        //else
        //{
            //PersistentData.Instance.SetLang(0);
            StartCoroutine(Cuantas(true));
            state = states.IDLE;            
      //  }
	}

	void SwipeRight()
	{
        if (state == states.LANG)
            SetLangNum(1);
        if (state == states.CUANTAS)
            SetVacunasNum(1);
	}
	void SwipeLeft()
	{
        if (state == states.LANG)
            SetLangNum(-1);
        if (state == states.CUANTAS)
            SetVacunasNum(-1);
	}
    void SetLangNum(int qty)
    {
        int totalAvailableLangs = VoicesManager.Instance.GetTotalAvailableLangs();
        lang_num += qty;
        if (lang_num < 0)
            lang_num = totalAvailableLangs-1;
        else if (lang_num > totalAvailableLangs-1)
            lang_num = 0;

        PersistentData.Instance.langSelected = true;
        PersistentData.Instance.SetLang(lang_num);
        VoicesManager.Instance.PlayAudio(0, VoicesManager.Instance.lang.ToString());
       // PersistentData.Instance.audios.PlayAudio(AudiosManager.AudioType.langName);
    }
    void SetVacunasNum(int qty)
	{
        num_of_vaccines += qty;
        if (num_of_vaccines < 1 && VoicesManager.Instance.GetTotalAvailableLangs() > 1)
        {
            VoicesManager.Instance.PlayAudio(0, "selectalanguage");
            num_of_vaccines = 0;
            return;
        }
        else if (num_of_vaccines < 1)
            num_of_vaccines = 1;
        else if (num_of_vaccines > totalVacunas)
            num_of_vaccines = totalVacunas;

		PlayNum (num_of_vaccines);
	}
	void PlayNum(int num)
	{
        VoicesManager.Instance.PlayAudio(0, num.ToString());
	}
    IEnumerator LangSelect(bool languageReady = false)
    {
        yield return new WaitForSeconds(1.2f);
        //  PersistentData.Instance.audios.PlayAudio(AudiosManager.AudioType.selectLang);
        VoicesManager.Instance.PlayAudio(0, "selectalanguage");
        yield return new WaitForSeconds(1.7f);

        // PersistentData.Instance.audios.PlayAudio(AudiosManager.AudioType.langName);
        VoicesManager.Instance.PlayAudio(0, VoicesManager.Instance.lang.ToString());
        state = states.LANG;
    }
    IEnumerator Cuantas(bool languageReady = false)
	{		
		yield return new WaitForSeconds (2);
        //PersistentData.Instance.audios.PlayAudio (AudiosManager.AudioType.howmany);
        VoicesManager.Instance.PlayAudio(0, "howmany");
        yield return new WaitForSeconds (2);
        // PersistentData.Instance.audios.PlayAudio (AudiosManager.AudioType.one);
        VoicesManager.Instance.PlayAudio(0, "1");
        state = states.CUANTAS;
	}
	IEnumerator Done()
	{
        narrationController.UpdateLang();
        if (state == states.LANG)
        {
           // PersistentData.Instance.audios.PlayAudio(AudiosManager.AudioType.ok);
            VoicesManager.Instance.PlayAudio(0, "ok");
            timer = 0;
            isDone = false;
            StartCoroutine(Cuantas(true));
            state = states.IDLE;
        } else { 
            isDone = true;
            state = states.DONE;
            // PersistentData.Instance.audios.PlayAudio(AudiosManager.AudioType.ok);
            VoicesManager.Instance.PlayAudio(0, "okready");
            PersistentData.Instance.num_of_vaccines = num_of_vaccines;
            yield return new WaitForSeconds(1.2f);
            PlayNum(num_of_vaccines);
            yield return new WaitForSeconds(1.7f);
            NextScene();
        }
	}

	states lastState ;
	bool CheckChangeState(states newState)
	{
		if (lastState == newState)
			return false;
		lastState = newState;
		return true;
	}

	void OnDestroy()
	{
        InputManagerPontura.Instance.OnInput -= OnInput;
	}
	
	void OnInput (InputManagerPontura.types type) 
	{
        print("_ " + type + " isDone: " + isDone + " timer: " + timer);
            switch (type) {
		case InputManagerPontura.types.SWIPE_RIGHT:
                if (!isDone)
                    SwipeRight ();
			break;
		case InputManagerPontura.types.SWIPE_LEFT:
                if (!isDone)
                SwipeLeft ();
			break;
		case InputManagerPontura.types.GATILLO_DOWN:
                if (!isDone && timer > 3)
                { 
                    if(num_of_vaccines == 0)
                    {
                        PersistentData.Instance.gameSettings = PersistentData.GameSettings.Kids;
                        Events.LoadScene("LangSelector");
                    } else
                    StartCoroutine(Done());
                }
			break;
        case InputManagerPontura.types.TWO_BUTTONS_DOWN:
            if (timer > 1)
            {
                PersistentData.Instance.gameSettings = PersistentData.GameSettings.Adults;
                Events.LoadScene("Adults");
            }
            break;
        }
	}
	void NextScene()
	{
		timeManager.Init ();
	}

}
