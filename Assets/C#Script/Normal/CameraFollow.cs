using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    [Header("基础设置")]
    public Transform target;

    [Tooltip("跟随平滑速度")]
    public float smoothSpeed = 0.1f;

    [Tooltip("镜头位置偏移")]
    public Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        if (target == null) return;

        // 计算目标位置
        Vector3 targetPosition = target.position + offset;

        // 平滑移动到目标位置
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime * 50 // 调整系数使参数范围更直观
        );
    }
}
