using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//kzlukos@gmai.com
//Tool class
[Serializable]
public class TimeSection 
{
	public float duration = 0f;
	public GameState state;
	public GameState nextState;
	public GameState fallbackState = GameState.None;
}
