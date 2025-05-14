using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSensor : MonoBehaviour
{
    public Transform detectionPoint; // プレイヤーを検出するための基準位置（床の中心）

    public float detectionRange = 0.2f; // 検出する高さの範囲
    public LayerMask playerLayer;

    private bool playerOn = false;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(detectionPoint.position + Vector3.up * 0.1f, Vector3.up, out hit, detectionRange, playerLayer))
        {
            Debug.Log("ヒット: " + hit.collider.name);
            if (!playerOn)
            {
                Debug.Log("プレイヤーが乗った！");
                playerOn = true;
                // ここにギミック起動処理を書く
            }
        }
        else
        {
            if (playerOn)
            {
                Debug.Log("プレイヤーが降りた！");
                playerOn = false;
            }
        }
    }
    
}
