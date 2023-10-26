using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CoolMaskShit : MonoBehaviour
{
    [SerializeField] private RectTransform masker;
    [SerializeField] private RectTransform masked;
    private RectTransform canvas;

    private void Awake()
    {
        canvas = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (Application.isPlaying)
        {

            Vector2 mousePos = Input.mousePosition;
            masker.anchoredPosition = mousePos-new Vector2(Screen.width/2f, Screen.height/2f);
        }

        if (masker == null || masked == null) return;
        masked.anchoredPosition = -masker.anchoredPosition;
    }
}
