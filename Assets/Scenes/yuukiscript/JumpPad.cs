using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpBoostMultiplier = 2f;
    public float resetDelay = 1f;

    private bool boosted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !boosted)
        {
            var controller = other.GetComponent<StarterAssets.ThirdPersonController>();
            if (controller != null)
            {
                controller.JumpHeight *= jumpBoostMultiplier;
                boosted = true;
                Debug.Log("ジャンプ力が増加！");
                StartCoroutine(ResetJump(controller));
            }
        }
    }

    private System.Collections.IEnumerator ResetJump(StarterAssets.ThirdPersonController controller)
    {
        yield return new WaitForSeconds(resetDelay);
        controller.JumpHeight /= jumpBoostMultiplier;
        boosted = false;
        Debug.Log("ジャンプ力を元に戻した");
    }
}
