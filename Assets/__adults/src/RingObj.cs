using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class RingObj : MonoBehaviour
{

    public ColorType type { get { return _type; } }

    public GameObject ringA, ringB, ringC;
    public GameObject fx0;
    public ColliderActivator colliderIn, colliderOut;
    public Transform particleSpawnPos;
    public TextMeshPro amountText;
    public TextMeshPro scoreText;
    public GameObject errorIcon;
    public SoundPlayer SOUND;
    public GameObject glow;
    public TextMeshPro breathText;
    public GameObject breathImgFix;

    private bool fxDone = false;
    private GameObject currRing = null;
    private RingAnim anim;
    private float originalY;
    private bool introComplete = false;
    private float tempY = 0f;
    private float tempCount = 0f;
    private int goal = 5;
    private int holeIns = 0;
    private int score = 0;
    private bool complete = false;
    private ColorType _type = ColorType.A;
    private RingNumberMode numberMode;
    private bool breathMsgActive = false;

    private const float OFFSET_Y = 1f;
    private const float SIZE = 1.5f;

    public void Start()
    {
        //amountText.gameObject.SetActive(false);
    }

    public void Show(RingAnim ringAnim, int goal)
    {
        float offsetY = OFFSET_Y;

        this.anim = ringAnim;
        this.goal = goal;
        this.holeIns = 0;

        //amountText.text = goal.ToString();
        //amountText.gameObject.SetActive(true);

        if (numberMode == RingNumberMode.NOTHING)
        {
            amountText.gameObject.SetActive(false);
        }
        else if (numberMode == RingNumberMode.INCREMENTAL)
        {
            amountText.text = holeIns.ToString();
            amountText.gameObject.SetActive(false);
        }
        else if (numberMode == RingNumberMode.DECREMENTAL)
        {
            amountText.text = goal.ToString();
            amountText.gameObject.SetActive(true);
        }

        if (ringAnim == RingAnim.UpDown)
            offsetY = 2f;
        else if (ringAnim == RingAnim.NoneButMiddle)
            offsetY = 2f;
        else if (ringAnim == RingAnim.Special)
            offsetY = 10f;
        
        transform.localScale = new Vector3(0f, 0f, 0f);
        //transform.Translate(0, OFFSET_Y, 0);
        this.gameObject.SetActive(true);
        transform.DOScale(SIZE, 1.5f);
        transform.DOMoveY(transform.position.y + offsetY, 4.5f).SetEase(Ease.OutElastic).OnComplete(OnIntroAnimComplete);
    }

    private void OnIntroAnimComplete()
    {
        originalY = transform.position.y;
        introComplete = true;
    }

    public void Hide()
    {
        introComplete = false;

        transform.DOKill();
        
        transform.DOMoveY(transform.position.y - 4f, 1.4f).SetEase(Ease.InBack).OnComplete(OnHideComplete);
        //transform.DOScale(0f, 1.5f).OnComplete(OnHideComplete);
    }

    private void OnHideComplete()
    {
        Kill();
    }

    public void SetType(ColorType type)
    {
        _type = type;

        ringA.SetActive(false);
        ringB.SetActive(false);
        ringC.SetActive(false);

        if (type == ColorType.A)
            currRing = ringA;
        else if (type == ColorType.B)
            currRing = ringB;
        else if (type == ColorType.C)
            currRing = ringC;

        currRing.SetActive(true);
    }

    public void SetNumbersMode(RingNumberMode mode)
    {
        numberMode = mode;
    }

    /*public void SetAnim(RingAnim anim)
    {
        this.anim = anim;
    }*/

    private void UpdateUpDownAnim()
    {
        tempCount += Time.deltaTime * 2f;//0.01f;
        tempY = Mathf.Sin(tempCount) * 1f;

        transform.position = new Vector3(transform.position.x, originalY + tempY, transform.position.z);
    }

    private void UpdateUpDownRotateAnim()
    {
        tempCount += Time.deltaTime * 2f;//0.01f;
        tempY = Mathf.Sin(tempCount) * 1f;

        transform.position = new Vector3(transform.position.x, originalY + tempY, transform.position.z);
        transform.Rotate(0f, tempY * 5f, 0f);
    }

    /*public bool IsComplete()
    {
        return complete;
    }*/

    public void Kill()
    {
        GameObject.Destroy(this.gameObject);
    }

    public void ExternalKill()
    {
        complete = true;
        Hide();
    }

    public void GiveScore()
    {
        TextMeshPro txt = GameObject.Instantiate(scoreText, scoreText.transform.position, Quaternion.identity);
        txt.text = txt.text.Replace("TXT", "+ " + score);
        txt.gameObject.SetActive(true);

        //InGameScore.me.AddScore(score);
    }

    public void DoError()
    {
        /*GameObject err = GameObject.Instantiate(errorIcon, errorIcon.transform.position, Quaternion.identity);
        //err.transform.localScale = new Vector3(0, 0, 0);
        err.SetActive(true);*/

        ScalerInOut.Create(errorIcon);
    }

    public void SetGlowActive()
    {
        glow.SetActive(true);
    }

    
    public void SetGlowPreMessage(bool active)
    {
        breathMsgActive = active;

        if (breathMsgActive)
        {
            //if (Language.langCode.ToString() == "ar") // FIX for arabic lang

            //{
            //    breathImgFix.SetActive(true);
            //}
            //else
            //{
                breathText.text = LangRes.Get(LangRes.KEY_BREATH);
                breathText.gameObject.SetActive(true);
            //}
            amountText.gameObject.SetActive(false);
        }
        else
        {
            if (breathImgFix.activeInHierarchy)
                breathImgFix.SetActive(false);

            breathText.gameObject.SetActive(false);

            if ((numberMode == RingNumberMode.INCREMENTAL) || (numberMode == RingNumberMode.DECREMENTAL))
            {
                amountText.gameObject.SetActive(true);
            }
        }
    }
     

    public void Update()
    {
        int shootValue;
        if (introComplete)
        {
            if (anim == RingAnim.UpDown)
                UpdateUpDownAnim();
            else if (anim == RingAnim.Special)
                UpdateUpDownRotateAnim();
        }

        if (!complete)
        {
            if (colliderIn.triggered && colliderOut.triggered)
            {
                colliderIn.ResetTrigger();
                colliderOut.ResetTrigger();

                //
                // CORRECT
                //
                if (colliderOut.type == type)
                {
                    fxDone = true;
                    GameObject obj = GameObject.Instantiate(fx0, particleSpawnPos.transform.position, Quaternion.identity);
                    obj.SetActive(true);

                    shootValue = 10 * (int)type;
                    goal--;
                    holeIns++;
                    score += shootValue;

                    if (colliderIn.ball != null)
                        colliderIn.ball.SetAsHoledIn();

                    InGameScore.me.AddScore(shootValue);

                    if (numberMode == RingNumberMode.DECREMENTAL)
                    {
                        amountText.text = goal.ToString();
                    }
                    else if (numberMode == RingNumberMode.INCREMENTAL)
                    {
                        if ((holeIns > 0) && !breathMsgActive)
                            amountText.gameObject.SetActive(true);

                        amountText.text = holeIns.ToString();
                    }

                    if (goal <= 0)
                    {
                        complete = true;
                        GiveScore();
                        RingManager.me.AddRingAsComplete();
                        Hide();
                    }
                }
                //
                // INCORRECT
                //
                else
                {
                    DoError();
                }
            }
        }
    }

}
