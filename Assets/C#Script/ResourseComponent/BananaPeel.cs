using UnityEngine;

public class BananaPeel : MonoBehaviour
{
	[Header("滑行参数")]
	public float slideSpeed = 5f; // 滑行速度
	public float slideDuration = 1f; // 滑行时间
	public string playerTag = "Player"; // 角色的Tag
	public bool slideRight = true; // 默认向右滑行
	public float destroyDelay = 0.5f; // 香蕉皮销毁前的延迟

	private GameObject slidingPlayer; // 当前正在滑行的角色
	private Rigidbody2D playerRb; // 角色的 Rigidbody2D
	private float slideTimer = 0f; // 滑行计时器
	private Vector2 slideDirection; // 滑行方向
	private bool isPlayerSliding = false; // 角色是否正在滑行

	private Vector3 initialPositionOffset; // 角色相对于香蕉皮的初始位置偏移量

	private void Update()
	{
		if (slidingPlayer != null && isPlayerSliding)
		{
			// 计时滑行时间
			slideTimer += Time.deltaTime;

			// 如果滑行时间结束，停止滑行并销毁香蕉
			if (slideTimer >= slideDuration)
			{
				StopSlidingAndDestroy();
			}
		}
	}

	private void FixedUpdate()
	{
		if (slidingPlayer != null && isPlayerSliding)
		{
			// 移动香蕉皮
			transform.Translate(slideDirection * slideSpeed * Time.fixedDeltaTime);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// 检测碰撞的物体是否是角色
		if (collision.gameObject.CompareTag(playerTag))
		{
			StartSliding(collision);
		}
	}

	private void StartSliding(Collision2D collision)
	{
		//设置角色滑行状态
		isPlayerSliding = true;

		// 获取碰撞到的角色
		slidingPlayer = collision.gameObject;

		// 获取角色的 Rigidbody2D
		playerRb = slidingPlayer.GetComponent<Rigidbody2D>();

		// 如果没有 Rigidbody2D，则输出错误信息并返回
		if (playerRb == null)
		{
			Debug.LogError("角色没有 Rigidbody2D 组件!");
			return;
		}

		// 保存相对位置
		initialPositionOffset = slidingPlayer.transform.position - transform.position;

		// 将角色设置为香蕉皮的子对象
		slidingPlayer.transform.SetParent(transform);

		// 设置滑行方向
		slideDirection = slideRight ? Vector2.right : Vector2.left;

		// 禁用角色的重力，防止掉落
		playerRb.gravityScale = 0;

		//停止角色自身的移动，防止和香蕉皮发生冲突
		playerRb.velocity = Vector2.zero;

		// 可以添加一个弹射效果，例如：
		//playerRb.AddForce(slideDirection * 5f, ForceMode2D.Impulse); // 根据需要调整弹射力度
	}

	private void StopSlidingAndDestroy()
	{
		//停止滑行将角色设置为非滑行状态
		isPlayerSliding = false;

		// 解除父子关系
		slidingPlayer.transform.SetParent(null);

		// 恢复角色的重力
		playerRb.gravityScale = 1;

		// 销毁香蕉皮（延迟一段时间）
		Destroy(gameObject, destroyDelay);
	}
}
