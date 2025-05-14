using UnityEngine;
using UnityEngine.UI; 

public class CountdownTimer : MonoBehaviour
{

	public Text countdownText;


	public float timeRemaining = 60f; // ʾ����ʼʱ��

	private bool timerIsRunning = false;
	// private Color originalTextColor; // �����ʹ�ø��ı�����Ĭ����ɫ�������ڴ洢ԭʼ��ɫ

	void Start()
	{
		if (countdownText == null)
		{
			Debug.LogError("CountdownText UI Ԫ��δָ�����ű������á�");
			enabled = false; // ���UIδ���ã�����ýű�
			return;
		}

		// �����ʹ�ø��ı�����ʾĬ��״̬�����߼ƻ��л������ı���ɫ��
		// �洢ԭʼ��ɫ��������塣
		// ���ڸ��ı���δ��ǵĲ��ֻᱣ�����ڼ����������õ�ԭʼ��ɫ��
		// originalTextColor = countdownText.color;

		StartTimer(timeRemaining); // ���ߴ������ط������˷���
	}

	public void StartTimer(float duration)
	{
		timeRemaining = duration;
		timerIsRunning = true;
		// ��������һ����ʾ���Ա�������ʱ���ӳ�
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
				timeRemaining = 0; // ȷ��ʱ�䲻����ʾΪ����
				timerIsRunning = false;
				UpdateCountdownDisplay(); // ������һ������ʾ0
										  // ��ѡ������һ���¼����� "TimerFinished()"
				Debug.Log("ʱ�䵽��");
			}
		}
	}

	void UpdateCountdownDisplay()
	{
		// ����ȡ���õ���ʾ������
		int displaySeconds = Mathf.CeilToInt(timeRemaining);

		// ȷ����ʾ�������������0����ʹtimeRemaining�򸡵㾫��������΢Ϊ��
		displaySeconds = Mathf.Max(0, displaySeconds);

		string timeText;

		if (displaySeconds <= 10)
		{
			// ʹ�ø��ı������ֲ��ֱ��
			timeText = $"<color=red>{displaySeconds}</color>";
		}
		else
		{
			// ����Ҫ��ɫ��ǩ��ʹ���ڼ����������õ�Ĭ����ɫ
			timeText = $"{displaySeconds}";
		}

		// ֻ�е��ı�����ʵ�ʷ����仯ʱ�Ÿ��£��Խ���΢С�Ż�����ѡ��
		if (countdownText.text != timeText)
		{
			countdownText.text = timeText;
		}

	}
}