using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCcontrol : MonoBehaviour
{

    public GameObject ESCmenu;
    public GameObject Cat;
    private bool isPause = false;
    private MonoBehaviour targetScript;

    void Start()
    {
        ESCmenu.SetActive(false);
        targetScript = Cat.GetComponent<MonoBehaviour>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ESCmenu.SetActive(true);

            if (isPause)
            {
                back();
            }
            else
            {
                keepstop();
            }
        }
    }

    public void keepstop()//������ͣ����
    {
        isPause = true;
        ESCmenu.SetActive(true);
        Time.timeScale = 0;
        targetScript.enabled = false;
    }
    public void back()//������Ϸ
    {
        ESCmenu.SetActive(false);
        isPause = false;
        Time.timeScale = 1;
        targetScript.enabled = true;
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
