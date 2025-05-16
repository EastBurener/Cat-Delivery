using UnityEngine;
using UnityEngine.UI; 

public class TutorialPanelManager : MonoBehaviour
{
	[Header("UI Elements")]
	public GameObject tutorialPanelObject; // ���̳����GameObject��ק���˴�
	public UnityEngine.UI.Button closeButton; // ���رհ�ť��ק���˴�


	public CatMove catMoveScript;                 // ������CatMove�ű���GameObject��ק���˴�
	public CountdownTimer countdownTimerScript;   // ������CountdownTimer�ű���GameObject��ק���˴�

	void Awake()
	{
		// ȷ����Ҫ�������Ѿ�����
		if (tutorialPanelObject == null)
		{
			Debug.LogError("���󣺽̳������� (Tutorial Panel Object) δ��TutorialPanelManager��ָ����", this);
			enabled = false; // ���ô˽ű��Է�ֹ��������
			return;
		}

		if (closeButton == null)
		{
			Debug.LogError("���󣺹رհ�ť (Close Button) δ��TutorialPanelManager��ָ����", this);
			enabled = false; // ���ô˽ű�
			return;
		}

		// Ϊ�رհ�ť�ĵ���¼���Ӽ�����
		closeButton.onClick.AddListener(OnCloseButtonClicked); 

	}

	// ����GameObject���Լ����ӵĴ˽ű�����Ϊ����״̬ʱ����
	// ��ͨ����ζ�Ž̳������ʾ������
	void OnEnable()
	{
		DisableTargetScripts();
	}

	private void DisableTargetScripts()
	{
		if (catMoveScript != null)
		{
			if (catMoveScript.enabled) // ���ڽű���ǰ����ʱ�Ž���
			{
				catMoveScript.enabled = false; 

				Debug.Log("CatMove �ű��ѽ��� (��Ϊ�̳�����Ѽ���)��");
			}
		}
		else
		{
			Debug.LogWarning("���棺CatMove �ű�����δ���ã��޷����á�");
		}

		if (countdownTimerScript != null)
		{
			if (countdownTimerScript.enabled) // ���ڽű���ǰ����ʱ�Ž���
			{
				countdownTimerScript.enabled = false; 

				Debug.Log("CountdownTimer �ű��ѽ��� (��Ϊ�̳�����Ѽ���)��");
			}
		}
		else
		{
			Debug.LogWarning("���棺CountdownTimer �ű�����δ���ã��޷����á�");
		}
	}

	private void EnableTargetScripts()
	{
		if (catMoveScript != null)
		{
			if (!catMoveScript.enabled) // ���ڽű���ǰ����ʱ������
			{
				catMoveScript.enabled = true;
				Debug.Log("CatMove �ű����������á�");
			}
		}

		if (countdownTimerScript != null)
		{
			if (!countdownTimerScript.enabled) // ���ڽű���ǰ����ʱ������
			{
				countdownTimerScript.enabled = true;
				Debug.Log("CountdownTimer �ű����������á�");
			}
		}
	}

	public void OnCloseButtonClicked()
	{
		Debug.Log("�رհ�ť�������");

		// ���� 1: ��������֮ǰ���������õĽű�
		EnableTargetScripts();

		// ���� 2: ���ؽ̳����
		if (tutorialPanelObject != null)
		{
			tutorialPanelObject.SetActive(false); 

			Debug.Log("�̳�����ѹرա�");
		}
	}
}