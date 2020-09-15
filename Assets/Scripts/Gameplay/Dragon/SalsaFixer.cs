using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrazyMinnow.SALSA;

// kzlukos@gmail.com
// Scales SALSA trigger thresholds proportionally to the AudioListener volume
public class SalsaFixer : MonoBehaviour {

    Salsa3D _salsaComponent;
    float _defaultSmallTrigger;
    float _defaultMediumTrigger;
    float _defaultLargeTrigger;
    float _prevAudioVolume = 0f;

    //
    void Start()
    {
        _salsaComponent = GetComponent<Salsa3D>();
        _defaultSmallTrigger = _salsaComponent.saySmallTrigger;
        _defaultMediumTrigger = _salsaComponent.sayMediumTrigger;
        _defaultLargeTrigger = _salsaComponent.sayLargeTrigger;
    }

    //
    void Update()
    {
        if(_prevAudioVolume != AudioListener.volume)
        {
            float factor = AudioListener.volume;
            if (AudioListener.volume < 0.01f)
                factor = 1f;

            _salsaComponent.saySmallTrigger = _defaultSmallTrigger * factor;
            _salsaComponent.sayMediumTrigger = _defaultMediumTrigger * factor;
            _salsaComponent.sayLargeTrigger = _defaultLargeTrigger * factor;
            _prevAudioVolume = AudioListener.volume;
        }
    }


}
