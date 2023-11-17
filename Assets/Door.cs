using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform playerGoingTo;
    [SerializeField] private string leadsTo = "MainMenu";
    [SerializeField] private float timeBeforeFade = 2f;
    [SerializeField] private float timeBeforeConfetti = 0.5f;
    [SerializeField] private float dragForce = 0.1f;
    private bool transitioning = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transitioning) return;
        if (collision.gameObject == Ability.Instance.gameObject)
        {
            Ability.Instance.GetComponent<PlayerInput>().enabled = false;
            Ability.Instance.GetComponent<Rigidbody2D>().simulated = false;
            Invoke(nameof(Confetti), timeBeforeConfetti);
            Invoke(nameof(StartFade), timeBeforeFade);
            transitioning = true;
        }
    }
    private void Update()
    {
        if (!transitioning) return;
        Transform player = Ability.Instance.transform;
        player.transform.position = Vector2.Lerp(player.transform.position, playerGoingTo.transform.position, dragForce);
    }
    private void Confetti()
    {
        AudioManager.Instance.Play("confetti", 0.3f);
        ParticleManager.Instance.Spawn("confetti", Ability.Instance.transform.position);
    }
    private void StartFade()
    {
        Dunkelheit.Instance.FadeTo(leadsTo);
        ZoomyBoy.Instance.StartExit();
    }
}
