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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
                        GameDate.blood -= 50;//momentum > maxHurtmomentum
                    }
                    else
                        GameDate.blood -= 30;//momentum > momentum2

                }
                else
                    GameDate.blood -= 20;//momentum > momentum1
            }
            else
                GameDate.blood -= 10;//momentum > minHurtmomentum
            
        }


    }
}
