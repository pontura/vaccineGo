using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class OpenChest : MonoBehaviour {

    AudioSource _audioSource;

    [Range(0.0f, 1.0f)]
    public float factor;

    Quaternion closedAngle;
    Quaternion openedAngle;

    bool closing;
    bool opening;

    public float speed = 0.5f;

    int newAngle = 127;

    // Use this for initialization
    void Start () {
        _audioSource = GetComponent<AudioSource>();
        openedAngle = transform.localRotation;
		closedAngle = Quaternion.Euler(transform.localEulerAngles + Vector3.right * newAngle);
    }
	
	// Update is called once per frame
	void Update () {

        if (closing)
        {
            factor += speed * Time.deltaTime;

            if (factor > 1.0f)
            {
                factor = 1.0f;
            }
        }
        if (opening)
        {
            factor -= speed * Time.deltaTime;

            if (factor < 0.0f)
            {
                factor = 0.0f;
            }
        }

		transform.localRotation = Quaternion.Lerp(openedAngle, closedAngle, factor);
	}

    //You probably want to call this somewhere
    public void Close()
    {
        closing = true;
        opening = false;
    }

    public void Open()
    {
        if(!opening && factor > 0f)
            _audioSource.Play();
        opening = true;
        closing = false;
    }
}
