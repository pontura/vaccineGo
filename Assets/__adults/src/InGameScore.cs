using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InGameScore : MonoBehaviour
{
    public Text scoreText;
    public Image progressBar;
    public Text totalText;
    public Text outputText;

    private int target;
    private int SCORE = 0;
    private Vector3 originalScale;
    private Level currLevel;
    private int HOLE_INS = 0;
    private int FAILED_COUNT = 0;

    private static InGameScore _me = null;


    public static InGameScore me
    {
        get 
        { 
            if(_me == null)
                _me = GameObject.FindObjectOfType<InGameScore>();

            return _me;
        }
    }

    public void Setup(int target, Level level)
    {
        this.target = target;
        this.currLevel = level;

        Reset();
    }

    private void Start()
    { 
        originalScale = transform.localScale;
    }

    public void Show()
    {
        if (transform.gameObject.activeInHierarchy)
        {
            //do nothing
        }
        else
        {
            transform.localScale = new Vector3(0, 0, 0);
            transform.gameObject.SetActive(true);
            transform.DOScale(originalScale, 1f);
        }
    }

    public void Hide()
    {
        transform.DOScale(0f, 1f).OnComplete(OnHideComplete);
    }

    private void OnHideComplete()
    {
        transform.gameObject.SetActive(false);
    }

    public void HideNow()
    {
        transform.gameObject.SetActive(false);
    }

    public void HideInSeconds(float hideInSeconds)
    {
        transform.DOScale(originalScale + new Vector3(0.01f, 0.01f, 0.01f), hideInSeconds).OnComplete(HideInSecondsComplete); // hardcode
    }

    private void HideInSecondsComplete()
    {
        Hide();
    }

    public void Reset()
    {
        SCORE = 0;
        HOLE_INS = 0;
        FAILED_COUNT = 0;

        if (currLevel == null)
        {
            scoreText.gameObject.SetActive(true);
            totalText.gameObject.SetActive(false);
            progressBar.transform.parent.gameObject.SetActive(true);
            outputText.text = SCORE.ToString();
        }
        else if (currLevel.COUNT_MODE == CountMode.COUNT_SCORE)
        {
            scoreText.gameObject.SetActive(true);
            totalText.gameObject.SetActive(false);
            progressBar.transform.parent.gameObject.SetActive(true);
            outputText.text = SCORE.ToString();
            RefreshBar();
        }
        else if (currLevel.COUNT_MODE == CountMode.COUNT_HOLE_IN)
        {
            scoreText.gameObject.SetActive(false);
            totalText.gameObject.SetActive(true);
            progressBar.transform.parent.gameObject.SetActive(false);
            outputText.text = HOLE_INS.ToString();
        }
    }
    
    public bool AnyBallFailed()
    {
        return FAILED_COUNT > 0;
    }

    public void AddFailed()
    {
        FAILED_COUNT++; 
        //Debug.Log("FAIL");
    }

    public void AddScore(int amount)
    {
        SCORE += amount;
        HOLE_INS++;

        if (currLevel.COUNT_MODE == CountMode.COUNT_SCORE)
        {
            outputText.text = SCORE.ToString();
            RefreshBar();
        }
        else if (currLevel.COUNT_MODE == CountMode.COUNT_HOLE_IN)
        {
            outputText.text = HOLE_INS.ToString();
        }
    }

    private void RefreshBar()
    {
        progressBar.fillAmount = (float)SCORE / target;
    }

    public bool IsComplete()
    {
        if (currLevel.COUNT_MODE == CountMode.COUNT_SCORE)
            return (SCORE >= target);
        else if (currLevel.COUNT_MODE == CountMode.COUNT_HOLE_IN)
            return (HOLE_INS >= target);
        else
            return (SCORE >= target);
    }

    public int GetScore()
    {
        return SCORE;
    }

}
