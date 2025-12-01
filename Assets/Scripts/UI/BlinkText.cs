using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    public float speed = 1.2f;  // ±ôºýÀÌ´Â ¼Óµµ (³·À»¼ö·Ï ´À¸²)

    private TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        float alpha = Mathf.Lerp(0.2f, 1f, Mathf.PingPong(Time.time * speed, 1f));
        text.alpha = alpha;
    }
}
