using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] private float bounceForce = 20f; // ��������С
    [SerializeField] private float horizontalBounceFactor = 0.5f; // ˮƽ����Ӱ������

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ȷ����ײ��������ң�����Tagɸѡ��
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // ���㵯����������Ϊ�����ɼ���ˮƽ����Ӱ�죩
                Vector2 bounceDirection = (Vector2.up + new Vector2(playerRb.velocity.x * horizontalBounceFactor, 0)).normalized;
                playerRb.velocity = new Vector2(playerRb.velocity.x, 0); // ���ô�ֱ�ٶ�
                playerRb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}