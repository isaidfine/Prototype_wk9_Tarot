using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public GameObject linePrefab; // LineRenderer预制件
    public float drawSpeed = 0.5f; // 绘制速度
    private HashSet<GameObject> connectedCircles = new HashSet<GameObject>(); // 已连接的圆圈集合
    private Dictionary<LineRenderer, GameObject[]> lineToCirclesMap = new Dictionary<LineRenderer, GameObject[]>(); // 线条到圆圈的映射

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("family") && !connectedCircles.Contains(hit.collider.gameObject))
                {
                    connectedCircles.Add(hit.collider.gameObject);
                    CreateLine(hit.collider.transform.position, hit.collider.gameObject);
                }
                else if (hit.collider.GetComponent<LineRenderer>())
                {
                    DeleteLine(hit.collider.GetComponent<LineRenderer>());
                }
            }
        }
    }

    void CreateLine(Vector3 targetPos, GameObject targetCircle)
    {
        GameObject newLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();
        BoxCollider2D boxCollider = newLine.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        lineToCirclesMap[lineRenderer] = new GameObject[] { gameObject, targetCircle };
        StartCoroutine(DrawLineCoroutine(lineRenderer, boxCollider, targetPos));
        newLine.transform.SetParent(targetCircle.transform); // 将新线条设为圆圈的子物体
    }

    IEnumerator DrawLineCoroutine(LineRenderer lineRenderer, BoxCollider2D boxCollider, Vector3 targetPos)
    {
        
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);

        float distanceCovered = 0.0f;
        float totalDistance = Vector3.Distance(transform.position, targetPos);

        while (distanceCovered < totalDistance)
        {
            distanceCovered += drawSpeed * Time.deltaTime;
            float fraction = distanceCovered / totalDistance;
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPos, fraction);
            lineRenderer.SetPosition(1, newPosition);

            UpdateBoxCollider(boxCollider, lineRenderer.GetPosition(0), newPosition);

            yield return null;
        }

        GameObject targetCircle = lineToCirclesMap[lineRenderer][1];
        CircleBehavior circleBehavior = targetCircle.GetComponent<CircleBehavior>();
        if (circleBehavior != null)
        {
            circleBehavior.SetConnected(true);
        }
    }

    void UpdateBoxCollider(BoxCollider2D boxCollider, Vector3 startPoint, Vector3 endPoint)
    {
        boxCollider.size = new Vector2(Vector3.Distance(startPoint, endPoint), linePrefab.GetComponent<LineRenderer>().startWidth);
        boxCollider.transform.position = (startPoint + endPoint) / 2;
        float angle = Mathf.Atan2(endPoint.y - startPoint.y, endPoint.x - startPoint.x) * Mathf.Rad2Deg;
        boxCollider.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void DeleteLine(LineRenderer lineRenderer)
    {
        if (lineToCirclesMap.TryGetValue(lineRenderer, out GameObject[] circles))
        {
            foreach (var circle in circles)
            {
                if (circle != null)
                {
                    CircleBehavior circleBehavior = circle.GetComponent<CircleBehavior>();
                    if (circleBehavior != null)
                    {
                        circleBehavior.SetConnected(false);
                    }
                }
            }
        }

        lineToCirclesMap.Remove(lineRenderer);
        Destroy(lineRenderer.gameObject);
    }
}
