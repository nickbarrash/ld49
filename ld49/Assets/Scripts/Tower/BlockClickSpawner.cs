using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockClickSpawner : MonoBehaviour
{
    private const int BLOCKS_PER_LEVEL = 10;
    private const int RANDOM_SEED = 57; //55

    public static BlockClickSpawner instance;

    public GameObject blockPrefab;

    private GameObject nextBlock;
    public List<Block> blocks = new List<Block>();

    public List<BlockLevel> levels;

    public TMP_Text levelLabel;
    public TMP_Text remainingBlocksLabel;


    private void Awake() {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;

        InitLevels();
    }

    private void InitLevels()
    {
        levels = new List<BlockLevel> {
            new BlockLevelSimple(blockPrefab),
            new BlockLevelSimple(blockPrefab, 0.5f, 2, 0.5f, 2),
            new BlockLevelSimple(blockPrefab, 1, 1, 1, 1, BlockColors.BLUE),
            new BlockLevelSimple(blockPrefab, 0.2f, 5, 0.2f, 5),
        };
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

        Random.InitState(RANDOM_SEED);

        blocks.Clear();

        if (nextBlock == null)
            NextBlock(false);
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

    public static BlockColors RandomColor()
    {
        return (BlockColors)(int)(UnityEngine.Random.value * Block.REACTIVE_COLORS);
    }

    public void NextBlock(bool realize = true)
    {
        if (realize)
        {
            var currentBlock = nextBlock.GetComponent<Block>();
            currentBlock.Realize(ScoreTracker.instance.gameCount);
            blocks.Add(currentBlock);
        }

        nextBlock = CreateBlock();
        nextBlock.transform.position = InputUtility.instance.MouseToWorldZeroed();

        UpdateUI();
    }

    public GameObject CreateBlock()
    {
        return levels[GetLevel()].CreateBlock();
    }

    private int GetLevel() // index
    {
        return Mathf.Min(blocks.Count / BLOCKS_PER_LEVEL, levels.Count - 1);
    }

    private int GetBlocksRemaining()
    {
        if (GetLevel() + 1>= levels.Count)
            return -1;

        return blocks.Count % BLOCKS_PER_LEVEL;
    }

    public void UpdateUI()
    {
        levelLabel.text = (GetLevel() + 1).ToString();

        var remainingBlocks = GetBlocksRemaining();
        remainingBlocksLabel.text = remainingBlocks == -1 ? "∞" : remainingBlocks.ToString();
    }
}
