using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPackage : MonoBehaviour
{
    public GameObject mobileThrow;

    void Start()
    {
        if(IsMobilePlatform)
            mobileThrow.SetActive(true);
        if (!IsMobilePlatform)
        {
            mobileThrow.SetActive(false);
            Destroy(this);
        }
    }

    // Update is called once per frame
    public void throwPackage()
    { 

    }
    public static bool IsMobilePlatform
    {
        get
        {
            RuntimePlatform platform = Application.platform;
            return platform == RuntimePlatform.Android ||
                   platform == RuntimePlatform.IPhonePlayer;
        }
    }
}
