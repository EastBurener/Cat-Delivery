using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
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
        GameDate.ResetData();
    }

    public void QuitGame() 
    {
        GameDate.ResetData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ThrowPackage() 
    {
        if (pickBagController != null)
        {
            pickBagController.DropTopPackage();
        } 
    }
}