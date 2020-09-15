using UnityEngine;
using System.Collections;

// kzlukos@gmail.com
// Camera movement in the editor
public class DEBUG_MoveCamera : MonoBehaviour
{
	#if UNITY_STANDALONE
    public float turnSpeed = 4.0f;      // Speed of camera turning when mouse moves in along an axis
    private Vector3 mouseOrigin;    // Position of cursor when mouse dragging starts
    private bool isRotating;    // Is the camera being rotated?
    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
		//if (CustomNetworkManager.Instance.NetworkMode == NetworkMode.Server)
		//	gameObject.SetActive (false);
    }

    void Update()
    {
        // Get the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // Get mouse origin
            mouseOrigin = Input.mousePosition;
            isRotating = true;
        }


        // Disable movements on button release
        if (!Input.GetMouseButton(0)) isRotating = false;

        // Rotate camera along X and Y axis
        if (isRotating)
        {
            Vector3 pos = _camera.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
            transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
        }
    }

	#endif
}