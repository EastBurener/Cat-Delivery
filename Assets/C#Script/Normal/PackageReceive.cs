using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageReceive : MonoBehaviour
{
    public GameDate_SO GameDate;
    public GameObject victoryUI;     // ����ʤ��UI��������ǰ���ã�
    public GameObject Cat;
    private MonoBehaviour targetScript;

    private void Start()
    {
        targetScript = Cat.GetComponent<MonoBehaviour>();
        if (victoryUI != null)
            victoryUI.SetActive(false); // ȷ����ʼʱ����ʤ��UI
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
            Time.timeScale = 0f; // ��ͣ��Ϸ����ѡ��
            targetScript.enabled = false;
        }
        else
        {
            Debug.LogWarning("Victory UIδ��ֵ��");
        }
    }
}
