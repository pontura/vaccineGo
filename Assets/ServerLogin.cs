using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ServerLogin : MonoBehaviour {

	public string usernameInserted;
	public string passwordInserted;
	public string dispositivo;
	public int daysWithnoInternet;
	public int dayWithNoInternet;
	public int totalDaysWithNoInternet;

	public bool CheckIfDeviceIsStillRegistered;
	public bool ResetDevice;
	public string url = "http://www.pontura.com/dermic/";
	public string deviceName;
	public int expiredYear;
	public int expiredMonth;
	public int expiredDay;

	public int nowYear;
	public int nowMonth;
	public int nowDay;

	bool hasInternet = false;

	JsonLoginData jsonLoginResult;

	[Serializable]
	public class JsonLoginData
	{
		public List<JsonLoginResult> result;
	}
	[Serializable]
	public class JsonLoginResult
	{
		public string id;
		public string total;
		public string nombre;
		public string password;
		public string licencia;
        public int app;
    }
	void Start()
	{
        SetDebbugText("Loading Voices");
        //	if(ResetDevice)
        //	PlayerPrefs.DeleteAll ();
        Events.OnKeyboardFieldEntered += OnKeyboardFieldEntered;
	}

	public void Init()
	{		
		print ("init");
		LoadDataSaved ();
		LoopForInternet ();
		SetDebbugText ("Loading data");
		Invoke ("Delayed", 3);
	}
	void LoadDataSaved()
	{
		dispositivo = PlayerPrefs.GetString ("dispositivo");
		deviceName = PlayerPrefs.GetString ("deviceName");
		expiredYear = PlayerPrefs.GetInt ("expiredYear", expiredYear);
		expiredMonth = PlayerPrefs.GetInt ("expiredMonth", expiredMonth);
		expiredDay = PlayerPrefs.GetInt ("expiredDay", expiredDay);
		daysWithnoInternet = PlayerPrefs.GetInt ("daysWithnoInternet", daysWithnoInternet);
	}
	void Delayed()
	{
        print("delayed" + dispositivo + " hasInternet:_ " + hasInternet);

        if (dispositivo != "")
		{
			if (!hasInternet) {
				if(PlayerPrefs.GetInt ("dayWithNoInternet", 0) != DateTime.Now.Day)
				{
					dayWithNoInternet = DateTime.Now.Day;
					PlayerPrefs.SetInt ("dayWithNoInternet", dayWithNoInternet);
					SetNewDayWithoutInternet ( daysWithnoInternet++ );
				}
				if (daysWithnoInternet >= totalDaysWithNoInternet) {
					SetDebbugText ("Please, connect your device to continue");
					CheckIfDeviceIsStillRegistered = true;
				} else {
					CancelInvoke ();	
					CheckExpirationValue ();
				}
			} else {
				SetNewDayWithoutInternet ( 0 );
				CancelInvoke ();
				StartCoroutine( CheckingIfDeviceIsStillRegistered () );
			}
		} else {		
			CancelInvoke ();	
			CheckExpirationValue ();
		}
	}
	void Connected()
	{
		if (CheckIfDeviceIsStillRegistered) {
			SetDebbugText ("Checking device...");
			StartCoroutine( CheckingIfDeviceIsStillRegistered () );
		}
	}
	void CheckExpirationValue()
	{
      
		expired = CheckExpiration ();

        print("CheckExpirationValue " + expired);
        if (expired)
        {
            SetDebbugText("Licence expired!");
            GotoLogin();
        }
        else if (expiredYear != 0)
        {
            if (!hasInternet)
                SetDebbugText("Not connected for " + daysWithnoInternet + " days");
            else
                SetDebbugText("Loading Settings...");
            GotoSettings();
        }
        else
        {
            SetDebbugText("Device not logged...");
            GotoLogin();
        }
	}

	public bool expired;
	bool CheckExpiration()
	{
		nowYear = DateTime.Now.Year;
		nowMonth = DateTime.Now.Month;
		nowDay = DateTime.Now.Day;

		LoadDataSaved ();

		if (deviceName.Length > 0) {
			if (expiredYear > nowYear)
				return false;
			else if (expiredYear >= nowYear && expiredMonth > nowMonth)
				return false;
			else if (expiredYear >= nowYear && expiredMonth >= nowMonth && expiredDay >= nowDay)
				return false;
			return true;
		}
		return false;
	}
	void OnKeyboardFieldEntered(string text)
	{
		CancelInvoke ();
		if (usernameInserted == "") {
			usernameInserted = text;
			LoadScene("001_Password", 0);
		} else if (passwordInserted == "") {
			passwordInserted = text;
			Login ();
		}			
	}
	void LoopForInternet()
	{
		Invoke ("LoopForInternet", 2);

		StartCoroutine (checkInternetConnection ());

		if(!hasInternet)
			SetDebbugText ("No internet connection, please connect your device and re-open the app");
		else
			SetDebbugText ("Internet connected!");

		if (hasInternet)
			Connected ();
	}
	void DeviceRegistered()
	{
		Events.OnKeyboardTitle( "Device registered: " + deviceName);
        GotoSettings();
	}
	public void Login()
	{
//		if (deviceName.Length > 0 && !expired ) {
//			SetDebbugText ("Device already registered: " + deviceName);
//		}
//		else 
		if (usernameInserted == "" || passwordInserted == "") {
			SetDebbugText ("Username or password incorrect");
			GotoLogin ();
		}
		else
			StartCoroutine(LoginDone());
	}
	IEnumerator LoginDone()
	{
       
		string post_url = url + "app_login.php" + "?nombre=" + WWW.EscapeURL(usernameInserted.ToLower()) + "&password=" + WWW.EscapeURL(passwordInserted.ToLower());
        Debug.Log("Do the Login......................" + post_url);
        SetDebbugText("Processing data");

		WWW hs_post = new WWW(post_url);
		yield return hs_post;
        Debug.Log("Do the Login...................... hs_post.error " + hs_post.error );
        if (hs_post.error != null)
		{
			SetDebbugText("No internet connection found. Please connect to the internet and try again.");
			GotoLogin ();
		}else
		{
			SetResult(hs_post.text); 
		}
	}
    void SetResult(string text)
    {
        Debug.Log("SetResult...................... text: " + text);
        jsonLoginResult = JsonUtility.FromJson<JsonLoginData>(text);

        if (jsonLoginResult == null || jsonLoginResult.result.Count == 0)
        {
            SetDebbugText("Login failed " + usernameInserted + " pass: " + passwordInserted);
            GotoLogin();
        }
        else if (jsonLoginResult.result[0].app != 0 && jsonLoginResult.result[0].app != 1)
        {
            SetDebbugText("Licence not available for this app");
            GotoLogin();
        }
        else if (deviceName.Length > 1)
        {
            SetLicencia(jsonLoginResult.result[0].licencia);
        }
        else
        {
            StartCoroutine(Register());
        }
    }
    void SetDebbugText(string text)
	{
		string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		CancelInvoke ();		

		Invoke ("ResetField", 5);

		//if (sceneName == "000_Oculus")
			Events.OnKeyboardText( text );
	}
	void ResetField()
	{
		Events.OnKeyboardText( "" );
	}
	string GetUniqueID()
	{
		int a = UnityEngine.Random.Range (1, 10000);
		int b = UnityEngine.Random.Range (1, 10000);
		int c = UnityEngine.Random.Range (1, 10000);
		return jsonLoginResult.result [0].nombre + "_" + a.ToString () + b.ToString () + c.ToString ();
	}
	IEnumerator Register()
	{
		dispositivo = GetUniqueID();
		PlayerPrefs.SetString ("dispositivo", dispositivo);
		string post_url = url + "app_register.php" + "?cliente_id=" + jsonLoginResult.result[0].id + "&password=" + passwordInserted + "&dispositivo=" + dispositivo ;
		Debug.Log (post_url);
		WWW hs_post = new WWW(post_url);
		yield return hs_post;

		if (hs_post.error != null)
		{
			SetDebbugText("No internet connection found.Please connect to the internet and try again.");

            GotoLogin ();
		}else if(hs_post.text == "error")
		{
			SetDebbugText("There was an error trying to register the device");
			GotoLogin ();
		}else if(hs_post.text == "full")
		{
			SetDebbugText("Too many devices (" + jsonLoginResult.result [0].total + ")");
			GotoLogin ();
		}else
		{
			SaveName(hs_post.text); 
		}
	}
	void SaveName(string _deviceName)
	{	
		deviceName = _deviceName;
		PlayerPrefs.SetString ("deviceName", deviceName);
		DeviceRegistered ();
		SetLicencia( jsonLoginResult.result [0].licencia );	
	}
	void SetLicencia(string licencia)
	{
		string[] dates = licencia.Split ("-" [0]);
		Events.OnKeyboardText("New Licence: " + licencia);

		if (dates.Length > 1) {
			expiredYear = int.Parse (dates [0]);
			expiredMonth = int.Parse (dates [1]);
			expiredDay = int.Parse (dates [2]);

			PlayerPrefs.SetInt ("expiredYear", expiredYear);
			PlayerPrefs.SetInt ("expiredMonth", expiredMonth);
			PlayerPrefs.SetInt ("expiredDay", expiredDay);
			CheckExpirationValue ();
		}
	}
	void GotoSettings()
	{
		CancelInvoke ();
		string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

		//if(sceneName == "000_Oculus")
			LoadScene ("Settings", 1);
	}
	void GotoLogin()
	{
        CancelInvoke();
#if UNITY_EDITOR
        if (PersistentData.Instance.DEBBUGER)
        {
            LoadScene("Settings", 1);
            return;
        }
 #endif         
            Invoke("Restart", 3);
	}
	void Restart()
	{
		usernameInserted = "";
		passwordInserted = "";
		LoadScene ("001_Password", 1);
	}
	void LoadScene(string sceneName, float delay)
	{
		CancelInvoke ();
		StopAllCoroutines ();
		StartCoroutine (LoadSceneCoroutine(sceneName, delay));
	}
	IEnumerator LoadSceneCoroutine(string sceneName, float delay)
	{		
		yield return new WaitForSeconds (delay);
		UnityEngine.SceneManagement.SceneManager.LoadScene (sceneName);
	}

	IEnumerator CheckingIfDeviceIsStillRegistered()
	{
		string post_url = url + "app_check_device.php?dispositivo=" + dispositivo;

		SetDebbugText("Checking if device is registered...");

		WWW hs_post = new WWW(post_url);
		yield return hs_post;

		if (hs_post.error != null)
		{
			SetDebbugText("No internet connection found. Please connect to the internet and try again.");
            GotoLogin ();
		} else if (hs_post.text == "ok")
		{
			CheckExpirationValue ();
		} else
		{
			SetDebbugText("Device not registered. ID:" + dispositivo + hs_post.text);
			PlayerPrefs.DeleteAll ();
			GotoLogin ();
		}
	}


	IEnumerator checkInternetConnection(){
		WWW www = new WWW("http://google.com");
		yield return www;
		if (www.error != null) {
			hasInternet = false;
		} else {
			hasInternet = true;;
		}
	} 
	void SetNewDayWithoutInternet(int value)
	{
		daysWithnoInternet = value;
		PlayerPrefs.SetInt ("daysWithnoInternet", daysWithnoInternet);
	}



}


