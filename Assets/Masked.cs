using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Masked : MonoBehaviour
{
    private bool RED;
    SpriteRenderer spriteRenderer;
    public void Set(bool global)
    {
        SetVisible(global == RED);
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        RED = Get();
    }
    private void SetVisible(bool visibleOutsideMask)
    {
        if (visibleOutsideMask) spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        else spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }
    private bool Get()
    {
        if (spriteRenderer.maskInteraction == SpriteMaskInteraction.VisibleOutsideMask)
            return true;
        else if (spriteRenderer.maskInteraction == SpriteMaskInteraction.VisibleInsideMask)
            return false;
        else
            Debug.LogWarning($"{gameObject} does not have spriteRenderer.maskInteraction");
        return false;
    }
}
