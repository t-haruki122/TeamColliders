using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour
{
    public float movingSpeed = 5f;
    public float movigReadiius = 3f;
    public float watingTime = 2f;

    private Vector3 originPosition;
    private Vector3 targetPosition;
    private bool isWating = false;

    void Start()
    {
        originPosition = transform.position;
        ChoooseNewDestination();
    }

    void Update()
    {
        if (isWating) return;

        // 目的地に近づいたら新しい目的地を選ぶ
        if (Vector3.Distance(transform.position, targetPosition) < 0.2f)
        {
            StartCoroutine(WaitAndChooseNewDestination());
        }
        else
        {
            // 移動処理
            Vector3 dir = (targetPosition - transform.position).normalized;
            transform.position += dir * movingSpeed * Time.deltaTime;
        }
    }

    private void ChoooseNewDestination()
    {
        float x = Random.Range(-movigReadiius, movigReadiius);
        float z = Random.Range(-movigReadiius, movigReadiius);
        targetPosition = originPosition + new Vector3(x, 0, z);
    }

    System.Collections.IEnumerator WaitAndChooseNewDestination()
    {
        isWating = true;
        yield return new WaitForSeconds(watingTime);
        ChoooseNewDestination();
        isWating = false;
    }
}
