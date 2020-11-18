using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GEX.components;
using GEX.utils;

public class MainMenuPanel : MonoBehaviour
{
    public static MainMenuPanel me
    {
        get
        {
            if (_me == null)
                _me = GameObject.FindObjectOfType<MainMenuPanel>();

            return _me;
        }
    }

    public enum MMode
    { 
        AGE,
        LANGUAGE,
        VACCINES
    }

    public GameObject LANGUAGE_SELECTION;
    public GameObject AGE_SELECTION;
    public GameObject VACCINES_SELECTION;

    public UButton kidsButton;
    public UButton adultsButton;
    public RingManager RING_MANAGER;
    public SoundPlayer SOUND;
    private MMode mode;
    private bool _finished = false;
    private bool live = false;
    private XTimer timer;
    private int extra_state;
    private String prevLangCode;
    private static MainMenuPanel _me;

    void Start()
    {
        Events.SceneLoaded();
        kidsButton.addEventOnClick(OnKidsPressed);
        adultsButton.addEventOnClick(OnAdultsPressed);
        timer = new XTimer();
        InputManagerPontura.Instance.OnInput += OnInput;
    }
    private void OnDestroy()
    {
        InputManagerPontura.Instance.OnInput -= OnInput;
    }
    public void Show(MMode mode)
    {
        this.mode = mode;

        if (mode == MMode.AGE)
        {
            LANGUAGE_SELECTION.SetActive(false);
            AGE_SELECTION.SetActive(true);
            VACCINES_SELECTION.SetActive(false);
            

            if(!Config.AGE_MENU_VISIBLE)
                AGE_SELECTION.SetActive(false);
        }
        else if (mode == MMode.LANGUAGE)
        {
            LANGUAGE_SELECTION.SetActive(true);
            AGE_SELECTION.SetActive(false);
            VACCINES_SELECTION.SetActive(false);


            if(!Config.LANGUAGE_MENU_VISIBLE)
                LANGUAGE_SELECTION.SetActive(false);
        }
        else if (mode == MMode.VACCINES)
        {
            LANGUAGE_SELECTION.SetActive(false);
            AGE_SELECTION.SetActive(false);
            VACCINES_SELECTION.SetActive(true);


            if (!Config.VACCINES_MENU_VISIBLE)
                VACCINES_SELECTION.SetActive(false);
        }

        gameObject.SetActive(true);
        live = true;
        _finished = false;
        extra_state = 0;
    }

    public void Hide()
    {
        SOUND.KillAll();
        gameObject.SetActive(false);
        _finished = true;
        live = false;
        extra_state = 0xFFF;

        
    }

    public bool HasFinished()
    {
        return _finished;
    }

    public void SelectAdults()
    {
        if (live && (mode == MMode.AGE))
            OnAdultsPressed(null);
    }

    public void SelectKids()
    {
        if (live && (mode == MMode.AGE))
            OnKidsPressed(null);
    }

    private int OnKidsPressed(GameObject sender)
    {
        //GameObject.Find("WaterPlane").SetActive(false);
        //GameMode.MODE = GMode.STATIC_DOWN;
        //RING_MANAGER.KillGame();

        if (mode == MMode.AGE)
        {
            GAME.ageMode = AgeMode.KIDS;
            PlayAgeSelected();
            extra_state = 1;
        }
        return 0;
    }

    private int OnAdultsPressed(GameObject sender)
    {
        //GameObject.Find("WaterPlane").SetActive(true);
        //GameMode.MODE = GMode.MOVE;
        //RING_MANAGER.KillGame();

        if (mode == MMode.AGE)
        {
            GAME.ageMode = AgeMode.ADULTS;
            PlayAgeSelected();
            extra_state = 1;
        }
        return 0;
    }

    private void PlayAge()
    {
        //if (GAME.ageMode == AgeMode.KIDS)
        //    SOUND.PlayLangUsingNativeLangIfExists("select_kids");//SOUND.PlayLang("select_kids");
        //else if (GAME.ageMode == AgeMode.ADULTS)
        //    SOUND.PlayLangUsingNativeLangIfExists("select_adults");//SOUND.PlayLang("select_adults");
    }

