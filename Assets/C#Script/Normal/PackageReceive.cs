using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageReceive : MonoBehaviour
{
    public GameDate_SO GameDate;
    public GameObject victoryUI;     // 拖入胜利UI对象（需提前禁用）
    public GameObject Cat;
    private MonoBehaviour targetScript;

    private void Start()
    {
        targetScript = Cat.GetComponent<MonoBehaviour>();
        if (victoryUI != null)
            victoryUI.SetActive(false); // 确保开始时隐藏胜利UI
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Package"))
        {
            GameDate.givePackage++;
            Destroy(other.gameObject);

            if (GameDate.givePackage >= GameDate.allPackage)
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
            targetScript.enabled = false;
        }
        else
        {
            Debug.LogWarning("Victory UI未赋值！");
        }
    }
}
