using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayDebugValues : MonoBehaviour
{
    public TMP_Text velocity;
    public TMP_Text lateralVelocity;

    public Rigidbody2D skier;

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity.text = skier.velocity.magnitude.ToString("F2");
        lateralVelocity.text = Vector2.Dot(skier.velocity, Vector2.right).ToString("F2");
    }
}
