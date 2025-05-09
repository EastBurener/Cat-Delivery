// PackageReceive.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageReceive : MonoBehaviour
{
	public GameDate_SO GameDate;
	// public GameObject victoryUI; // ����ֶ����ڿ��ܱ�ö��࣬��ΪLevelCompletionHandler����ʾ��victoryPanel
	// ���������������ʾһ����ͬ��UI�����Ա�����ȡ��ע������������
	public GameObject Cat;
	private MonoBehaviour targetScriptToDisable; // �������Ը�����������

	[Header("�ؿ�����߼�������")]
	public LevelCompletionHandler levelCompletionHandler; // �������й��� LevelCompletionHandler �Ķ����ϵ�����

	private void Start()
	{
		if (Cat != null)
		{
			// ���Ի�ȡ��ϣ�����õ��ض��ű������� PlayerMovement, CatController ��
			// �����ȷ����MonoBehaviour ������Ϊͨ�û��࣬��������õ�һ���ҵ��� MonoBehaviour
			// targetScriptToDisable = Cat.GetComponent<YourCatScriptType>(); // �滻 YourCatScriptType
			targetScriptToDisable = Cat.GetComponent<MonoBehaviour>(); // ����ͨ�ã�����ע������Ϊ
			if (targetScriptToDisable == null)
			{
				Debug.LogWarning($"[PackageReceive] �� Cat ������δ���ҵ��ɽ��õ� MonoBehaviour �ű���", Cat);
			}
		}
		else
		{
			Debug.LogError("[PackageReceive] Cat ����δ�� Inspector �и�ֵ!", this);
		}

		// ��� LevelCompletionHandler �Ḻ����ʾʤ��UI������� victoryUI ���ܲ���Ҫ�� Start �д�����
		// if (victoryUI != null)
		// {
		//     victoryUI.SetActive(false);
		// }
		// else
		// {
		//     Debug.LogWarning("[PackageReceive] PackageReceive �ű��е� Victory UI δ��ֵ���������ʹ�ã���", this);
		// }

		if (levelCompletionHandler == null)
		{
			Debug.LogError("[PackageReceive] LevelCompletionHandler δ�� Inspector �и�ֵ! �ؿ������߼����޷�ִ�С�", this);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!enabled) return; // ����ű��ѱ����ã����磬��ͨ�أ�����ִ��

		if (other.CompareTag("Package"))
		{
			GameDate.givePackage++;
			Destroy(other.gameObject);
			Debug.Log($"[PackageReceive] �������ռ�����ǰ����: {GameDate.givePackage} / {GameDate.allPackage}");


			if (GameDate.givePackage >= GameDate.allPackage)
			{
				Debug.Log("[PackageReceive] ʤ������������!");

				if (levelCompletionHandler != null)
				{
					levelCompletionHandler.TriggerPlayerVictory(); // ����ͨ���߼���PlayerPrefs���¡���ʾLCH��ʤ��UI����ͣ��Ϸ
				}
				else
				{
					Debug.LogError("[PackageReceive] LevelCompletionHandler δ��ֵ���޷�ִ�йؿ�����߼���");
				}

				// ����˽ű����е�ʤ������Ϊ
				PerformPostVictoryActions();
			}
		}
	}

	private void PerformPostVictoryActions()
	{
		Debug.Log("[PackageReceive] ִ��ʤ������ض����� (�������Cat�ű�)��");

		// LevelCompletionHandler.ShowVictoryPanel �Ѿ������� Time.timeScale = 0f;
		// �ͼ��� LevelCompletionHandler.victoryPanel
		// ������Ҫ���� PackageReceive ���е��߼�

		if (targetScriptToDisable != null)
		{
			Debug.Log("[PackageReceive] ���� Cat ��Ŀ��ű���");
			targetScriptToDisable.enabled = false;
		}
		else if (Cat != null)
		{
			Debug.LogWarning("[PackageReceive] δ�ܽ��� Cat �ű�����Ϊ��Start��δ�ܳɹ���ȡ�� targetScriptToDisable��", Cat);
		}

		// ��� PackageReceive �����Լ��� victoryUI ��Ҫ��ʾ (�� LevelCompletionHandler �Ĳ�ͬ����Ϊ����)
		// if (victoryUI != null && !victoryUI.activeSelf)
		// {
		//     victoryUI.SetActive(true);
		//     Debug.Log("[PackageReceive] PackageReceive �� victoryUI Ҳ�����");
		// }
	}
}