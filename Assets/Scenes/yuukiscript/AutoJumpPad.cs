using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class AutoJumpPad : MonoBehaviour
{
    public float boostedJumpHeight = 5.0f;

    private void OnTriggerEnter(Collider other)
    {
        var controller = other.GetComponent<ThirdPersonController>();
        if (controller != null && controller.Grounded)
        {
            // ジャンプ力を一時的に強化
            float originalJump = controller.JumpHeight;
            controller.JumpHeight = boostedJumpHeight;

            // ジャンプさせる
            controller.ForceJump();

            // 一定時間後に元に戻す
            StartCoroutine(ResetJump(controller, originalJump));
        }
    }

    private System.Collections.IEnumerator ResetJump(ThirdPersonController controller, float original)
    {
        yield return new WaitForSeconds(1.0f);
        controller.JumpHeight = original;
    }
}

