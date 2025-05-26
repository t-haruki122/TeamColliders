using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkwayTrigger : MonoBehaviour
{
    private WalkwayMover mover;

    void Start()
    {
        mover = GetComponentInParent<WalkwayMover>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && mover != null)
        {
            Debug.Log("トリガーにプレイヤーが触れた！");
            mover.StartMoving();
        }
    }
}
