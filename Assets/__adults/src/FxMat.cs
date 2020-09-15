using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class FxMat : MonoBehaviour
{

    private GameObject _gobj;
    private Color col;
    private float toAlpha, time;
    //private float currAlpha;
    private string state = "";
    private Material mat;
    private Action onComplete;

    public static void FadeIn(Material mat, Color color, float fromAlpha, float toAlpha, float time, Action callbackOnComplete)
    {
        var fx = Create(mat, color, callbackOnComplete);
        fx._FadeIn(fromAlpha, toAlpha, time);
    }

    public static void FadeInBlack(Material mat, float toAlpha, float time, Action callbackOnComplete)
    {
        FadeIn(mat, Color.black, 0.0f, toAlpha, time, callbackOnComplete);
    }

    public static void FadeOut(Material mat, Color color, float fromAlpha, float toAlpha, float time, Action callbackOnComplete)
    {
        var fx = Create(mat, color, callbackOnComplete);
        fx._FadeOut(fromAlpha, toAlpha, time);
    }

    public static void FadeOutBlack(Material mat, float toAlpha, float time, Action callbackOnComplete)
    {
        FadeOut(mat, Color.black, 1.0f, toAlpha, time, callbackOnComplete);
    }


    private static FxMat Create(Material material, Color color, Action callbackOnComplete)
    {
        var g = new GameObject("FxMat");
        FxMat fx = g.AddComponent<FxMat>();
        fx.mat = material;
        fx.col = color;
        fx.mat.color = fx.col;
        fx.onComplete = callbackOnComplete;
        return fx;
    }


    private void _FadeIn(float fromAlpha, float toAlpha, float time)
    {
        this.toAlpha = toAlpha;
        this.time = time;
        SetAlpha(fromAlpha);

        this.state = "fade_in";
    }

    private void _FadeOut(float fromAlpha, float toAlpha, float time)
    {
        this.toAlpha = toAlpha;
        this.time = time;
        SetAlpha(fromAlpha);

        this.state = "fade_out";
    }

    private void Start()
    { 
        
    }

    private void Update()
    {
        bool completeDone = false;

        if (state == "fade_in")
        {
            if (mat.color.a < toAlpha)
                SetAlpha(mat.color.a + (time * Time.deltaTime));//mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a + (time * Time.deltaTime));
            else
                completeDone = true;
        }
        else if (state == "fade_out")
        {
            if (mat.color.a > toAlpha)
                SetAlpha(mat.color.a - (time * Time.deltaTime));
            else
                completeDone = true;
        }

        if(completeDone)
        {
            onComplete();
            GameObject.Destroy(this.gameObject);
        }
    }


    private void SetAlpha(float alpha)
    {
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, alpha);
    }

    /*public static GameObject FadeIn(float val, float duration, TweenCallback callback)
    {

        GameObject gob = new GameObject("fx");
        Tweener asd = gob.transform.DOMoveX(val, duration).OnComplete(callback);
        
        return gob;

    }*/

}
