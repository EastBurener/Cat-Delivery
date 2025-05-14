using UnityEngine;
using UnityEngine.UI; 

public class CountdownTimer : MonoBehaviour
{

	public Text countdownText;


	public float timeRemaining = 60f; // 示例起始时间

	private bool timerIsRunning = false;
	// private Color originalTextColor; // 如果不使用富文本处理默认颜色，则用于存储原始颜色

	void Start()
	{
		if (countdownText == null)
		{
			Debug.LogError("CountdownText UI 元素未指定！脚本将禁用。");
			enabled = false; // 如果UI未设置，则禁用脚本
			return;
		}

		// 如果不使用富文本来显示默认状态，或者计划切换整个文本颜色，
		// 存储原始颜色会更有意义。
		// 对于富文本，未标记的部分会保持其在检视器中设置的原始颜色。
		// originalTextColor = countdownText.color;

		StartTimer(timeRemaining); // 或者从其他地方触发此方法
	}

	public void StartTimer(float duration)
	{
		timeRemaining = duration;
		timerIsRunning = true;
		// 立即更新一次显示，以避免启动时的延迟
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
				timeRemaining = 0; // 确保时间不会显示为负数
				timerIsRunning = false;
				UpdateCountdownDisplay(); // 最后更新一次以显示0
										  // 可选：触发一个事件，如 "TimerFinished()"
				Debug.Log("时间到！");
			}
		}
	}

	void UpdateCountdownDisplay()
	{
		// 向上取整得到显示的秒数
		int displaySeconds = Mathf.CeilToInt(timeRemaining);

		// 确保显示的秒数不会低于0，即使timeRemaining因浮点精度问题略微为负
		displaySeconds = Mathf.Max(0, displaySeconds);

		string timeText;

		if (displaySeconds <= 10)
		{
			// 使用富文本将数字部分标红
			timeText = $"<color=red>{displaySeconds}</color>";
		}
		else
		{
			// 不需要颜色标签，使用在检视器中设置的默认颜色
			timeText = $"{displaySeconds}";
		}

		// 只有当文本内容实际发生变化时才更新，以进行微小优化（可选）
		if (countdownText.text != timeText)
		{
			countdownText.text = timeText;
		}

	}
}