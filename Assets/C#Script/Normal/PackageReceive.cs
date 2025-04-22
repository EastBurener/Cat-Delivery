using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageReceive : MonoBehaviour
{
    public int allPackage;          // 需要运送的总包裹数（在Inspector设置）
    public GameObject victoryUI;     // 拖入胜利UI对象（需提前禁用）
    private int package;

    private void Start()
    {
        if (victoryUI != null)
            victoryUI.SetActive(false); // 确保开始时隐藏胜利UI
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Package"))
        {
            package++;
            Destroy(other.gameObject);

            if (package >= allPackage)
            {
                ShowVictoryUI();
            }
        }
    }

    private void ShowVictoryUI()
    {
        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
            Time.timeScale = 0f; // 暂停游戏（可选）
        }
        else
        {
            Debug.LogWarning("Victory UI未赋值！");
        }
    }
}
