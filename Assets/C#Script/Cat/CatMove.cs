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
            //rb.velocity = GameDate.force*50*Time.fixedDeltaTime;
            lr.enabled = false;
        }
    }

    private void UpdateTrajectory()
    {
        Vector2 startPos = transform.position;
        float mass = GameDate.totalWeight;
        Vector2 initialVelocity = GameDate.force / mass;
        Vector2 gravity = Physics2D.gravity * mass; // 考虑全局重力与质量关系
        float drag = GetComponent<Rigidbody2D>().drag; // 获取空气阻力系数
        float timeStep = 0.05f; // 更小的时间步长提高精度
        int maxSteps = Mathf.CeilToInt(lrPoints * lrStep / timeStep);

        Vector2 currentPos = startPos;
        Vector2 currentVelocity = initialVelocity;

        for (int i = 0; i < lrPoints; i++)
        {
            // 累积时间（避免浮点误差）
            double t = i * timeStep;

            // 计算阻力影响（速度衰减）
            Vector2 dragForce = -drag * currentVelocity;
            Vector2 acceleration = (gravity + dragForce) / mass;

            // 更新速度和位置（使用更精确的积分方法）
            currentVelocity += acceleration * timeStep;
            currentPos += currentVelocity * timeStep;

            // 设置轨迹点
            lr.SetPosition(i, currentPos);
        }
    }
}
