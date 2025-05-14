using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRotatorTrigger : MonoBehaviour
{
    public Transform wallToRotate;
    public float rotationAngle = 180f;
    public float rotationSpeed = 90f;
    private bool hasRotated = false;
    private Quaternion targetRotation;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasRotated && other.CompareTag("Player")) // もしくはレイヤー判定もOK
        {
            Debug.Log("トリガーにプレイヤーが入った！ 壁を回転開始");
            hasRotated = true;
            targetRotation = Quaternion.Euler(0, rotationAngle, 0) * wallToRotate.rotation;
        }
    }

    private void Update()
    {
        if (hasRotated && wallToRotate.rotation != targetRotation)
        {
            wallToRotate.rotation = Quaternion.RotateTowards(wallToRotate.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

