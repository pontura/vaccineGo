using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// kzlukos@gmail.com
// Display connecting information
public class ConnectiongScreen : MonoBehaviour {

	[SerializeField]
	private Text text;
	[SerializeField]
	private GameObject logo;
	[SerializeField]
	private Camera menuCamera;

	private float _startTime;

	//
	void Start()
	{
		_startTime = Time.time;
	}

	//
	void Update()
	{
		//if (CustomNetworkManager.Instance.NetworkMode == NetworkMode.Client) 
		//{
  //          text.text = "Searching for server";
  //          float time = Time.time - _startTime;
  //          for (int i = -1; i < (int)time % 3; i++)
  //              text.text += ".";
		//} else 
		//{
  //          text.text = "Loading...";
  //      }

	}


}
