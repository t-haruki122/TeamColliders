using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 moveDirection = new Vector3(1, 0, 0);
    public float moveDistance = 5f;
    public float speed = 2f;

    private Vector3 startPos;
    private bool forward = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (forward)
        {
            transform.Translate(moveDirection * speed * Time.deltaTime);
            if (Vector3.Distance(startPos, transform.position) >= moveDistance)
                forward = false;
        }
        else
        {
            transform.Translate(-moveDirection * speed * Time.deltaTime);
            if (Vector3.Distance(startPos, transform.position) <= 0.1f)
                forward = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(this.transform);  // 床の子にする
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);  // 子関係解除
        }
    }
}
