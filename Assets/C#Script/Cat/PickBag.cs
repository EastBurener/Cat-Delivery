using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickBag : MonoBehaviour
{
	public GameDate_SO GameDate;

	[Header("堆叠设置")]
	[Tooltip("堆叠起始点（需保持零旋转）")]
	public Transform stackAnchor;
	[Tooltip("垂直堆叠间距")]
	public float verticalSpacing = 0.02f;
	public KeyCode dropKey = KeyCode.R;
	[Tooltip("投掷初始力量")]
	public float throwForce = 1f;

	[Header("包裹设置")]
	[Tooltip("最大携带包裹数量")]
	public int maxPackages = 5;

	[Header("重量设置")]
	[Tooltip("单个包裹的重量（千克）")]
	public float weightPerPackage = 0.2f;

	[Header("UI显示")]
	public Text packageCounterText;

	[Header("状态监测")]
	[SerializeField] private List<GameObject> packageStack = new List<GameObject>();
	[SerializeField] private List<GameObject> detectedPackages = new List<GameObject>();

	void Start()
	{
		if (GameDate != null)
		{
			GameDate.ownPackage = 0;
			GameDate.totalWeight = 0f;
			UpdatePackageCounter();
		}
		else
		{
			Debug.LogError("GameDate_SO 未在 PickBag 脚本中赋值!");
		}
	}

	void Update()
	{
		TryAutoStack();
		if (Input.GetKeyDown(dropKey)) DropTopPackage();
	}

	void TryAutoStack()
	{
		if (detectedPackages.Count == 0) return;

		foreach (var package in detectedPackages.ToArray())
		{
			if (!packageStack.Contains(package) && packageStack.Count < maxPackages)
			{
				AddToStack(package);
				detectedPackages.Remove(package);
				break;
			}
		}
	}

	void AddToStack(GameObject package)
	{
		if (GameDate == null)
		{
			Debug.LogError("GameDate_SO 未赋值，无法添加包裹!");
			return;
		}

		if (packageStack.Count >= maxPackages)
		{
			Debug.LogWarning("已达最大携带包裹数量！");
			return;
		}

		Vector3 originalScale = package.transform.lossyScale;
		float originalZRot = package.transform.eulerAngles.z;

		Rigidbody2D rb = package.GetComponent<Rigidbody2D>();
		Collider2D col = package.GetComponent<Collider2D>();
		if (rb != null)
		{
			rb.velocity = Vector2.zero;
			rb.angularVelocity = 0f;
			rb.isKinematic = true;
		}
		if (col != null)
		{
			col.enabled = false;
		}

		package.transform.SetParent(stackAnchor);

		Vector3 parentScale = stackAnchor.lossyScale;
		package.transform.localScale = new Vector3(
			originalScale.x / parentScale.x,
			originalScale.y / parentScale.y,
			originalScale.z / parentScale.z
		);

		package.transform.position = GetStackPosition(package);
		package.transform.localRotation = Quaternion.Euler(0, 0, originalZRot);

		packageStack.Add(package);
		GameDate.ownPackage = packageStack.Count;
		GameDate.totalWeight += weightPerPackage;
		UpdatePackageCounter();
		Debug.Log($"当前收集包裹数：{GameDate.ownPackage}/{maxPackages}");
	}

	Vector3 GetStackPosition(GameObject newPackage)
	{
		if (packageStack.Count == 0) return stackAnchor.position;

		GameObject topPackage = packageStack[^1];
		Collider2D topCollider = topPackage.GetComponent<Collider2D>();
		Collider2D newPackageCollider = newPackage.GetComponent<Collider2D>();

		if (topCollider == null || newPackageCollider == null)
		{
			Debug.LogWarning("包裹上缺少 Collider2D 组件，使用基础间距计算");
			return new Vector3(
				stackAnchor.position.x,
				stackAnchor.position.y + (packageStack.Count * (newPackageCollider?.bounds.size.y ?? 0.1f) + (packageStack.Count * verticalSpacing)),
				stackAnchor.position.z
			);
		}

		Bounds topBounds = topCollider.bounds;
		Bounds newBounds = newPackageCollider.bounds;

		float newY = topBounds.max.y + newBounds.extents.y + verticalSpacing;

		return new Vector3(
			stackAnchor.position.x,
			newY,
			stackAnchor.position.z
		);
	}

	public void DropTopPackage()
	{
		if (GameDate == null || GameDate.ownPackage == 0) return;

		GameObject top = packageStack[^1];
		packageStack.RemoveAt(packageStack.Count - 1);

		GameDate.totalWeight -= weightPerPackage;
		if (GameDate.totalWeight < 0) GameDate.totalWeight = 0;
		GameDate.ownPackage = packageStack.Count;

		UpdatePackageCounter();

		top.transform.SetParent(null);
		Rigidbody2D rb = top.GetComponent<Rigidbody2D>();
		Collider2D col = top.GetComponent<Collider2D>();
		if (rb != null)
		{
			rb.isKinematic = false;
			float direction = Mathf.Sign(transform.localScale.x);
			rb.AddForce(new Vector2(
				direction * throwForce,
				throwForce * 0.3f
			), ForceMode2D.Impulse);
		}
		if (col != null)
		{
			col.enabled = true;
		}
	}

	void UpdatePackageCounter()
	{
		if (packageCounterText != null && GameDate != null)
		{
			packageCounterText.text = $"包裹：{GameDate.ownPackage}/{maxPackages}";
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Package") &&
			!packageStack.Contains(other.gameObject) &&
			!detectedPackages.Contains(other.gameObject))
		{
			detectedPackages.Add(other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Package"))
		{
			detectedPackages.Remove(other.gameObject);
		}
	}

	void OnDrawGizmosSelected()
	{
		if (stackAnchor == null) return;
		Gizmos.color = Color.green;
		foreach (var package in packageStack)
		{
			if (package != null)
			{
				Collider2D col = package.GetComponent<Collider2D>();
				if (col != null)
				{
					Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
				}
			}
		}
	}
}