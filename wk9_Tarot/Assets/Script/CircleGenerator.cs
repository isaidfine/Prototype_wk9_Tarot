using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CircleGenerator : MonoBehaviour
{
    public GameObject circlePrefab; // 圆圈Prefab
    public float spawnRangeX = 5f; // X轴生成范围
    public float spawnRangeY = 5f; // Y轴生成范围
    public float minDistanceBetweenCircles = 1f; // 圆圈之间的最小距离
    public float spawnInterval = 5f; // 生成圆圈的间隔时间

    private List<GameObject> spawnedCircles = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnCircleRoutine());
    }

    IEnumerator SpawnCircleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            TrySpawnCircle();
        }
    }

    void TrySpawnCircle()
    {
        // 清除已销毁的圆圈引用
        spawnedCircles.RemoveAll(item => item == null);

        Vector3 potentialPosition;
        bool positionFound = false;

        for (int tries = 0; tries < 100; tries++)
        {
            potentialPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), Random.Range(-spawnRangeY, spawnRangeY), 0);

            bool tooCloseToOtherCircle = false;
            foreach (var circle in spawnedCircles)
            {
                if (circle != null && Vector3.Distance(circle.transform.position, potentialPosition) < minDistanceBetweenCircles)
                {
                    tooCloseToOtherCircle = true;
                    break;
                }
            }

            if (!tooCloseToOtherCircle)
            {
                positionFound = true;
                GameObject newCircle = Instantiate(circlePrefab, potentialPosition, Quaternion.identity);
                spawnedCircles.Add(newCircle);
                break;
            }
        }

        if (!positionFound)
        {
            Debug.Log("未找到合适的位置生成新圆圈");
        }
    }
}