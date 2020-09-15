using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

// kzlukos@gmail.com
// Scene changes management & Application setup
public partial class AppController : Singleton<AppController>
{
    public bool Standalone { get; set; }
    private bool _loading = false;

    //
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SetupApplication();

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        _loading = true;
        SceneManager.LoadSceneAsync(1);
    }

    //
    void SetupApplication()
    {
        UnityEngine.XR.XRSettings.LoadDeviceByName("cardboard");
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        StartCoroutine(PreventSleepModeCoroutine());
    }


    //
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log ("AppController : OnSceneLoaded [" + scene.name + "]");

        _loading = false;
        if (scene.buildIndex == 1)
        {
            SceneManager.SetActiveScene(scene);
        }
    }

    //
    void OnSceneUnloaded(Scene scene)
    {
        //Debug.Log ("AppController : OnSceneUnloaded [" + scene.name + "]");
    }

    //
    public void ChangeScene(int scene)
    {
        if (scene == 0)
        {
            Debug.LogError("AppController Error : NEVER EVER OPEN SCENE INIT(0)");
            return;
        }

        if (_loading)
            return;

        _loading = true;
        StartCoroutine(ChangeSceneCoroutine(scene));
    }

    //
    IEnumerator ChangeSceneCoroutine(int scene)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(scene);
    }


    // Prevents app in VR mode from sleeping
    IEnumerator PreventSleepModeCoroutine()
    {
        while (true)
        {
            Input.gyro.enabled = false;
            yield return new WaitForEndOfFrame();
            Input.gyro.enabled = true;
            yield return new WaitForSeconds(5f);
        }
    }

}
