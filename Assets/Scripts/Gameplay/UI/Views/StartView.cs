using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//kzlukos@gmail.com
//
public class StartView : View {

    public Button startButton;
    public Button resetViewButton;
    public Dropdown languageDropdown;
    public InputField vaccineCounter;

    public int LanguageSelected
    {
        get { return languageDropdown.value; }
    }

    // 
    void Start()
    {
        vaccineCounter.text = TimeManager.Instance.Repetitions.ToString();
    }

    //
    public void ChangeCounterValue(int valueChange)
    {
        TimeManager.Instance.Repetitions += valueChange;
    }

    //
    public void OnRepetitionsNumberChangeHandler(int repetitions)
    {
        vaccineCounter.text = repetitions.ToString();
    }

}
