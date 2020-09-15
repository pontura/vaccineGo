using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations;

//kzlukos@gmail.com
//Controls dragon behaviour according to the GameState
public partial class DragonBehaviour : GameStateObserver {

	//
	[Header("Energy transfer")]
	[SerializeField] private float requiredLookingTime = 3f;
	VRLookAtDragon _lookAtScript;

	public GameObject coloresParticles;

	//
    [Header("Transforms")]
    [SerializeField]
    private Transform startTransfom;
    [SerializeField]
    private Transform vaccineTransform;
    [SerializeField]
    private Transform followingTransform;
    [SerializeField]
    private Transform gazeFixedDistanceTransform;

    private Transform _targetTransform;

    override protected void OnStateChangeHandler(GameState prev, GameState current)
    {

		//print ("DragonBehaviour state___________ " + current);

        StopAllCoroutines();

		if (current == GameState.None) 
		{
			transform.position = _targetTransform.position;
			transform.rotation = _targetTransform.rotation;

			if (followingTransform != null) {
				_targetTransform = startTransfom;
				_targetTransform = followingTransform;
				iTween.MoveTo (_targetTransform.gameObject, iTween.Hash ("path", iTweenPath.GetPath ("Approach"), "time", 4));
			}
		}

        // Init
        if (current == GameState.Init) 
		{
			_targetTransform = startTransfom;
        }

        if (prev == GameState.Curtain)
        {
            if (Vector3.Distance(transform.position, _targetTransform.position) > 8f)
            {
                transform.position = _targetTransform.position;
                transform.rotation = _targetTransform.rotation;
            }
        }

        // Approach
        if (current == GameState.Approach)
        {
			if (followingTransform != null) {
				_targetTransform = followingTransform;
				iTween.MoveTo (_targetTransform.gameObject, iTween.Hash ("path", iTweenPath.GetPath ("Approach"), "time", 1.3));
			}
        }

		// Badge - Pause
		if (current == GameState.BadgePause || current == GameState.NewVaccinePause || current == GameState.LoopLargo1)
        {
			if (followingTransform != null) {
				_targetTransform = followingTransform;
				iTween.MoveTo (_targetTransform.gameObject, iTween.Hash ("path", iTweenPath.GetPath ("Evolutions"), "time", 22f));
			}
		}


        // Explanation
        if (current == GameState.Explanation)
        {
            StartCoroutine(ExplanationCoroutine(0f));
        }

        // Vaccine
		if (current == GameState.Vaccine || current == GameState.NewVaccineDone || current == GameState.NewVaccineDone2 || current == GameState.NewVaccineDone3 || current == GameState.NewVaccineDone4)
        {
            StartCoroutine(EnableEnergyTransfer());
        }
        else
        {
            _lookAtScript.enabled = false;
        }

		if (current == GameState.PausaCorta || current == GameState.LoopLargo_luces2 || current == GameState.RepeatLast || current == GameState.RepeatNewVaccine3) {
			//print (">>>>>>>>>> coloresParticles <<<<<<<<<< en " + current);

			coloresParticles.SetActive (true);
		} else {
			coloresParticles.SetActive (false);
		}
		// Medal/Repeat
        if (current == GameState.Repeat || current == GameState.Medal)
            _targetTransform = vaccineTransform;
        

		// Farewell
		if (current == GameState.Farewell)
		{
			StartCoroutine (FlyAwayCoroutine ());
		}




    }




    //
    private IEnumerator ExplanationCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        _targetTransform = vaccineTransform;
    }


    //
    private IEnumerator EnableEnergyTransfer()
    {
        //pontura: TimeManager.Instance.Lock = true;
        _lookAtScript.enabled = true;
        yield return new WaitUntil(() => requiredLookingTime < _lookAtScript.ContinouesLookingTime);
        _lookAtScript.enabled = false;
        TimeManager.Instance.Lock = false;
        
    }


	//
	private IEnumerator FlyAwayCoroutine() 
	{
		yield return new WaitForSeconds (6f);
		if (followingTransform != null) {
			iTween.MoveTo (followingTransform.gameObject, iTween.Hash ("path", iTweenPath.GetPath ("FlyAway"), "time", 20));
			_targetTransform = followingTransform;
		}

	}

}
