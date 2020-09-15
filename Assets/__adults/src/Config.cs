using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

public class Config
{

    //
    // Flow, Basics
    //
    //public static bool LANG_SELECT = false;
    public static bool MODE_SELECT = true;
    public static bool SHOW_INTRO = true;
    public static bool USE_INTRO_SECONDS = true;
    public static float INTRO_WAIT_SECONDS = 1f;
    public static float TIMES_UP = 20f;

    //
    // Others, Extras
    //
    public static bool USE_VOICE_COMMANDS = false;
    public static bool PARROT_TOGGLE_FLY = false;
    public static bool PARROT_INTRO_FLY_TO_LEFT = false;
    public static bool PARROT_MOVE_TO_BOARD_AFTER_WIN = false;
    public static bool AGE_MENU_VISIBLE = true;
    public static bool LANGUAGE_MENU_VISIBLE = false;
    public static bool VACCINES_MENU_VISIBLE = false;
    public static bool EXTRACTION_USE_FIREWORKS = true;
    public static bool EXTRACTION_USE_BREATH_AUDIO = true;

    public static bool SAME_MUSIC_ALL_APP = true;
    public static bool PARROT_MOVE_MOUTH_WHEN_TALK = false;


    public static void Initialize()
    {
        //
        // Language
        //
        
        //Language.Clear();
        
        //foreach (VoicesManager.AvailableLang l  in VoicesManager.Instance.availableLangs)
        //{
        //     if (l.lang == VoicesManager.languages.EN && l.available)
        //        Language.Add("en", "english");
        //    else if (l.lang == VoicesManager.languages.ES && l.available)
        //        Language.Add("es", "spanish");
        //    else if (l.lang == VoicesManager.languages.AR && l.available)
        //        Language.Add("ar", "arabic");
        //}

        //SetLang();

    }
    public static void SetLang()
    {

        //string lang = VoicesManager.Instance.lang.ToString().ToLower();

        //if (PersistentData.Instance.langSelected || PersistentData.Instance.GetTotalAvailableLangs() == 1)
        //{
        //    Debug.Log("langSelected: " + lang);
        //    Language.UseNativeLangCode(lang);
        //    Language.SetDefaultLangCode(lang);
        //    Language.ChangeLanguage(lang);
        //    Language.AutoSaveLastLanguage(true);
        //}
        //Language.ApplyChanges();


    }


}