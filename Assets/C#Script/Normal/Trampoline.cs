using UnityEngine;

public class SpringPlatform : MonoBehaviour
{
	[Header("弹跳参数")]
	[Tooltip("垂直弹跳力，值越大跳得越高")]
	public float bounceForce = 20f;

	[Tooltip("是否保留物体的水平速度")]
	public bool keepHorizontalVelocity = true;

	// 当碰撞发生时
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// 遍历所有碰撞接触点
		foreach (ContactPoint2D contact in collision.contacts)
		{
			// 检查法线方向（Y值>0.5表示碰撞来自上方）
			if (contact.normal.y > 0.5f)
			{
				// 获取碰撞物体的Rigidbody2D
				Rigidbody2D rb = collision.rigidbody;
				if (rb != null)
				{
					// 计算新速度
					float newVelocityX = keepHorizontalVelocity ? rb.velocity.x : 0;
					rb.velocity = new Vector2(newVelocityX, bounceForce);
				}
				break; // 处理一次有效碰撞后退出循环
			}
		}
	}
}