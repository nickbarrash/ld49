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
        string description,
        List<GameObject> prefabs,
        float minWidth = 1,
        float maxWidth = 1,
        float minHeight = 1,
        float maxHeight = 1,
        BlockColors setColor = BlockColors.INERT
    ) : base(prefabs, description) {

        this.minWidth = minWidth;
        this.maxWidth = maxWidth;
        this.minHeight = minHeight;
        this.maxHeight = maxHeight;
        this.setColor = setColor;
    }

    public BlockLevelSimple(
        string description,
        GameObject prefab,
        float minWidth = 1,
        float maxWidth = 1,
        float minHeight = 1,
        float maxHeight = 1,
        BlockColors setColor = BlockColors.INERT
    ) : base(prefab, description) {

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
        var x = Mathf.Lerp(minWidth, maxWidth, ConsistentRandom.NextRandom());
        var y = Mathf.Lerp(minWidth, maxWidth, ConsistentRandom.NextRandom());
        tmpBlock.transform.localScale = new Vector3(x, y, 1);
        tmpBlock.transform.Find("States").transform.localScale = new Vector3(Block.STATE_SCALE * (1f / x), Block.STATE_SCALE * (1f / y), 1);
        tmpBlock.GetComponent<Block>().Start();

        var color = setColor != BlockColors.INERT ? setColor : BlockClickSpawner.RandomColor();
        tmpBlock.GetComponent<Block>().SetColor(color, false);
        return tmpBlock;
    }
}
