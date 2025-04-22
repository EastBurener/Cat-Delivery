using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
	[Header("基础设置")]
	public float totalTime = 60f;     // 总计时长（秒）
	public Text timerText;           // 拖入显示时间的Text组件
	public GameObject failureUI;     // 失败界面UI

	private float currentTime;

	void Start()
	{
		currentTime = totalTime;
		failureUI.SetActive(false);  // 初始隐藏失败界面
		Time.timeScale = 1f;         // 确保游戏未暂停
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
			GameOver();
		}
	}

	// 基本时间显示（仅数字）
	void UpdateTimerDisplay()
	{
		// 显示整数秒（如：60）
		timerText.text = Mathf.CeilToInt(currentTime).ToString();

		// 或者显示分:秒格式（如：01:30）
		// int minutes = Mathf.FloorToInt(currentTime / 60);
		// int seconds = Mathf.FloorToInt(currentTime % 60);
		// timerText.text = $"{minutes:00}:{seconds:00}";
	}

	void GameOver()
	{
		Time.timeScale = 0f;         // 暂停游戏
		failureUI.SetActive(true);   // 显示失败界面
	}
}