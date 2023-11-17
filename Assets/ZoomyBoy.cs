using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomyBoy : MonoBehaviour
{
    public static ZoomyBoy Instance { get; private set; }

    [SerializeField] private float doorSize = 0.1f;
    [SerializeField] private float zoomDuration = 2f;
    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    private new Camera camera;
    private Vector2 initialPos;
    private Vector2 minPos;
    private float initialSize;
    private float progress;
    private bool Exiting { get; set; }

    public void StartExit()
    {
        //minPos = FindFirstObjectByType<Door>().gameObject.transform.position;
        RememberZoom.Instance.SkipZoom = true;
        Exiting = true;
    }
    private void Awake()
    {
        Instance = this;
        camera = GetComponentInChildren<Camera>();
        initialPos = transform.position;
        initialSize = camera.orthographicSize;
    }

    private void Start()
    {
        if (!RememberZoom.Instance.SkipZoom)
            progress = 1f;
        else
        {
            camera.orthographicSize = doorSize;
            transform.position = minPos;
        }
        RememberZoom.Instance.SkipZoom = false;
    }

    private void Update()
    {
        float d = Time.deltaTime/zoomDuration;
        if (Exiting)
            progress -= d;
        else
            progress += d;
        progress = Mathf.Clamp(progress, 0f, 1f);

        float curvedProgress = curve.Evaluate(progress);
        minPos = Head.Instance.transform.position;
        camera.orthographicSize = Mathf.Lerp(doorSize, initialSize, curvedProgress);
        Vector2 pos = Vector2.Lerp(minPos, initialPos, curvedProgress);
        transform.position = new Vector3(pos.x, pos.y, -10f);
    }
}
