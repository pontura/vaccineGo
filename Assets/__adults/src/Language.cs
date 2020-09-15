using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

public class Language
{
  //  public static String nativeLangCode { get { return _nativeLangCode; } }
  //  public static String langCode { get { return _langCode; } }
  //  public static String language { get { return _languageList[selectedIndex]; } }
  // // public static String[] langCodeList { get { return _langCodeList.ToArray(); } }
  //  public static String[] languageList { get { return _languageList.ToArray(); } }

  //  private static String _langCode =   "";
  // // private static List<String> _langCodeList = new List<string>{  "en",        "es",       "ar" };
  ////  private static List<String> _languageList = new List<string> { "english",   "spanish",  "arab" };
  //  private static String _nativeLangCode = "";

  //  private static int selectedIndex = -1;
  // // private static String DEFAULT_LANG = "en"; // this language will be the default one
  //  private static bool _autoSaveLastLanguage = false;

  //  public static void ApplyChanges()
  //  {
  //      _langCode = DEFAULT_LANG;
  //      Refresh();

  //    //  if (_autoSaveLastLanguage)
  //       //   LoadLang();
  //  }

  //  public static void Clear()
  //  {
  //      _langCodeList = new List<string>();
  //      _languageList = new List<string>();
  //  }

  //  public static void SetDefaultLangCode(String defaultLangCode)
  //  {
  //      DEFAULT_LANG = defaultLangCode;
  //  }

  //  public static void UseNativeLangCode(String newNativeLangCode)
  //  {
  //      _nativeLangCode = newNativeLangCode;
  //  }

  //  public static void Add(String activeLangCode, String activeLanguage)
  //  {
  //      _langCodeList.Add(activeLangCode);
  //      _languageList.Add(activeLanguage);
  //  }

  //  public static void AutoSaveLastLanguage(bool active)
  //  {
  //      _autoSaveLastLanguage = active;
  //  }

  //  public static String ApplyConvention(String str)
  //  {
  //      return _langCode + "_" + str;
  //  }

  //  public static String ApplyConvention(String str, String newLangCode)
  //  {
  //      return newLangCode + "_" + str;
  //  }

  //  public static void ChangeLanguage(String newLangCode)
  //  {
        
  //      _langCode = newLangCode;
  //      Refresh();
  //  }

  //  public static void ChangeNextLanguage()
  //  {
        
  //      selectedIndex++;
  //      if (selectedIndex > (_langCodeList.Count - 1))
  //          selectedIndex = 0;
  //      _langCode = _langCodeList[selectedIndex];
  //      Refresh();
  //      SaveToSettings();
  //      Debug.Log("ChangeNextLanguage ____________ " + _langCode);
  //  }

  //  //pontura: graba en los settings generales:
  //  public static void SaveToSettings()
  //  {
  //      switch (_langCode)
  //      {
  //          case "en":
  //              VoicesManager.Instance.lang = VoicesManager.languages.EN;
  //              break;
  //          case "es":
  //              VoicesManager.Instance.lang = VoicesManager.languages.ES;
  //              break;
  //          case "ar":
  //              VoicesManager.Instance.lang = VoicesManager.languages.AR;
  //              break;
  //      }
  //  }


  //  public static void SaveLang()
  //  {
  //      SaveToSettings();
  //      Config.SetLang();
  //      if (_autoSaveLastLanguage)
  //      {
  //         // PlayerPrefs.SetString("x_selected_lang", _langCode);
  //      }
  //  }

  //  public static void LoadLang()
  //  {
  //      //if (PlayerPrefs.HasKey("x_selected_lang"))
  //      //{
  //      //    _langCode = PlayerPrefs.GetString("x_selected_lang");
  //      //    Refresh();
  //      //}
  //  }

  //  public static bool HasNativeLangCode()
  //  {
  //      return (_nativeLangCode != "");
  //  }

  //  private static void Refresh()
  //  {
  //      for (int a = 0; a < langCodeList.Length; a++)
  //      {
  //          if (langCodeList[a] == _langCode)
  //          {
  //              selectedIndex = a;
  //              return;
  //          }
  //      }
  //  }
}
