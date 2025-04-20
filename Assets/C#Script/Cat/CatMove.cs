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
    [Header("����ϵ��")]
    public float powerSize;
    public float maxPower;

    [Header("�������")]

    public int lrPoints = 100; // �켣����
    public float lrStep = 0.1f; // �켣ʱ�䲽��
    public float gravityAcceleration = 9.81f;

    [Header("�켣��ʾ")]
    public LineRenderer lr; // �����߹켣��Ⱦ��
    private Vector2 currentVelocity; // ��ǰԤ���ٶ�


    private void Start()
    {
        GameDate.totalWeight = 1f;
        rb = Cat.GetComponent<Rigidbody2D>();
        pickBag = Cat.GetComponent<PickBag>();
        mainCamera = Camera.main;
        lr = Cat.GetComponent<LineRenderer>();
        lr.positionCount = lrPoints;
        lr.enabled = false; // ��ʼʱ����
    }

    private void Update()
    {
        if (IsMobilePlatform)
        {
            // �ݿգ������Ż�
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
            lr.enabled = true; // ��ʾ�켣
        }
        if (start)
        {
            GameDate.endPos = Input.mousePosition;
            GameDate.distance = Vector2.Distance(GameDate.startPos, GameDate.endPos);
            GameDate.distance = GameDate.distance > maxPower ? maxPower : GameDate.distance;
            GameDate.direction = (GameDate.startPos - GameDate.endPos).normalized;
            // ������Խ�󣬵�����ԽС
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
        Vector2 gravity = Physics2D.gravity * mass; // ����ȫ��������������ϵ
        float drag = GetComponent<Rigidbody2D>().drag; // ��ȡ��������ϵ��
        float timeStep = 0.05f; // ��С��ʱ�䲽����߾���
        int maxSteps = Mathf.CeilToInt(lrPoints * lrStep / timeStep);

        Vector2 currentPos = startPos;
        Vector2 currentVelocity = initialVelocity;

        for (int i = 0; i < lrPoints; i++)
        {
            // �ۻ�ʱ�䣨���⸡����
            double t = i * timeStep;

            // ��������Ӱ�죨�ٶ�˥����
            Vector2 dragForce = -drag * currentVelocity;
            Vector2 acceleration = (gravity + dragForce) / mass;

            // �����ٶȺ�λ�ã�ʹ�ø���ȷ�Ļ��ַ�����
            currentVelocity += acceleration * timeStep;
            currentPos += currentVelocity * timeStep;

            // ���ù켣��
            lr.SetPosition(i, currentPos);
        }
    }
}
