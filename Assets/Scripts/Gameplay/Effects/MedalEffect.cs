using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//kzlukos@gmail.com
//GameState observer object reponsible for controlling medal position and related special effects
public class MedalEffect : GameStateObserver {

	[SerializeField] GameObject medalObject;
    [SerializeField] ParticlesPuff medalParticles;
    [SerializeField] AudioSource appearAudio;
    [SerializeField] AudioSource disappearAudio;

    private Animator _animator;
	private Vector3 _defaultPosition;
	private Quaternion _defaultRotation;
	private bool _initialized = false;


	//
	void Start() 
	{
        _animator = medalObject.GetComponent<Animator>();
        _defaultPosition = medalObject.transform.position;
		_defaultRotation = medalObject.transform.rotation;
		//Reset ();
        _initialized = true;
    }

	//
	private void Reset() 
	{
		if (!_initialized)
			return;

		StopAllCoroutines ();
		//if(iTween.
      //  iTween.Stop();
		medalObject.transform.position = _defaultPosition;
		medalObject.transform.rotation = _defaultRotation;
        medalParticles.Show = false;
        _animator.SetBool("Moving", false);
    }

	//
	protected override void OnStateChangeHandler (GameState prev, GameState current)
	{
		if (current == GameState.Conclusion) 
		{
			StartCoroutine (MedalCoroutine());
		} else if(current == GameState.Init)
		{
			Reset ();
		}
	}

	//
	private IEnumerator MedalCoroutine() 
	{
		TimeManager.Instance.Lock = true;

        medalParticles.Show = true;
        appearAudio.Play();

        _animator.SetBool("Moving", true);
        yield return new WaitForSeconds (1f);
		//iTween.MoveTo(medalObject.gameObject, iTween.Hash("path", iTweenPath.GetPath("Medal"), "time", 6f));
		yield return new WaitForSeconds (7f);
        medalParticles.Show = false;
        disappearAudio.Play();
        yield return new WaitForSeconds(2f);
        TimeManager.Instance.Lock = false;
	}

}
