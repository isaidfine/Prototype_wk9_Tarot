using UnityEngine;
using System.Collections;

public class CircleBehavior : MonoBehaviour
{
    public bool isConnected = false;
    public float fadeDuration = 0.5f;
    public float lifetimeWithoutConnection = 10f;
    private Color blackColor = new Color(0.22f, 0.165f, 0.039f, 1f); // 黑色
    private Color whiteColor = new Color(0.945f, 0.882f, 0.745f, 1f); // 白色（修改了 Alpha 值为 1）
    private SpriteRenderer spriteRenderer;
    private float timeSinceSpawned;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = whiteColor;
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        if (!isConnected)
        {
            timeSinceSpawned += Time.deltaTime;
            if (timeSinceSpawned >= lifetimeWithoutConnection)
            {
                StartCoroutine(FadeOut());
            }
        }
    }

    public void ChangeToBlack()
    {
        spriteRenderer.color = blackColor;
        UpdateLineColor(blackColor);
        // 在 3 到 60 秒之间的随机时间后变回白色
        float delay = Random.Range(3f, 60f);
        Invoke("ChangeToWhite", delay);
    }

    private void ChangeToWhite()
    {
        spriteRenderer.color = whiteColor;
        UpdateLineColor(whiteColor);
    }

    private void UpdateLineColor(Color color)
    {
        // 更新所有子物体（线条）的颜色
        foreach (Transform child in transform)
        {
            LineRenderer lineRenderer = child.GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;
            }
        }
    }

    IEnumerator FadeIn()
    {
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime / fadeDuration;
            spriteRenderer.color = new Color(whiteColor.r, whiteColor.g, whiteColor.b, alpha);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float alpha = spriteRenderer.color.a;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeDuration;
            spriteRenderer.color = new Color(whiteColor.r, whiteColor.g, whiteColor.b, alpha);
            yield return null;
        }
        Destroy(gameObject);
    }

    public void SetConnected(bool connected)
    {
        isConnected = connected;
    }
}
