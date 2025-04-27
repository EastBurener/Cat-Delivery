using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiPackageIndicator : MonoBehaviour
{
	public GameObject arrowPrefab; // 箭头预制体，需要在 Inspector 中指定
	public RectTransform canvasRect; // Canvas 的 RectTransform，需要在 Inspector 中指定
	public float borderOffset = 50f; // 箭头距离屏幕边缘的偏移量

	private Camera mainCamera;
	private List<GameObject> packages = new List<GameObject>(); // 存储所有包裹
	private Dictionary<GameObject, Image> packageArrows = new Dictionary<GameObject, Image>(); // 存储包裹和箭头的对应关系

	void Start()
	{
		mainCamera = Camera.main;

		// 确保 arrowPrefab 和 canvasRect 已正确赋值
		if (arrowPrefab == null)
		{
			Debug.LogError("请在 Inspector 中为 arrowPrefab 赋值");
			enabled = false; // 禁用脚本
		}
		if (canvasRect == null)
		{
			Debug.LogError("请在 Inspector 中为 canvasRect 赋值");
			enabled = false; // 禁用脚本
		}

		// 查找所有带有 "Package" Tag 的对象
		GameObject[] packageArray = GameObject.FindGameObjectsWithTag("Package");
		packages.AddRange(packageArray);
	}

	void Update()
	{
		// 遍历所有包裹，更新箭头状态
		foreach (GameObject package in packages)
		{
			if (package == null) continue; // 如果包裹被销毁，则跳过

			// 如果包裹还没有对应的箭头，则创建
			if (!packageArrows.ContainsKey(package))
			{
				CreateArrowForPackage(package);
			}

			UpdateArrowPosition(package, packageArrows[package]);
		}
	}

	// 创建一个包裹对应的箭头
	void CreateArrowForPackage(GameObject package)
	{
		GameObject arrowInstance = Instantiate(arrowPrefab, canvasRect);
		arrowInstance.transform.SetParent(canvasRect, false); // 确保缩放和位置正确

		Image arrowImage = arrowInstance.GetComponent<Image>();
		if (arrowImage == null)
		{
			Debug.LogError("箭头预制体上没有 Image 组件!");
			Destroy(arrowInstance);
			return;
		}

		packageArrows.Add(package, arrowImage);
	}

	// 更新箭头的位置和旋转
	void UpdateArrowPosition(GameObject package, Image arrowImage)
	{
		// 将包裹的世界坐标转换为屏幕坐标
		Vector3 packageScreenPosition = mainCamera.WorldToViewportPoint(package.transform.position);

		// 如果包裹在摄像机视野内，则隐藏箭头
		if (packageScreenPosition.x > 0 && packageScreenPosition.x < 1 &&
			packageScreenPosition.y > 0 && packageScreenPosition.y < 1 &&
			packageScreenPosition.z > 0)
		{
			arrowImage.enabled = false;
			return;
		}

		arrowImage.enabled = true;

		// 将屏幕坐标转换为 Canvas 坐标
		Vector2 anchoredPosition;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, new Vector2(packageScreenPosition.x * Screen.width, packageScreenPosition.y * Screen.height), null, out anchoredPosition);

		// 将 Canvas 坐标限制在屏幕边缘
		Vector2 clampedPosition = anchoredPosition;
		clampedPosition.x = Mathf.Clamp(clampedPosition.x, canvasRect.rect.xMin + borderOffset, canvasRect.rect.xMax - borderOffset);
		clampedPosition.y = Mathf.Clamp(clampedPosition.y, canvasRect.rect.yMin + borderOffset, canvasRect.rect.yMax - borderOffset);

		// 设置箭头的位置
		arrowImage.rectTransform.anchoredPosition = clampedPosition;

		// 计算箭头旋转角度，指向包裹
		Vector2 direction = (anchoredPosition - canvasRect.rect.center).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		arrowImage.rectTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		// 特殊处理包裹在相机后方的情况，箭头指向中心
		if (packageScreenPosition.z < 0)
		{
			arrowImage.rectTransform.anchoredPosition = Vector2.zero; // 放在Canvas中心
			arrowImage.rectTransform.rotation = Quaternion.identity; // 不旋转
		}
	}

	// 当包裹被销毁时，移除箭头
	public void RemovePackage(GameObject package)
	{
		if (packageArrows.ContainsKey(package))
		{
			Image arrowImage = packageArrows[package];
			Destroy(arrowImage.gameObject);
			packageArrows.Remove(package);
		}
		packages.Remove(package);
	}
}
