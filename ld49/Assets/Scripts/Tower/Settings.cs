using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    public float BLOCK_COLLISION_FORCE_FACTOR;

    private void Start() {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }
}
