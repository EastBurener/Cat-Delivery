using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    [Header("��������")]
    public Transform target;

    [Tooltip("����ƽ���ٶ�")]
    public float smoothSpeed = 0.1f;

    [Tooltip("��ͷλ��ƫ��")]
    public Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        if (target == null) return;

        // ����Ŀ��λ��
        Vector3 targetPosition = target.position + offset;

        // ƽ���ƶ���Ŀ��λ��
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime * 50 // ����ϵ��ʹ������Χ��ֱ��
        );
    }
}
