using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Score
{
    public float score;
    public string name;
}

[Serializable]
public class ScorePost
{
    public string type;
    public string score;//ts
    public string name;
    public string ts;   //score
}

[Serializable]
public class ScoreRequest
{
    public List<ScorePost> scores;

    public string ResetType()
    {
        var result = new StringBuilder();
        foreach(char c in Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(this))))
        {
            result.Append((char)(c + 1));
        }
        return result.ToString();
    }
}

[Serializable]
public class LeaderboardResponse
{
    public List<Score> blocks;
    public List<Score> height;
}

public class Leaderboard : MonoBehaviour
{
    public const int MAX_SCORES = 10;

    public static Leaderboard instance;

    public LeaderboardResponse leaderboard;

    // Leaderboards
    public GameObject heightBoard;
    public TMP_Text heightNames;
    public TMP_Text heightScores;

    public GameObject blocksBoard;
    public TMP_Text blocksNames;
    public TMP_Text blocksScores;

    // Submit
    public GameObject leaderboardSubmissionPanel;
    public TMP_InputField leaderboardName;
    public Button leaderboardSubmitButton;



    const string URL = "https://rg57ptiugd.execute-api.us-east-2.amazonaws.com/leaderboard";
    
    private void Awake() {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    public void Start() {
        RefreshLeaderboard();
        UpdateBoards();
    }

    public void RefreshLeaderboard()
    {
        GetLeaderboard();
    }

    public void DisplaySubmit()
    {
        leaderboardSubmissionPanel.SetActive(NewHighScore());
        UpdateSubmitButton();
    }

    public bool NewHighScore()
    {
        return NewHeightHighScore() || NewBlockHighScore();
    }

    public bool NewHeightHighScore()
    {
        if (leaderboard == null)
            return false;

        if (leaderboard.height.Count >= MAX_SCORES && leaderboard.height.Last().score > ScoreTracker.instance.height)
            return false;

        return true;
    }

    public bool NewBlockHighScore()
    {
        if (leaderboard == null)
            return false;
        
        if (leaderboard.blocks.Count >= MAX_SCORES && leaderboard.blocks.Last().score > ScoreTracker.instance.blocks)
            return false;

        if (leaderboard.blocks.Count > 0) Debug.Log($"Min Blocks: {leaderboard.blocks.Last()}");

        return true;
    }

    public bool ValidSubmission()
    {
        return leaderboardName.text.Length > 0;
    }

    public void UpdateBoards()
    {
        heightBoard.SetActive(leaderboard != null && leaderboard.height.Count > 0);
        blocksBoard.SetActive(leaderboard != null && leaderboard.blocks.Count > 0);

        if (leaderboard != null)
        {
            heightNames.text = string.Join("\n", leaderboard.height.Select(s => s.name));
            heightScores.text = string.Join("\n", leaderboard.height.Select(s => s.score));

            blocksNames.text = string.Join("\n", leaderboard.blocks.Select(s => s.name));
            blocksScores.text = string.Join("\n", leaderboard.blocks.Select(s => s.score));
        }
    }

    public void UpdateSubmitButton()
    {
        if (ValidSubmission())
            leaderboardSubmitButton.enabled = true;
    }

    public void Submit()
    {
        var scores = new List<ScorePost>();

        if (NewBlockHighScore())
        {
            scores.Add(new ScorePost(){
                type = "blocks",
                score = Time.time.ToString(),
                name = leaderboardName.text,
                ts = ScoreTracker.instance.blocks.ToString()
            });
        }

        if (NewHeightHighScore())
        {
            scores.Add(new ScorePost(){
                type = "height",
                score = Time.time.ToString(),
                name = leaderboardName.text,
                ts = ScoreTracker.instance.height.ToString()
            });
        }

        if(scores.Count > 0)
            PostLeaderboard(new ScoreRequest { scores=scores }.ResetType());

        leaderboardSubmissionPanel.SetActive(false);
    }

    public void GetLeaderboard()
    {
        try
        {
            Http.get(
                this,
                URL,
                (code, body) => {
                    Debug.Log($"Leaderboard GET: {body}");
                    leaderboard = JsonUtility.FromJson<LeaderboardResponse>(body);

                    leaderboard.height = leaderboard.height.OrderByDescending(s => s.score).Take(MAX_SCORES).ToList();
                    leaderboard.blocks = leaderboard.blocks.OrderByDescending(s => s.score).Take(MAX_SCORES).ToList();

                    UpdateBoards();
                },
                new Dictionary<string, string> {}
            );
        }
        catch(Exception e)
        {
            Debug.LogError("Failed getting leaderboard");
            Debug.LogError(e);
        }
    }

    public void PostLeaderboard(string payload)
    {
        try
        {
            Http.post(
                this,
                URL,
                payload,
                (code, body) => {
                    Debug.Log($"Leaderboard POST result: {body}");
                    RefreshLeaderboard();
                }
            );
        }
        catch(Exception e)
        {
            Debug.LogError($"Failed posting {payload} to leaderboard");
            Debug.LogError(e);
        }
    }

    //// Start is called before the first frame update
    //void Start()
    //{
    //    try
    //    {
    //        Http.get(
    //            this,
    //            URL,
    //            (code, body) => {
    //                Debug.Log($"Leaderboard GET: {body}");

    //                //if (Utilities.isSuccessCode((int)code)) {
    //                //    onSuccess(JsonUtility.FromJson<ListUserLobbiesResponse>(body));
    //                //} else {
    //                //    // TODO: make failures more general
    //                //    onFailure(JsonUtility.FromJson<AwsFailureResponse>(body));
    //                //}
    //            },
    //            new Dictionary<string, string> {}
    //        );
    //    }
    //    catch(Exception e)
    //    {
    //        Debug.LogError("Failed getting leaderboard");
    //        Debug.LogError(e);
    //    }

    //    var testScore = new ScoreRequest {
    //        scores = new List<ScorePost> {
    //            new ScorePost {
    //                name="nick",
    //                type="height",
    //                score="209.1"
    //            },
    //            new ScorePost {
    //                name="nick",
    //                type="blocks",
    //                score="18"
    //            },
    //        }
    //    };
    //    Debug.Log(testScore.ResetType());

    //    Http.post(
    //        this,
    //        URL,
    //        testScore.ResetType(),
    //        (code, body) => {
    //            Debug.Log($"Leaderboard POST: {body}");
    //        }
    //    );
    //}
}
