using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 猫咪移动控制脚本，处理弹射机制和物理运动
public class CatMove : MonoBehaviour
{
	// 游戏数据脚本对象（ScriptableObject）
	public GameDate_SO GameDate;

	private Rigidbody2D rb;          // 2D刚体组件
	private PickBag pickBag;          // 拾取背包组件
	public LayerMask clickableLayer;  // 可点击层的遮罩
	private Camera mainCamera;        // 主摄像机引用

	private bool start;               // 弹射是否开始的标志
	private float myPhysicalPower;    // 物理力量值（当前未使用）

	[Header("弹力系数")]
	public float powerSize;           // 弹射力量乘数
	public float maxPower;            // 最大弹射力量限制

	[Header("弹射参数")]
	public int lrPoints = 100;        // 轨迹线点的数量

	[Header("轨迹显示")]
	public LineRenderer lr;           // 轨迹线渲染组件

	// 当脚本启用时注册场景加载事件
	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	// 当脚本禁用时注销场景加载事件
	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	// 场景加载完成时的回调处理
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// 检查是否为可玩关卡
		bool isPlayableLevel = (scene.name == "Scene1" ||
							  scene.name == "Scene2" ||
							  scene.name == "Scene3");

		if (isPlayableLevel)
		{
			Time.timeScale = 1f;                     // 确保游戏时间正常流动
			ResetStateForNewLevel();                 // 重置角色状态
			if (!this.enabled) this.enabled = true;   // 确保脚本启用
		}
	}

	// 重置角色状态到初始状态
	void ResetStateForNewLevel()
	{
		start = false;
		if (lr != null) lr.enabled = false;          // 隐藏轨迹线
		if (rb != null)
		{
			rb.velocity = Vector2.zero;              // 重置速度
			rb.angularVelocity = 0f;                 // 重置角速度
			if (rb.IsSleeping()) rb.WakeUp();        // 唤醒刚体
		}
	}

	// 初始化组件
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		pickBag = GetComponent<PickBag>();
		mainCamera = Camera.main;

		// 初始化轨迹线
		lr = GetComponent<LineRenderer>();
		if (lr != null) lr.positionCount = lrPoints;
		else Debug.LogWarning("[CatMove] 未找到LineRenderer组件，轨迹将不会显示", this);
	}

	// 初始化游戏数据
	private void Start()
	{
		if (GameDate != null)
		{
			GameDate.totalWeight = 1f;               // 设置默认重量
			if (rb != null) rb.mass = GameDate.totalWeight; // 同步刚体质量
		}
		else Debug.LogError("[CatMove] 游戏数据未在Inspector中分配!", this);

		ResetStateForNewLevel();
	}

	// 每帧更新逻辑
	private void Update()
	{
		// 同步刚体质量与游戏数据
		if (GameDate != null && rb != null && rb.mass != GameDate.totalWeight)
		{
			rb.mass = GameDate.totalWeight;
		}

		// 根据平台类型调用对应的输入处理
		if (IsMobilePlatform) mobilePower();
		else PCpower();
	}

	// 判断当前是否为移动平台
	public static bool IsMobilePlatform
	{
		get
		{
			RuntimePlatform platform = Application.platform;
			return platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer;
		}
	}

	// PC端弹射力量计算
	public void PCpower()
	{
		if (mainCamera == null || GameDate == null || Time.timeScale == 0f) return;

		// 鼠标按下开始弹射
		if (Input.GetMouseButtonDown(0))
		{
			GameDate.startPos = Input.mousePosition;
			start = true;
			if (lr != null) lr.enabled = true; // 显示轨迹线
		}

		// 计算弹射参数
		if (start)
		{
			GameDate.endPos = Input.mousePosition;
			// 计算力量大小（限制最大力量）
			GameDate.distance = Mathf.Min(Vector2.Distance(GameDate.startPos, GameDate.endPos) / 3f, maxPower);
			GameDate.direction = (GameDate.startPos - GameDate.endPos).normalized; // 计算方向
			if (GameDate.totalWeight <= 0) GameDate.totalWeight = 1f;
			GameDate.force = powerSize * GameDate.direction * GameDate.distance / GameDate.totalWeight;
			UpdateTrajectory(); // 更新轨迹预测
		}

		// 鼠标释放时施加力量
		if (Input.GetMouseButtonUp(0) && start)
		{
			start = false;
			if (rb != null)
			{
				if (rb.IsSleeping()) rb.WakeUp();
				rb.AddForce(GameDate.force, ForceMode2D.Impulse); // 施加冲量
			}
			if (lr != null) lr.enabled = false; // 隐藏轨迹线
		}
	}

	// 移动端弹射力量计算（触摸输入版本）
	public void mobilePower()
	{
		if (mainCamera == null || GameDate == null || Input.touchCount == 0 || Time.timeScale == 0f) return;
		Touch touch = Input.GetTouch(0);

		// 触摸开始
		if (touch.phase == TouchPhase.Began)
		{
			GameDate.startPos = touch.position;
			start = true;
			if (lr != null) lr.enabled = true;
		}

		// 计算弹射参数（逻辑与PC版类似）
		if (start)
		{
			GameDate.endPos = touch.position;
			GameDate.distance = Mathf.Min(Vector2.Distance(GameDate.startPos, GameDate.endPos) / 3f, maxPower);
			GameDate.direction = (GameDate.startPos - GameDate.endPos).normalized;
			if (GameDate.totalWeight <= 0) GameDate.totalWeight = 1f;
			GameDate.force = powerSize * GameDate.direction * GameDate.distance / GameDate.totalWeight;
			UpdateTrajectory();

			// 触摸结束时施加力量
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

	// 更新弹射轨迹预测
	private void UpdateTrajectory()
	{
		if (lr == null || GameDate == null || rb == null || !start) return;

		Vector2 trajectoryStartPos = transform.position;
		float currentMass = Mathf.Max(0.001f, GameDate.totalWeight); // 防止除零错误
		Vector2 initialVelocity = GameDate.force / currentMass;      // 初速度计算
		Vector2 gravityEffect = Physics2D.gravity * rb.gravityScale; // 计算重力影响
		float drag = rb.drag;                                        // 获取阻力系数

		// 物理模拟预测轨迹
		float timeStep = 0.05f;
		lr.positionCount = lrPoints;
		Vector2 currentPredictedPos = trajectoryStartPos;
		Vector2 currentPredictedVelocity = initialVelocity;

		// 遍历每个轨迹点进行预测
		for (int i = 0; i < lrPoints; i++)
		{
			currentPredictedVelocity -= currentPredictedVelocity * drag * timeStep; // 计算阻力影响
			currentPredictedVelocity += gravityEffect * timeStep;                  // 计算重力影响
			currentPredictedPos += currentPredictedVelocity * timeStep;            // 更新位置
			lr.SetPosition(i, currentPredictedPos);                               // 设置轨迹点位置
		}
	}
}