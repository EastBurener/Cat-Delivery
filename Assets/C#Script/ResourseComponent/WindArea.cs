using UnityEngine;

public class WindArea : MonoBehaviour
{
	[Header("��������")]
	public Vector2 windDirection = Vector2.right; // Ĭ�����Ҵ���
	[Header("������С")]
	public float windForce = 10f;

	private void OnTriggerStay2D(Collider2D other)
	{
		// ������������Ƿ��ǽ�ɫ (tag Ϊ "Player")
		if (other.CompareTag("Player"))
		{
			// ��ȡ��ɫ�� Rigidbody2D ���
			Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

			// ȷ����ɫ�� Rigidbody2D ���
			if (rb != null)
			{
				// ʩ����
				rb.AddForce(windDirection.normalized * windForce * Time.deltaTime, ForceMode2D.Impulse);
			}
		}
	}
}
