using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public string nextScene;

    public void NextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
