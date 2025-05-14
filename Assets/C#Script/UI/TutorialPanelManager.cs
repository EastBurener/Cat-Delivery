using UnityEngine;
using UnityEngine.UI; // 需要此命名空间来处理 UI.Button

public class TutorialPanelManager : MonoBehaviour
{
	[Header("UI Elements")]
	public GameObject tutorialPanelObject; // 将您的教程面板GameObject拖拽到此处
	public UnityEngine.UI.Button closeButton; // 将您的关闭按钮拖拽到此处


	public CatMove catMoveScript;                 // 将带有CatMove脚本的GameObject拖拽到此处
	public CountdownTimer countdownTimerScript;   // 将带有CountdownTimer脚本的GameObject拖拽到此处

	void Awake()
	{
		// 确保必要的引用已经设置
		if (tutorialPanelObject == null)
		{
			// 如果此脚本就挂载在教程面板GameObject上，
			// 那么隐藏时可以直接用 this.gameObject.SetActive(false)
			// 为保持通用性，我们明确使用 tutorialPanelObject 来控制显隐
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

	// （可选）当此GameObject变为非激活状态时调用
	// 如果面板是通过点击关闭按钮隐藏的，OnCloseButtonClicked 已经处理了脚本的重新启用。
	// 如果面板可能通过其他方式被隐藏（例如父对象被禁用），并且您希望在这种情况下也重新启用脚本，
	// 可以在这里添加 EnableTargetScripts();
	// void OnDisable()
	// {
	//     // 考虑是否需要在非按钮关闭的情况下也启用脚本
	//     // EnableTargetScripts();
	// }

	/// <summary>
	/// 禁用目标脚本。
	/// </summary>
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

	/// <summary>
	/// 启用目标脚本。
	/// </summary>
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
		// 此处不打印警告，因为脚本可能并非由本面板禁用

		if (countdownTimerScript != null)
		{
			if (!countdownTimerScript.enabled) // 仅在脚本当前禁用时才启用
			{
				countdownTimerScript.enabled = true;
				Debug.Log("CountdownTimer 脚本已重新启用。");
			}
		}
	}

	/// <summary>
	/// 当关闭按钮被点击时调用的方法。
	/// </summary>
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
			// 当 tutorialPanelObject.SetActive(false) 执行后，如果此脚本挂载在该对象上，
			// OnDisable() 方法会被调用。
		}
	}
}