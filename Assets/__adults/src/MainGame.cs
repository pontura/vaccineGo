#pragma warning disable 414

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GEX.components;
using GEX.utils;
using GEX.graphics.effects;
using GEX.components.message_box;
using UnityEngine.SceneManagement;
using DG.Tweening;
using GEX.media.sound;
using InGameBoard;

public class MainGame : MonoBehaviour
{
    public struct SavedState
    {
        public int STATE;
        public int SUB_STATE;
        public int GAME_STATE;
        public int extra_state;

        public SavedState(int STATE, int SUB_STATE, int GAME_STATE, int extra_state)
        {
            this.STATE = STATE; this.SUB_STATE = SUB_STATE; this.GAME_STATE = GAME_STATE; this.extra_state = extra_state;
        }
    }

    private static bool CODE_LOADED = false;

    private MainGame m_Instance;
    public MainGame Instance { get { return m_Instance; } }

    private static int STATE;
    private static int SUB_STATE;
    private static int GAME_STATE;
    public static int extra_state;

    private static SavedState savedState;

    private static GameObject canvasGameObject;
    private static Canvas canvas;
    private static GameObject eventSystemGameObject;
    private static EventSystem eventSystem;
    private static GameObject canvasExtension;

    private static XTimer timer, timer2;
    private static bool _playerReachSky = false;
    private static Vector3 savedPlayerPos;
    private static bool _playerIsFlying = false;
    private static bool SPECIAL_MODE_COMPLETE = false;
    private static bool LANGUAGE_SELECTION = false;
    private static bool VACCINES_SELECTION = false;
    private static int lang_sel_state = 0;
    private static bool isInVaccinesSelPrev = false;

    private static bool beQuietVarOnce = false;
    private static int extractionLvlCount = 0;
    private static bool BLOCK_SPECIAL_MODE = true;
    private static bool TIMES_UP = false;

    private static XTimer NEXT_LVL_TIMER;

    private static Rect labelRect = new Rect(20, 20, 400, 400);

    void Awake()
    {
       // DontDestroyOnLoad(this.gameObject);
        m_Instance = this;
    }

    void Start()
    {
        VACCINES_SELECTION = false;
        CODE_LOADED = false;
        beQuietVarOnce = false;
          extractionLvlCount = 0;
         BLOCK_SPECIAL_MODE = true;
        TIMES_UP = false;

        
    STATE = 0;
        SUB_STATE = 0;
        GAME_STATE = 0;
        extra_state = 0;

        RESET_GAME();

        GameObject config_obj;
        if (CODE_LOADED)
            return;

        Events.SceneLoaded();

        //Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //SETUP_CANVAS_AND_EVENT_SYSTEM();

        //---------------------------  setup locales   ----------------------
        //TextAsset langFile = Resources.Load("locale/languages") as TextAsset;
        //TEXT.setupAsPlainText(langFile.text);
        //------------------------------------
        // Main User Configuration File
        /*config_obj = (GameObject)Resources.Load("editable/UConfig");
        config_obj.GetComponent<UConfig>().init();

        GameConfigData.Load();*/

        //TEXT.setLocale(Stats.LOCALE);

        // Set English by default at the beginning
        //UMessageBox.setLanguage(UMessageBox.Language.ENGLISH);
        //UInputDialog.setLanguage(UInputDialog.Language.ENGLISH);

        /*GSound.initialize(this.gameObject);
        GameSound.load(GameSound.ALL_SOUNDS);

        UButton.setGlobalSound(SoundRes.fx_pop);*/

        LangRes.Initialize();

        Environment.me.PLAY_ONE_MUSIC_ALL_APP();

        // will never enter here again
        CODE_LOADED = true;

        // Load Game
        //Stats.initialize();

        timer = new XTimer();
        timer2 = new XTimer();
        NEXT_LVL_TIMER = new XTimer();
        
        // First Game State
        STATE =         State.OnGameLoad;
        SUB_STATE =     SubState.SetupAll;
        GAME_STATE =    0;
        extra_state =   0;
        
        VRLauncher.me.SetAutoRelaunch(false);
    }
  
    public  float timerNum = 0;
    //void OnInput(InputManagerPontura.types type)
    //{
    //    if (timerNum > 2)
    //    {
    //      //  print("OnInput " + type);
    //      //  print("State.OnGameLoad " + State.OnGameLoad);

