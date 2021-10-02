using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerGround : MonoBehaviour
{
    public BlockClickSpawner blockSpawner;

    public void GameOver()
    {
        ScoreTracker.instance.GameOver();
        blockSpawner.GameOver();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (ScoreTracker.instance.gameInProgress && collision.gameObject.TryGetComponent<Block>(out _))
        {
            GameOver();
        }
    }
}
