using UnityEngine;

public class WindArea : MonoBehaviour
{
	[Header("风力方向")]
	public Vector2 windDirection = Vector2.right; // 默认向右吹风
	[Header("风力大小")]
	public float windForce = 10f;

	private void OnTriggerStay2D(Collider2D other)
	{
		// 检查进入的物体是否是角色 (tag 为 "Player")
		if (other.CompareTag("Player"))
		{
			// 获取角色的 Rigidbody2D 组件
			Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

			// 确保角色有 Rigidbody2D 组件
			if (rb != null)
			{
				// 施加力
				rb.AddForce(windDirection.normalized * windForce * Time.deltaTime, ForceMode2D.Impulse);
			}
		}
	}
}
