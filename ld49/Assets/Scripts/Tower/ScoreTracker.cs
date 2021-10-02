using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public static ScoreTracker instance;

    [HideInInspector]
    public int blocks = 0;
    [HideInInspector]
    public float height = 0;

    public TMP_Text heightValue;
    public TMP_Text countValue;

    public bool gameInProgress = false;
    public int gameCount = 0;

    private void Awake() {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;

        NewGame();
    }

    private void Start() {
        NewGame();
    }

    public void GameOver()
    {
        gameInProgress = false;
    }

    public void NewGame() {
        gameCount++;

        // set scoring;
        this.height = 0;
        SetHeight(0f);

        this.blocks = 0;
        SetCount(false);
        gameInProgress = true;
    }

    public void SetHeight(float height)
    {
        this.height = Mathf.Max(this.height, height);
        heightValue.text = this.height.ToString("F2");
    }

    public void SetCount(bool increment = true)
    {
        if (increment)
            blocks++;

        countValue.text = blocks.ToString();
    }
}
