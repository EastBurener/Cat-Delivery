using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatMove : MonoBehaviour
{
	public GameDate_SO GameDate;

	private Rigidbody2D rb;
	private PickBag pickBag;
	public LayerMask clickableLayer;
	private Camera mainCamera;

	private bool start;
	private float myPhysicalPower;

	[Header("弹力系数")]
	public float powerSize;
	public float maxPower;

	[Header("弹射参数")]
	public int lrPoints = 100;

	[Header("轨迹显示")]
	public LineRenderer lr;

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		bool isPlayableLevel = (scene.name == "Scene1" ||
								scene.name == "Scene2" ||
								scene.name == "Scene3"); // << --- 修改为你实际的关卡场景名列表

		if (isPlayableLevel)
		{
			Time.timeScale = 1f;
			ResetStateForNewLevel();
			if (!this.enabled)
			{
				this.enabled = true;
			}
		}

	}

	void ResetStateForNewLevel()
	{
		start = false;
		if (lr != null) lr.enabled = false;
		if (rb != null)
		{
			rb.velocity = Vector2.zero;
			rb.angularVelocity = 0f;
			if (rb.IsSleeping()) rb.WakeUp();
		}
	}

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		pickBag = GetComponent<PickBag>();

		mainCamera = Camera.main;

		lr = GetComponent<LineRenderer>();
		if (lr != null) lr.positionCount = lrPoints;
		else Debug.LogWarning("[CatMove] LineRenderer component not found. Trajectory will not be shown.", this);

	}

	private void Start()
	{
		if (GameDate != null)
		{
			GameDate.totalWeight = 1f;
			if (rb != null) rb.mass = GameDate.totalWeight;
		}
		else Debug.LogError("[CatMove] GameDate_SO is not assigned in Inspector!", this);

		ResetStateForNewLevel();
	}

	private void Update()
	{
		if (GameDate != null && rb != null && rb.mass != GameDate.totalWeight)
		{
			rb.mass = GameDate.totalWeight;
		}
		if (IsMobilePlatform) mobilePower();
		else PCpower();
	}

	public static bool IsMobilePlatform
	{
		get
		{
			RuntimePlatform platform = Application.platform;
			return platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer;
		}
	}
	public void PCpower()
	{
		if (mainCamera == null || GameDate == null || Time.timeScale == 0f) return;
		if (Input.GetMouseButtonDown(0))
		{
			GameDate.startPos = Input.mousePosition;
			start = true;
			if (lr != null) lr.enabled = true;
		}
		if (start)
		{
			GameDate.endPos = Input.mousePosition;
			GameDate.distance = (Vector2.Distance(GameDate.startPos, GameDate.endPos)) / 3f;
			GameDate.distance = Mathf.Min(GameDate.distance, maxPower);
			GameDate.direction = (GameDate.startPos - GameDate.endPos).normalized;
			if (GameDate.totalWeight <= 0) GameDate.totalWeight = 1f;
			GameDate.force = powerSize * GameDate.direction * GameDate.distance / GameDate.totalWeight;
			UpdateTrajectory();

		}
		if (Input.GetMouseButtonUp(0) && start)
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
	public void mobilePower()
	{
		if (mainCamera == null || GameDate == null || Input.touchCount == 0 || Time.timeScale == 0f) return;
		Touch touch = Input.GetTouch(0);
		if (touch.phase == TouchPhase.Began )
		{
			GameDate.startPos = touch.position;
			start = true;
			if (lr != null) lr.enabled = true;
		}
		if (start)
		{
			GameDate.endPos = touch.position;
			GameDate.distance = (Vector2.Distance(GameDate.startPos, GameDate.endPos)) / 3f;
			GameDate.distance = Mathf.Min(GameDate.distance, maxPower);
			GameDate.direction = (GameDate.startPos - GameDate.endPos).normalized;
			if (GameDate.totalWeight <= 0) GameDate.totalWeight = 1f;
			GameDate.force = powerSize * GameDate.direction * GameDate.distance / GameDate.totalWeight;
			UpdateTrajectory();
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
	private void UpdateTrajectory()
	{
		if (lr == null || GameDate == null || rb == null || !start) return;
		Vector2 trajectoryStartPos = transform.position;
		float currentMass = Mathf.Max(0.001f, GameDate.totalWeight);
		Vector2 initialVelocity = GameDate.force / currentMass;
		Vector2 gravityEffect = Physics2D.gravity * rb.gravityScale;
		float drag = rb.drag;
		float timeStep = 0.05f;
		lr.positionCount = lrPoints;
		Vector2 currentPredictedPos = trajectoryStartPos;
		Vector2 currentPredictedVelocity = initialVelocity;
		for (int i = 0; i < lrPoints; i++)
		{
			currentPredictedVelocity -= currentPredictedVelocity * drag * timeStep;
			currentPredictedVelocity += gravityEffect * timeStep;
			currentPredictedPos += currentPredictedVelocity * timeStep;
			lr.SetPosition(i, currentPredictedPos);
		}
	}
	
}