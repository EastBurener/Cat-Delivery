using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickBag : MonoBehaviour
{
    public GameDate_SO GameDate; // ȷ����Inspector�и�ֵ

    [Header("�ѵ�����")]
    [Tooltip("�ѵ���ʼ�㣨�豣������ת��")]
    public Transform stackAnchor;
    [Tooltip("��ֱ�ѵ����")]
    public float verticalSpacing = 0.02f; // ������GameDate_SO���޶�Ӧ
    public KeyCode dropKey = KeyCode.R; // ������GameDate_SO���޶�Ӧ
    [Tooltip("Ͷ����ʼ����")]
    public float throwForce = 1f; // ������GameDate_SO�е�force��Vector2����;���ܲ�ͬ

    [Header("��������")]
    [Tooltip("����������������ǧ�ˣ�")]
    public float weightPerPackage = 0.2f; // ������GameDate_SO���޶�Ӧ

    [Header("UI��ʾ")]
    public Text packageCounterText;

    [Header("״̬���")]
    [SerializeField] private List<GameObject> packageStack = new List<GameObject>();
    [SerializeField] private List<GameObject> detectedPackages = new List<GameObject>();

    void Start()
    {
        // ��ʼ�����״̬��ȷ����ʼʱ������������Ϊ0
        if (GameDate != null)
        {
            GameDate.ownPackage = 0;
            GameDate.totalWeight = 0f; // GameDate_SO��totalWeightĬ��Ϊ1f����������Ϊ0
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

        foreach (var package in detectedPackages.ToArray()) // ToArray()�����ڵ���ʱ�޸��б�
        {
            if (!packageStack.Contains(package))
            {
                AddToStack(package);
                detectedPackages.Remove(package);
                break; // ÿ�γ��Զѵ�һ��
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
        GameDate.ownPackage = packageStack.Count; // ���� GameDate �еĳ��а�����
        GameDate.totalWeight += weightPerPackage; // ����������
        UpdatePackageCounter();
        Debug.Log($"��ǰ�ռ���������{GameDate.ownPackage}"); // ʹ�� GameDate.ownPackage
    }

    Vector3 GetStackPosition(GameObject newPackage)
    {
        if (packageStack.Count == 0) return stackAnchor.position; // �������Add֮ǰ�ĵ�һ����������packageStack��������һ��Ԫ�أ�

        GameObject topPackage = packageStack[^1]; // ��ȡ��ǰջ���İ��� (List�����һ��Ԫ��)
        Collider2D topCollider = topPackage.GetComponent<Collider2D>();
        Collider2D newPackageCollider = newPackage.GetComponent<Collider2D>();

        if (topCollider == null || newPackageCollider == null)
        {
            Debug.LogWarning("������ȱ�� Collider2D ������޷���ȷ����ѵ�λ�ã���ʹ�û�����ࡣ");
            // �ṩһ�����������ͼ��Ļ��˼���
            return new Vector3(
                stackAnchor.position.x,
                stackAnchor.position.y + (packageStack.Count * (newPackageCollider != null ? newPackageCollider.bounds.size.y : 0.1f) + (packageStack.Count * verticalSpacing)), // �򻯵�Yλ��
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

        GameDate.totalWeight -= weightPerPackage; // ����������
        if (GameDate.totalWeight < 0) GameDate.totalWeight = 0; // ȷ�����ز�Ϊ��
        GameDate.ownPackage = packageStack.Count; // ���� GameDate �еĳ��а�����

        UpdatePackageCounter();

        top.transform.SetParent(null);
        Rigidbody2D rb = top.GetComponent<Rigidbody2D>();
        Collider2D col = top.GetComponent<Collider2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
            float direction = Mathf.Sign(transform.localScale.x); // �����ɫͨ��localScale.x��ת
            rb.AddForce(new Vector2(
                direction * throwForce, // ʹ�ñ��ص� throwForce
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
            // UI��ʾʹ�� GameDate �е� ownPackage �� totalWeight
            packageCounterText.text = $"��������{GameDate.ownPackage}\n��������{GameDate.totalWeight:F2}kg"; // F2��ʽ��Ϊ��λС��
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Package") && !packageStack.Contains(other.gameObject) && !detectedPackages.Contains(other.gameObject))
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
                if (col != null) // ���col�Ƿ�����Ա������
                {
                    Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
                }
            }
        }
    }
}