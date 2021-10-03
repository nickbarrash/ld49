using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public const float HEIGHT_ADJUST_FACTOR = 10;

    public static ScoreTracker instance;

    [HideInInspector]
    public int blocks = 0;
    [HideInInspector]
    public float height = 0;

    //public GameObject pbPanel;

    // game over
    public TMP_Text pbHeightValue;
    public TMP_Text pbBlocksValue;
    public TMP_Text heightGameOverValue;
    public TMP_Text countGameOverValue;

    // playing
    public TMP_Text heightValue;
    public TMP_Text countValue;
    public TMP_Text levelValue;

    public bool gameInProgress = false;
    public int gameCount = 0;

    public int personalHighScoreBlocks = -1;
    public float personalHighScoreHeight = -1f;

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
        //pbPanel.SetActive(false);
        NewGame();
    }

    public void GameOver()
    {
        gameInProgress = false;
        SaveScore();
    }

    public void SaveScore()
    {
        countGameOverValue.text = blocks.ToString();
        heightGameOverValue.text = (height * HEIGHT_ADJUST_FACTOR).ToString("F1") + "m";

        personalHighScoreBlocks = Mathf.Max(blocks, personalHighScoreBlocks);
        personalHighScoreHeight = Mathf.Max(height, personalHighScoreHeight);    

        pbHeightValue.text = $"(Personal Best: {(personalHighScoreHeight * HEIGHT_ADJUST_FACTOR).ToString("F1")}m)";
        pbBlocksValue.text = $"(Personal Best: {personalHighScoreBlocks.ToString()})";
        levelValue.text = (BlockClickSpawner.instance.GetLevel() + 1).ToString();

        Leaderboard.instance.DisplaySubmit();
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
        heightValue.text = (this.height * HEIGHT_ADJUST_FACTOR).ToString("F1") + "m";
    }

    public void SetCount(bool increment = true)
    {
        if (increment)
            blocks++;

        countValue.text = blocks.ToString();
    }
}
