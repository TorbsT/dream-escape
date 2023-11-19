using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CoolMaskShit : MonoBehaviour
{
    [SerializeField] public RectTransform masker;
    [SerializeField] private RectTransform masked;
    [SerializeField] private bool followMouse = true;
    [SerializeField] private bool expand = false;
    [SerializeField] private float targetScale = 1f;
    [SerializeField] private float expandDuration = 3f;
    [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    private RectTransform canvas;
    private float progress;

    public void Show()
    {
        progress = 0f;
        RefreshScale();
        Refresh();
        masker.gameObject.SetActive(true);
    }
    private void Awake()
    {
        canvas = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        if (!Application.isPlaying) return;
        progress = 0f;
        Refresh();
    }
    private void Update()
    {
        if (Application.isPlaying)
        {
            if (!expand) progress = 1f;
            progress += Time.unscaledDeltaTime/expandDuration;
            progress = Mathf.Clamp(progress, 0f, 1f);
            RefreshScale();
            if (followMouse)
            {
                Vector2 mousePos = Input.mousePosition;
                masker.anchoredPosition = mousePos - new Vector2(Screen.width / 2f, Screen.height / 2f);
            } 
        }
        
        Refresh();
    }
    private void RefreshScale()
    {
        float scale = curve.Evaluate(progress) * targetScale;
        scale = Mathf.Max(scale, 0.001f);
        masker.localScale = Vector3.one * scale;
    }
    private void Refresh()
    {
        if (masker == null || masked == null) return;
        masked.anchoredPosition = -masker.anchoredPosition;
        masked.localScale = Vector3.one * (1f / masker.localScale.x);
    }
}
