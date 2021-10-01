using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputUtility: MonoBehaviour
{
    public static InputUtility instance;

    Camera mainCam;

    public void Start() {
        mainCam = Camera.main;

        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    public Vector3 MouseToWorldZeroed()
    {
        var mousePoint = MouseToWorld();
        return new Vector3(mousePoint.x, mousePoint.y, 0);
    }

    public Vector3 MouseToWorld()
    {
        return mainCam.ScreenToWorldPoint(Input.mousePosition);
    }
}
