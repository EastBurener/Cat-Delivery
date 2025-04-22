using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
	[Header("��������")]
	public float totalTime = 60f;     // �ܼ�ʱ�����룩
	public Text timerText;           // ������ʾʱ���Text���
	public GameObject failureUI;     // ʧ�ܽ���UI

	private float currentTime;

	void Start()
	{
		currentTime = totalTime;
		failureUI.SetActive(false);  // ��ʼ����ʧ�ܽ���
		Time.timeScale = 1f;         // ȷ����Ϸδ��ͣ
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

	// ����ʱ����ʾ�������֣�
	void UpdateTimerDisplay()
	{
		// ��ʾ�����루�磺60��
		timerText.text = Mathf.CeilToInt(currentTime).ToString();

		// ������ʾ��:���ʽ���磺01:30��
		// int minutes = Mathf.FloorToInt(currentTime / 60);
		// int seconds = Mathf.FloorToInt(currentTime % 60);
		// timerText.text = $"{minutes:00}:{seconds:00}";
	}

	void GameOver()
	{
		Time.timeScale = 0f;         // ��ͣ��Ϸ
		failureUI.SetActive(true);   // ��ʾʧ�ܽ���
	}
}