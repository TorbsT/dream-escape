using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionIgnorer : MonoBehaviour
{
    public bool Ignore { get; set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IgnoreIfMust(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        IgnoreIfMust(collision);
    }
    private void IgnoreIfMust(Collision2D collision)
    {
    }
}
