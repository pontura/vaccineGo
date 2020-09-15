using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownView : View {

    [SerializeField]
    public Text countdowntext;
    [SerializeField]
    public GameObject countdownPanel;

    //
    public void StartCountDown(int seconds)
    {
        StopCoroutine("CountdownCoroutine");
        StartCoroutine(CountdownCoroutine(seconds));
    }

    //
    public IEnumerator CountdownCoroutine(int seconds)
    {
        while(seconds >= 0) {

            countdownPanel.SetActive(seconds <= 5);

            if (seconds > 0) {
                countdowntext.text = seconds.ToString() + "...";
            } else
            {
                countdowntext.text = "NOW";
            }
            yield return new WaitForSeconds(1f);
            seconds--;
        }
    }

}
