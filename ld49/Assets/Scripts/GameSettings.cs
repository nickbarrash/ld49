using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;

    public float MOGUL_FORCE_FACTOR;
    public float MAX_SKI_VELOCITY;
    public float SKI_ACCELERATION;

    private void Start() {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }
}