    //       // print("SUB_STATE = SubState.SetupAll " + SUB_STATE);
    //       // print("GAME_STATE " + GAME_STATE);
    //       // print("extra_state " + extra_state);

    //        switch (type)
    //        {
    //            //case InputManagerPontura.types.PAD_DOWN:
    //            //    if(STATE == 3 && SUB_STATE == 0)
    //            //        OPEN_LANGUAGE_SELECTION();
    //            //   // else
    //            //      //  RETURN_TO_SIMPLE_MODE();
    //            //    break;
    //            case InputManagerPontura.types.TWO_BUTTONS_DOWN:
                   
    //                CODE_LOADED = false;
    //                if (SUB_STATE == 0)
    //                {
    //                    PersistentData.Instance.gameSettings = PersistentData.GameSettings.Kids;
    //                    Events.LoadScene("LangSelector");
    //                    InputManagerPontura.Instance.OnInput -= OnInput;
    //                }
    //                else
    //                {
                      
    //                    // anulado el reseteo:
    //                    //si estas en metralleta
    //                    if(GAME_STATE != 3)
    //                        Events.LoadScene("Adults");
    //                }

    //                // UnityEngine.SceneManagement.SceneManager.LoadScene("Kids");
    //                break;
    //        }
    //    }
    //}
    void Update()
    {
     //   print("GAME_STATE: " + GAME_STATE + " extra_state" +  extra_state + "  state = " + STATE + "  SUB_STATE " + SUB_STATE);

        timerNum += Time.deltaTime;
        //if (Input.GetKeyDown(KeyCode.J))
        //Environment.me.Fireworks();
        /*Mouse.update();
        GSound.update();
        SPXManager.update();*/
        switch (STATE)
        {
            //*******************************************************
            //***                                                 ***
            //***             STATE: OnGameLoad                   ***
            //***                                                 ***
            //*******************************************************
            //***                                                 ***
            //***    Desc: load all resources and init modules    ***
            //***          for the game.                          ***
            //***                                                 ***
            //***                                                 ***
            //***                                                 ***
            //***                                                 ***
            //*******************************************************
            case State.OnGameLoad:

                switch (SUB_STATE)
                {
                    //*****************************************
                    //****            SETUP ALL            ****
                    //*****************************************
                    case SubState.SetupAll:

                        Config.Initialize();

                        SUB_STATE = SubState.SetupExtras;
                        extra_state = 0;

                        break;


                    //*****************************************
                    //****          SETUP EXTRAS           ****
                    //*****************************************
                    case SubState.SetupExtras:
                        
                        Debug.Log("Setting Quality...");

                        MainMenuPanel.me.Hide();
                        InGameScore.me.HideNow();

                        UnityEngine.XR.XRSettings.eyeTextureResolutionScale = 2.0f;

                        if (Config.MODE_SELECT)
                            GoTo(State.OnGame, SubState.OnMenu);
                        else
                            GoTo(State.OnGame, SubState.OnPlay);

                        break;

                    //*****************************************
                    //****          SHOW SPLASH            ****
                    //*****************************************
                    case SubState.ShowSplash:

                        /*// ---------------------------------------
                        if (!Config.SHOW_SPLASH)
                        {
                            STATE = State.OnLogin;
                            SUB_STATE = SubState.OnLoginInit;
                            extra_state = 0;
                        }
                        // ---------------------------------------
                        else
                        {
                            if (extra_state == 0)
                            {
                                
                                //IntroManager.initialize(new IntroElement[] {    new IntroElement(Res.VIDEO_UNTREF_LOGO, IntroType.VIDEO),
                                //                                                new IntroElement(Res.GUI_PUBLISHER_LOGO, IntroType.GUI, 2f),
                                //                                                new IntroElement(Res.VIDEO_MAZES_LOGO, IntroType.VIDEO)});
                                //IntroManager.show();
                                 
                                SplashPanel.initialize();
                                SplashPanel.show();
                                extra_state = 1;
                            }
                            else if (extra_state == 1)
                            {
                                SplashPanel.update();
                                extra_state = 2;
                            }
                            else if (extra_state == 2)
                            {
                                SplashPanel.update();
                                if (SplashPanel.hasFinished())
                                {
                                    // Remove from screen and
                                    // release resources
                                    SplashPanel.destroy();

                                    GoTo(State.OnLogin, SubState.OnLoginInit);
                                }
                            }
                        }*/
                        break;
                }
                break;

            //*******************************************************
            //***                                                 ***
            //***                STATE: OnLogin                   ***
            //***                                                 ***
            //*******************************************************
            //***                                                 ***
            //***    Desc: sign in/up for the users               ***
            //***                                                 ***
            //***                                                 ***
            //***                                                 ***
            //***                                                 ***
            //***                                                 ***
            //*******************************************************
            case State.OnLogin:

               /* // -----------------------------
                if (!Config.SHOW_LOGIN)
                {
                    GoTo(State.OnMainMenu, SubState.MMenuSelect);
                    break;
                }
                // -----------------------------

                switch (SUB_STATE)
                {
                    //*****************************************
                    //****          ON LOGIN INIT          ****
                    //*****************************************
                    case SubState.OnLoginInit:
                        if (extra_state == 0)
                        {
                            extra_state = 1;
                        }
                        else if (extra_state == 1)
                        {
                            GoTo(State.OnMainMenu, SubState.MMenuSelect);
                        }
                        break;
                }*/
                break;


            //*******************************************************
            //***                                                 ***
            //***             STATE: OnMainMenu                   ***
            //***                                                 ***
            //*******************************************************
            //***                                                 ***
            //***    Desc: main menu only                         ***
            //***                                                 ***
            //***                                                 ***
            //***                                                 ***
            //***                                                 ***
            //***                                                 ***
            //*******************************************************
            case State.OnMainMenu:

                switch (SUB_STATE)
                {
                    //*****************************************
                    //****              SELECT             ****
                    //*****************************************
                    case SubState.MMenuSelect:
                        /*
                        MainMenuModule.me = new MainMenuModule();
                        MainMenuModule.me.show();

                        GSound.stopAll();
                        GameSound.playMusic(SoundRes.music_title);
                        */
                        break;


                    //*****************************************
                    //****             OPTIONS             ****
                    //*****************************************
                    case SubState.MMenuOptions:

                        /*if (extra_state == 0)
                        {
                            InGamePauseMenu.me = new InGamePauseMenu();
                            InGamePauseMenu.me.setup(true);
                            InGamePauseMenu.me.show();

                            GameSound.play(SoundRes.fx_pause);

                            extra_state = 1;
                        }
                        else if (extra_state == 1)
                        {
                            InGamePauseMenu.me.update();
                            if (InGamePauseMenu.me.hasFinished())
                            {
                                STATE =     State.OnMainMenu;
                                SUB_STATE = SubState.MMenuSelect;
                                extra_state = 0;
                            }
                        }*/

                        break;

                    //*****************************************
                    //****             CREDITS             ****
                    //*****************************************
                    case SubState.MMenuCredits:

                        /*if (extra_state == 0)
                        {
                            CreditsModule.me = new CreditsModule();
                            CreditsModule.me.show();
                            extra_state = 1;
                        }
                        else if (extra_state == 1)
                        {
                            CreditsModule.me.update();
                            if (CreditsModule.me.hasFinished())
                            {
                                STATE = State.OnMainMenu;
                                SUB_STATE = SubState.MMenuSelect;
                                extra_state = 0;
                            }
                        }*/

                        break;


                   

                }
                break;


            //*******************************************************
            //***                                                 ***
            //***                STATE: OnGame                    ***
            //***                                                 ***
            //*******************************************************
            //***                                                 ***
            //***    Desc: main gameplay loop                     ***
            //***                                                 ***
            //***                                                 ***
            //***                                                 ***
            //***                                                 ***
            //***                                                 ***
            //*******************************************************
            case State.OnGame:


                if (LANGUAGE_SELECTION)
                {
                    BLOCK_SPECIAL_MODE = true;

                    if (lang_sel_state == 0)
                    {
                        MainMenuPanel.me.Show(MainMenuPanel.MMode.LANGUAGE);
                        lang_sel_state = 1;
                    }
                    else if (lang_sel_state == 1)
                    {
                        if (MainMenuPanel.me.HasFinished())
                        {
                            if (isInVaccinesSelPrev)
                            {
                                GoTo(SubState.OnMenu);
                            }

                            LANGUAGE_SELECTION = false;
                        }
                    }

                    return;
                }



                switch (SUB_STATE)
                {

                    //*****************************************
                    //****              OnPlay             ****
                    //*****************************************
                    case SubState.OnMenu:

                        BLOCK_SPECIAL_MODE = true;

                        // ------------------------------------------
                        // ---             AGE SELECT             ---
                        // ------------------------------------------
                        if (GAME_STATE == 0)
                        {
                            if (extra_state == 0)
                            {
                                VACCINES_SELECTION = true;
                                isInVaccinesSelPrev = true;
                                Environment.me.StopSound();
                                MainMenuPanel.me.Show(MainMenuPanel.MMode.VACCINES);
                                extra_state = 1;
                            }
                            else if (extra_state == 1)
                            {
                                if (MainMenuPanel.me.HasFinished())
                                {
                                    VACCINES_SELECTION = false;
                                    isInVaccinesSelPrev = false;
                                    GoTo(State.OnGame, SubState.OnPlay);
                                }
                            }

                        }
                        break;

                    //*****************************************
                    //****              OnPlay             ****
                    //*****************************************
                    case SubState.OnPlay:

                        // ------------------------------------------
                        // ---                INTRO               ---
                        // ------------------------------------------
                        if (GAME_STATE == 0)
                        {
                            GAME.Initialize();

                            if (!Config.SHOW_INTRO)
                            {
                                Parrot2.me.MOVE_TO_WOOD();
                                GAME_STATE = 1;
                                extra_state = 0;
                                Config.SHOW_INTRO = true;
                                break;
                            }

                            if (extra_state == 0)
                            {
                                BLOCK_SPECIAL_MODE = false;


                                if (GAME.ageMode == AgeMode.ADULTS)
                                {
                                    Environment.me.PlayAmbientIntroSound();
                                }
                                else if (GAME.ageMode == AgeMode.KIDS)
                                {
                                    Environment.me.PlayAmbientIntroSound();
                                    //Environment.me.PlayAmbientSound();
                                }

                                InGameScore.me.HideNow();
                                timer.setDelay(Config.INTRO_WAIT_SECONDS);
                                timer.start();
                                extra_state = 1;
                            }
                            else if (extra_state == 1)
                            {
                                if (Config.USE_INTRO_SECONDS)
                                {
                                    if (timer.update())
                                        extra_state = 2;
                                }
                                else
                                    extra_state = 2;
                            }
                            else if (extra_state == 2)
                            {
                                InGameBoardModule.me.ShowIntro();
                                extra_state = 3;
                            }
                            else if (extra_state == 3)
                            {
                                if (InGameBoardModule.me.HasFinished())
                                {
                                    VRLauncher.me.DisableAndKill();
                                    InGameBoardModule.me.ResetComplete();
                                    GAME_STATE = 1;
                                    extra_state = 0;
                                }
                            }
                        }
                        // ------------------------------------------
                        // ---              GAMEPLAY              ---
                        // ------------------------------------------
                        else if (GAME_STATE == 1)
                        {
                            if (extra_state == 0)
                            {
                                InGameScore.me.Setup(GAME.LEVEL.TARGET, GAME.LEVEL);
                                InGameScore.me.Reset();
                                InGameScore.me.Show();

                                Environment.me.PlayAmbientSound();

                                RingManager.me.Begin(GAME.LEVEL);
                                
                                VRLauncher.me.Enable();

                                
                                // corre al inicio:
                                Debug.Log("1 GAMEPLAY " + GAME.LEVEL_NUMBER + " VAC: " + GAME.vaccines);

                                float time_up = Config.TIMES_UP;
                                if (GAME.LEVEL_NUMBER == GAME.vaccines)
                                {
                                    time_up += 30;
                                    Debug.Log("AGREGO 30 SEGUNDOS POR SER LA ULTIMA VACUNA");
                                }

                                NEXT_LVL_TIMER.setDelay(time_up);
                                NEXT_LVL_TIMER.start();

                                extra_state = 1;
                            }
                            else if (extra_state == 1)
                            {
                                // corre en cada frame:
                                if (NEXT_LVL_TIMER.update() && (GAME.vaccines > GAME.LEVEL_NUMBER))
                                {
                                    RingManager.me.KillGame();
                                    TIMES_UP = true;
                                    GAME_STATE =    2;
                                    extra_state =   0;
                                }
                                else if (InGameScore.me.IsComplete())
                                {
                                    RingManager.me.KillGame();
                                    TIMES_UP = false;
                                    GAME_STATE = 2;
                                    extra_state = 0;
                                }
                                else
                                {
                                    if (InGameScore.me.AnyBallFailed())
                                    {
                                        if (GAME.LEVEL.EXTRA_CONDITION == ExtraCondition.FAIL_WILL_GOTO_NEXT_GAME)
                                        {
                                            GAME.LEVEL_NUMBER++;
                                            if (GAME.AllLevelsComplete())
                                                GAME.ResetLevels();

                                            RingManager.me.KillGame();
                                            VRLauncher.me.DisableAndKill();
                                            timer.setDelay(3f);
                                            timer.start();
                                            extra_state = 10;
                                        }
                                        else if (GAME.LEVEL.EXTRA_CONDITION == ExtraCondition.FAIL_WILL_RESET)
                                        {
                                            RingManager.me.KillGame();
                                            VRLauncher.me.DisableAndKill();
                                            timer.setDelay(3f);
                                            timer.start();
                                            extra_state = 11;
                                        }
                                    }
                                }
                            }
                            // special case -> fail -> go to next level
                            else if (extra_state == 10)
                            {
                                if (timer.update())
                                {
                                    //
                                    // TODO: hardcode here! for Talk!
                                    //
                                    InGameBoardModule.me.Talk(7);
                                    // -------------------------------
                                    extra_state = 0;
                                }
                            }
                            // special case -> wait time to reboot game
                            else if (extra_state == 11)
                            {
                                if (timer.update())
                                {
                                    extra_state = 0;
                                }
                            }
                        }
                        // ------------------------------------------
                        // ---           LEVEL COMPLETE           ---
                        // ------------------------------------------
                        else if (GAME_STATE == 2)
                        {
                            if (extra_state == 0)
                            {
                                Debug.Log("LEVEL COMPLETE  CURRENT LEVEL NUMBER: " + GAME.LEVEL_NUMBER + " Vaccines: " + GAME.vaccines);

                                if (GAME.LEVEL_NUMBER == GAME.vaccines)
                                {
                                    JUMP_TO_END_GAME();
                                }
                                else
                                {
                                    VRLauncher.me.DisableAndKill();
                                    //InGameScore.me.HideNow();
                                    InGameBoardModule.me.ShowNextLevelInfo(GAME.LEVEL_NUMBER, TIMES_UP);
                                    Environment.me.PlayWinFx();
                                    GAME.LEVEL_NUMBER++;
                                    extra_state = 1;

                                    if (GAME.LEVEL.LEVEL_KIND == LevelKind.SPECIAL)
                                    {
                                        JUMP_TO_SPECIAL_MODE();
                                    }
                                }
                            }
                            else if (extra_state == 1)
                            {
                                if (InGameBoardModule.me.HasFinished())
                                {
                                    InGameBoardModule.me.ResetComplete();

                                    if (GAME.AllLevelsComplete())
                                        GAME.ResetLevels();
                                   
                                    GAME_STATE = 1;
                                    extra_state = 0;
                                }
                            }
                        }
                        // ------------------------------------------
                        // ---            SPECIAL MODE             --
                        // ------------------------------------------
                        else if (GAME_STATE == 3)
                        {
                            if (extra_state == 0)
                            {
                                BLOCK_SPECIAL_MODE = true;
                                
                                //VRLauncher.me.SetAutoRelaunch(true);
                                VRLauncher.me.DisableAndKill();
                                //InGameScore.me.HideNow();
                                InGameScore.me.Reset();
                                RingManager.me.KillGame();

                                InGameBoardModule.me.ForceFinished();
                                InGameBoardModule.me.ResetComplete();

                                //Environment.me.StartSpecialMode();
                                //Parrot2.me.SPECIAL_MODE();
                                InGameBoardModule.me.TalkSpecial();

                                extractionLvlCount = 0;
                                beQuietVarOnce = true;
                                extra_state = 1;
                            }
                            else if (extra_state == 1)
                            {
                                //if (Parrot2.me.IsOnSpecialMode())
                                if(InGameBoardModule.me.TalkFinished())
                                {
                                    if (InGameBoardModule.me.TalkSpecialBeQuiet(0))
                                        timer2.setDelay(4f);
                                    else
                                        timer2.setDelay(0f);

                                    timer2.start();
                                    //extractionLvlCount++;

                                    extra_state = 0x100;
                                }
                            }
                            else if (extra_state == 0x100)
                            {
                                if (timer2.update())
                                {
                                    BLOCK_SPECIAL_MODE = false;

                                    GAME.SetSpecialLevel(Level.CreateSpecialLvl(20 + (extractionLvlCount * 10)));
                                    RingManager.me.Begin(GAME.SPECIAL_LEVEL);//GAME.SPECIAL_LEVEL);
                                    InGameScore.me.Setup(GAME.SPECIAL_LEVEL.TARGET, GAME.SPECIAL_LEVEL);
                                    //InGameScore.me.Show();
                                    VRLauncher.me.Enable();
                                    VRLauncher.me.SetAutoRelaunch(true);
                                    Environment.me.StartNewSpecialMode();

                                    extra_state = 2;
                                }
                            }
                            // --------------------------------------------------
                            //              GAMEPLAY 
                            // --------------------------------------------------
                            else if (extra_state == 2)
                            {
                                if (RingManager.me.CompleteRing(true))
                                {
                                    print("______________ extra_state == 2" + Config.EXTRACTION_USE_FIREWORKS);

                                    RingManager.me.KillGame();

                                    if(Config.EXTRACTION_USE_FIREWORKS)
                                        Environment.me.Fireworks();

                                    VRLauncher.me.DisableAndKill();
                                    timer2.setDelay(4f);
                                    timer2.start();

                                    extra_state = 0x200;
                                }

                                /*if (SPECIAL_MODE_COMPLETE)
                                {
                                    BLOCK_SPECIAL_MODE = true;
                                    SPECIAL_MODE_COMPLETE = false;
                                    extra_state = 3;
                                }*/
                            }
                            else if (extra_state == 0x200)
                            {
                                if (timer2.update())
                                {
                                    if (Config.EXTRACTION_USE_BREATH_AUDIO)
                                    {
                                        if (InGameBoardModule.me.TalkSpecialBeQuiet(++extractionLvlCount))
                                            timer2.setDelay(4f);
                                        else
                                            timer2.setDelay(0f);
                                    }
                                    else
                                        timer2.setDelay(0f);

                                    timer2.start();

                                    RingManager.me.Begin(Level.CreateSpecialLvl(20 + (extractionLvlCount * 10)));//GAME.SPECIAL_LEVEL);
                                    
                                    
                                    extra_state = 0x201;
                                }
                            }
                            else if (extra_state == 0x201)
                            {
                                if (timer2.update())
                                {
                                    VRLauncher.me.Enable();
                                    VRLauncher.me.SetAutoRelaunch(true);
                                    extra_state = 2;
                                }
                            }
                            //
                            // Special Game Finished
                            //
                            /*else if (extra_state == 3)
                            {
                                if (Parrot2.me.IsOnIsland())
                                {
                                    timer2.setDelay(12f);
                                    timer2.start();
                                    extra_state = 4;
                                }
                            }
                            else if (extra_state == 4)
                            {
                                if (timer2.update())
                                {
                                    InGameBoardModule.me.ByeBye();
                                    timer2.setDelay(12f);
                                    timer2.start();
                                    extra_state = 5;
                                }
                            }
                            else if (extra_state == 5)
                            {
                                if (timer2.update())
                                {
                                    BLOCK_SPECIAL_MODE = false;
                                    RESET_GAME();
                                }
                            }*/
                        }
                        // ------------------------------------------
                        // ---             END GAME                --
                        // ------------------------------------------
                        else if (GAME_STATE == 4)
                        {
                            if (extra_state == 0)
                            {
                                BLOCK_SPECIAL_MODE = true;

                                VRLauncher.me.SetAutoRelaunch(false);
                                VRLauncher.me.DisableAndKill();
                                InGameScore.me.HideInSeconds(3f);//InGameScore.me.HideNow();
                                RingManager.me.KillGame();

                                InGameBoardModule.me.ForceFinished();
                                InGameBoardModule.me.ResetComplete();

                                Environment.me.StopNewSpecialMode();

                                Parrot2.me.RESET_ANIMATOR();

                                Parrot2.me.BACK();
                                InGameBoardModule.me.GameFinishedAudio();
                                Environment.me.PlayAmbientEndGame();


                                Debug.Log("LEVEL -> " + GAME.LEVEL_NUMBER);

                                //Debug.Log("RETURN_TO_SIMPLE_MODE");

                                extra_state = 1;
                            }
                            else if (extra_state == 1)
                            {
                                if (Parrot2.me.IsOnIsland())
                                {
                                    timer2.setDelay(12f);
                                    timer2.start();
                                    extra_state = 2;
                                }
                            }
                            else if (extra_state == 2)
                            {
                                if (timer2.update())
                                {
                                    InGameBoardModule.me.ByeBye();
                                    timer2.setDelay(12f);
                                    timer2.start();
                                    extra_state = 3;
                                }
                            }
                            else if (extra_state == 3)
                            {
                                if (timer2.update())
                                {
                                    BLOCK_SPECIAL_MODE = false;
                                    RESET_GAME();
                                }
                            }
                        }

                        break;




                }
                break;
        }
    }


