using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeInteractor : MonoBehaviour
{
    [SerializeField] private Vector2 slopeNormalPerp;
    [SerializeField] private float slopeDownAngle;
    [SerializeField] private Vector2 colliderSize;
    [SerializeField] private float slopeDownAngleOld;
    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private bool isOnSlope;
    [SerializeField] private LayerMask whatIsGround;

    private void Start()
    {
        colliderSize = GetComponent<Collider2D>().bounds.size;
    }
    private void FixedUpdate()
    {
        SlopeCheckVertical(transform.position -
            new Vector3(0f, colliderSize.y / 2f, 0f));
    }
    void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            checkPos, Vector2.down, slopeCheckDistance,
            whatIsGround);

        if (!hit) return;
        slopeNormalPerp = Vector2.Perpendicular(hit.normal);
        slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

        if (slopeDownAngle != slopeDownAngleOld)
            isOnSlope = true;

        slopeDownAngleOld = slopeDownAngle;

        Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
        Debug.DrawRay(hit.point, hit.normal, Color.green);
    }
}