using UnityEngine;
using UnityEngine.UI; // ��Ҫ�������ռ������� UI.Button

public class TutorialPanelManager : MonoBehaviour
{
	[Header("UI Elements")]
	public GameObject tutorialPanelObject; // �����Ľ̳����GameObject��ק���˴�
	public UnityEngine.UI.Button closeButton; // �����Ĺرհ�ť��ק���˴�


	public CatMove catMoveScript;                 // ������CatMove�ű���GameObject��ק���˴�
	public CountdownTimer countdownTimerScript;   // ������CountdownTimer�ű���GameObject��ק���˴�

	void Awake()
	{
		// ȷ����Ҫ�������Ѿ�����
		if (tutorialPanelObject == null)
		{
			// ����˽ű��͹����ڽ̳����GameObject�ϣ�
			// ��ô����ʱ����ֱ���� this.gameObject.SetActive(false)
			// Ϊ����ͨ���ԣ�������ȷʹ�� tutorialPanelObject ����������
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

	// ����ѡ������GameObject��Ϊ�Ǽ���״̬ʱ����
	// ��������ͨ������رհ�ť���صģ�OnCloseButtonClicked �Ѿ������˽ű����������á�
	// ���������ͨ��������ʽ�����أ����縸���󱻽��ã���������ϣ�������������Ҳ�������ýű���
	// ������������� EnableTargetScripts();
	// void OnDisable()
	// {
	//     // �����Ƿ���Ҫ�ڷǰ�ť�رյ������Ҳ���ýű�
	//     // EnableTargetScripts();
	// }

	/// <summary>
	/// ����Ŀ��ű���
	/// </summary>
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

	/// <summary>
	/// ����Ŀ��ű���
	/// </summary>
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
		// �˴�����ӡ���棬��Ϊ�ű����ܲ����ɱ�������

		if (countdownTimerScript != null)
		{
			if (!countdownTimerScript.enabled) // ���ڽű���ǰ����ʱ������
			{
				countdownTimerScript.enabled = true;
				Debug.Log("CountdownTimer �ű����������á�");
			}
		}
	}

	/// <summary>
	/// ���رհ�ť�����ʱ���õķ�����
	/// </summary>
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
			// �� tutorialPanelObject.SetActive(false) ִ�к�����˽ű������ڸö����ϣ�
			// OnDisable() �����ᱻ���á�
		}
	}
}