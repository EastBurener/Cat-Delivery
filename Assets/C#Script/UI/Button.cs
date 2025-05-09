using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }

    public void QuitGame() // 之前是 quit()，建议用 QuitGame() 遵循C#命名规范
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // --- 新增：丢弃包裹的方法 ---
    [Tooltip("此方法用于触发丢弃包裹的动作。请确保 pickBagController 已被正确赋值，并且 PickBag 脚本中的 DropTopPackage 方法是 public 的。")]
    public void ThrowPackage() // 方法名可以自定义，例如 ExecuteDropPackageAction
    {
        if (pickBagController != null)
        {
            pickBagController.DropTopPackage();
        } 
    }
}