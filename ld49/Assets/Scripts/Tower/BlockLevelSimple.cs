using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLevelSimple : BlockLevel
{
    private float minWidth;
    private float maxWidth;
    private float minHeight;
    private float maxHeight;

    private BlockColors setColor;

    public BlockLevelSimple(
        GameObject prefab,
        float minWidth = 1,
        float maxWidth = 1,
        float minHeight = 1,
        float maxHeight = 1,
        BlockColors setColor = BlockColors.INERT
    ) : base(prefab) {

        this.minWidth = minWidth;
        this.maxWidth = maxWidth;
        this.minHeight = minHeight;
        this.maxHeight = maxHeight;
        this.setColor = setColor;
    }

    private GameObject GetPrefab()
    {
        return prefabs[(int)(ConsistentRandom.NextRandom() * prefabs.Count)];
    }

    public override GameObject CreateBlock() {
        var tmpBlock = BlockClickSpawner.Instantiate(GetPrefab(), BlockClickSpawner.instance.transform);
        tmpBlock.name = $"Block_{BlockClickSpawner.instance.blocks.Count}";
        tmpBlock.transform.localScale = new Vector3(
            Mathf.Lerp(minWidth, maxWidth, ConsistentRandom.NextRandom()),
            Mathf.Lerp(minHeight, maxHeight, ConsistentRandom.NextRandom()),
            1
        );
        tmpBlock.GetComponent<Block>().Start();

        var color = setColor != BlockColors.INERT ? setColor : BlockClickSpawner.RandomColor();
        tmpBlock.GetComponent<Block>().SetColor(color, false);
        return tmpBlock;
    }
}
