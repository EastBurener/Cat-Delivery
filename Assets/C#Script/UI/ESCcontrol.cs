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
        //移动端丢弃按钮显示与否
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

    public void keepstop()//进入暂停界面
    {
        isPause = true;
        ESCmenu.SetActive(true);
        Time.timeScale = 0;
        targetScript.enabled = false;
    }
    public void back()//继续游戏
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


    public static bool IsMobilePlatform//运行平台检测
    {
        get
        {
            RuntimePlatform platform = Application.platform;
            return platform == RuntimePlatform.Android ||
                   platform == RuntimePlatform.IPhonePlayer;
        }
    }
}   
