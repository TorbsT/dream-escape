using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;

    private characterMovement movement;
    private characterJump jump;

    [SerializeField] private Transform head;
    [SerializeField] private float jumpVelocityBase = 1f;
    [SerializeField] private float jumpVelocityScalar = 1f;
    private Vector2 baseHeadPos;
    private Rigidbody2D rb;
    private Rigidbody2D headRb;
    private bool awaitHeadBounce;
    private float timeOut;

    private void Awake()
    {
        baseHeadPos = head.localPosition;
        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<characterMovement>();
        rb = GetComponent<Rigidbody2D>();
        headRb = head.GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        jump = GetComponent<characterJump>();
        jump.Jumped += Jumped;
    }
    private void OnDisable()
    {
        jump.Jumped -= Jumped;
    }

    private void Update()
    {
        bool move = Mathf.Abs(movement.velocity.x) > 0.01f;
        float walk = movement.directionX;
        animator.SetBool("move", move);

        head.localPosition = new(0f, Mathf.Max(head.localPosition.y, baseHeadPos.y));
    }
    private void FixedUpdate()
    {
        float y = head.transform.localPosition.y - baseHeadPos.y;
        if (y <= 0f)
        {
            headRb.velocityY = Mathf.Max(0f, headRb.velocityY);
        }
        if (y >= 0.1f && Mathf.Abs(headRb.velocityY) <= 0.01f)
        {
            timeOut += Time.fixedDeltaTime;
            if (timeOut >= 1f)
            {
                head.localPosition = baseHeadPos;
                timeOut = 0f;
            }
        }
        else timeOut = 0f;
        Bounce();
    }
    private void Jumped()
    {
        awaitHeadBounce = true;
    }
    private void Bounce()
    {
        if (!awaitHeadBounce) return;
        float y = head.localPosition.y - baseHeadPos.y;
        if (y > 0.01f) return;
        // Do a bounce
        headRb.velocityY += jumpVelocityScalar*rb.velocityY + jumpVelocityBase;
        awaitHeadBounce = false;
    }
}
