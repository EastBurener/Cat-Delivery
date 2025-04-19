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
    public float throwForce = 10f;

    [Header("重量设置")]
    [Tooltip("单个包裹的重量（千克）")]
    public float weightPerPackage = 5;

    [Header("UI显示")]
    public Text packageCounterText;

    [Header("状态监测")]
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

        // 修正缩放的核心代码
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
        Debug.Log($"当前收集包裹数：{packageStack.Count}");
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
            packageCounterText.text = $"包裹数：{packageStack.Count}\n总重量：{GameDate.totalWeight}kg";
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
