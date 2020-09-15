using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//kzlukos@gmail.com
//Tool class
public class AudioClipCollection : MonoBehaviour
{
    [System.Serializable]
    public class Clip
    {
        public GameState state;
        public AudioClip clip;
    }

    public Clip[] audioClips;
}
