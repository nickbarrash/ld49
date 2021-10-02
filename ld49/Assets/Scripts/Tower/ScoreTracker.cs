﻿using System.Collections;
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

    private void Start() {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;

        NewGame();
    }

    public void GameOver()
    {
        gameInProgress = false;
    }

    public void NewGame() {
        SetHeight(0f);

        blocks = 0;
        SetCount(false);
        gameInProgress = true;
    }

    public void SetHeight(float height)
    {
        this.height = Mathf.Max(this.height, height);
        heightValue.text = height.ToString("F2");
    }

    public void SetCount(bool increment = true)
    {
        if (increment)
            blocks++;

        countValue.text = blocks.ToString();
    }
}