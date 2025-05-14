using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkwayMover : MonoBehaviour
{
    [Header("プレイヤー検出設定")]
    public Transform detectionPoint; // 床の中心位置
    public float detectionRange = 5f; // 検出範囲
    public LayerMask playerLayer;

    [Header("経路設定")]
    public List<Transform> pathPoints; // 経路上のポイント
    public float moveSpeed = 2f;

    private int currentIndex = 0;
    private bool isMoving = false;

    void Update()
    {
        // プレイヤーを上方向にレイキャストで検出
        RaycastHit hit;
        if (Physics.Raycast(detectionPoint.position + Vector3.up * 0.1f, Vector3.up, out hit, detectionRange, playerLayer))
        {
            if (!isMoving)
            {
                Debug.Log("プレイヤー検出！移動開始");
                isMoving = true;
            }
        }

        // 経路に沿って床を移動
        if (isMoving && currentIndex < pathPoints.Count)
        {
            Vector3 target = pathPoints[currentIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                currentIndex++;
            }
        }
    }

    
}
