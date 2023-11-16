using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsInteractionEffects : MonoBehaviour
{
    [SerializeField] private string landSound = "footstep";
    [SerializeField] private float landMaxVel = 37f;
    [SerializeField] private string slideSound = "stoneslide";
    [SerializeField] private float slideMaxVel = 1f;

    [SerializeField] private AudioSource slideSource;
    private Rigidbody2D rb;
    private characterGround grounded;
    private void Awake()
    {
        grounded = GetComponent<characterGround>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        grounded.Landed += Landed;
    }
    private void OnDisable()
    {
        grounded.Landed -= Landed;
    }
    private void FixedUpdate()
    {
        if (slideSource == null) return;
        float slideVolume = Mathf.Abs(rb.velocityX) / slideMaxVel;
        slideSource.volume = slideVolume;
    }
    private void Landed(float velY)
    {
        AudioManager.Instance.PlayRandom(landSound, Mathf.Abs(velY) / landMaxVel);
    }
}
