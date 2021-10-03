using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockChildCollider : MonoBehaviour
{
    Block block;

    private void Awake() {
        block = GetComponentInParent<Block>();
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (!block.spawned)
            return;
    }
}
