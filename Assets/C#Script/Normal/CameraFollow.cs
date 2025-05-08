using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("��������")]
    public Transform target;

    [Tooltip("����ƽ���ٶ�")]
    public float smoothSpeed = 0.1f;

    [Tooltip("��ͷλ��ƫ��")]
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("����ƶ���Χ����")]
    [Tooltip("�Ƿ������ƶ���Χ����")]
    public bool enableBounds = true;

    [Tooltip("���X����С�ƶ�λ��")]
    public float minX;
    [Tooltip("���X������ƶ�λ��")]
    public float maxX;

    [Tooltip("���Y����С�ƶ�λ��")]
    public float minY;
    [Tooltip("���Y������ƶ�λ��")]
    public float maxY;

    void LateUpdate()
    {
        if (target == null) return;

        // ����Ŀ��λ��
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition;

        smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime * 50 
        );

        // ��������˷�Χ����
        if (enableBounds)
        {
            // ����X��
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
            // ����Y��
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);
        }

        transform.position = smoothedPosition;
    }
}
