using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
	public Text countdownText;
	public GameObject endPanel; // ��������������Panel������

	public float timeRemaining = 60f;
	private bool timerIsRunning = false;

	void Start()
	{
		if (countdownText == null)
		{
			Debug.LogError("CountdownText UI Ԫ��δָ�����ű������á�");
			enabled = false;
			return;
		}

		// ��ʼ��ʱ���ؽ�������
		if (endPanel != null)
			endPanel.SetActive(false);

		StartTimer(timeRemaining);
	}

	public void StartTimer(float duration)
	{
		timeRemaining = duration;
		timerIsRunning = true;

		// ��ʼ��ʱʱȷ�����ؽ�������
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
				Debug.Log("ʱ�䵽��");

				// �����������
				if (endPanel != null)
					endPanel.SetActive(true);
				else
					Debug.LogWarning("δָ����������Panel");
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