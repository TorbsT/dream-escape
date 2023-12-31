using System;
using UnityEngine;

//This script is used by both movement and jump to detect when the character is touching the ground

public class characterGround : MonoBehaviour
{

    public event Action<float> Landed;
    private bool onGround = true;
       
    [Header("Collider Settings")]
    [SerializeField][Tooltip("Length of the ground-checking collider")] private float groundLength = 0.95f;
    [SerializeField][Tooltip("Distance between the ground-checking colliders")] private Vector3 colliderOffset;

    [Header("Layer Masks")]
    [SerializeField][Tooltip("Which layers are read as the ground (as RED)")] private LayerMask groundLayersRED;
    [SerializeField][Tooltip("Which layers are read as the ground (as BLU)")] private LayerMask groundLayersBLU;

    private LayerMask groundLayers;
    private Rigidbody2D rb;
    private float prevVelY;
    private bool wasOnGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        //Determine if the player is stood on objects on the ground layer, using a pair of raycasts
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayers) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayers);
        float velY = rb.velocityY;
        if (onGround && !wasOnGround)
        {
            Landed?.Invoke(prevVelY);
        }
        prevVelY = velY;
        wasOnGround = onGround;
    }

    private void OnDrawGizmos()
    {
        //Draw the ground colliders on screen for debug purposes
        if (onGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }
    private void OnEnable() => Ability.Instance.Used += AbilityUsed;
    private void OnDisable() => Ability.Instance.Used -= AbilityUsed;
    private void AbilityUsed(bool RED, bool initial)
    {
        groundLayers = RED ? groundLayersRED : groundLayersBLU;
    }

    //Send ground detection to other scripts
    public bool GetOnGround() { return onGround; }
}