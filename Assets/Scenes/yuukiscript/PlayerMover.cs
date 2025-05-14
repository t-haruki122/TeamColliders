using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public List<Transform> pathPoints; // 経路ポイント
    public float speed = 3f;

    private bool isMoving = false;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter 発火: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player") && !isMoving)
        {
            Debug.Log("プレイヤーが接触し、移動開始！");
            StartCoroutine(MovePlayer(collision.gameObject.transform));
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("接触したオブジェクト: " + hit.gameObject.name);
    }

    private IEnumerator MovePlayer(Transform player)
    {
        isMoving = true;

        for (int i = 0; i < pathPoints.Count; i++)
        {
            Transform point = pathPoints[i];
            Debug.Log($"次のポイント [{i}]: {point.name} に移動開始");

            while (Vector3.Distance(player.position, point.position) > 0.1f)
            {
                Vector3 direction = (point.position - player.position).normalized;
                Debug.Log($"移動中: player.pos={player.position}, target={point.position}, dir={direction}");

                player.position = Vector3.MoveTowards(
                    player.position,
                    point.position,
                    speed * Time.deltaTime
                );
                yield return null;
            }

            Debug.Log($"ポイント [{i}] 到達: {point.name}");
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("すべてのポイントに到達、移動終了");
        isMoving = false;
    }
    
}
