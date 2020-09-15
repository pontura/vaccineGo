using UnityEngine;
using System;
using System.Collections.Generic;


public class MainGameLoader : MonoBehaviour
{


    public void Start()
    {
        if (GameObject.Find("MAIN_GAME_LOGIC") == null)
        {
            this.gameObject.AddComponent<MainGame>();
            this.gameObject.name = "MAIN_GAME_LOGIC";

            Destroy(this);//Destroy script
        }
        else
        {
            Destroy(this.gameObject);//destroy second mainGameObj
        }
    }



}
