using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickBag : MonoBehaviour
{
	public GameDate_SO GameDate;

	[Header("�ѵ�����")]
	[Tooltip("�ѵ���ʼ�㣨�豣������ת��")]
	public Transform stackAnchor;
	[Tooltip("��ֱ�ѵ����")]
	public float verticalSpacing = 0.02f;
	public KeyCode dropKey = KeyCode.R;
	[Tooltip("Ͷ����ʼ����")]
	public float throwForce = 1f;

	[Header("��������")]
	[Tooltip("���Я����������")]
	public int maxPackages = 5;

	[Header("��������")]
	[Tooltip("����������������ǧ�ˣ�")]
	public float weightPerPackage = 0.2f;

	[Header("UI��ʾ")]
	public Text packageCounterText;

	[Header("״̬���")]
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
			Debug.LogError("GameDate_SO δ�� PickBag �ű��и�ֵ!");
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
			Debug.LogError("GameDate_SO δ��ֵ���޷���Ӱ���!");
			return;
		}

		if (packageStack.Count >= maxPackages)
		{
			Debug.LogWarning("�Ѵ����Я������������");
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
		Debug.Log($"��ǰ�ռ���������{GameDate.ownPackage}/{maxPackages}");
	}

	Vector3 GetStackPosition(GameObject newPackage)
	{
		if (packageStack.Count == 0) return stackAnchor.position;

		GameObject topPackage = packageStack[^1];
		Collider2D topCollider = topPackage.GetComponent<Collider2D>();
		Collider2D newPackageCollider = newPackage.GetComponent<Collider2D>();

		if (topCollider == null || newPackageCollider == null)
		{
			Debug.LogWarning("������ȱ�� Collider2D �����ʹ�û���������");
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
			packageCounterText.text = $"������{GameDate.ownPackage}/{maxPackages}";
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