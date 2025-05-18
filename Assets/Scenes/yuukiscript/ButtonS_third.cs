using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonS_third : MonoBehaviour
{

    private int pressCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pressCount++;

            // 壁の移動方向：奇数回 → +15、偶数回 → -15
            int direction = (pressCount % 2 == 1) ? 5 : -5;

            // 赤：y軸方向、青：x軸方向、黄：z軸方向
            if (gameObject.tag == "Redbutton")
            {
                MoveWalls("C_Red", new Vector3(0, direction, 0));
            }
            else if (gameObject.tag == "Bluebutton")
            {
                MoveWalls("C_Blue", new Vector3(direction * 3, 0, 0));
            }
            else if (gameObject.tag == "Yellowbutton")
            {
                MoveWalls("C_Yellow", new Vector3(0, 0, direction * -3));
            }
        }
    }

    private void MoveWalls(string wallPrefix, Vector3 moveDirection)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith(wallPrefix))
            {
                obj.transform.position += moveDirection;
            }
        }
    }
}