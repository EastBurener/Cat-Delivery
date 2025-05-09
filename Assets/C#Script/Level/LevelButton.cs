// Scripts/UI/LevelButton.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 用于 UnityEngine.UI.Button
using TMPro;          // 如果你用的是 TextMeshPro

public class LevelButton : MonoBehaviour
{
	[Header("关卡信息")]
	public int levelIndex;
	public string sceneNameToLoad;

	[Header("UI组件")]
	public UnityEngine.UI.Button buttonComponent; // 明确类型
	public TMPro.TextMeshProUGUI levelText;       // 明确类型 (如果使用TMPro)
												  // 如果不用TMPro, 请改为: public UnityEngine.UI.Text levelText;
	public GameObject lockIcon;

	private bool isLocked = true;

	void Start()
	{
		if (buttonComponent == null)
		{
			buttonComponent = GetComponent<UnityEngine.UI.Button>();
		}
		if (levelText == null)
		{
			levelText = GetComponentInChildren<TMPro.TextMeshProUGUI>(); // 如果不用TMPro, 改为 GetComponentInChildren<UnityEngine.UI.Text>();
		}

		if (buttonComponent == null)
		{
			Debug.LogError($"LevelButton on '{gameObject.name}' is missing a UnityEngine.UI.Button component.", this);
			return;
		}

		UpdateButtonState();
		buttonComponent.onClick.AddListener(OnButtonClick);
	}

	void OnEnable()
	{
		if (buttonComponent != null)
		{
			Debug.Log($"[LevelButton - 按钮代表关卡: {levelIndex}] OnEnable - 调用 UpdateButtonState");
			UpdateButtonState();
		}
	}

	public void UpdateButtonState()
	{
		if (levelText != null)
		{
			levelText.text = levelIndex.ToString();
		}

		int maxLevelReached = PlayerPrefs.GetInt(PlayerPrefsKeys.MaxLevelReached, 1); // 默认值为1
		Debug.Log($"[LevelButton - 按钮代表关卡: {levelIndex}] UpdateButtonState - 从PlayerPrefs读取的 '{PlayerPrefsKeys.MaxLevelReached}': {maxLevelReached}");

		if (levelIndex <= maxLevelReached)
		{
			isLocked = false;
			if (buttonComponent != null) buttonComponent.interactable = true;
			if (lockIcon != null) lockIcon.SetActive(false);
			Debug.Log($"[LevelButton - 按钮代表关卡: {levelIndex}] 状态更新为: UNLOCKED (条件: {levelIndex} <= {maxLevelReached})");
		}
		else
		{
			isLocked = true;
			if (buttonComponent != null) buttonComponent.interactable = false;
			if (lockIcon != null) lockIcon.SetActive(true);
			Debug.Log($"[LevelButton - 按钮代表关卡: {levelIndex}] 状态更新为: LOCKED (条件: {levelIndex} > {maxLevelReached})");
		}
	}

	void OnButtonClick()
	{
		Debug.Log($"[LevelButton - 按钮代表关卡: {levelIndex}] OnButtonClick - isLocked: {isLocked}, sceneNameToLoad: '{sceneNameToLoad}'");

		if (!isLocked && !string.IsNullOrEmpty(sceneNameToLoad))
		{
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneNameToLoad);
		}
		else if (isLocked)
		{
			Debug.LogWarning($"[LevelButton - 按钮代表关卡: {levelIndex}] 尝试点击锁定的关卡。");
		}
		else if (string.IsNullOrEmpty(sceneNameToLoad))
		{
			Debug.LogError($"[LevelButton - 按钮代表关卡: {levelIndex}] 场景名称未设置！", this);
		}
	}
}