    private static void RESET_GAME()
    {
        Level.Initialize(true);
        GAME.ResetLevels();
        GoTo(State.OnGame, SubState.OnMenu);
    }

    private static void MOVE_PLAYER_TO_SKY()
    {
        GameObject player = GameObject.Find("OVRPlayerController");
        savedPlayerPos = player.transform.position;
        _playerReachSky = false;
        player.transform.DOMoveY(7f, 10f).OnComplete(OnMovePJToSkyComplete);
        _playerIsFlying = true;
    }

    private static void MOVE_PLAYER_TO_GROUND()
    {
        if (_playerIsFlying)
        {
            GameObject player = GameObject.Find("OVRPlayerController");
            player.transform.DOKill();
            player.transform.position = savedPlayerPos;
            _playerIsFlying = false;
        }
    }

    private static void OnMovePJToSkyComplete()
    {
        _playerReachSky = true;
    }

    public static void TOGGLE_SPECIAL_MODE()
    {
        Debug.Log("Is SP Mode blocked: " + BLOCK_SPECIAL_MODE);

        if (BLOCK_SPECIAL_MODE)
            return;

        // Already in Special Mode
        if (GAME_STATE == 3)
        {
            RETURN_TO_SIMPLE_MODE();
        }
        else
        {
            //JUMP_TO_SPECIAL_MODE();
        }
    }

