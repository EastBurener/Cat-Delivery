using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCcontrol : MonoBehaviour
{
    public GameObject GameUI;
    public GameObject ESCmenu;
    private bool isPause = false;
    public GameObject Cat;
    private MonoBehaviour targetScript;

    private void Awake()
    {
        //�ƶ��˰�ť��ʾ���
        if (IsMobilePlatform)
            GameUI.SetActive(true);
        if (!IsMobilePlatform)
        {
            GameUI.SetActive(false);

        }
    }
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
        if(IsMobilePlatform) GameUI.SetActive(false);
        ESCmenu.SetActive(true);
        Time.timeScale = 0;
        targetScript.enabled = false;
    }
    public void back()//������Ϸ
    {
        ESCmenu.SetActive(false);
        if(IsMobilePlatform) GameUI.SetActive(true);
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


    public static bool IsMobilePlatform//����ƽ̨���
    {
        get
        {
            RuntimePlatform platform = Application.platform;
            return platform == RuntimePlatform.Android ||
                   platform == RuntimePlatform.IPhonePlayer;
        }
    }
}   
