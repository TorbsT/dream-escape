using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLand : MonoBehaviour
{
    private Rigidbody2D rb;
    private characterGround grounded;
    private float lastYV;
    private void Awake()
    {
        grounded = GetComponent<characterGround>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        lastYV = rb.velocityY;
    }
}
