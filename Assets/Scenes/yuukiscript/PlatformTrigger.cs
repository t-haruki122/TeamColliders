using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    private PlatformMover mover;

    public float DelayTime = 2f;

    void Start()
    {
        mover = GetComponentInParent<PlatformMover>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && mover != null)
        {
            Debug.Log("プレイヤーがトリガーに触れた！");
            StartCoroutine(DelayMoveCoroutine());
        }
    }

    private System.Collections.IEnumerator DelayMoveCoroutine()
    {
        yield return new WaitForSeconds(DelayTime);
        mover.StartMoving();
    }
}
