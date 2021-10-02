using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSkier : MonoBehaviour
{
    Camera mainCam;
    public GameObject skier;

    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mainCam.transform.position = new Vector3 (skier.transform.position.x, skier.transform.position.y - 5, mainCam.transform.position.z);
    }
}
