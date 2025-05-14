using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWalkway : MonoBehaviour
{
    public List<Transform> pathPoints;
    public float speed = 5f;

    private bool playerOn = false;
    private Transform player;
    private int currentIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOn = true;
            player = other.transform;
            currentIndex = 0;
            Debug.Log("Player entered walkway");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOn = false;
            player = null;
            Debug.Log("Player exited walkway");
        }
    }

    private void Update()
    {
        if (playerOn && player != null && currentIndex < pathPoints.Count)
        {
            Vector3 target = pathPoints[currentIndex].position;
            Vector3 moveDir = (target - player.position).normalized;
            Vector3 move = moveDir * speed * Time.deltaTime;
            player.position += move;

            Debug.Log($"Moving player towards point {currentIndex}: {target}, Current Position: {player.position}");

            if (Vector3.Distance(player.position, target) < 0.5f)
            {
                Debug.Log($"Reached point {currentIndex}");
                currentIndex++;
            }
        }
    }
}
