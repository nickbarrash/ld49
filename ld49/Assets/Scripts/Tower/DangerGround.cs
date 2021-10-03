using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
using UnityEngine.Analytics;
#endif

public class DangerGround : MonoBehaviour
{
    public void GameOver()
    {
        AudioManager.instance.Play("gameover");
        ScoreTracker.instance.GameOver();
        BlockClickSpawner.instance.GameOver();
        GameOverUI.instance.SetVisible(true);
        LevelDescriptionDisplay.instance.GameOver();

#if ENABLE_CLOUD_SERVICES_ANALYTICS
        Analytics.CustomEvent("gameOver", new Dictionary<string, object>
        {
            { "height", ScoreTracker.instance.height },
            { "blocks", ScoreTracker.instance.blocks }
        });
#endif
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (ScoreTracker.instance.gameInProgress && collision.gameObject.TryGetComponent<Block>(out _))
        {
            GameOver();
        }
    }
}
