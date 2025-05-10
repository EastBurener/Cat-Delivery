using UnityEngine;
using UnityEngine.UI; // ȷ�������� UnityEngine.UI �����ռ�

public class CountdownTimer : MonoBehaviour
{
	[Header("��������")]
	public float totalTime = 60f;    // �ܼ�ʱ�����룩
	public Text timerText;          // ������ʾʱ���Text���
	public GameObject failureUI;    // ʧ�ܽ���UI

	private float currentTime;

	void Start()
	{
		// ��ӿ����ü��
		if (timerText == null)
		{
			Debug.LogError("CountdownTimer: timerText û���� Inspector �б�ָ����");
			// ����ѡ����ô˽ű����������ȷ�Ĵ�����ʾ���Ա�������� NullReferenceException
			enabled = false; // ���ô˽ű��Է�ֹ��һ������
			return;
		}

		if (failureUI == null)
		{
			Debug.LogError("CountdownTimer: failureUI û���� Inspector �б�ָ����");
			enabled = false;
			return;
		}

		currentTime = totalTime;
		failureUI.SetActive(false);  // ��ʼ����ʧ�ܽ���
		Time.timeScale = 1f;        // ȷ����Ϸδ��ͣ
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
			// �ڵ��� GameOver ֮ǰ��ȷ�� UpdateTimerDisplay ������һ�Σ�����ʾ0
			UpdateTimerDisplay();
			GameOver();
		}
	}

	// ����ʱ����ʾ�������֣�
	void UpdateTimerDisplay()
	{
		// �ٴμ�飬�Է���һ������ʱ����Ϊ null
		if (timerText != null)
		{
			// ��ʾ�����루�磺60��
			timerText.text = Mathf.CeilToInt(currentTime).ToString();

			// ������ʾ��:���ʽ���磺01:30��
			// int minutes = Mathf.FloorToInt(currentTime / 60);
			// int seconds = Mathf.FloorToInt(currentTime % 60);
			// timerText.text = $"{minutes:00}:{seconds:00}";
		}
		else
		{
			Debug.LogError("CountdownTimer: timerText �� UpdateTimerDisplay ��Ϊ null��");
		}
	}

	void GameOver()
	{
		Time.timeScale = 0f;        // ��ͣ��Ϸ
		if (failureUI != null)
		{
			failureUI.SetActive(true);  // ��ʾʧ�ܽ���
		}
		else
		{
			Debug.LogError("CountdownTimer: failureUI �� GameOver ��Ϊ null��");
		}
	}
}