    public static void OPEN_LANGUAGE_SELECTION()
    {
        if (VACCINES_SELECTION)
        {
            VACCINES_SELECTION = false;
            LANGUAGE_SELECTION = true;
            lang_sel_state = 0;
            PersistentData.Instance.langSelected = true;
        }
    }

    public static void JUMP_TO_SPECIAL_MODE()
    {
        Parrot2.me.RESET_ANIMATOR();

        GAME_STATE = 3;
        extra_state = 0;
        SPECIAL_MODE_COMPLETE = false;
        Debug.Log("JUMP_TO_SPECIAL_MODE");
    }

    public static void JUMP_TO_END_GAME()
    {
        GAME_STATE = 4;
        extra_state = 0;
        Debug.Log("JUMP_TO_END_GAME");
    }

    // return from special game
    public static void RETURN_TO_SIMPLE_MODE() 
    {
        /*VRLauncher.me.SetAutoRelaunch(false);
        VRLauncher.me.DisableAndKill();
        InGameScore.me.HideInSeconds(3f);//InGameScore.me.HideNow();
        RingManager.me.KillGame();

        InGameBoardModule.me.ForceFinished();
        InGameBoardModule.me.ResetComplete();

        Environment.me.StopNewSpecialMode();

        Parrot2.me.RESET_ANIMATOR();

        Parrot2.me.BACK();
        InGameBoardModule.me.GameFinishedAudio();
        Environment.me.PlayAmbientEndGame();

        SPECIAL_MODE_COMPLETE = true;

        Debug.Log("LEVEL -> " + GAME.LEVEL_NUMBER);*/


        VRLauncher.me.SetAutoRelaunch(false);
        VRLauncher.me.DisableAndKill();
        //InGameScore.me.HideInSeconds(3f);//InGameScore.me.HideNow();
        RingManager.me.KillGame();

        InGameBoardModule.me.ForceFinished();
        InGameBoardModule.me.ResetComplete();

        Environment.me.StopNewSpecialMode();

        Parrot2.me.RESET_ANIMATOR();

        //Parrot2.me.BACK();
        InGameBoardModule.me.GameFinishedAudio();
        //Environment.me.PlayAmbientEndGame();

        SPECIAL_MODE_COMPLETE = true;


        GAME.RemoveLevel(GAME.LEVEL);
        if (GAME.LEVEL_NUMBER > 0)
            GAME.LEVEL_NUMBER--;

        GAME_STATE = 2;
        extra_state = 0;

        Debug.Log("RETURN_TO_SIMPLE_MODE");


        //pontura: resetea al salir del metralla mode:
        UnityEngine.XR.InputTracking.Recenter();
    }

