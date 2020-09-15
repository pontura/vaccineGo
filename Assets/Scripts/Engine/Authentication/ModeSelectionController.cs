using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

// kzlukos@gmail.com
// 
public class ModeSelectionController : MonoBehaviour {

    //
    public void RunAsServer()
    {
        AppController.Instance.Standalone = false;
        StartCoroutine(LoadConnectingSceneCoroutine(false));
    }

    //
    public void RunAsClient()
    {
        AppController.Instance.Standalone = false;
        StartCoroutine(LoadConnectingSceneCoroutine(true));
    }

    //
    public void RunStandalone()
    {
        AppController.Instance.Standalone = true;
        StartCoroutine(LoadConnectingSceneCoroutine(true));
    }

    //
    IEnumerator LoadConnectingSceneCoroutine(bool vrEnabled)
    {
        UnityEngine.XR.XRSettings.enabled = vrEnabled;
		yield return new WaitForEndOfFrame ();
        AppController.Instance.ChangeScene(3);
    }

}
