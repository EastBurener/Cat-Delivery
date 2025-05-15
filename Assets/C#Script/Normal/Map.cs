using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("��ͼ��ص���")]
    public GameObject mainCamera;
    public float mapWidth;
    public int mapNums;//��ͼ����
    private Vector2 myPosition;

    private float totalWidth;
    void Start()
    {
        myPosition = transform.position;
        mapNums = 4;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mapWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        totalWidth=mapWidth*mapNums;
    }

  
    void Update()
    {
        Vector2 tempPosition = transform.position;
        if (mainCamera.transform.position.x > transform.position.x + totalWidth / 2)
        {
            tempPosition.x = myPosition.x + totalWidth;
            transform.position=tempPosition ;
        }
        else if (mainCamera.transform.position.x < transform.position.x - totalWidth / 2) {
            tempPosition.x += totalWidth;
            transform.position = tempPosition;
        }
    }
}
