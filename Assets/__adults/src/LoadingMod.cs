using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using GEX.utils;
using UnityEngine.SceneManagement;

public class LoadingMod : MonoBehaviour
{
    private XTimer timer;

    private void Start()
    {
        timer = new XTimer();
        timer.setDelay(3f);
        timer.start();
    }


    private void Update()
    {
        if (timer != null)
        {
            if (timer.update())
            {

                SceneManager.LoadScene("MainScene5", LoadSceneMode.Single);

                timer = null;
            }
        }
    }

}
