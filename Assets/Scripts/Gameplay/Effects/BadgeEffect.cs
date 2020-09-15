using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// kzlukos@gmail.com
// Badge appear & fly effect (steps defined in BadgeEffectCoroutine)
public class BadgeEffect : GameStateObserver {

	//
	[Header("Chest related references")]
	[SerializeField]
	private GameObject magicDust;
	[SerializeField]
	private OpenChest chestOpeningScript;
	private ParticleSystem _magicDustParticles;

	//
	[Header("Badge related references")]
    [SerializeField]
	private ParticlesPuff leftBadge;
    [SerializeField]
	private ParticlesPuff rightBadge;
    [SerializeField]
    private GameObject travelingBadge;
	private Animator _travelingBadgeAnimator;
	private ParticlesPuff _travelingBadgePuff;

	//
	private Vector3 _defaultTravelingBadgePosition;
	private Quaternion _defaultTravelingBadgeRotation;
	private Vector3 _defaultMagicDustPosition;

    [SerializeField] private AudioSource audioSource;

    private bool _initialized = false;

    //
	void Start()
    {
		if (_initialized)
			return;

		_defaultMagicDustPosition = magicDust.transform.position;
		_magicDustParticles = magicDust.GetComponent<ParticleSystem> ();
		_defaultTravelingBadgePosition = travelingBadge.transform.position;
		_defaultTravelingBadgeRotation = travelingBadge.transform.rotation;
		_travelingBadgeAnimator = travelingBadge.GetComponentInChildren<Animator> ();
		_travelingBadgePuff = travelingBadge.GetComponentInChildren<ParticlesPuff> ();
		_initialized = true;
		Reset1();
    }
	//
	private void Reset1()
	{
		if (!_initialized)
			return;
		StopAllCoroutines();

		chestOpeningScript.Close();
		_magicDustParticles.Stop ();


		leftBadge.Show = false;
		leftBadge.Reset ();
		rightBadge.Show = false;
		rightBadge.Reset ();
		_travelingBadgePuff.Show = false;
		rightBadge.Reset ();

		_travelingBadgeAnimator.SetBool ("Closed", false);
		travelingBadge.transform.position = _defaultTravelingBadgePosition;
		travelingBadge.transform.rotation = _defaultTravelingBadgeRotation;
		magicDust.transform.position = _defaultMagicDustPosition;

	}
    //
    private void Reset()
    {
		if (!_initialized)
			return;
		StopAllCoroutines();

        chestOpeningScript.Close();
		_magicDustParticles.Stop ();


		leftBadge.Show = false;
		leftBadge.Reset ();
		rightBadge.Show = false;
		rightBadge.Reset ();
		_travelingBadgePuff.Show = false;
		rightBadge.Reset ();

		//iTween.Stop();
		
        _travelingBadgeAnimator.SetBool ("Closed", false);
		travelingBadge.transform.position = _defaultTravelingBadgePosition;
		travelingBadge.transform.rotation = _defaultTravelingBadgeRotation;
		magicDust.transform.position = _defaultMagicDustPosition;

    }

    //
    override protected void OnStateChangeHandler(GameState pref , GameState current)
    {
        if (current == GameState.Badge) {
			StartCoroutine(BadgeEffectCoroutine());
		} else if(current == GameState.Init)
        {
			Reset();
        }
    }


    //
    private IEnumerator BadgeEffectCoroutine()
    {
		
		travelingBadge.transform.rotation = _defaultTravelingBadgeRotation;
		travelingBadge.GetComponent<Floating> ().ResetTime ();

		TimeManager.Instance.Lock = true;
		_travelingBadgePuff.Show = true;

		// Magic dust appears
		magicDust.SetActive (true);
		_magicDustParticles.Play ();
		yield return new WaitForSeconds(0.1f);
		iTween.MoveTo(magicDust, iTween.Hash("path", iTweenPath.GetPath("KeyPath"), "time", 8f));
		yield return new WaitForSeconds(3.5f);
		_magicDustParticles.Stop ();
		yield return new WaitForSeconds(1f);


        // Chest opens
        chestOpeningScript.Open();
		yield return new WaitForSeconds(1.5f);


        // The badge appears and flies towarts the character
        travelingBadge.GetComponent<AudioSource>().Play();
        iTween.MoveTo(travelingBadge, iTween.Hash("path", iTweenPath.GetPath("BadgeShowPath"), "time", 9f));
		yield return new WaitForSeconds(3f);
		_travelingBadgeAnimator.SetBool ("Closed", true);
		yield return new WaitForSeconds(1.5f);
		_travelingBadgePuff.Show = false;
        //leftBadge.PlayParticles();
        //rightBadge.PlayParticles();

        audioSource.Play();
        yield return new WaitForSeconds(2f);

        Reset();
        chestOpeningScript.Open();
		leftBadge.Show = true;
		rightBadge.Show = true;


        TimeManager.Instance.Lock = false;
    }




}
