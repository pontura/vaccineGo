using UnityEngine;
using System.Collections;
using KeenResearch;
using UnityEngine.Android;
using GEX.utils;

// A rudimentary showcase of how to use KeenASR SDK from within Unity.
// Upon pressing Start Listening button the app will listen for several words defined in the phrases array below (feel free to augment the array to your liking)
// Results will only be shown on the consol (XCode or Android Studio logcat)

public class VoiceCommand : MonoBehaviour
{

    private XTimer holdTimer;
    private XTimer recordTimer;
    private float LISTEN_REPEAT_TIME = 4f;
    private bool INIT = false;

    //GUI.T
    // Use this for initialization
    void Start()
    {
        holdTimer = new XTimer();
        recordTimer = new XTimer();
        recordTimer.setDelay(LISTEN_REPEAT_TIME);
        recordTimer.start();

        if (Config.USE_VOICE_COMMANDS)
        {

            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
                //dialog = new GameObject();
            }



            // setup a few things before we initialize the SDK
            KeenASR.SetLogLevel(KeenASR.LogLevelInfo);
            KeenASR.onInitializedReceived += KeenASRInitialized;

            // init the SDK with the ASR bundle name
            Println("Keen: Initializing KeenASR Plugin");
            // Initialization works in a slghtly different manner on Android and iOS. On Android
            // it will be done asynchroniously, whereas on iOS it's done 
            KeenASR.Initialize("keenB2mQT-nnet3chain-en-us");
            KeenASR recognizer = KeenASR.Instance;

            // setup events with the instance of the recognizer
            recognizer.onFinalASRResultReceived += FinalASRResult;
            recognizer.onPartialASRResultReceived += PartialASRResult;
            recognizer.onRecognizerReadyToListenAfterInterruptReceived += KeenASRReadyToListenAfterInterrupt;
            recognizer.onUnwindAppAudioBeforeAudioInterruptReceived += UnwindAppAudio;
        }
    }

    // Update is called once per frame
    //void Update () {
    //	if (KeenASR.Instance!=null && KeenASR.Instance.GetRecognizerState()==KeenASR.RecognizerStateListening) {
    //		// here we just show how input levels change; you would poll the input levels somewhere else
    //		// and use it to drive a UI component (not print in the log file)
    //		//Println ("\tLevel: " + KeenASR.Instance.InputLevel ());
    //	}
    //}

    // After recognizer is initalized we can setup other resources (this could be done else where and in some cases
    // multiple decoding graphs may be setup and used independently
    public void KeenASRInitialized(bool status)
    {
        if (!status)
        {
            Println("KeenASR SDK was not initialized properly");
            return;
        }

        KeenASR recognizer = KeenASR.Instance;
        recognizer.SetCreateJSONMetadata(true);

        string dgName = "sampleDG";
        //		if (!recognizer.CustomDecodingGraphWithNameExists (dgName)) {
        Println("Keen: Creating decoding graph");
        string[] phrases = new string[] { "CHANGE LANGUAGE", "OK OCULUS", "EXTRACTION POINT", "EXTRACTION COMPLETE", "SELECT ADULTS", "SELECT KIDS", /*"OK DOLPHIN", */"YES", "NO", "MAYBE", "SURE", "HOW ARE YOU", "I AM GOOD", "I'M GOOD",
			"I DON'T FEEL GOOD", "I FEEL GOOD", "I AM OKAY", "I'M OKAY", "I AM ALRIGHT", "I'M ALRIGHT" };
        // we don't have to recreate the decoding graph every time; we can instead just
        // check for the existance of the graph with specific name. Note however, that with
        // the latter approach you WILL NEED to force recreation of the graph if you change
        // the list of input phrases
        recognizer.CreateCustomDecodingGraphFromSentences(dgName, phrases);
        //		} else {
        //			Println ("Keen: Decoding graph already exists");
        //		}
        //		if (recognizer.IsEchoCancellationAvaialable ()) {
        //			Println ("Echo cancellation is available, turining it on");
        //			recognizer.PerformEchoCancellation (true);
        //		} else {
        //			Println ("Echo cancellation is not available on this device");
        //		}

        // we now use this decoding graph for recognition. Multiple decoding graphs can
        // exist on the device and be switched back and forth
        recognizer.PrepareForListeningWithCustomDecodingGraph(dgName);

        // when set to true, SDK will create audio recordings capturing audio that was 
        // passed to the engine (between start and end listening)
        // you can get the file pat via GetLastRecordingFilename(), once the recognizer 
        // stopped listening (e.g. in onFinalASRResultReceived callback
        recognizer.SetCreateAudioRecordings(true);

        // VAD (Voice Activity Detection) is used to automatically stop listening
        // It can be changed at any time (e.g. slightly reduced in partial callbacks, based 
        // on semantic interpretation of the partial result)
        // final result will be reported after this many seconds end silence
        recognizer.SetVADParameter(KeenASR.VadParamTimeoutEndSilenceForGoodMatch, 1f);
        recognizer.SetVADParameter(KeenASR.VadParamTimeoutEndSilenceForAnyMatch, 1f);
        // also review  KeenASR.VadParamTimeoutForNoSpeech and KeenASR.VadParamTimeoutMaxDuration
        // which also control when stopListening kicks in automatically



        // leirbag4
        INIT = true;
    }

    public void FinalASRResult(ASRResult result)
    {
        Println("Keen FINAL RESULT:" + result.cleanText + ", conf: " + result.confidence + ", numWords: " + result.words.Length);

        string str = result.cleanText.ToLower();

        if (str == "OK OCULUS".ToLower())
        {
            MainGame.TOGGLE_SPECIAL_MODE();//LAUNCH_CMD_A();
        }
        else if (str == "EXTRACTION POINT".ToLower())
        {
            MainGame.JUMP_TO_SPECIAL_MODE();
        }
        else if (str == "EXTRACTION COMPLETE".ToLower())
        {
            MainGame.RETURN_TO_SIMPLE_MODE();
        }
        else if (str == "SELECT ADULTS".ToLower())
        {
            MainMenuPanel.me.SelectAdults();
        }
        else if (str == "SELECT KIDS".ToLower())
        {
            MainMenuPanel.me.SelectKids();
        }
        else if (str == "CHANGE LANGUAGE".ToLower())
        {
            MainGame.OPEN_LANGUAGE_SELECTION();
        }

        /*else if (str == "OK DOLPHIN".ToLower())
        {
            MainGame.JUMP_TO_SPECIAL_MODE();//LAUNCH_CMD_B();
        }*/

        foreach (ASRWord word in result.words)
        {
            if (word.isTag)
            {
                Println("Word " + word.text + " is a tag word");
            }
            if (word.confidence < 0.8)
                Println("Word " + word.text + " has LOW confidence");
        }
        KeenASR recognizer = KeenASR.Instance;
        if (recognizer != null)
            recognizer.ResetSpeakerAdaptation();
        // For testing/demo purposes only; it's unlikely you would need to call this method from within
        // the FinalASRResult callback
        //		Println("final callback recognizer state returns: " + KeenASR.Instance.GetRecognizerState());
        //Println("Audio file saved in: " + KeenASR.Instance.GetLastRecordingFilename());

    }

    public void PartialASRResult(string result)
    {
        Println("Keen PARTIAL RESULT:" + result);
        // For testing/demo purposes only; it's unlikely you would need to call this method from within
        // the FinalASRResult callback
        //		Println("partial callback, GetRecognizerState returns: " + KeenASR.Instance.GetRecognizerState());
    }

    public void UnwindAppAudio()
    {
        Println("Unwinding app audio");
    }

    public void KeenASRReadyToListenAfterInterrupt()
    {
        Println("App ready to listen again...");
        // TODO reanable UI elements, etc.
    }

    private void LAUNCH_CMD_A()
    {
        //GameObject.FindObjectOfType<Dolphin>().LaunchAnim("breath");
        MainGame.JUMP_TO_SPECIAL_MODE();
    }

    private void LAUNCH_CMD_B()
    {
        //GameObject.FindObjectOfType<Dolphin>().LaunchAnim("attack");
        MainGame.RETURN_TO_SIMPLE_MODE();
    }

    void Update()
    {

        //
        //  EXTRACTION MODE
        //
        if (Input.GetKeyDown(KeyCode.E) || OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
        {
            // holdTimer.setDelay(2f);

            //pontura hack para que salga del modo metralla instantaneamente:
            holdTimer.setDelay(0.1f);

            holdTimer.start();
        }
        else if (Input.GetKey(KeyCode.E) || OVRInput.GetUp(OVRInput.Button.One) )//OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
        {
            if (holdTimer.update())
            {
                holdTimer.stop();
                MainGame.TOGGLE_SPECIAL_MODE();
            }
        }
        //if (Input.GetKeyDown(KeyCode.E) ||  Pvr_ControllerManager.controllerlink.Controller0.Trigger.State)
        //    {
        //    // holdTimer.setDelay(2f);

        //    //pontura hack para que salga del modo metralla instantaneamente:
        //    holdTimer.setDelay(0.1f);

        //    holdTimer.start();
        //}
        //else if (Input.GetKey(KeyCode.E) ||  Pvr_ControllerManager.controllerlink.Controller0.App.State)
        //    {
        //    if (holdTimer.update())
        //    {
        //        holdTimer.stop();
        //        MainGame.TOGGLE_SPECIAL_MODE();
        //    }
        //}

        //
        //  LANGUAGE MODE
        //
        if (Input.GetKeyDown(KeyCode.Q) || (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)))
        {
            holdTimer.setDelay(2f);
            holdTimer.start();
        }
        else if (Input.GetKey(KeyCode.Q) || OVRInput.GetUp(OVRInput.Button.One) ) //(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && OVRInput.Get(OVRInput.Button.PrimaryTouchpad)))
        {
            if (holdTimer.update())
            {
                holdTimer.stop();
                MainGame.OPEN_LANGUAGE_SELECTION();
            }
        }
        //PiCO
        //if (Input.GetKeyDown(KeyCode.Q) || Pvr_ControllerManager.controllerlink.Controller0.Trigger.State)
        //{
        //    holdTimer.setDelay(2f);
        //    holdTimer.start();
        //}
        //else if (Input.GetKey(KeyCode.Q) || Pvr_ControllerManager.controllerlink.Controller0.App.State)
        //{
        //    if (holdTimer.update())
        //    {
        //        holdTimer.stop();
        //        MainGame.OPEN_LANGUAGE_SELECTION();
        //    }
        //}


#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.A))
        {
            LAUNCH_CMD_A();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            LAUNCH_CMD_B();
        }
           
        // ------------------------------------
        // OLD
        else if (Input.GetKeyDown(KeyCode.O))
        {
            MainGame.OLD_JUMP_TO_SPECIAL_MODE();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            MainGame.OLD_RETURN_TO_SIMPLE_MODE();
        }
        // ------------------------------------


#elif UNITY_IPHONE || UNITY_ANDROID
        /*if(OVRInput.GetDown(OVRInput.Button.Back))
        {
            Println("Start LISTENING!!!");
            KeenASR.Instance.StartListening();
        }*/

        /*if(OVRInput.GetDown(OVRInput.Button.Back))
        {
            toggle = !toggle;
            if(toggle)
                LAUNCH_CMD_A();
            else
                LAUNCH_CMD_B();
        }*/

        if(INIT)
        {
            if (recordTimer.update())
            {
                Println("Start LISTENING!!!");
                KeenASR.Instance.StartListening();
                recordTimer.setDelay(LISTEN_REPEAT_TIME);
                recordTimer.start();
            }
        }

#endif
    }

    private bool toggle = false;

    /*void OnGUI()
    {
        GUIStyle buttonStyle = new GUIStyle();
        buttonStyle.fontSize = 70;
        if (GUI.Button(new Rect(100, 450, 400, 100), "Start Listening", buttonStyle))
        {
#if UNITY_IPHONE || UNITY_ANDROID
            KeenASR.Instance.StartListening();
#endif
        }
    }*/

    private void Println(string str)
    {
        Debug.Log(str);
        MainCanvas.me.outp.text = str;
    }

    private void Awake()
    {

    }
}