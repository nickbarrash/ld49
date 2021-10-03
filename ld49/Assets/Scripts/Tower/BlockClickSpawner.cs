using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockClickSpawner : MonoBehaviour
{
    private const int BLOCKS_PER_LEVEL = 10;
    private const float ROTATE_SPEED = 1f;
    private const float MIN_MOUSE_Y = 0.5f;

    public static BlockClickSpawner instance;

    public GameObject blockPrefab;
    public List<GameObject> otherShapePrefabs;

    private GameObject nextBlock;
    [HideInInspector]
    public List<Block> blocks = new List<Block>();

    public List<BlockLevel> levels;

    public GameObject scorePanel;

    public TMP_Text levelLabel;
    public TMP_Text remainingBlocksLabel;
    public GameObject gameStartPanel;
    public GameObject leaderboard;

    public LevelDescriptionDisplay levelDescription;

    public Fader clickToPlaceInstruction;

    [HideInInspector]
    public bool placingFirstBlock = false;

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
            new BlockLevelSimple("Basic blocks", blockPrefab),
            new BlockLevelSimple("Funky blocks", blockPrefab, 0.3f, 2.5f, 0.3f, 2.5f),
            new BlockLevelSimple("Blue blocks", blockPrefab, 1, 1, 1, 1, BlockColors.BLUE),
            //new BlockLevelSimple("Funky shapes", otherShapePrefabs, 0.3f, 5, 0.3f, 5),
            new BlockLevelSimple("Funky shapes", otherShapePrefabs),
            new BlockLevelSimple("Respite", blockPrefab),
            new BlockLevelSimple("Blue funky shapes", otherShapePrefabs, 0.75f, 1.25f, 0.75f, 1.25f, BlockColors.BLUE),
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
        leaderboard.SetActive(true);
        scorePanel.SetActive(false);
    }

    public void NewGame()
    {
        scorePanel.SetActive(false);
        foreach(var block in blocks)
        {
            Destroy(block.gameObject);
        }

        blocks.Clear();

        if (nextBlock == null)
            NextBlock(false);

        SetPlacingFirstBlock(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreTracker.instance.gameInProgress && Input.GetMouseButtonDown(0)/* && InputUtility.instance.MouseToWorldZeroed().y > MIN_MOUSE_Y*/) {
            NextBlock();
        }

        if (Input.GetKey(KeyCode.W))
        {
            RotateNextBlock(true);
        } else if (Input.GetKey(KeyCode.Q)) {
            RotateNextBlock(false);
        }

        if (nextBlock != null)
        {
            nextBlock.transform.position = InputUtility.instance.MouseToWorldZeroed();
            //if (nextBlock.transform.position.y <= MIN_MOUSE_Y)
            //{
            //    nextBlock.transform.position = Vector3.down * 100f;
            //}
        }
    }

    public void RotateNextBlock(bool clockwise)
    {
        if (ScoreTracker.instance.gameInProgress && nextBlock != null )
            nextBlock.transform.Rotate(Vector3.forward, clockwise ? -1 * ROTATE_SPEED : ROTATE_SPEED);
    }

    public static BlockColors RandomColor()
    {
        return (BlockColors)(int)(ConsistentRandom.NextRandom() * Block.REACTIVE_COLORS);
    }

    public void NextBlock(bool realize = true)
    {
        if (realize)
        {
            var currentBlock = nextBlock.GetComponent<Block>();
            currentBlock.Realize(ScoreTracker.instance.gameCount);
            blocks.Add(currentBlock);

            if (blocks.Count % BLOCKS_PER_LEVEL == 0 || blocks.Count == 1)
            {
                if (blocks.Count != 1)
                    AudioManager.instance.Play("levelup");

                levelDescription.QueueDescription($"Level {GetLevel() + 1}: {levels[GetLevel()].description}");
            }

            AudioManager.instance.Play("spawn");
        }

        nextBlock = CreateBlock();
        nextBlock.transform.position = InputUtility.instance.MouseToWorldZeroed();

        UpdateUI();
        SetPlacingFirstBlock(false);
    }

    public GameObject CreateBlock()
    {
        return levels[GetLevel()].CreateBlock();
    }

    public int GetLevel() // index
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
        var blockCountLabel = remainingBlocks == -1 ? "∞" : (BLOCKS_PER_LEVEL - remainingBlocks).ToString();
        remainingBlocksLabel.text = $"(Next level in {blockCountLabel} blocks)";
    }

    public void SetPlacingFirstBlock(bool isPlacing)
    {
        if (placingFirstBlock && !isPlacing) {
            //    clickToPlaceInstruction.TriggerFade();
            scorePanel.SetActive(true);
            leaderboard.SetActive(false);
        }

        placingFirstBlock = isPlacing;
        gameStartPanel.SetActive(isPlacing);
    }
}
