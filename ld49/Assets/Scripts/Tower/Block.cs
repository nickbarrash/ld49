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
    
    public const int REACTIVE_COLORS = 3;

    public readonly static Dictionary<BlockColors, Color> BLOCK_COLOR_MAP = new Dictionary<BlockColors, Color>() {
        {BlockColors.RED, Color.red},
        {BlockColors.BLUE, Color.blue},
        {BlockColors.YELLOW, Color.yellow},
        {BlockColors.INERT, Color.green}
    };

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    private BlockColors color;
    private bool spawned;

    public void Realize()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        rb.simulated = true;
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
