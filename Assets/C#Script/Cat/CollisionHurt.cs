using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHurt : MonoBehaviour
{
    public GameDate_SO GameDate;
    [Header("��ײ�˺��ļ����߽�ֵ")]
    public float minHurtmomentum;
    public float momentum1;
    public float momentum2;
    public float maxHurtmomentum;

    [Header("��Ѫֵ")]
    public int hurt1;
    public int hurt2;
    public int hurt3;
    public int hurt4;
    // Start is called before the first frame update
    void Start()
    {
        GameDate.blood = 100;
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        float relativeV = collision.relativeVelocity.magnitude;
        float momentum=relativeV*GameDate.totalWeight;//���㶯�������˺��ж�������
        Debug.Log(momentum);
        // �������˶���
        if (momentum > minHurtmomentum)
        {
            if (momentum > momentum1)
            {
                if (momentum > momentum2)
                {
                    if (momentum > maxHurtmomentum)
                    {
                        GameDate.blood -= hurt4;//momentum > maxHurtmomentum
                    }
                    else
                        GameDate.blood -= hurt3;//momentum > momentum2

                }
                else
                    GameDate.blood -= hurt2;//momentum > momentum1
            }
            else
                GameDate.blood -= hurt1;//momentum > minHurtmomentum
            
        }


    }
}
