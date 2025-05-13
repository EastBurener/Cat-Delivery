using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// è���ƶ����ƽű�����������ƺ������˶�
public class CatMove : MonoBehaviour
{
	// ��Ϸ���ݽű�����ScriptableObject��
	public GameDate_SO GameDate;

	private Rigidbody2D rb;          // 2D�������
	private PickBag pickBag;          // ʰȡ�������
	public LayerMask clickableLayer;  // �ɵ���������
	private Camera mainCamera;        // �����������

	private bool start;               // �����Ƿ�ʼ�ı�־
	private float myPhysicalPower;    // ��������ֵ����ǰδʹ�ã�

	[Header("����ϵ��")]
	public float powerSize;           // ������������
	public float maxPower;            // �������������

	[Header("�������")]
	public int lrPoints = 100;        // �켣�ߵ������

	[Header("�켣��ʾ")]
	public LineRenderer lr;           // �켣����Ⱦ���

	// ���ű�����ʱע�᳡�������¼�
	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	// ���ű�����ʱע�����������¼�
	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	// �����������ʱ�Ļص�����
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// ����Ƿ�Ϊ����ؿ�
		bool isPlayableLevel = (scene.name == "Scene1" ||
							  scene.name == "Scene2" ||
							  scene.name == "Scene3");

		if (isPlayableLevel)
		{
			Time.timeScale = 1f;                     // ȷ����Ϸʱ����������
			ResetStateForNewLevel();                 // ���ý�ɫ״̬
			if (!this.enabled) this.enabled = true;   // ȷ���ű�����
		}
	}

	// ���ý�ɫ״̬����ʼ״̬
	void ResetStateForNewLevel()
	{
		start = false;
		if (lr != null) lr.enabled = false;          // ���ع켣��
		if (rb != null)
		{
			rb.velocity = Vector2.zero;              // �����ٶ�
			rb.angularVelocity = 0f;                 // ���ý��ٶ�
			if (rb.IsSleeping()) rb.WakeUp();        // ���Ѹ���
		}
	}

	// ��ʼ�����
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		pickBag = GetComponent<PickBag>();
		mainCamera = Camera.main;

		// ��ʼ���켣��
		lr = GetComponent<LineRenderer>();
		if (lr != null) lr.positionCount = lrPoints;
		else Debug.LogWarning("[CatMove] δ�ҵ�LineRenderer������켣��������ʾ", this);
	}

	// ��ʼ����Ϸ����
	private void Start()
	{
		if (GameDate != null)
		{
			GameDate.totalWeight = 1f;               // ����Ĭ������
			if (rb != null) rb.mass = GameDate.totalWeight; // ͬ����������
		}
		else Debug.LogError("[CatMove] ��Ϸ����δ��Inspector�з���!", this);

		ResetStateForNewLevel();
	}

	// ÿ֡�����߼�
	private void Update()
	{
		// ͬ��������������Ϸ����
		if (GameDate != null && rb != null && rb.mass != GameDate.totalWeight)
		{
			rb.mass = GameDate.totalWeight;
		}

		// ����ƽ̨���͵��ö�Ӧ�����봦��
		if (IsMobilePlatform) mobilePower();
		else PCpower();
	}

	// �жϵ�ǰ�Ƿ�Ϊ�ƶ�ƽ̨
	public static bool IsMobilePlatform
	{
		get
		{
			RuntimePlatform platform = Application.platform;
			return platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer;
		}
	}

	// PC�˵�����������
	public void PCpower()
	{
		if (mainCamera == null || GameDate == null || Time.timeScale == 0f) return;

		// ��갴�¿�ʼ����
		if (Input.GetMouseButtonDown(0))
		{
			GameDate.startPos = Input.mousePosition;
			start = true;
			if (lr != null) lr.enabled = true; // ��ʾ�켣��
		}

		// ���㵯�����
		if (start)
		{
			GameDate.endPos = Input.mousePosition;
			// ����������С���������������
			GameDate.distance = Mathf.Min(Vector2.Distance(GameDate.startPos, GameDate.endPos) / 3f, maxPower);
			GameDate.direction = (GameDate.startPos - GameDate.endPos).normalized; // ���㷽��
			if (GameDate.totalWeight <= 0) GameDate.totalWeight = 1f;
			GameDate.force = powerSize * GameDate.direction * GameDate.distance / GameDate.totalWeight;
			UpdateTrajectory(); // ���¹켣Ԥ��
		}

		// ����ͷ�ʱʩ������
		if (Input.GetMouseButtonUp(0) && start)
		{
			start = false;
			if (rb != null)
			{
				if (rb.IsSleeping()) rb.WakeUp();
				rb.AddForce(GameDate.force, ForceMode2D.Impulse); // ʩ�ӳ���
			}
			if (lr != null) lr.enabled = false; // ���ع켣��
		}
	}

	// �ƶ��˵����������㣨��������汾��
	public void mobilePower()
	{
		if (mainCamera == null || GameDate == null || Input.touchCount == 0 || Time.timeScale == 0f) return;
		Touch touch = Input.GetTouch(0);

		// ������ʼ
		if (touch.phase == TouchPhase.Began)
		{
			GameDate.startPos = touch.position;
			start = true;
			if (lr != null) lr.enabled = true;
		}

		// ���㵯��������߼���PC�����ƣ�
		if (start)
		{
			GameDate.endPos = touch.position;
			GameDate.distance = Mathf.Min(Vector2.Distance(GameDate.startPos, GameDate.endPos) / 3f, maxPower);
			GameDate.direction = (GameDate.startPos - GameDate.endPos).normalized;
			if (GameDate.totalWeight <= 0) GameDate.totalWeight = 1f;
			GameDate.force = powerSize * GameDate.direction * GameDate.distance / GameDate.totalWeight;
			UpdateTrajectory();

			// ��������ʱʩ������
			if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
			{
				start = false;
				if (rb != null)
				{
					if (rb.IsSleeping()) rb.WakeUp();
					rb.AddForce(GameDate.force, ForceMode2D.Impulse);
				}
				if (lr != null) lr.enabled = false;
			}
		}
		else if (start && Input.touchCount == 0)
		{
			start = false;
			if (lr != null) lr.enabled = false;
		}
	}

	// ���µ���켣Ԥ��
	private void UpdateTrajectory()
	{
		if (lr == null || GameDate == null || rb == null || !start) return;

		Vector2 trajectoryStartPos = transform.position;
		float currentMass = Mathf.Max(0.001f, GameDate.totalWeight); // ��ֹ�������
		Vector2 initialVelocity = GameDate.force / currentMass;      // ���ٶȼ���
		Vector2 gravityEffect = Physics2D.gravity * rb.gravityScale; // ��������Ӱ��
		float drag = rb.drag;                                        // ��ȡ����ϵ��

		// ����ģ��Ԥ��켣
		float timeStep = 0.05f;
		lr.positionCount = lrPoints;
		Vector2 currentPredictedPos = trajectoryStartPos;
		Vector2 currentPredictedVelocity = initialVelocity;

		// ����ÿ���켣�����Ԥ��
		for (int i = 0; i < lrPoints; i++)
		{
			currentPredictedVelocity -= currentPredictedVelocity * drag * timeStep; // ��������Ӱ��
			currentPredictedVelocity += gravityEffect * timeStep;                  // ��������Ӱ��
			currentPredictedPos += currentPredictedVelocity * timeStep;            // ����λ��
			lr.SetPosition(i, currentPredictedPos);                               // ���ù켣��λ��
		}
	}
}