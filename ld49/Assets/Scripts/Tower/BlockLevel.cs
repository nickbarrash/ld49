using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BlockLevel
{
    public List<GameObject> prefabs = new List<GameObject>();
    public string description;

    public BlockLevel(List<GameObject> prefabs, string description)
    {
        this.prefabs = prefabs;
        this.description = description;
    }

    public BlockLevel(GameObject prefab, string description)
    {
        prefabs.Add(prefab);
        this.description = description;
    }

    public abstract GameObject CreateBlock();
}
