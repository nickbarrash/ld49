using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class Score
{
    public string score;
    public string name;
}

[Serializable]
public class ScorePost
{
    public string type;
    public string score;
    public string name;
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
    const string URL = "https://rg57ptiugd.execute-api.us-east-2.amazonaws.com/leaderboard";

    // Start is called before the first frame update
    void Start()
    {
        Http.get(
            this,
            URL,
            (code, body) => {
                Debug.Log($"Leaderboard GET: {body}");

                //if (Utilities.isSuccessCode((int)code)) {
                //    onSuccess(JsonUtility.FromJson<ListUserLobbiesResponse>(body));
                //} else {
                //    // TODO: make failures more general
                //    onFailure(JsonUtility.FromJson<AwsFailureResponse>(body));
                //}
            },
            new Dictionary<string, string> {}
        );

        var testScore = new ScoreRequest {
            scores = new List<ScorePost> {
                new ScorePost {
                    name="nick",
                    type="height",
                    score="209.1"
                },
                new ScorePost {
                    name="nick",
                    type="blocks",
                    score="18"
                },
            }
        };
        Debug.Log(testScore.ResetType());

        Http.post(
            this,
            URL,
            testScore.ResetType(),
            (code, body) => {
                Debug.Log($"Leaderboard POST: {body}");
            }
        );
    }
}
