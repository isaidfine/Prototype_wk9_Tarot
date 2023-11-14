using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject blockPrefab; // 方块的预制体
    public float spawnInterval = 1f; // 生成方块的间隔时间
    public float startDelay = 10f; // 开始生成方块前的延迟时间

    private void Start()
    {
        // 在指定的延迟时间后开始定期生成方块
        Invoke("StartSpawning", startDelay);
    }

    void StartSpawning()
    {
        InvokeRepeating("SpawnBlock", 0f, spawnInterval);
    }

    void SpawnBlock()
    {
        float randomX = Random.Range(-7f, 7f);
        float randomY = Random.value < 0.5f ? -15f : 15f;

        Vector2 spawnPosition = new Vector2(randomX, randomY);
        Instantiate(blockPrefab, spawnPosition, Quaternion.identity);
    }
}