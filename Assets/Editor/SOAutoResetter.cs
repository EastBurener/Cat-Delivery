// Assets/Editor/SOAutoResetter.cs
using UnityEditor;
using UnityEngine;

[InitializeOnLoad] // ȷ��������ڱ༭������ʱ�����غͳ�ʼ��
public class SOAutoResetter
{
    static SOAutoResetter()
    {
        // ���Ĳ���ģʽ״̬�ı��¼�
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        // ���ǹ��ĵ��ǵ��༭���˳�����ģʽ������༭ģʽʱ
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
            // 2. ����GUID��ȡ��Դ·��
            string path = AssetDatabase.GUIDToAssetPath(guid);

            // 3. ���� ScriptableObject ʵ��
            GameDate_SO soInstance = AssetDatabase.LoadAssetAtPath<GameDate_SO>(path);

            if (soInstance != null)
            {
                // 4. ���������÷���
                soInstance.ResetData(); // �������Ӧ���� GameDate_SO.cs �ж���

                // 5. ��� ScriptableObject Ϊ "dirty"�����޸ģ�
                // ���� Unity �༭����֪������Ҫ������
                EditorUtility.SetDirty(soInstance);
            }
            else
            {
                Debug.LogWarning($"Could not load GameDate_SO at path: {path}");
            }
        }

        // 6. ���������������޸ĵ���Դ
        // ��ȷ�������ú�����ݱ�д�����
        AssetDatabase.SaveAssets();

        // 7. (��ѡ) ˢ����Դ���ݿ⣬��ȷ���༭��UI��ӳ����
        AssetDatabase.Refresh();
    }
}