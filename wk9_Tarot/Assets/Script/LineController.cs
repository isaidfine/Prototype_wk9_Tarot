using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float targetWidth = 0.1f;
    private float widthIncreasePerSecond = 0.005f;
    private float currentWidth = 0.01f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = currentWidth;
        lineRenderer.endWidth = currentWidth;
    }

    void Update()
    {
        if (currentWidth < targetWidth)
        {
            currentWidth += widthIncreasePerSecond * Time.deltaTime;
            currentWidth = Mathf.Min(currentWidth, targetWidth);
            lineRenderer.startWidth = currentWidth;
            lineRenderer.endWidth = currentWidth;
        }
    }
}