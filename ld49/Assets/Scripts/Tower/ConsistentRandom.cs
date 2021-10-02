using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsistentRandom : MonoBehaviour
{
    private const int RANDOM_SEED = 81;

    public void Awake() {
        Random.InitState(RANDOM_SEED);
    }

    public static float NextRandom()
    {
        var val = Random.value;
        //Debug.Log($"Random: {val}");
        return val;
    }
}
