using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake() {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void Start() {
        NewGame();
    }

    public void NewGame()
    {
        ScoreTracker.instance.NewGame();
        BlockClickSpawner.instance.NewGame();
        GameOverUI.instance.SetVisible(false);
    }
}