    private void PlayAgeSelected()
    {
        if (GAME.ageMode == AgeMode.KIDS)
            SOUND.PlayUsingNativeLangIfExists("select_kids", true, "ok", true);//SOUND.PlayLang("kids_option_selected");
        else if (GAME.ageMode == AgeMode.ADULTS)
            SOUND.PlayUsingNativeLangIfExists("select_adults", true, "ok", true); //SOUND.PlayLang("adults_option_selected");
    }
    //private bool AcceptController()
    //{
    //    return Pvr_ControllerManager.controllerlink.Controller0.Trigger.State;
    //    return ((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) == true) && (OVRInput.Get(OVRInput.Button.PrimaryTouchpad) == false));
    //}
    void OnInput(InputManagerPontura.types type)
    {
        switch (type)
        {
            case InputManagerPontura.types.SWIPE_LEFT:
                if (extra_state > 2) return;
                if (GAME.vaccines == 1 && VoicesManager.Instance.GetTotalAvailableLangs()>1)
                {
                    GAME.vaccines--;
                    VoicesManager.Instance.PlayAudio(0, "selectalanguage");
                    return;
                }
                if (GAME.vaccines > 1)
                    GAME.vaccines--;
                VoicesManager.Instance.PlayAudio(0, GAME.vaccines.ToString());
                break;
            case InputManagerPontura.types.SWIPE_RIGHT:
                if (extra_state > 2) return;
                if (GAME.vaccines < 6)
                    GAME.vaccines++;
                VoicesManager.Instance.PlayAudio(0, GAME.vaccines.ToString());
                // SOUND.PlayLangUsingNativeLangIfExists(GAME.vaccines.ToString());
                break;
            case InputManagerPontura.types.GATILLO_DOWN:
                if(extra_state > 2) return;
                if (GAME.vaccines == 0)
                {
                    Events.LoadScene("LangSelector");
                    PersistentData.Instance.gameSettings = PersistentData.GameSettings.Adults;
                    GAME.vaccines = 1;
                    InputManagerPontura.Instance.OnInput -= OnInput;
                }
                else
                {
                    VoicesManager.Instance.PlayAudio(0, "ok");
                    extra_state = 2;
                }
                break;
            case InputManagerPontura.types.TWO_BUTTONS_DOWN:
                //si cambia:
                if (extra_state > 2)
                {
                    PersistentData.Instance.gameSettings = PersistentData.GameSettings.Adults;
                    Events.LoadScene("Adults");
                }
                else
                    Events.LoadScene("Kids");
                

                InputManagerPontura.Instance.OnInput -= OnInput;
                break;
        }
        

    }
    void Update()
    {
        String languageInUse;


        // ***********************************
        // ***             AGE             ***
        // ***********************************
        if (mode == MMode.AGE)
        {
            if (extra_state == 0)
            {
                PlayAge();//SOUND.PlayLang("age_selection");
                extra_state = 1;
            }
            else if (extra_state == 1)
            {
                //if (Input.GetKeyDown(KeyCode.LeftArrow) || Pvr_ControllerManager.controllerlink.Controller0.SwipeDirection == Pvr_UnitySDKAPI.SwipeDirection.SwipeLeft)
                // {
                //    GAME.ageMode = AgeMode.KIDS;
                //    PlayAge();
                //}
                //else if (Input.GetKeyDown(KeyCode.RightArrow) || Pvr_ControllerManager.controllerlink.Controller0.SwipeDirection == Pvr_UnitySDKAPI.SwipeDirection.SwipeRight)
                //{
                //    GAME.ageMode = AgeMode.ADULTS;
                //    PlayAge();
                //}
                //else if (Input.GetKeyDown(KeyCode.Space) || Pvr_ControllerManager.controllerlink.Controller0.Trigger.State)
                //{
                //    PlayAgeSelected();
                //    extra_state = 2;
                //}
            }
            else if (extra_state == 2)
            {
                timer.setDelay(3f);
                timer.start();
                extra_state = 3;
            }
            else if(extra_state == 3)
            {
                if (timer.update())
                {
                    Hide();
                }
            }
        }
        // ***********************************
        // ***********************************
        // ***********************************
        // ***             AGE             ***
        // ***********************************
        else if (mode == MMode.LANGUAGE)
        {
            if (extra_state == 2)
            {
                timer.setDelay(3f);
                timer.start();
                extra_state = 3;
            }
            else if (extra_state == 3)
            {
                if (timer.update())
                {
                    Hide();
                }
            }
        }
        // ***********************************
        // ***          VACCINES           ***
        // ***********************************
        else if (mode == MMode.VACCINES)
        {
            if (extra_state == 0)
            {
                VoicesManager.Instance.PlayAudio(0, GAME.vaccines.ToString());
              //  SOUND.PlayLang(GAME.vaccines.ToString());
                extra_state = 1;
            }
            else if (extra_state == 1)
            {
                //inputs
            }
            else if (extra_state == 2)
            {
                timer.setDelay(3f);
                timer.start();
                extra_state = 3;
            }
            else if (extra_state == 3)
            {
                if (timer.update())
                {
                    Hide();
                }
            }
        }



        /*if (Input.GetMouseButtonDown(0))
        {
            GameMode.MODE = GMode.MODE_KIDS;
            RING_MANAGER.KillGame();
        }*/
    }

}
