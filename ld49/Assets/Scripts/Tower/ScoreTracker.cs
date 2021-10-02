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

    public GameObject pbPanel;

    public TMP_Text pbHeightValue;
    public TMP_Text pbBlocksValue;
    public TMP_Text heightValue;
    public TMP_Text countValue;

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
        pbPanel.SetActive(false);
        NewGame();
    }

    public void GameOver()
    {
        gameInProgress = false;
        SaveScore();
    }

    public void SaveScore()
    {
        personalHighScoreBlocks = Mathf.Max(blocks, personalHighScoreBlocks);
        personalHighScoreHeight = Mathf.Max(height, personalHighScoreHeight);    

        if (personalHighScoreHeight > -1)
            pbPanel.SetActive(true);
        else
            pbPanel.SetActive(false);

        pbHeightValue.text = personalHighScoreHeight.ToString("F2");
        pbBlocksValue.text = personalHighScoreBlocks.ToString();
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
