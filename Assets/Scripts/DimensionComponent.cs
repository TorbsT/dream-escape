using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionComponent : MonoBehaviour
{
    [field: SerializeField] public Dimension Dimension { get; set; } = Dimension.PURP;
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
        if (Animator == null) return;
        Animator.SetBool("physical", physical);
    }
}
public enum Dimension
{
    PURP,
    RED,
    BLU,
}