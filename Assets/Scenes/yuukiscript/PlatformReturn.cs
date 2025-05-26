using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformReturn : MonoBehaviour
{
    public PlatformMover mover;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && mover != null)
        {
            Debug.Log("戻るボタンが押された！");
            mover.ReturnToStart();
        }
    }
}
