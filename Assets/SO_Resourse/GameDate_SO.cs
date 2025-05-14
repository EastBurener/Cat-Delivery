using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameData", menuName = "SO_Resource/GameData")]
public class GameDate_SO : ScriptableObject
{
    public float physicalPower;
    public float maxPhysicalPower;
    public double blood;
    public double maxBlood;
    public int allPackage;//�ܰ�����
    public int ownPackage;//è��ͷ�ϵİ�����
    public int givePackage;//���ύ������
    public float totalWeight = 1f;

    public Vector2 startPos = new Vector2();
    public Vector2 endPos = new Vector2();
    public Vector2 direction = new Vector2();
    public Vector2 force = new Vector2();
    public float distance;

    private const int initialOwnPackage = 0;
    private const int initialGivePackage = 0;
    private const float initialTotalWeight = 1f; // ���ֶ�������Ĭ��ֵһ��
    private static readonly Vector2 initialVector = Vector2.zero;
    private const float initialDistance = 0f;
    private const float initialPhysicalPower=100f;
    // --- ���÷��� ---
    public void ResetData()
    {
        blood = maxBlood;
        physicalPower=initialPhysicalPower;
        ownPackage = initialOwnPackage;
        givePackage = initialGivePackage;

        totalWeight = initialTotalWeight;

        startPos = initialVector; 
        endPos = initialVector;
        direction = initialVector;
        force = initialVector;
        distance = initialDistance;

    }
}

