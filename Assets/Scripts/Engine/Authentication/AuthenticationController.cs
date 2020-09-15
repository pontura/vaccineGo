using System.Collections;
using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// kzlukos@gmail.com
// 
public class AuthenticationController : MonoBehaviour {

    [SerializeField] InputField emailInput;
    [SerializeField] InputField passwordInput;
    [SerializeField] Text resultText;

    [SerializeField] string waitText = "Attempting to log in...";

    //
    private void Start()
    {

       

        if (PlayerPrefs.HasKey(PlayerPrefKeys.UserEmail))
        {
            emailInput.text = PlayerPrefs.GetString(PlayerPrefKeys.UserEmail);
        }
        if (PlayerPrefs.HasKey(PlayerPrefKeys.UserPassword))
        {
            passwordInput.text = PlayerPrefs.GetString(PlayerPrefKeys.UserPassword);
        }

    }

    //
    void InitializeFirebase()
    {
       // _auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    //
    public void SignIn()
    {
        resultText.text = waitText;
//		return;
//
//        _auth.SignInWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWith(task => {
//            if (task.IsCanceled)
//            {
//                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
//                return;
//            }
//            if (task.IsFaulted)
//            {
//                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
//                if (task.Exception.InnerExceptions.Count > 0)
//                    resultText.text = task.Exception.InnerExceptions[0].Message;
//                return;
//            }
//
//            Firebase.Auth.FirebaseUser newUser = task.Result;
//            Debug.LogFormat("User signed in successfully: {0} ({1})",
//                newUser.DisplayName, newUser.UserId);
//
//            StoreCreditionals();
//
//            AppController.Instance.ChangeScene(2);
//        });

    }

    //
    public void StoreCreditionals()
    {

        PlayerPrefs.SetString(PlayerPrefKeys.UserEmail, emailInput.text);
        PlayerPrefs.SetString(PlayerPrefKeys.UserPassword, passwordInput.text);

    }


}
