using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockClickSpawner : MonoBehaviour
{
    public GameObject blockPrefab;

    private GameObject nextBlock;
    private GameObject tmpBlock;
    private int blocks = 0;

    // Start is called before the first frame update
    void Start()
    {
        nextBlock = CreateBlock(InputUtility.instance.MouseToWorldZeroed()); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            NextBlock();
        }

        nextBlock.transform.position = InputUtility.instance.MouseToWorldZeroed();
    }

    public BlockColors RandomColor()
    {
        return (BlockColors)Random.Range(0, Block.REACTIVE_COLORS);
    }

    public void NextBlock()
    {
        nextBlock.GetComponent<Block>().Realize();
        nextBlock = CreateBlock(InputUtility.instance.MouseToWorldZeroed());
    }

    public GameObject CreateBlock(Vector2 position)
    {
        tmpBlock = Instantiate(blockPrefab, transform);
        tmpBlock.name = $"Point_{blocks++}";
        tmpBlock.transform.position = position;
        tmpBlock.GetComponent<Block>().Start();
        tmpBlock.GetComponent<Block>().SetColor(RandomColor(), false);
        return tmpBlock;
    }
}
