using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public float moveDistance = 5f;
    public float moveSpeed = 2f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isMoving = false;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.up * moveDistance;
    }

    public void StartMoving()
    {
        if (!isMoving)
            StartCoroutine(MoveUp());
    }

    // 戻る用
    public void ReturnToStart()
    {
        if (!isMoving)
            StartCoroutine(MoveDown());
    }

    private System.Collections.IEnumerator MoveUp()
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }

    // 戻る用
    private System.Collections.IEnumerator MoveDown()
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, startPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = startPos;
        isMoving = false;
    }
}