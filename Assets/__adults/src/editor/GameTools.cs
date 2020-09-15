using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace editor
{
    class GameTools
    {
        [MenuItem("GAME TOOLS/Set APK keystore")]
        static void setAPKKeystore()
        {
            PlayerSettings.Android.keystorePass = "shad76tui937";
            PlayerSettings.Android.keyaliasPass = "shad76tui937";
        }

    }
}
