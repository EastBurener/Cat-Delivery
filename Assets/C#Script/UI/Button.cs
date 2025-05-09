using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour // �ű��������ṩ��һ��
{
    [Header("�����л�����")]
    public string nextScene;
    public string startScene;

    [Header("��Ϸ����")]
    public GameDate_SO GameDate;

    [Header("����ʰȡ�������� (���Զ���������ť��Ч)")]
    [Tooltip("��������ť���������������ģ���������ָ��PickBag�ű���ʵ����")]
    public PickBag pickBagController; // �������� PickBag �ű�

    // --- ���������� ---
    public void NextScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nextScene);
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(startScene);
    }

    public void ReTry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() // ֮ǰ�� quit()�������� QuitGame() ��ѭC#�����淶
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // --- ���������������ķ��� ---
    [Tooltip("�˷������ڴ������������Ķ�������ȷ�� pickBagController �ѱ���ȷ��ֵ������ PickBag �ű��е� DropTopPackage ������ public �ġ�")]
    public void ThrowPackage() // �����������Զ��壬���� ExecuteDropPackageAction
    {
        if (pickBagController != null)
        {
            pickBagController.DropTopPackage();
        } 
    }
}