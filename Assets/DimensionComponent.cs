using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DimensionComponent : MonoBehaviour
{
    [field: SerializeField] public bool RED { get; set; } = true;
    [field: SerializeField, Range(0f, 1f)] public float Opacity { get; set; } = 1f;
    [field: SerializeField] private Animator Animator { get; set; }
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
    private bool physical;

    private void OnValidate()
    {
        if (Animator == null)
            Animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if (SpriteRenderer == null) return;
        Color color = SpriteRenderer.color;
        SpriteRenderer.color = new(color.r, color.g, color.b, Opacity);
    }
    public void SetPhysical(bool physical)
    {
        this.physical = physical;
        Animator.SetBool("physical", physical);
    }
}
