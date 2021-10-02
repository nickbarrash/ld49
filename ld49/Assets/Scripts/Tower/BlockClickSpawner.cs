using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockClickSpawner : MonoBehaviour
{
    public static BlockClickSpawner instance;

    public GameObject blockPrefab;

    private GameObject nextBlock;
    private GameObject tmpBlock;
    private List<Block> blocks = new List<Block>();

    private void Awake() {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void Start() {
        NewGame();
    }

    public void GameOver()
    {
        foreach(var block in blocks)
        {
            block.GameOver();
            block.rb.simulated = false;
        }

        Destroy(nextBlock.gameObject);
        nextBlock = null;
    }

    public void NewGame()
    {
        foreach(var block in blocks)
        {
            Destroy(block.gameObject);
        }

        blocks.Clear();

        if (nextBlock == null)
            nextBlock = CreateBlock(InputUtility.instance.MouseToWorldZeroed());
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreTracker.instance.gameInProgress && Input.GetMouseButtonDown(0)) {
            NextBlock();
        }

        if (nextBlock != null)
        {
            nextBlock.transform.position = InputUtility.instance.MouseToWorldZeroed();
        }
    }

    public BlockColors RandomColor()
    {
        return (BlockColors)Random.Range(0, Block.REACTIVE_COLORS);
    }

    public void NextBlock()
    {
        var currentBlock = nextBlock.GetComponent<Block>();
        currentBlock.Realize(ScoreTracker.instance.gameCount);
        blocks.Add(currentBlock);

        nextBlock = CreateBlock(InputUtility.instance.MouseToWorldZeroed());
    }

    public GameObject CreateBlock(Vector2 position)
    {
        tmpBlock = Instantiate(blockPrefab, transform);
        tmpBlock.name = $"Point_{blocks.Count}";
        tmpBlock.transform.position = position;
        tmpBlock.GetComponent<Block>().Start();
        tmpBlock.GetComponent<Block>().SetColor(RandomColor(), false);
        return tmpBlock;
    }
}
