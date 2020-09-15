using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class LangRes
{
    public const int KEY_BREATH = 0;

    private static Dictionary<String, String> stringArr = new Dictionary<String, String>();
   static bool started;

    public static void Initialize()
    {
        if (started)
            return;
        started = true;

        Add(KEY_BREATH, "en", "BREATH");
        Add(KEY_BREATH, "es", "RESPIRA");
        Add(KEY_BREATH, "ar", "تنفس");
    }

    public static void Add(int key, String languageCode, String str)
    {
        String searchKey = (languageCode + "_" + key).ToLower();
        stringArr.Add(searchKey, str);
    }

    public static String Get(int key, String languageCode)
    {
        String searchKey = (languageCode + "_" + key).ToLower();
        return stringArr[searchKey];
    }

    public static String Get(int key)
    {
        return VoicesManager.Instance.lang.ToString().ToLower();// Get(key, Language.langCode.ToLower());
    }

}
