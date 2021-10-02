using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private const float CAMERA_ABOVE_MAX_HEIGHT = 1.5f;
    private const float ZOOM_SPEED = 1.02f;
    private const float PAN_SPEED_FACTOR = 0.5f;
    private const float BASELINE_HEIGHT = -1f;

    public static CameraZoom instance;
    Camera mainCam;
    float zoomHeight = 9;

    private float initialSize;
    private float initialYOffset;

    private void Awake() {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;

        mainCam = Camera.main;
        initialSize = mainCam.orthographicSize;
        initialYOffset = mainCam.transform.position.y;
    }

    public void FixedUpdate() {
        if (mainCam.ScreenToWorldPoint(Vector3.up * Screen.height).y < CAMERA_ABOVE_MAX_HEIGHT * zoomHeight) {
            mainCam.orthographicSize *= ZOOM_SPEED;
        }

        var bottomHeight = mainCam.ScreenToWorldPoint(Vector3.zero).y;

        mainCam.transform.position = new Vector3(
            mainCam.transform.position.x,
            mainCam.transform.position.y - (bottomHeight - BASELINE_HEIGHT)/* * PAN_SPEED_FACTOR*/,
            mainCam.transform.position.z
        );
    }

    public void ProcessHeight(float height)
    {
        if (height > zoomHeight)
        {
            zoomHeight = height;
            //mainCam.orthographicSize = zoomHeight - initialYOffset;
        }
    }
}
