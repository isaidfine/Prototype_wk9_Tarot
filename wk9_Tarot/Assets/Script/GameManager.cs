using UnityEngine;
using UnityEngine.SceneManagement; // 引入场景管理

public class GameManager : MonoBehaviour
{
    private float score = 0f;

    void Update()
    {
        // 计算分数
        LineRenderer[] allLines = FindObjectsOfType<LineRenderer>();
        foreach (var line in allLines)
        {
            // 检查是否标记为"line"
            if (line.gameObject.CompareTag("line"))
            {
                float lineWidth = line.startWidth; // 假设startWidth和endWidth是相同的

                if (lineWidth >= 0.09f)
                    score += 4 * Time.deltaTime;
                else if (lineWidth >= 0.06f)
                    score += 3 * Time.deltaTime;
                else if (lineWidth >= 0.03f)
                    score += 2 * Time.deltaTime;
                else
                    score += Time.deltaTime;
            }
        }

        // 显示分数
        //Debug.Log("当前分数: " + score);

        // 按下R键重置关卡
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }
    }

    void ResetLevel()
    {
        // 重载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        score = 0; // 分数重置
    }
}