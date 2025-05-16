
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageReceive : MonoBehaviour
{
	public GameDate_SO GameDate;
	public GameObject Cat;
	private MonoBehaviour targetScriptToDisable;

	[Header("�ؿ�����߼�������")]
	public LevelCompletionHandler levelCompletionHandler; // �������й��� LevelCompletionHandler �Ķ����ϵ�����

	private void Start()
	{
		if (Cat != null)
		{
			targetScriptToDisable = Cat.GetComponent<MonoBehaviour>(); 
			if (targetScriptToDisable == null)
			{
				Debug.LogWarning($"[PackageReceive] �� Cat ������δ���ҵ��ɽ��õ� MonoBehaviour �ű���", Cat);
			}
		}
		else
		{
			Debug.LogError("[PackageReceive] Cat ����δ�� Inspector �и�ֵ!", this);
		}


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

		if (targetScriptToDisable != null)
		{
			Debug.Log("[PackageReceive] ���� Cat ��Ŀ��ű���");
			targetScriptToDisable.enabled = false;
		}
		else if (Cat != null)
		{
			Debug.LogWarning("[PackageReceive] δ�ܽ��� Cat �ű�����Ϊ��Start��δ�ܳɹ���ȡ�� targetScriptToDisable��", Cat);
		}

	}
}