using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public string nextScene;
    public string startScene;

    public void NextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
    
    public void Menu()
    {
        SceneManager.LoadScene(startScene);
    }

    public void ReTry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
