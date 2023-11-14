using UnityEngine;

public class BlockBehavior : MonoBehaviour
{
    public float speed = 5f; // 方块的移动速度
    public float lifetime = 3f; // 方块存在的时间

    private Vector2 movementDirection;

    void Start()
    {
        // 设置运动方向
        movementDirection = transform.position.y < 0 ? Vector2.up : Vector2.down;
        
        // 3秒后销毁自己
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 移动方块
        transform.Translate(movementDirection * speed * Time.deltaTime);
    }
}