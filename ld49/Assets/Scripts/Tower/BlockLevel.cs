using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BlockLevel
{
    public List<GameObject> prefabs = new List<GameObject>();

    public BlockLevel(List<GameObject> prefabs)
    {
        this.prefabs = prefabs;
    }

    public BlockLevel(GameObject prefab)
    {
        prefabs.Add(prefab);
    }

    public abstract GameObject CreateBlock();
}
