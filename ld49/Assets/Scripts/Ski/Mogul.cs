using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mogul : MonoBehaviour
{
    public float force;
    public TMP_Text forceValue;
    public bool collided = false;

    Rigidbody2D mogulRB;

    void Start()
    {
        mogulRB = GetComponent<Rigidbody2D>();
        forceValue.text = force.ToString();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (collided)
            return;

        Skier skier;
        if (col.gameObject.TryGetComponent<Skier>(out skier))
        {
            skier.skierRB.AddForce(GetForce() * Vector2.right, ForceMode2D.Impulse);
            collided = true;
        }
    }

    public float GetForce()
    {
        return GameSettings.instance.MOGUL_FORCE_FACTOR * force;
    }
}