    public static void OLD_JUMP_TO_SPECIAL_MODE()
    {
        GAME_STATE = 4;
        extra_state = 0;
        Debug.Log("JUMP_TO_SPECIAL_MODE");
    }

    public static void OLD_RETURN_TO_SIMPLE_MODE()
    {
        VRLauncher.me.DisableAndKill();
        InGameScore.me.HideNow();
        RingManager.me.KillGame();

        InGameBoardModule.me.ForceFinished();
        InGameBoardModule.me.ResetComplete();

        Environment.me.StopSpecialMode();

        MOVE_PLAYER_TO_GROUND();

        GAME_STATE = 0;
        extra_state = 0;

        Debug.Log("RETURN_TO_SIMPLE_MODE");
    }

    private static void GOTO_MAIN_MENU()
    {
        SceneManager.LoadScene("mainGame");
        //DESTROY_GAMEPLAY();
        GoTo(State.OnMainMenu, SubState.MMenuSelect);
    }

    private static void GoTo(int subState)
    {
        extra_state = 0;
        GAME_STATE = 0;
        SUB_STATE = subState;
    }

    private static void GoTo(int state, int subState)
    {
        extra_state = 0;
        GAME_STATE = 0;
        STATE = state;
        SUB_STATE = subState;
    }

    private static void PushState()
    {
        savedState = new SavedState(STATE, SUB_STATE, GAME_STATE, extra_state);
    }

