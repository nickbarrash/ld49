using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockImpulseAnimation : MonoBehaviour
{
    public const float ANIMATION_SPEED = 80f;

    public Block block;

    public Color impulseColor1;
    public Color impulseColor2;

    private float Offset => ((float)(block.spawnFrame % 100) / 20f);
    private float AnimValue => Mathf.Sin((Time.time + Offset) * ANIMATION_SPEED) / 2f + 1f;

    // Update is called once per frame
    void Update()
    {
        if (block.color == BlockColors.INERT && ScoreTracker.instance.gameInProgress)
        {
            block.SetColor(Color.Lerp(impulseColor1, impulseColor2, AnimValue));
        }
    }
}
