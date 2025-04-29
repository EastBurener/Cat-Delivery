using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("弹跳参数")]
    [SerializeField] private float bounceForce = 20f; // 弹跳力大小
    [SerializeField] private float horizontalBounceFactor = 0.5f; // 水平方向影响因子

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 确认碰撞对象是玩家（根据Tag筛选）
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // 计算弹跳方向（向上为主，可加入水平方向影响）
                Vector2 bounceDirection = (Vector2.up + new Vector2(playerRb.velocity.x * horizontalBounceFactor, 0)).normalized;
                playerRb.velocity = new Vector2(playerRb.velocity.x, 0); // 重置垂直速度
                playerRb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}