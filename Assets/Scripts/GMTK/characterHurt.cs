using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

//This script handles the character being killed and respawning

public class characterHurt : MonoBehaviour
{
    public static characterHurt Instance { get; private set; }
    [Header("Components")]
    [SerializeField] movementLimiter myLimit;
    [SerializeField] optionsManagement optionsScript;
    [SerializeField] AudioSource hurtSFX;
    private Coroutine flashRoutine;
    Rigidbody2D body;
    [SerializeField] public SpriteRenderer spriteRenderer;

    [Header("Settings")]
    [SerializeField] float respawnTime;
    [SerializeField] private float flashDuration;

    [Header("Events")]
    [SerializeField] public UnityEvent onHurt = new UnityEvent();

    [Header("Current State")]
    bool waiting = false;
    bool hurting = false;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If the player hits layer 3 (hazard), start the hurt routine
        if (collision.gameObject.layer == 3)
        {
            if (hurting == false)
            {
                //If it's spikes, stop the character's velocity
                if (collision.gameObject.layer == 3)
                {
                    body.velocity = Vector2.zero;
                }

                hurting = true;
                onHurt?.Invoke();
                gameObject.SetActive(false);
                //hurtRoutine();
            }
        }
    }

    public void hurtRoutine()
    {
        if (optionsScript.screenShake)
        {
            //The screenshake is played in a Unity Event, provided the option is turned on
            
        }


        //StartCoroutine(Wait(1.));
        //myAnim.SetTrigger("Hurt");
        Flash();

        //Start a timer, before respawning the player. This uses the (excellent) free Unity asset DOTween
        float timer = 0;
        DOTween.To(() => timer, x => timer = x, 1, respawnTime).OnComplete(respawnRoutine);
    }

    public void RestartAfter(float duration)
    {
        if (waiting)
            return;
        //Time.timeScale = timeScale;
        
    }

    IEnumerator Wait(float duration)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        waiting = false;
        Reset.Instance.Restart();
    }

    //These two functions handle the flashing white effect when Kit dies
    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Show the flash
        spriteRenderer.enabled = true;

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(flashDuration);

        // Hide the flash
        spriteRenderer.enabled = false;

        // Set the routine to null, signaling that it's finished.
        flashRoutine = null;
    }

    //After the timer ends, respawn Kit at the nearest checkpoint and let her move again
    private void respawnRoutine()
    {
        //transform.position = checkpointFlag;
        myLimit.characterCanMove = true;
        //myAnim.SetTrigger("Okay");
        hurting = false;
    }
}