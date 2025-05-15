using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
	public Text countdownText;
	public GameObject endPanel; // 新增：结束界面Panel的引用

	public float timeRemaining = 60f;
	private bool timerIsRunning = false;

	void Start()
	{
		if (countdownText == null)
		{
			Debug.LogError("CountdownText UI 元素未指定！脚本将禁用。");
			enabled = false;
			return;
		}

		// 初始化时隐藏结束界面
		if (endPanel != null)
			endPanel.SetActive(false);

		StartTimer(timeRemaining);
	}

	public void StartTimer(float duration)
	{
		timeRemaining = duration;
		timerIsRunning = true;

		// 开始计时时确保隐藏结束界面
		if (endPanel != null)
			endPanel.SetActive(false);

		UpdateCountdownDisplay();
	}

	void Update()
	{
		if (timerIsRunning)
		{
			if (timeRemaining > 0)
			{
				timeRemaining -= Time.deltaTime;
				UpdateCountdownDisplay();
			}
			else
			{
				timeRemaining = 0;
				timerIsRunning = false;
				UpdateCountdownDisplay();
				Debug.Log("时间到！");

				// 激活结束界面
				if (endPanel != null)
					endPanel.SetActive(true);
				else
					Debug.LogWarning("未指定结束界面Panel");
			}
		}
	}

	void UpdateCountdownDisplay()
	{
		int displaySeconds = Mathf.CeilToInt(timeRemaining);
		displaySeconds = Mathf.Max(0, displaySeconds);
		string timeText = displaySeconds <= 10
			? $"<color=red>{displaySeconds}</color>"
			: $"{displaySeconds}";

		if (countdownText.text != timeText)
		{
			countdownText.text = timeText;
		}
	}
}