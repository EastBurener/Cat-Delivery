using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameData", menuName = "SO_Resource/GameData")]
public class GameDate_SO : ScriptableObject
{
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


}

