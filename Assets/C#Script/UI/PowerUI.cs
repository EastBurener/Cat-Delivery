using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUI : MonoBehaviour
{
    public Image physicalPower;
    public GameDate_SO GameDate;
    private float lerpSpeed = 3f;


    private void Update()
    {

        PowerFill();
    }
    private void PowerFill()
    {
       physicalPower.fillAmount = Mathf.Lerp(physicalPower.fillAmount, (float)(GameDate.physicalPower / GameDate.maxPhysicalPower), lerpSpeed * Time.deltaTime);
    }
}
