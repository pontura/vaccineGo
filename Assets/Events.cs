using UnityEngine;
using System;

public static class Events {

    public static Action<string> LoadScene = delegate { };
    public static Action SceneLoaded = delegate { };
    public static Action<string> OnKeyboardTitle = delegate {};
	public static Action<string> OnKeyboardText = delegate {};
	public static Action<string> OnKeyboardFieldEntered = delegate {};
}
