using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCcontrol : MonoBehaviour
{
    public GameObject mobileThrow;
    public GameObject ESCmenu;
    private bool isPause = false;
    public GameObject Cat;
    private MonoBehaviour targetScript;

    private void Awake()
    {
        //�ƶ��˶�����ť��ʾ���
        if (IsMobilePlatform)
            mobileThrow.SetActive(true);
        if (!IsMobilePlatform)
        {
            mobileThrow.SetActive(false);
            Destroy(mobileThrow);
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
            Debug.Log("114514");
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
