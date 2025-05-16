
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageReceive : MonoBehaviour
{
	public GameDate_SO GameDate;
	public GameObject Cat;
	private MonoBehaviour targetScriptToDisable;

	[Header("关卡完成逻辑处理器")]
	public LevelCompletionHandler levelCompletionHandler; // 将场景中挂载 LevelCompletionHandler 的对象拖到这里

	private void Start()
	{
		if (Cat != null)
		{
			targetScriptToDisable = Cat.GetComponent<MonoBehaviour>(); 
			if (targetScriptToDisable == null)
			{
				Debug.LogWarning($"[PackageReceive] 在 Cat 对象上未能找到可禁用的 MonoBehaviour 脚本。", Cat);
			}
		}
		else
		{
			Debug.LogError("[PackageReceive] Cat 对象未在 Inspector 中赋值!", this);
		}


		if (levelCompletionHandler == null)
		{
			Debug.LogError("[PackageReceive] LevelCompletionHandler 未在 Inspector 中赋值! 关卡解锁逻辑将无法执行。", this);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!enabled) return; // 如果脚本已被禁用（例如，已通关），则不执行

		if (other.CompareTag("Package"))
		{
			GameDate.givePackage++;
			Destroy(other.gameObject);
			Debug.Log($"[PackageReceive] 包裹已收集，当前数量: {GameDate.givePackage} / {GameDate.allPackage}");


			if (GameDate.givePackage >= GameDate.allPackage)
			{
				Debug.Log("[PackageReceive] 胜利条件已满足!");

				if (levelCompletionHandler != null)
				{
					levelCompletionHandler.TriggerPlayerVictory(); // 调用通关逻辑、PlayerPrefs更新、显示LCH的胜利UI、暂停游戏
				}
				else
				{
					Debug.LogError("[PackageReceive] LevelCompletionHandler 未赋值，无法执行关卡完成逻辑！");
				}

				// 处理此脚本特有的胜利后行为
				PerformPostVictoryActions();
			}
		}
	}

	private void PerformPostVictoryActions()
	{
		Debug.Log("[PackageReceive] 执行胜利后的特定操作 (例如禁用Cat脚本)。");

		if (targetScriptToDisable != null)
		{
			Debug.Log("[PackageReceive] 禁用 Cat 的目标脚本。");
			targetScriptToDisable.enabled = false;
		}
		else if (Cat != null)
		{
			Debug.LogWarning("[PackageReceive] 未能禁用 Cat 脚本，因为在Start中未能成功获取到 targetScriptToDisable。", Cat);
		}

	}
}