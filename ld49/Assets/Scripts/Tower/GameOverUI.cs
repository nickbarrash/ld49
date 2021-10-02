using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI instance;
    bool started = false;

    private void Awake() {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void Start() {
        if (!started)
        {
            SetVisible(false);
            started = true;
        }

    }

    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
        started = true;
    }
}
