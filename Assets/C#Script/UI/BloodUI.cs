using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodUI : MonoBehaviour
{
    
    public Image blood;
    public GameDate_SO GameDate;
    private float lerpSpeed=3f;

    
    private void Update()
    {

        BloodFill();
    }
    private void BloodFill()
    {
        blood.fillAmount = Mathf.Lerp(blood.fillAmount,(float)(GameDate.physicalPower /GameDate.maxPhysicalPower),lerpSpeed*Time.deltaTime);
    }

}
