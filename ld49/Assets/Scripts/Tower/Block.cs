using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockColors
{
    RED,
    BLUE,
    YELLOW,
    INERT
}

public class Block : MonoBehaviour
{
    private const float INERT_TIME = 1f;
    private const float SCORE_CHECK_INTERVAL = 1.2f;
    private const float POSITION_DIFFERENCE_THRESHOLD = 0.1f;
    private const int SCORE_PASS_INTERVALS = 3;
    
    public const int REACTIVE_COLORS = 3;

    public readonly static Dictionary<BlockColors, Color> BLOCK_COLOR_MAP = new Dictionary<BlockColors, Color>() {
        {BlockColors.RED, Color.red},
        {BlockColors.BLUE, Color.blue},
        {BlockColors.YELLOW, Color.yellow},
        {BlockColors.INERT, Color.green}
    };

    SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;

    private BlockColors color;
    private bool spawned;
    
    // Score check
    private Vector2 position = new Vector2(float.MaxValue, float.MaxValue);
    private int checksPassed;
    private float maxHeight = float.MinValue;
    private bool incrementedScoreCount = false;
    private int gameCount = -1;

    public void CheckScore()
    {
        if (!ScoreTracker.instance.gameInProgress || ScoreTracker.instance.gameCount != gameCount)
            return;

        if ((position - (Vector2)transform.position).magnitude < POSITION_DIFFERENCE_THRESHOLD)
        {
            if (++checksPassed > SCORE_PASS_INTERVALS)
            {
                if (maxHeight < transform.position.y)
                {
                    maxHeight = transform.position.y;
                    ScoreTracker.instance.SetHeight(maxHeight);
                }

                if (!incrementedScoreCount)
                {
                    incrementedScoreCount = true;
                    ScoreTracker.instance.SetCount();
                }
            }
        }
        else
        {
            checksPassed = 0;
        }
        position = transform.position;
    }

    public void GameOver()
    {
        CancelInvoke();
    }

    public void Realize(int gameCount)
    {
        this.gameCount = gameCount;

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        rb.simulated = true;
        InvokeRepeating("CheckScore", 0f, SCORE_CHECK_INTERVAL);
    }

    public void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetColor(BlockColors color, bool realized = true)
    {
        this.color = color;
        var baseColor = BLOCK_COLOR_MAP[color];
        spriteRenderer.color = new Color (baseColor.r, baseColor.g, baseColor.b, realized ? 1f : 0.2f);
    }

    private void SetDelayedColor(BlockColors nextColor)
    {
        StartCoroutine(SetDelayedColorRoutine(nextColor));
    }

    private IEnumerator SetDelayedColorRoutine(BlockColors nextColor)
    {
        yield return new WaitForSeconds(INERT_TIME);

        SetColor(nextColor);
    }

    public BlockColors NextColor(bool forward)
    {
        var next = ((int)color + (forward ? 1 : -1)) % Block.REACTIVE_COLORS;
        if (next < 0)
            next += Block.REACTIVE_COLORS;
        return (BlockColors) next;
    }

    public void Impulse(Vector2 otherCenter, bool first)
    {
        SetDelayedColor(NextColor(first));
        SetColor(BlockColors.INERT);
        rb.AddForce(Settings.instance.BLOCK_COLLISION_FORCE_FACTOR * ((Vector2)transform.position - otherCenter).normalized, ForceMode2D.Impulse);

    }

    void OnCollisionStay2D(Collision2D collision) {
        Block otherBlock;
        if (color != BlockColors.INERT
            && collision.gameObject.TryGetComponent<Block>(out otherBlock)
            && otherBlock.color == color)
        {
            Impulse(collision.transform.position, true);
            otherBlock.Impulse(transform.position, false);
        }
    }
}
