using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletionHandler : MonoBehaviour
{
	[Header("关卡设置")]
	public int currentLevelIndex; // 在Inspector中为每个关卡场景设置 (例如: 第一关为1, 第二关为2)
	public string levelSelectSceneName = "LevelSelectScene"; // 选关场景的名称

	[Header("UI 引用")]
	public GameObject victoryPanel; // 将主胜利UI面板拖拽到此字段
	public GameObject stopDisappear;

	void Start()
	{
		if (victoryPanel != null)
		{
			victoryPanel.SetActive(false); // 初始隐藏
		}
		else
		{
			Debug.LogError($"[{gameObject.scene.name}] Victory Panel (在LevelCompletionHandler中) 未指定!", this);
		}
	}

	public void TriggerPlayerVictory()
	{
		Debug.Log($"[LCH - {gameObject.scene.name}] >>> TriggerPlayerVictory 方法被调用! CurrentLevelIndex: {currentLevelIndex}");
		ProcessLevelCompletionLogic();
		ShowVictoryPanel(); // 调用此方法来显示UI和暂停游戏
	}

	private void ProcessLevelCompletionLogic()
	{
		Debug.Log($"[LCH - {gameObject.scene.name}] 开始处理通关逻辑 - currentLevelIndex: {currentLevelIndex}");

		int maxLevelReachedPreviously = PlayerPrefs.GetInt(PlayerPrefsKeys.MaxLevelReached, 1); // 默认值为1
		Debug.Log($"[LCH - {gameObject.scene.name}] 从PlayerPrefs读取的 '{PlayerPrefsKeys.MaxLevelReached}' (旧值/默认值): {maxLevelReachedPreviously}");

		int nextLevelIndexIfUnlocked = currentLevelIndex + 1;
		Debug.Log($"[LCH - {gameObject.scene.name}] 计算出的 nextLevelIndexIfUnlocked: {nextLevelIndexIfUnlocked}");

		if (nextLevelIndexIfUnlocked > maxLevelReachedPreviously)
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.MaxLevelReached, nextLevelIndexIfUnlocked);
			PlayerPrefs.Save();
			Debug.Log($"[LCH - {gameObject.scene.name}] --- PlayerPrefs '{PlayerPrefsKeys.MaxLevelReached}' 已更新为: {nextLevelIndexIfUnlocked} ---");
		}
		else
		{
			Debug.Log($"[LCH - {gameObject.scene.name}] 无需更新PlayerPrefs。Next: {nextLevelIndexIfUnlocked}, PreviouslyReached: {maxLevelReachedPreviously} (条件 {nextLevelIndexIfUnlocked} > {maxLevelReachedPreviously} 不满足)");
		}
	}

	private void ShowVictoryPanel()
	{
		if (victoryPanel != null)
		{
			Debug.Log($"[LCH - {gameObject.scene.name}] 显示胜利界面 (来自LevelCompletionHandler) 并暂停游戏。");
			victoryPanel.SetActive(true);
			stopDisappear.SetActive(false);
			Time.timeScale = 0f; // 在这里统一处理时间暂停
		}
		else
		{
			Debug.LogError($"[LCH - {gameObject.scene.name}] 无法显示胜利界面，Victory Panel (在LevelCompletionHandler中) 未指定！", this);
		}
	}

	// 以下方法可以被胜利UI面板上的按钮调用
	public void GoToLevelSelectScreen()
	{
		Time.timeScale = 1f; // 确保游戏时间恢复
		if (string.IsNullOrEmpty(levelSelectSceneName))
		{
			Debug.LogError($"[LCH - {gameObject.scene.name}] 选关场景名称未在LevelCompletionHandler中设置！", this);
			return;
		}
		SceneManager.LoadScene(levelSelectSceneName);
	}

	public void RetryCurrentLevel()
	{
		Time.timeScale = 1f; // 确保游戏时间恢复
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void GoToNextLevel()
	{
		Time.timeScale = 1f; // 确保游戏时间恢复
		int levelToLoadIndex = currentLevelIndex + 1;
		int maxLevelReachable = PlayerPrefs.GetInt(PlayerPrefsKeys.MaxLevelReached, 1);

		if (levelToLoadIndex < maxLevelReachable)
		{
			string nextSceneName = "Scene" + levelToLoadIndex; // 根据你的场景命名 "Scene1", "Scene2"
			if (Application.CanStreamedLevelBeLoaded(nextSceneName))
			{
				SceneManager.LoadScene(nextSceneName);
			}
			else
			{
				Debug.LogWarning($"[LCH - {gameObject.scene.name}] 场景 '{nextSceneName}' (索引 {levelToLoadIndex}) 无法加载。返回选关界面。");
				GoToLevelSelectScreen();
			}
		}
		else
		{
			Debug.LogWarning($"[LCH - {gameObject.scene.name}] 尝试加载的下一关 (索引 {levelToLoadIndex}) 未解锁或不存在 (MaxReachable: {maxLevelReachable})。返回选关界面。");
			GoToLevelSelectScreen();
		}
	}
}