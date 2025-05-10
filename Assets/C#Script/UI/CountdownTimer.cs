using UnityEngine;
using UnityEngine.UI; // 确保引入了 UnityEngine.UI 命名空间

public class CountdownTimer : MonoBehaviour
{
	[Header("基础设置")]
	public float totalTime = 60f;    // 总计时长（秒）
	public Text timerText;          // 拖入显示时间的Text组件
	public GameObject failureUI;    // 失败界面UI

	private float currentTime;

	void Start()
	{
		// 添加空引用检查
		if (timerText == null)
		{
			Debug.LogError("CountdownTimer: timerText 没有在 Inspector 中被指定！");
			// 可以选择禁用此脚本或给出更明确的错误提示，以避免后续的 NullReferenceException
			enabled = false; // 禁用此脚本以防止进一步错误
			return;
		}

		if (failureUI == null)
		{
			Debug.LogError("CountdownTimer: failureUI 没有在 Inspector 中被指定！");
			enabled = false;
			return;
		}

		currentTime = totalTime;
		failureUI.SetActive(false);  // 初始隐藏失败界面
		Time.timeScale = 1f;        // 确保游戏未暂停
	}

	void Update()
	{
		if (currentTime > 0)
		{
			currentTime -= Time.deltaTime;
			UpdateTimerDisplay();
		}
		else
		{
			currentTime = 0;
			// 在调用 GameOver 之前，确保 UpdateTimerDisplay 被调用一次，以显示0
			UpdateTimerDisplay();
			GameOver();
		}
	}

	// 基本时间显示（仅数字）
	void UpdateTimerDisplay()
	{
		// 再次检查，以防万一在运行时被设为 null
		if (timerText != null)
		{
			// 显示整数秒（如：60）
			timerText.text = Mathf.CeilToInt(currentTime).ToString();

			// 或者显示分:秒格式（如：01:30）
			// int minutes = Mathf.FloorToInt(currentTime / 60);
			// int seconds = Mathf.FloorToInt(currentTime % 60);
			// timerText.text = $"{minutes:00}:{seconds:00}";
		}
		else
		{
			Debug.LogError("CountdownTimer: timerText 在 UpdateTimerDisplay 中为 null！");
		}
	}

	void GameOver()
	{
		Time.timeScale = 0f;        // 暂停游戏
		if (failureUI != null)
		{
			failureUI.SetActive(true);  // 显示失败界面
		}
		else
		{
			Debug.LogError("CountdownTimer: failureUI 在 GameOver 中为 null！");
		}
	}
}