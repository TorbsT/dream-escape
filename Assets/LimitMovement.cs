using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float ratio = 0.8f;
    [SerializeField] private bool ignoreIfPlayerTooHigh = true;
    private bool collidingWithPlayer = false;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collidingWithPlayer = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collidingWithPlayer = false;
        }
    }
    private void FixedUpdate()
    {
        if (!collidingWithPlayer ||
            (ignoreIfPlayerTooHigh && Feet.Instance.transform.position.y > transform.position.y))
        {
            rb.velocity = new(rb.velocity.x * ratio, rb.velocity.y);
        }
    }
}
