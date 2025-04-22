using UnityEngine;

public class SpringPlatform : MonoBehaviour
{
	[Header("��������")]
	[Tooltip("��ֱ��������ֵԽ������Խ��")]
	public float bounceForce = 20f;

	[Tooltip("�Ƿ��������ˮƽ�ٶ�")]
	public bool keepHorizontalVelocity = true;

	// ����ײ����ʱ
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// ����������ײ�Ӵ���
		foreach (ContactPoint2D contact in collision.contacts)
		{
			// ��鷨�߷���Yֵ>0.5��ʾ��ײ�����Ϸ���
			if (contact.normal.y > 0.5f)
			{
				// ��ȡ��ײ�����Rigidbody2D
				Rigidbody2D rb = collision.rigidbody;
				if (rb != null)
				{
					// �������ٶ�
					float newVelocityX = keepHorizontalVelocity ? rb.velocity.x : 0;
					rb.velocity = new Vector2(newVelocityX, bounceForce);
				}
				break; // ����һ����Ч��ײ���˳�ѭ��
			}
		}
	}
}