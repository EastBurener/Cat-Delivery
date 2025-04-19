using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMove : MonoBehaviour
{
    public GameDate_SO GameDate;
    public GameObject Cat;

    private Rigidbody2D rb;
    private PickBag pickBag;
    public LayerMask clickableLayer;
    private Camera mainCamera;
    private bool start;
    [Header("弹力系数")]
    public float powerSize;
    public float maxPower;

    [Header("弹射参数")]

    public int lrPoints = 100; // 轨迹点数
    public float lrStep = 0.1f; // 轨迹时间步长
    public float gravityAcceleration = 9.81f;

    [Header("轨迹显示")]
    public LineRenderer lr; // 抛物线轨迹渲染器
    private Vector2 currentVelocity; // 当前预测速度


    private void Start()
    {
        GameDate.totalWeight = 1f;
        rb = Cat.GetComponent<Rigidbody2D>();
        pickBag = Cat.GetComponent<PickBag>();
        mainCamera = Camera.main;
        lr = Cat.GetComponent<LineRenderer>();
        lr.positionCount = lrPoints;
        lr.enabled = false; // 初始时隐藏
    }

    private void Update()
    {
        if (IsMobilePlatform)
        {
            // 暂空，后续优化
        }
        else
        {
            PCpower();
        }
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

    public void PCpower()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero, Mathf.Infinity, clickableLayer);

            GameDate.startPos = Input.mousePosition;

            start = true;
            lr.enabled = true; // 显示轨迹
        }
        if (start)
        {
            GameDate.endPos = Input.mousePosition;
            GameDate.distance = Vector2.Distance(GameDate.startPos, GameDate.endPos);
            GameDate.distance = GameDate.distance > maxPower ? maxPower : GameDate.distance;
            GameDate.direction = (GameDate.startPos - GameDate.endPos).normalized;
            // 总重量越大，弹射力越小
            GameDate.force = powerSize * GameDate.direction * GameDate.distance / GameDate.totalWeight;
            UpdateTrajectory();
        }
        if (Input.GetMouseButtonUp(0) && start)
        {
            start = false;
            rb.AddForce(GameDate.force, ForceMode2D.Impulse);
            lr.enabled = false;
        }
    }
    
    private void UpdateTrajectory()
    {
        Vector2 startPos = transform.position;
        Vector2 initialVelocity = GameDate.force / GameDate.totalWeight; // 冲量公式：velocity = impulse / mass
        Vector2 gravity = new Vector2(0, -gravityAcceleration); // 自定义重力方向

        for (int i = 0; i < lrPoints; i++)
        {
            float t = i * lrStep;
            Vector2 point = startPos + initialVelocity * t + 0.5f * gravity * t * t;

            lr.SetPosition(i, point);
        }
    }
}
