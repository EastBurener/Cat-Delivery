using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiPackageIndicator : MonoBehaviour
{
	public GameObject arrowPrefab; // ��ͷԤ���壬��Ҫ�� Inspector ��ָ��
	public RectTransform canvasRect; // Canvas �� RectTransform����Ҫ�� Inspector ��ָ��
	public float borderOffset = 50f; // ��ͷ������Ļ��Ե��ƫ����

	private Camera mainCamera;
	private List<GameObject> packages = new List<GameObject>(); // �洢���а���
	private Dictionary<GameObject, Image> packageArrows = new Dictionary<GameObject, Image>(); // �洢�����ͼ�ͷ�Ķ�Ӧ��ϵ

	void Start()
	{
		mainCamera = Camera.main;

		// ȷ�� arrowPrefab �� canvasRect ����ȷ��ֵ
		if (arrowPrefab == null)
		{
			Debug.LogError("���� Inspector ��Ϊ arrowPrefab ��ֵ");
			enabled = false; // ���ýű�
		}
		if (canvasRect == null)
		{
			Debug.LogError("���� Inspector ��Ϊ canvasRect ��ֵ");
			enabled = false; // ���ýű�
		}

		// �������д��� "Package" Tag �Ķ���
		GameObject[] packageArray = GameObject.FindGameObjectsWithTag("Package");
		packages.AddRange(packageArray);
	}

	void Update()
	{
		// �������а��������¼�ͷ״̬
		foreach (GameObject package in packages)
		{
			if (package == null) continue; // ������������٣�������

			// ���������û�ж�Ӧ�ļ�ͷ���򴴽�
			if (!packageArrows.ContainsKey(package))
			{
				CreateArrowForPackage(package);
			}

			UpdateArrowPosition(package, packageArrows[package]);
		}
	}

	// ����һ��������Ӧ�ļ�ͷ
	void CreateArrowForPackage(GameObject package)
	{
		GameObject arrowInstance = Instantiate(arrowPrefab, canvasRect);
		arrowInstance.transform.SetParent(canvasRect, false); // ȷ�����ź�λ����ȷ

		Image arrowImage = arrowInstance.GetComponent<Image>();
		if (arrowImage == null)
		{
			Debug.LogError("��ͷԤ������û�� Image ���!");
			Destroy(arrowInstance);
			return;
		}

		packageArrows.Add(package, arrowImage);
	}

	// ���¼�ͷ��λ�ú���ת
	void UpdateArrowPosition(GameObject package, Image arrowImage)
	{
		// ����������������ת��Ϊ��Ļ����
		Vector3 packageScreenPosition = mainCamera.WorldToViewportPoint(package.transform.position);

		// ����������������Ұ�ڣ������ؼ�ͷ
		if (packageScreenPosition.x > 0 && packageScreenPosition.x < 1 &&
			packageScreenPosition.y > 0 && packageScreenPosition.y < 1 &&
			packageScreenPosition.z > 0)
		{
			arrowImage.enabled = false;
			return;
		}

		arrowImage.enabled = true;

		// ����Ļ����ת��Ϊ Canvas ����
		Vector2 anchoredPosition;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, new Vector2(packageScreenPosition.x * Screen.width, packageScreenPosition.y * Screen.height), null, out anchoredPosition);

		// �� Canvas ������������Ļ��Ե
		Vector2 clampedPosition = anchoredPosition;
		clampedPosition.x = Mathf.Clamp(clampedPosition.x, canvasRect.rect.xMin + borderOffset, canvasRect.rect.xMax - borderOffset);
		clampedPosition.y = Mathf.Clamp(clampedPosition.y, canvasRect.rect.yMin + borderOffset, canvasRect.rect.yMax - borderOffset);

		// ���ü�ͷ��λ��
		arrowImage.rectTransform.anchoredPosition = clampedPosition;

		// �����ͷ��ת�Ƕȣ�ָ�����
		Vector2 direction = (anchoredPosition - canvasRect.rect.center).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		arrowImage.rectTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		// ���⴦�����������󷽵��������ͷָ������
		if (packageScreenPosition.z < 0)
		{
			arrowImage.rectTransform.anchoredPosition = Vector2.zero; // ����Canvas����
			arrowImage.rectTransform.rotation = Quaternion.identity; // ����ת
		}
	}

	// ������������ʱ���Ƴ���ͷ
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
