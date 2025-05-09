using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickBag : MonoBehaviour
{
    public GameDate_SO GameDate; // 确保在Inspector中赋值

    [Header("堆叠设置")]
    [Tooltip("堆叠起始点（需保持零旋转）")]
    public Transform stackAnchor;
    [Tooltip("垂直堆叠间距")]
    public float verticalSpacing = 0.02f; // 保留，GameDate_SO中无对应
    public KeyCode dropKey = KeyCode.R; // 保留，GameDate_SO中无对应
    [Tooltip("投掷初始力量")]
    public float throwForce = 1f; // 保留，GameDate_SO中的force是Vector2，用途可能不同

    [Header("重量设置")]
    [Tooltip("单个包裹的重量（千克）")]
    public float weightPerPackage = 0.2f; // 保留，GameDate_SO中无对应

    [Header("UI显示")]
    public Text packageCounterText;

    [Header("状态监测")]
    [SerializeField] private List<GameObject> packageStack = new List<GameObject>();
    [SerializeField] private List<GameObject> detectedPackages = new List<GameObject>();

    void Start()
    {
        // 初始化玩家状态，确保开始时包裹数和重量为0
        if (GameDate != null)
        {
            GameDate.ownPackage = 0;
            GameDate.totalWeight = 0f; // GameDate_SO中totalWeight默认为1f，这里重置为0
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

        foreach (var package in detectedPackages.ToArray()) // ToArray()允许在迭代时修改列表
        {
            if (!packageStack.Contains(package))
            {
                AddToStack(package);
                detectedPackages.Remove(package);
                break; // 每次尝试堆叠一个
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
        GameDate.ownPackage = packageStack.Count; // 更新 GameDate 中的持有包裹数
        GameDate.totalWeight += weightPerPackage; // 增加总重量
        UpdatePackageCounter();
        Debug.Log($"当前收集包裹数：{GameDate.ownPackage}"); // 使用 GameDate.ownPackage
    }

    Vector3 GetStackPosition(GameObject newPackage)
    {
        if (packageStack.Count == 0) return stackAnchor.position; // 如果这是Add之前的第一个包裹（即packageStack即将包含一个元素）

        GameObject topPackage = packageStack[^1]; // 获取当前栈顶的包裹 (List中最后一个元素)
        Collider2D topCollider = topPackage.GetComponent<Collider2D>();
        Collider2D newPackageCollider = newPackage.GetComponent<Collider2D>();

        if (topCollider == null || newPackageCollider == null)
        {
            Debug.LogWarning("包裹上缺少 Collider2D 组件，无法精确计算堆叠位置！将使用基础间距。");
            // 提供一个基于数量和间距的回退计算
            return new Vector3(
                stackAnchor.position.x,
                stackAnchor.position.y + (packageStack.Count * (newPackageCollider != null ? newPackageCollider.bounds.size.y : 0.1f) + (packageStack.Count * verticalSpacing)), // 简化的Y位置
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

        GameDate.totalWeight -= weightPerPackage; // 减少总重量
        if (GameDate.totalWeight < 0) GameDate.totalWeight = 0; // 确保总重不为负
        GameDate.ownPackage = packageStack.Count; // 更新 GameDate 中的持有包裹数

        UpdatePackageCounter();

        top.transform.SetParent(null);
        Rigidbody2D rb = top.GetComponent<Rigidbody2D>();
        Collider2D col = top.GetComponent<Collider2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
            float direction = Mathf.Sign(transform.localScale.x); // 假设角色通过localScale.x翻转
            rb.AddForce(new Vector2(
                direction * throwForce, // 使用本地的 throwForce
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
            // UI显示使用 GameDate 中的 ownPackage 和 totalWeight
            packageCounterText.text = $"包裹数：{GameDate.ownPackage}\n总重量：{GameDate.totalWeight:F2}kg"; // F2格式化为两位小数
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
                if (col != null) // 检查col是否存在以避免错误
                {
                    Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
                }
            }
        }
    }
}