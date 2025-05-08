using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("基础设置")]
    public Transform target;

    [Tooltip("跟随平滑速度")]
    public float smoothSpeed = 0.1f;

    [Tooltip("镜头位置偏移")]
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("相机移动范围限制")]
    [Tooltip("是否启用移动范围限制")]
    public bool enableBounds = true;

    [Tooltip("相机X轴最小移动位置")]
    public float minX;
    [Tooltip("相机X轴最大移动位置")]
    public float maxX;

    [Tooltip("相机Y轴最小移动位置")]
    public float minY;
    [Tooltip("相机Y轴最大移动位置")]
    public float maxY;

    void LateUpdate()
    {
        if (target == null) return;

        // 计算目标位置
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition;

        smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime * 50 
        );

        // 如果启用了范围限制
        if (enableBounds)
        {
            // 限制X轴
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
            // 限制Y轴
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);
        }

        transform.position = smoothedPosition;
    }
}
