using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRotator : MonoBehaviour
{
    public Transform detectionPoint;          // 検出位置
    public float detectionRange = 0.2f;       // 検出範囲
    public LayerMask playerLayer;             // プレイヤーレイヤー
    public Transform wallToRotate;            // 回転させる壁オブジェクト
    public float rotationAngle = 90f;         // 回転角度
    public float rotationSpeed = 100f;        // 回転速度

    private bool hasRotated = false;          // 1度だけ回転させる用
    private Quaternion targetRotation;

    void Update()
    {
        RaycastHit hit;
        // 🔵 デバッグ表示（見えるレイ）
        Debug.DrawRay(detectionPoint.position + Vector3.up * 0.1f, Vector3.down * detectionRange, Color.cyan);

        if (Physics.Raycast(detectionPoint.position + Vector3.down * 0.1f, Vector3.down, out hit, detectionRange, playerLayer))
        {
            if (!hasRotated)
            {
                Debug.Log("プレイヤーを検出。壁を回転します。");
                hasRotated = true;
                targetRotation = Quaternion.Euler(0, rotationAngle, 0) * wallToRotate.rotation;
            }
        }

        // スムーズに回転
        if (hasRotated && wallToRotate.rotation != targetRotation)
        {
            wallToRotate.rotation = Quaternion.RotateTowards(wallToRotate.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
