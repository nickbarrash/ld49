using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skier : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D skierRB;

    void Start()
    {
        skierRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (Mathf.Abs(skierRB.velocity.y) < GameSettings.instance.MAX_SKI_VELOCITY)
        {
            skierRB.AddForce(Vector2.down * GameSettings.instance.SKI_ACCELERATION, ForceMode2D.Force);
        }
        else
        {
            skierRB.velocity = new Vector3(skierRB.velocity.x, GameSettings.instance.MAX_SKI_VELOCITY * -1, 0);
        }
    }
}
