// PackageReceive.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageReceive : MonoBehaviour
{
	public GameDate_SO GameDate;
	// public GameObject victoryUI; // 这个字段现在可能变得多余，因为LevelCompletionHandler会显示其victoryPanel
	// 如果你仍想用它显示一个不同的UI，可以保留并取消注释下面的相关行
	public GameObject Cat;
	private MonoBehaviour targetScriptToDisable; // 重命名以更清晰其作用

	[Header("关卡完成逻辑处理器")]
	public LevelCompletionHandler levelCompletionHandler; // 将场景中挂载 LevelCompletionHandler 的对象拖到这里

	private void Start()
	{
		if (Cat != null)
		{
			// 尝试获取你希望禁用的特定脚本，例如 PlayerMovement, CatController 等
			// 如果不确定，MonoBehaviour 可以作为通用基类，但它会禁用第一个找到的 MonoBehaviour
			// targetScriptToDisable = Cat.GetComponent<YourCatScriptType>(); // 替换 YourCatScriptType
			targetScriptToDisable = Cat.GetComponent<MonoBehaviour>(); // 保持通用，但请注意其行为
			if (targetScriptToDisable == null)
			{
				Debug.LogWarning($"[PackageReceive] 在 Cat 对象上未能找到可禁用的 MonoBehaviour 脚本。", Cat);
			}
		}
		else
		{
			Debug.LogError("[PackageReceive] Cat 对象未在 Inspector 中赋值!", this);
		}

		// 如果 LevelCompletionHandler 会负责显示胜利UI，这里的 victoryUI 可能不需要在 Start 中处理了
		// if (victoryUI != null)
		// {
		//     victoryUI.SetActive(false);
		// }
		// else
		// {
		//     Debug.LogWarning("[PackageReceive] PackageReceive 脚本中的 Victory UI 未赋值（如果仍需使用）。", this);
		// }

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

		// LevelCompletionHandler.ShowVictoryPanel 已经处理了 Time.timeScale = 0f;
		// 和激活 LevelCompletionHandler.victoryPanel
		// 这里主要处理 PackageReceive 独有的逻辑

		if (targetScriptToDisable != null)
		{
			Debug.Log("[PackageReceive] 禁用 Cat 的目标脚本。");
			targetScriptToDisable.enabled = false;
		}
		else if (Cat != null)
		{
			Debug.LogWarning("[PackageReceive] 未能禁用 Cat 脚本，因为在Start中未能成功获取到 targetScriptToDisable。", Cat);
		}

		// 如果 PackageReceive 仍有自己的 victoryUI 需要显示 (与 LevelCompletionHandler 的不同或作为补充)
		// if (victoryUI != null && !victoryUI.activeSelf)
		// {
		//     victoryUI.SetActive(true);
		//     Debug.Log("[PackageReceive] PackageReceive 的 victoryUI 也被激活。");
		// }
	}
}