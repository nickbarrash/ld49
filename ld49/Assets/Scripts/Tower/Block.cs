using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private const float SCORE_CHECK_INTERVAL = 0.4f;
    private const float POSITION_DIFFERENCE_THRESHOLD = 0.1f;
    private const int SCORE_PASS_INTERVALS = 2;
    public const float STATE_SCALE = 0.25f;
    
    public const int REACTIVE_COLORS = 3;

    public readonly static Dictionary<BlockColors, Color> BLOCK_COLOR_MAP = new Dictionary<BlockColors, Color>() {
        {BlockColors.RED, Color.red},
        {BlockColors.BLUE, new Color(0.1f, 0.1f, 1)},
        {BlockColors.YELLOW, Color.yellow},
        {BlockColors.INERT, Color.green}
    };

    List<SpriteRenderer> spriteRenderers;

    [HideInInspector]
    public Rigidbody2D rb;

    public BlockColors color;
    [HideInInspector]
    public bool spawned;
    [HideInInspector]
    public bool collided = false;
    [HideInInspector]
    public int spawnFrame = -1;
    
    // Score check
    private Vector2 position = new Vector2(float.MaxValue, float.MaxValue);
    private int checksPassed;
    private float maxHeight = float.MinValue;
    private float maxMovingHeight = float.MinValue;
    private bool incrementedScoreCount = false;
    private int gameCount = -1;
    private Color tmpColor;

    // state info
    public GameObject stateParent;
    public GameObject stableAffordance;
    public GameObject stabilizingAffordance;
    public GameObject unstableAffordance;

    public void Start() {
        SetRenderers();
        rb = GetComponent<Rigidbody2D>();
        SetState();
    }

    public virtual void SetRenderers()
    {
        spriteRenderers = new List<SpriteRenderer> {};

        SpriteRenderer blockRenderer;
        if(TryGetComponent(out blockRenderer))
            spriteRenderers.Add(blockRenderer);

        var shapes = transform.Find("Shapes");
        if (shapes != null)
        {
            spriteRenderers.AddRange(shapes.GetComponentsInChildren<SpriteRenderer>());
        }
    }

    public void SetState()
    {
        if (!spawned)
        {
            // hide
            stateParent.SetActive(false);
            return;
        }

        stableAffordance.SetActive(false);
        stabilizingAffordance.SetActive(false);
        unstableAffordance.SetActive(false);
        stateParent.SetActive(true);

        if (incrementedScoreCount)
        {
            stableAffordance.SetActive(true);
            return;
        }

        if (checksPassed == 0)
        {
            unstableAffordance.SetActive(true);
            return;
        }

        stabilizingAffordance.SetActive(true);
    }

    public bool IsStable()
    {
        return spawned && checksPassed >= SCORE_PASS_INTERVALS;
    }

    public void CheckScore()
    {
        if (!ScoreTracker.instance.gameInProgress || ScoreTracker.instance.gameCount != gameCount)
            return;

        if ((position - (Vector2)transform.position).magnitude < POSITION_DIFFERENCE_THRESHOLD)
        {
            checksPassed++;
            if (IsStable())
            {
                if (maxHeight < transform.position.y)
                {
                    maxHeight = transform.position.y;
                    ScoreTracker.instance.SetHeight(maxHeight);
                }

                if (!incrementedScoreCount)
                {
                    incrementedScoreCount = true;
                    AudioManager.instance.Play("lockin");
                    ScoreTracker.instance.SetCount();
                }
            }
        }
        else
        {
            checksPassed = 0;
        }
        position = transform.position;
        SetState();
    }

    public void GameOver()
    {
        CancelInvoke();
        SetColor(BlockClickSpawner.RandomColor());
    }

    public void Realize(int gameCount)
    {
        this.gameCount = gameCount;

        spawnFrame = Time.frameCount;
        spawned = true;

        SetColor(color, true);
        //spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        rb.simulated = true;
        InvokeRepeating("CheckScore", 0f, SCORE_CHECK_INTERVAL);
        SetState();
    }

    public void SetColor(BlockColors color, bool realized = true)
    {
        this.color = color;
        var baseColor = BLOCK_COLOR_MAP[color];
        if (!realized)
            baseColor = new Color (baseColor.r, baseColor.g, baseColor.b, 0.35f);

        SetColor(baseColor);
    }

    public virtual void SetColor(Color color)
    {
        foreach(var sr in spriteRenderers)
        {
            sr.color = color;
        }
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
        rb.AddForce(
            Settings.instance.BLOCK_COLLISION_FORCE_FACTOR
                //* transform.localScale.x
                //* transform.localScale.y
                * rb.mass
                * ((Vector2)transform.position - otherCenter).normalized,
            ForceMode2D.Impulse
        );

    }

    private void Update() {
        if (spawned && collided && (Time.frameCount - spawnFrame) % 10 == 0 && maxMovingHeight < transform.position.y)
        {
            maxMovingHeight = transform.position.y + Mathf.Max(transform.localScale.x, transform.localScale.y);
            CameraZoom.instance.ProcessHeight(maxMovingHeight);
        }
    }

    public void ProcessCollidedBlocks(Block other)
    {
        if (!other.spawned)
            return;

        collided = true;

        if (color != BlockColors.INERT && other.color == color)
        {
            Impulse(other.gameObject.transform.position, true);
            other.Impulse(transform.position, false);
            AudioManager.instance.Play("impulse");
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        Block otherBlock;
        if (spawned && collision.gameObject.TryGetComponent<Block>(out otherBlock))
        {
            ProcessCollidedBlocks(otherBlock);
        }
    }
}
