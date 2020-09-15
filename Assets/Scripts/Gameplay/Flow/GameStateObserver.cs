using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// kzlukos@gmail.com
// Parent class for simulation state observers
public abstract class GameStateObserver : MonoBehaviour {

    [SerializeField] bool listenOnServer = true;
    [SerializeField] bool listenOnClient = true;

    protected virtual void OnEnable()
    {
		GameController.Instance.OnStateChange += OnStateChangeHandler;
    }

    //
    protected virtual void OnDisable()
    {
        GameController.Instance.OnStateChange -= OnStateChangeHandler;
		CancelInvoke ();
		StopAllCoroutines ();
    }

    protected abstract void OnStateChangeHandler(GameState prev, GameState current);

}
