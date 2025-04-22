using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageReceive : MonoBehaviour
{
    public int allPackage;          // ��Ҫ���͵��ܰ���������Inspector���ã�
    public GameObject victoryUI;     // ����ʤ��UI��������ǰ���ã�
    private int package;

    private void Start()
    {
        if (victoryUI != null)
            victoryUI.SetActive(false); // ȷ����ʼʱ����ʤ��UI
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
            Time.timeScale = 0f; // ��ͣ��Ϸ����ѡ��
        }
        else
        {
            Debug.LogWarning("Victory UIδ��ֵ��");
        }
    }
}