    private static void PopState()
    {
        STATE =         savedState.STATE;
        SUB_STATE =     savedState.SUB_STATE;
        GAME_STATE =    savedState.GAME_STATE;
        extra_state =   savedState.extra_state;
    }

    private static void SETUP_CANVAS_AND_EVENT_SYSTEM()
    {
        //**************************************
        //**************************************
        //           CREATE CANVAS
        //**************************************
        //**************************************
        if (canvasGameObject == null)
        {
            canvasGameObject = new GameObject();
            canvasGameObject.name = "Canvas";
            canvas = canvasGameObject.AddComponent<Canvas>();
            canvas.pixelPerfect = true;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGameObject.AddComponent<GraphicRaycaster>();

          //  DontDestroyOnLoad(canvasGameObject);
        }
        //**************************************
        //**************************************
        //       CREATE CANVAS EXTENSION
        //**************************************
        //**************************************
        if (canvasExtension == null)
        {
            canvasExtension = new GameObject();
            canvasExtension.name = "CanvasExt";

         //   DontDestroyOnLoad(canvasExtension);
        }
        //**************************************
        //**************************************
        //         CREATE EVENT SYSTEM
        //**************************************
        //**************************************
        if (eventSystemGameObject == null)
        {
            eventSystemGameObject = new GameObject();
            eventSystemGameObject.name = "EventSystem";
            eventSystem = eventSystemGameObject.AddComponent<EventSystem>();
            eventSystemGameObject.AddComponent<StandaloneInputModule>();
            eventSystemGameObject.AddComponent<TouchInputModule>(); // obsolete

           // DontDestroyOnLoad(eventSystemGameObject);
        }

    }

    public static void PAUSE()
    {
        //Entity.PauseAll();
    }

    public static void RESUME()
    {
        //Entity.ResumeAll();
    }

    public static void SET_3D_AT_FRONT()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
    }

    public static void SET_CANVAS_AT_FRONT()
    {
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    public static void SET_CANVAS_VISIBILITY(bool visible)
    {
        canvas.gameObject.SetActive(visible);
    }
}
