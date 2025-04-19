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
    public float throwForce = 10f;

    [Header("��������")]
    [Tooltip("����������������ǧ�ˣ�")]
    public float weightPerPackage = 5;

    [Header("UI��ʾ")]
    public Text packageCounterText;

    [Header("״̬���")]
    [SerializeField] private List<GameObject> packageStack = new List<GameObject>();
    [SerializeField] private List<GameObject> detectedPackages = new List<GameObject>();


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
            if (!packageStack.Contains(package))
            {
                AddToStack(package);
                detectedPackages.Remove(package);
                break;
            }
        }
    }

    void AddToStack(GameObject package)
    {
        Vector3 originalScale = package.transform.lossyScale;
        float originalZRot = package.transform.eulerAngles.z;

        Rigidbody2D rb = package.GetComponent<Rigidbody2D>();
        Collider2D col = package.GetComponent<Collider2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        col.enabled = false;

        package.transform.SetParent(stackAnchor);

        // �������ŵĺ��Ĵ���
        Vector3 parentScale = stackAnchor.lossyScale;
        package.transform.localScale = new Vector3(
            originalScale.x / parentScale.x,
            originalScale.y / parentScale.y,
            originalScale.z / parentScale.z
        );

        package.transform.position = GetStackPosition(package);
        package.transform.localRotation = Quaternion.Euler(0, 0, originalZRot);

        packageStack.Add(package);
        GameDate.totalWeight += weightPerPackage;
        UpdatePackageCounter();
        Debug.Log($"��ǰ�ռ���������{packageStack.Count}");
    }

    Vector3 GetStackPosition(GameObject newPackage)
    {
        if (packageStack.Count == 0) return stackAnchor.position;

        GameObject topPackage = packageStack[^1];
        Bounds topBounds = topPackage.GetComponent<Collider2D>().bounds;
        Bounds newBounds = newPackage.GetComponent<Collider2D>().bounds;

        float newY = topBounds.max.y + newBounds.extents.y + verticalSpacing;

        return new Vector3(
            stackAnchor.position.x,
            newY,
            stackAnchor.position.z
        );
    }

    void DropTopPackage()
    {
        if (packageStack.Count == 0) return;

        GameObject top = packageStack[^1];
        packageStack.RemoveAt(packageStack.Count - 1);
        GameDate.totalWeight -= weightPerPackage;
        UpdatePackageCounter();

        top.transform.SetParent(null);
        Rigidbody2D rb = top.GetComponent<Rigidbody2D>();
        Collider2D col = top.GetComponent<Collider2D>();
        rb.isKinematic = false;
        col.enabled = true;

        float direction = Mathf.Sign(transform.localScale.x);
        rb.AddForce(new Vector2(
            direction * throwForce,
            throwForce * 0.3f
        ), ForceMode2D.Impulse);
    }

    void UpdatePackageCounter()
    {
        if (packageCounterText != null)
        {
            packageCounterText.text = $"��������{packageStack.Count}\n��������{GameDate.totalWeight}kg";
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Package") && !packageStack.Contains(other.gameObject))
            detectedPackages.Add(other.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Package"))
            detectedPackages.Remove(other.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var package in packageStack)
        {
            if (package != null)
            {
                Collider2D col = package.GetComponent<Collider2D>();
                Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
            }
        }
    }
}
