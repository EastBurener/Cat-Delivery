// Assets/Editor/SOAutoResetter.cs
using UnityEditor;
using UnityEngine;

[InitializeOnLoad] // 确保这个类在编辑器启动时被加载和初始化
public class SOAutoResetter
{
    static SOAutoResetter()
    {
        // 订阅播放模式状态改变事件
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        // 我们关心的是当编辑器退出播放模式并进入编辑模式时
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            ResetAllGameDateSOs();
        }
    }

    private static void ResetAllGameDateSOs()
    {

        string[] guids = AssetDatabase.FindAssets("t:GameDate_SO");

        if (guids.Length == 0)
        {
            Debug.Log("No GameDate_SO instances found to reset.");
            return;
        }

        foreach (string guid in guids)
        {
            // 2. 根据GUID获取资源路径
            string path = AssetDatabase.GUIDToAssetPath(guid);

            // 3. 加载 ScriptableObject 实例
            GameDate_SO soInstance = AssetDatabase.LoadAssetAtPath<GameDate_SO>(path);

            if (soInstance != null)
            {
                // 4. 调用其重置方法
                soInstance.ResetData(); // 这个方法应该在 GameDate_SO.cs 中定义

                // 5. 标记 ScriptableObject 为 "dirty"（已修改）
                // 这样 Unity 编辑器才知道它需要被保存
                EditorUtility.SetDirty(soInstance);
            }
            else
            {
                Debug.LogWarning($"Could not load GameDate_SO at path: {path}");
            }
        }

        // 6. 立即保存所有已修改的资源
        // 这确保了重置后的数据被写入磁盘
        AssetDatabase.SaveAssets();

        // 7. (可选) 刷新资源数据库，以确保编辑器UI反映更改
        AssetDatabase.Refresh();
    }
}