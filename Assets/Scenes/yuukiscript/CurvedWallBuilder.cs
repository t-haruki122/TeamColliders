using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedWallBuilder : MonoBehaviour
{
    public GameObject wallPrefab; // 小さなCubeなど
    public int segmentCount = 20;
    public float radius = 5f;
    public float angleRange = 90f; // 度数（例：90度のカーブ）

    void Start()
    {
        float angleStep = angleRange / (segmentCount - 1);
        for (int i = 0; i < segmentCount; i++)
        {
            float angle = -angleRange / 2 + angleStep * i;
            float rad = Mathf.Deg2Rad * angle;

            Vector3 position = new Vector3(
                Mathf.Cos(rad) * radius,
                0f,
                Mathf.Sin(rad) * radius
            );

            Quaternion rotation = Quaternion.Euler(0, -angle, 0);
            Instantiate(wallPrefab, position, rotation, transform);
        }
    }
}
