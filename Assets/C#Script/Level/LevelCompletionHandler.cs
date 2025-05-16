using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletionHandler : MonoBehaviour
{
	[Header("�ؿ�����")]
	public int currentLevelIndex; // ��Inspector��Ϊÿ���ؿ��������� (����: ��һ��Ϊ1, �ڶ���Ϊ2)
	public string levelSelectSceneName = "LevelSelectScene"; // ѡ�س���������

	[Header("UI ����")]
	public GameObject victoryPanel; // ����ʤ��UI�����ק�����ֶ�
	public GameObject stopDisappear;

	void Start()
	{
		if (victoryPanel != null)
		{
			victoryPanel.SetActive(false); // ��ʼ����
		}
		else
		{
			Debug.LogError($"[{gameObject.scene.name}] Victory Panel (��LevelCompletionHandler��) δָ��!", this);
		}
	}

	public void TriggerPlayerVictory()
	{
		Debug.Log($"[LCH - {gameObject.scene.name}] >>> TriggerPlayerVictory ����������! CurrentLevelIndex: {currentLevelIndex}");
		ProcessLevelCompletionLogic();
		ShowVictoryPanel(); // ���ô˷�������ʾUI����ͣ��Ϸ
	}

	private void ProcessLevelCompletionLogic()
	{
		Debug.Log($"[LCH - {gameObject.scene.name}] ��ʼ����ͨ���߼� - currentLevelIndex: {currentLevelIndex}");

		int maxLevelReachedPreviously = PlayerPrefs.GetInt(PlayerPrefsKeys.MaxLevelReached, 1); // Ĭ��ֵΪ1
		Debug.Log($"[LCH - {gameObject.scene.name}] ��PlayerPrefs��ȡ�� '{PlayerPrefsKeys.MaxLevelReached}' (��ֵ/Ĭ��ֵ): {maxLevelReachedPreviously}");

		int nextLevelIndexIfUnlocked = currentLevelIndex + 1;
		Debug.Log($"[LCH - {gameObject.scene.name}] ������� nextLevelIndexIfUnlocked: {nextLevelIndexIfUnlocked}");

		if (nextLevelIndexIfUnlocked > maxLevelReachedPreviously)
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.MaxLevelReached, nextLevelIndexIfUnlocked);
			PlayerPrefs.Save();
			Debug.Log($"[LCH - {gameObject.scene.name}] --- PlayerPrefs '{PlayerPrefsKeys.MaxLevelReached}' �Ѹ���Ϊ: {nextLevelIndexIfUnlocked} ---");
		}
		else
		{
			Debug.Log($"[LCH - {gameObject.scene.name}] �������PlayerPrefs��Next: {nextLevelIndexIfUnlocked}, PreviouslyReached: {maxLevelReachedPreviously} (���� {nextLevelIndexIfUnlocked} > {maxLevelReachedPreviously} ������)");
		}
	}

	private void ShowVictoryPanel()
	{
		if (victoryPanel != null)
		{
			Debug.Log($"[LCH - {gameObject.scene.name}] ��ʾʤ������ (����LevelCompletionHandler) ����ͣ��Ϸ��");
			victoryPanel.SetActive(true);
			stopDisappear.SetActive(false);
			Time.timeScale = 0f; // ������ͳһ����ʱ����ͣ
		}
		else
		{
			Debug.LogError($"[LCH - {gameObject.scene.name}] �޷���ʾʤ�����棬Victory Panel (��LevelCompletionHandler��) δָ����", this);
		}
	}

	// ���·������Ա�ʤ��UI����ϵİ�ť����
	public void GoToLevelSelectScreen()
	{
		Time.timeScale = 1f; // ȷ����Ϸʱ��ָ�
		if (string.IsNullOrEmpty(levelSelectSceneName))
		{
			Debug.LogError($"[LCH - {gameObject.scene.name}] ѡ�س�������δ��LevelCompletionHandler�����ã�", this);
			return;
		}
		SceneManager.LoadScene(levelSelectSceneName);
	}

	public void RetryCurrentLevel()
	{
		Time.timeScale = 1f; // ȷ����Ϸʱ��ָ�
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void GoToNextLevel()
	{
		Time.timeScale = 1f; // ȷ����Ϸʱ��ָ�
		int levelToLoadIndex = currentLevelIndex + 1;
		int maxLevelReachable = PlayerPrefs.GetInt(PlayerPrefsKeys.MaxLevelReached, 1);

		if (levelToLoadIndex < maxLevelReachable)
		{
			string nextSceneName = "Scene" + levelToLoadIndex; // ������ĳ������� "Scene1", "Scene2"
			if (Application.CanStreamedLevelBeLoaded(nextSceneName))
			{
				SceneManager.LoadScene(nextSceneName);
			}
			else
			{
				Debug.LogWarning($"[LCH - {gameObject.scene.name}] ���� '{nextSceneName}' (���� {levelToLoadIndex}) �޷����ء�����ѡ�ؽ��档");
				GoToLevelSelectScreen();
			}
		}
		else
		{
			Debug.LogWarning($"[LCH - {gameObject.scene.name}] ���Լ��ص���һ�� (���� {levelToLoadIndex}) δ�����򲻴��� (MaxReachable: {maxLevelReachable})������ѡ�ؽ��档");
			GoToLevelSelectScreen();
		}
	}
}