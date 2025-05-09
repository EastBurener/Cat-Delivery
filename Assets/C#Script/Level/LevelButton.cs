// Scripts/UI/LevelButton.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // ���� UnityEngine.UI.Button
using TMPro;          // ������õ��� TextMeshPro

public class LevelButton : MonoBehaviour
{
	[Header("�ؿ���Ϣ")]
	public int levelIndex;
	public string sceneNameToLoad;

	[Header("UI���")]
	public UnityEngine.UI.Button buttonComponent; // ��ȷ����
	public TMPro.TextMeshProUGUI levelText;       // ��ȷ���� (���ʹ��TMPro)
												  // �������TMPro, ���Ϊ: public UnityEngine.UI.Text levelText;
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
			levelText = GetComponentInChildren<TMPro.TextMeshProUGUI>(); // �������TMPro, ��Ϊ GetComponentInChildren<UnityEngine.UI.Text>();
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
			Debug.Log($"[LevelButton - ��ť����ؿ�: {levelIndex}] OnEnable - ���� UpdateButtonState");
			UpdateButtonState();
		}
	}

	public void UpdateButtonState()
	{
		if (levelText != null)
		{
			levelText.text = levelIndex.ToString();
		}

		int maxLevelReached = PlayerPrefs.GetInt(PlayerPrefsKeys.MaxLevelReached, 1); // Ĭ��ֵΪ1
		Debug.Log($"[LevelButton - ��ť����ؿ�: {levelIndex}] UpdateButtonState - ��PlayerPrefs��ȡ�� '{PlayerPrefsKeys.MaxLevelReached}': {maxLevelReached}");

		if (levelIndex <= maxLevelReached)
		{
			isLocked = false;
			if (buttonComponent != null) buttonComponent.interactable = true;
			if (lockIcon != null) lockIcon.SetActive(false);
			Debug.Log($"[LevelButton - ��ť����ؿ�: {levelIndex}] ״̬����Ϊ: UNLOCKED (����: {levelIndex} <= {maxLevelReached})");
		}
		else
		{
			isLocked = true;
			if (buttonComponent != null) buttonComponent.interactable = false;
			if (lockIcon != null) lockIcon.SetActive(true);
			Debug.Log($"[LevelButton - ��ť����ؿ�: {levelIndex}] ״̬����Ϊ: LOCKED (����: {levelIndex} > {maxLevelReached})");
		}
	}

	void OnButtonClick()
	{
		Debug.Log($"[LevelButton - ��ť����ؿ�: {levelIndex}] OnButtonClick - isLocked: {isLocked}, sceneNameToLoad: '{sceneNameToLoad}'");

		if (!isLocked && !string.IsNullOrEmpty(sceneNameToLoad))
		{
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneNameToLoad);
		}
		else if (isLocked)
		{
			Debug.LogWarning($"[LevelButton - ��ť����ؿ�: {levelIndex}] ���Ե�������Ĺؿ���");
		}
		else if (string.IsNullOrEmpty(sceneNameToLoad))
		{
			Debug.LogError($"[LevelButton - ��ť����ؿ�: {levelIndex}] ��������δ���ã�", this);
		}
	}
}