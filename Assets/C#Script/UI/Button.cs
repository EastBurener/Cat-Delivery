using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour // 脚本名与您提供的一致
{
    [Header("场景切换设置")]
    public string nextScene;
    public string startScene;

    [Header("游戏数据")]
    public GameDate_SO GameDate;

    [Header("包裹拾取功能引用 (仅对丢弃包裹按钮有效)")]
    [Tooltip("如果这个按钮是用来丢弃包裹的，请在这里指定PickBag脚本的实例。")]
    public PickBag pickBagController; // 用于引用 PickBag 脚本

    // --- 场景管理方法 ---
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