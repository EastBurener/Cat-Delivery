// CatMove.cs
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
	private int jumpNum;

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
		Debug.Log($"[CatMove - {gameObject.scene.name}] OnEnable: Subscribed to sceneLoaded. Current jumpNum: {jumpNum}. Script enabled: {this.enabled}");
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		Debug.Log($"[CatMove - {gameObject.scene.name}] OnDisable: Unsubscribed from sceneLoaded. Script enabled: {this.enabled}");
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log($"[CatMove] Scene '{scene.name}' loaded by mode '{mode}'.");
		bool isPlayableLevel = (scene.name == "Scene1" ||
								scene.name == "Scene2" ||
								scene.name == "Scene3"); // << --- 修改为你实际的关卡场景名列表

		if (isPlayableLevel)
		{
			Debug.Log($"[CatMove] Playable level '{scene.name}' detected. Ensuring Time.timeScale is 1 and resetting Cat state.");
			Time.timeScale = 1f;
			Debug.Log($"[CatMove] Time.timeScale set to 1f for playable level '{scene.name}'.");
			ResetStateForNewLevel();
			if (!this.enabled)
			{
				this.enabled = true;
				Debug.Log($"[CatMove] Script was disabled, re-enabled itself for new playable level '{scene.name}'.");
			}
		}
		else
		{
			Debug.Log($"[CatMove] Scene '{scene.name}' is not a designated playable level. Not resetting state. Current jumpNum: {jumpNum}, Time.timeScale: {Time.timeScale}");
		}
	}

	void ResetStateForNewLevel()
	{
		Debug.Log($"[CatMove] ResetStateForNewLevel called. Resetting jumpNum and other states.");
		jumpNum = 2;
		start = false;
		if (lr != null) lr.enabled = false;
		if (rb != null)
		{
			rb.velocity = Vector2.zero;
			rb.angularVelocity = 0f;
			if (rb.IsSleeping()) rb.WakeUp();
		}
		Debug.Log($"[CatMove] State reset. jumpNum: {jumpNum}, start: {start}");
	}

	private void Awake()
	{
		Debug.Log($"[CatMove - {gameObject.scene.name}] Awake called. Initializing components...");
		rb = GetComponent<Rigidbody2D>();
		if (rb == null) Debug.LogError("[CatMove] Rigidbody2D not found on this GameObject!", this);

		pickBag = GetComponent<PickBag>();
		if (pickBag == null) Debug.LogWarning("[CatMove] PickBag component not found. (If not needed, this is fine)", this);

		mainCamera = Camera.main;
		if (mainCamera == null) Debug.LogError("[CatMove] Main Camera not found! Ensure it has 'MainCamera' tag.", this);

		lr = GetComponent<LineRenderer>();
		if (lr != null) lr.positionCount = lrPoints;
		else Debug.LogWarning("[CatMove] LineRenderer component not found. Trajectory will not be shown.", this);

		Debug.Log($"[CatMove - {gameObject.scene.name}] Awake - Initial Time.timeScale: {Time.timeScale}");
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
		Debug.Log($"[CatMove - {gameObject.scene.name}] Start completed. Time.timeScale: {Time.timeScale}, Script Enabled: {this.enabled}, jumpNum: {jumpNum}");
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
		if (Input.GetMouseButtonDown(0) && jumpNum > 0)
		{
			Debug.Log($"[CatMove - PCpower] MouseButtonDown detected. jumpNum: {jumpNum}");
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
			Debug.Log($"[CatMove - PCpower] MouseButtonUp detected. Applying force: {GameDate.force}");
			start = false;
			if (rb != null)
			{
				if (rb.IsSleeping()) rb.WakeUp();
				rb.AddForce(GameDate.force, ForceMode2D.Impulse);
			}
			if (lr != null) lr.enabled = false;
			jumpNum -= 1;
			Debug.Log($"[CatMove] PC Jump! jumpNum remaining: {jumpNum}");
		}
	}
	public void mobilePower()
	{
		if (mainCamera == null || GameDate == null || Input.touchCount == 0 || Time.timeScale == 0f) return;
		Touch touch = Input.GetTouch(0);
		if (touch.phase == TouchPhase.Began && jumpNum > 0)
		{
			Debug.Log($"[CatMove - mobilePower] TouchBegan detected. jumpNum: {jumpNum}");
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
				Debug.Log($"[CatMove - mobilePower] TouchEnded/Canceled detected. Applying force: {GameDate.force}");
				start = false;
				if (rb != null)
				{
					if (rb.IsSleeping()) rb.WakeUp();
					rb.AddForce(GameDate.force, ForceMode2D.Impulse);
				}
				if (lr != null) lr.enabled = false;
				jumpNum -= 1;
				Debug.Log($"[CatMove] Mobile Jump! jumpNum remaining: {jumpNum}");
			}
		}
		else if (start && Input.touchCount == 0)
		{
			start = false;
			if (lr != null) lr.enabled = false;
			Debug.LogWarning("[CatMove] Touch ended unexpectedly (no Ended/Canceled phase during drag).");
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
	private void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log($"[CatMove] Collision with {collision.gameObject.name}. Starting RecoverJump coroutine. Time.timeScale: {Time.timeScale}");
		if (Time.timeScale > 0f && this.enabled)
		{
			StartCoroutine(RecoverJump());
		}
		else
		{
			Debug.LogWarning($"[CatMove] Collision detected but Time.timeScale is {Time.timeScale} or script is disabled. RecoverJump coroutine NOT started.");
		}
	}
	private System.Collections.IEnumerator RecoverJump()
	{
		Debug.Log($"[CatMove] RecoverJump coroutine started. Waiting 3 seconds.");
		yield return new WaitForSeconds(3f);
		if (this.enabled)
		{
			jumpNum = 2;
			Debug.Log($"[CatMove] jumpNum recovered to: {jumpNum} after 3s wait.");
		}
		else
		{
			Debug.LogWarning($"[CatMove] RecoverJump completed but CatMove script is currently disabled. jumpNum not recovered.");
		}
	}
}