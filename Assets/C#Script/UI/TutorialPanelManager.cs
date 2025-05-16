using UnityEngine;
using UnityEngine.UI; 

public class TutorialPanelManager : MonoBehaviour
{
	[Header("UI Elements")]
	public GameObject tutorialPanelObject; // 将教程面板GameObject拖拽到此处
	public UnityEngine.UI.Button closeButton; // 将关闭按钮拖拽到此处


	public CatMove catMoveScript;                 // 将带有CatMove脚本的GameObject拖拽到此处
	public CountdownTimer countdownTimerScript;   // 将带有CountdownTimer脚本的GameObject拖拽到此处

	void Awake()
	{
		// 确保必要的引用已经设置
		if (tutorialPanelObject == null)
		{
			Debug.LogError("错误：教程面板对象 (Tutorial Panel Object) 未在TutorialPanelManager中指定！", this);
			enabled = false; // 禁用此脚本以防止后续错误
			return;
		}

		if (closeButton == null)
		{
			Debug.LogError("错误：关闭按钮 (Close Button) 未在TutorialPanelManager中指定！", this);
			enabled = false; // 禁用此脚本
			return;
		}

		// 为关闭按钮的点击事件添加监听器
		closeButton.onClick.AddListener(OnCloseButtonClicked); 

	}

	// 当此GameObject（以及附加的此脚本）变为激活状态时调用
	// 这通常意味着教程面板显示出来了
	void OnEnable()
	{
		DisableTargetScripts();
	}

	private void DisableTargetScripts()
	{
		if (catMoveScript != null)
		{
			if (catMoveScript.enabled) // 仅在脚本当前启用时才禁用
			{
				catMoveScript.enabled = false; 

				Debug.Log("CatMove 脚本已禁用 (因为教程面板已激活)。");
			}
		}
		else
		{
			Debug.LogWarning("警告：CatMove 脚本引用未设置，无法禁用。");
		}

		if (countdownTimerScript != null)
		{
			if (countdownTimerScript.enabled) // 仅在脚本当前启用时才禁用
			{
				countdownTimerScript.enabled = false; 

				Debug.Log("CountdownTimer 脚本已禁用 (因为教程面板已激活)。");
			}
		}
		else
		{
			Debug.LogWarning("警告：CountdownTimer 脚本引用未设置，无法禁用。");
		}
	}

	private void EnableTargetScripts()
	{
		if (catMoveScript != null)
		{
			if (!catMoveScript.enabled) // 仅在脚本当前禁用时才启用
			{
				catMoveScript.enabled = true;
				Debug.Log("CatMove 脚本已重新启用。");
			}
		}

		if (countdownTimerScript != null)
		{
			if (!countdownTimerScript.enabled) // 仅在脚本当前禁用时才启用
			{
				countdownTimerScript.enabled = true;
				Debug.Log("CountdownTimer 脚本已重新启用。");
			}
		}
	}

	public void OnCloseButtonClicked()
	{
		Debug.Log("关闭按钮被点击。");

		// 步骤 1: 重新启用之前被此面板禁用的脚本
		EnableTargetScripts();

		// 步骤 2: 隐藏教程面板
		if (tutorialPanelObject != null)
		{
			tutorialPanelObject.SetActive(false); 

			Debug.Log("教程面板已关闭。");
		}
	}
}