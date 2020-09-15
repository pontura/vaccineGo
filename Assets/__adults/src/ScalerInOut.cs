using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

public class ScalerInOut : MonoBehaviour
{

    private bool destroyAtEnd = false;
    private GameObject savedObj;


    public static void Create(GameObject gobj, bool destroyAtEnd = true)
    {
        GameObject temp = new GameObject();
        temp.name = "ScalerInOut_FX";
        ScalerInOut s = temp.AddComponent<ScalerInOut>();
        s.Begin(gobj, destroyAtEnd);
    }

    public void Begin(GameObject gobj, bool destroyAtEnd = true)
    {
        this.destroyAtEnd = destroyAtEnd;
        Vector3 savedScale;

        savedObj = GameObject.Instantiate(gobj, gobj.transform.position, Quaternion.identity);
        savedScale = savedObj.transform.localScale;
        savedObj.transform.localScale = new Vector3(0, 0, 0);
        savedObj.SetActive(true);
        savedObj.transform.DOScale(savedScale, 0.5f).OnComplete(OnFadeInComplete);
    }

    private void OnFadeInComplete()
    {
        savedObj.transform.DOScale(0f, 0.5f).OnComplete(OnFadeOutComplete);
    }

    private void OnFadeOutComplete()
    {
        if (destroyAtEnd)
            GameObject.Destroy(savedObj);

        GameObject.Destroy(this.gameObject);
    }

}
