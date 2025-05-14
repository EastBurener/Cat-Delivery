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
    public int allPackage;//总包裹数
    public int ownPackage;//猫手头上的包裹数
    public int givePackage;//已提交包裹数
    public float totalWeight = 1f;

    public Vector2 startPos = new Vector2();
    public Vector2 endPos = new Vector2();
    public Vector2 direction = new Vector2();
    public Vector2 force = new Vector2();
    public float distance;

    private const int initialOwnPackage = 0;
    private const int initialGivePackage = 0;
    private const float initialTotalWeight = 1f; // 与字段声明的默认值一致
    private static readonly Vector2 initialVector = Vector2.zero;
    private const float initialDistance = 0f;
    private const float initialPhysicalPower=100f;
    // --- 重置方法 ---
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

