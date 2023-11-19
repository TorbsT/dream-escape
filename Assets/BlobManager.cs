using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlobManager : MonoBehaviour
{
    [SerializeField] private CoolMaskShit thanksCanvas;
    [SerializeField] private Treasury treasury;
    [SerializeField] private float spinSpeed = 10f;
    [SerializeField] private float ejectSpeed = 5f;
    [SerializeField] private GameObject blobPrefab;
    [SerializeField] private float timeToStealOne = 10f;
    [SerializeField] private bool shuffleTreasury;
    [SerializeField] private bool shuffleGoods;
    [SerializeField] private float playtime;
    [SerializeField] private float timeBetweenBlobs = 1f;
    private float timeSinceLastBlob = 10000f;
    private List<string> stolenGoods = new();

    private Transform displaying;
    void Start()
    {
        List<TextMeshPro> blobs = new();
        foreach (var child in GetComponentsInChildren<TextMeshPro>())
            blobs.Add(child);
        TextMeshPro timeBlob = blobs.Find(x => x.text.Contains("{time}"));
        float playtime = this.playtime;
        if (Memory.Instance != null)
        {
            Memory.Instance.STOPTHECOUNT();
            Memory.Instance.HighestLevel = 1;
            playtime = Memory.Instance.Playtime;
        }
        if (timeBlob != null)
        {
            int minutes = Mathf.FloorToInt(playtime / 60);
            int seconds = Mathf.FloorToInt(playtime % 60);
            string minutesText = minutes <= 9 ? "0" + minutes.ToString() : minutes.ToString();
            string secondsText = seconds <= 9 ? "0" + seconds.ToString() : seconds.ToString();
            string text = $"{minutesText}:{secondsText}";
            timeBlob.text = timeBlob.text.Replace("{time}", text);
        }

        int stolenCount = Mathf.FloorToInt(playtime / timeToStealOne);
        List<string> treasure = treasury.GetAll();
        if (shuffleTreasury)
            Shuffle(treasure);
        List<string> actuallyStolen = new();
        for (int i = 0; i < stolenCount; i++)
        {
            if (i >= treasure.Count) break;
            actuallyStolen.Add(treasure[i]);
        }
        if (shuffleGoods)
            Shuffle(actuallyStolen);
        foreach (var stolen in actuallyStolen)
            stolenGoods.Add(stolen);


        foreach (var blob in blobs)
        {
            blob.transform.parent.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        timeSinceLastBlob += Time.unscaledDeltaTime;
        if (timeSinceLastBlob > timeBetweenBlobs)
        {
            if (transform.childCount == 0) return;
            Transform t = transform.GetChild(0);
            Display(t, 1f);
        }
    }
    private void Display(Transform t, float volume)
    {
        timeSinceLastBlob = 0;
        Eject();

        t.position = Vector2.zero;
        t.SetParent(null);
        string text = t.GetComponentInChildren<TextMeshPro>().text;
        if (text.Contains("{goods}"))
        {
            StartCoroutine(SpawnStolenGoods());
        } else if (text.Contains("{clear}"))
        {
            thanksCanvas.Show();
        } else
        {
            t.gameObject.SetActive(true);
            displaying = t;
            AudioManager.Instance.PlayRandom("footstep", volume);
        }
    }
    private void Eject()
    {
        if (displaying != null)
        {
            Rigidbody2D rb = displaying.GetComponent<Rigidbody2D>();
            rb.simulated = true;
            rb.velocity = Random.insideUnitCircle * ejectSpeed;
            rb.angularVelocity = Random.Range(-spinSpeed, spinSpeed);
            displaying = null;
        }
    }
    private IEnumerator SpawnStolenGoods()
    {
        Debug.Log(stolenGoods.Count);
        float timeBetweenSpawns = timeBetweenBlobs/stolenGoods.Count;
        for (int i = 0;  i < stolenGoods.Count; i++)
        {
            Transform tr = Instantiate(blobPrefab).transform;
            TextMeshPro t = tr.GetComponentInChildren<TextMeshPro>();
            t.text = stolenGoods[i];
            Display(tr, 0.25f);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        Eject();
    }
    private static System.Random rng = new();

    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
