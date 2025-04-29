using UnityEngine;

public class BananaPeel : MonoBehaviour
{
	[Header("���в���")]
	public float slideSpeed = 5f; // �����ٶ�
	public float slideDuration = 1f; // ����ʱ��
	public string playerTag = "Player"; // ��ɫ��Tag
	public bool slideRight = true; // Ĭ�����һ���
	public float destroyDelay = 0.5f; // �㽶Ƥ����ǰ���ӳ�

	private GameObject slidingPlayer; // ��ǰ���ڻ��еĽ�ɫ
	private Rigidbody2D playerRb; // ��ɫ�� Rigidbody2D
	private float slideTimer = 0f; // ���м�ʱ��
	private Vector2 slideDirection; // ���з���
	private bool isPlayerSliding = false; // ��ɫ�Ƿ����ڻ���

	private Vector3 initialPositionOffset; // ��ɫ������㽶Ƥ�ĳ�ʼλ��ƫ����

	private void Update()
	{
		if (slidingPlayer != null && isPlayerSliding)
		{
			// ��ʱ����ʱ��
			slideTimer += Time.deltaTime;

			// �������ʱ�������ֹͣ���в������㽶
			if (slideTimer >= slideDuration)
			{
				StopSlidingAndDestroy();
			}
		}
	}

	private void FixedUpdate()
	{
		if (slidingPlayer != null && isPlayerSliding)
		{
			// �ƶ��㽶Ƥ
			transform.Translate(slideDirection * slideSpeed * Time.fixedDeltaTime);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// �����ײ�������Ƿ��ǽ�ɫ
		if (collision.gameObject.CompareTag(playerTag))
		{
			StartSliding(collision);
		}
	}

	private void StartSliding(Collision2D collision)
	{
		//���ý�ɫ����״̬
		isPlayerSliding = true;

		// ��ȡ��ײ���Ľ�ɫ
		slidingPlayer = collision.gameObject;

		// ��ȡ��ɫ�� Rigidbody2D
		playerRb = slidingPlayer.GetComponent<Rigidbody2D>();

		// ���û�� Rigidbody2D�������������Ϣ������
		if (playerRb == null)
		{
			Debug.LogError("��ɫû�� Rigidbody2D ���!");
			return;
		}

		// �������λ��
		initialPositionOffset = slidingPlayer.transform.position - transform.position;

		// ����ɫ����Ϊ�㽶Ƥ���Ӷ���
		slidingPlayer.transform.SetParent(transform);

		// ���û��з���
		slideDirection = slideRight ? Vector2.right : Vector2.left;

		// ���ý�ɫ����������ֹ����
		playerRb.gravityScale = 0;

		//ֹͣ��ɫ������ƶ�����ֹ���㽶Ƥ������ͻ
		playerRb.velocity = Vector2.zero;

		// �������һ������Ч�������磺
		//playerRb.AddForce(slideDirection * 5f, ForceMode2D.Impulse); // ������Ҫ������������
	}

	private void StopSlidingAndDestroy()
	{
		//ֹͣ���н���ɫ����Ϊ�ǻ���״̬
		isPlayerSliding = false;

		// ������ӹ�ϵ
		slidingPlayer.transform.SetParent(null);

		// �ָ���ɫ������
		playerRb.gravityScale = 1;

		// �����㽶Ƥ���ӳ�һ��ʱ�䣩
		Destroy(gameObject, destroyDelay);
	}
}
