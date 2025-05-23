using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 firstPos;

    // Start is called before the first frame update
    void Start()
    {
        firstPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /* MOVEMENT KEYBINDS */
        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            // move forward
            transform.position += transform.forward * 0.1f;
        }
        if (Input.GetKey("down") || Input.GetKey("s"))
        {
            // move backward
            transform.position -= transform.forward * 0.1f;
        }
        if (Input.GetKey("right") || Input.GetKey("d"))
        {
            // strafe right
            transform.position += transform.right * 0.1f;
        }
        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            // strafe left
            transform.position -= transform.right * 0.1f;
        }

        /* ROTATION KEYBINDS */
        if (Input.GetKey ("q"))
        {
            // rotate left
            transform.Rotate(0f, -3.0f, 0f);
        }
        if (Input.GetKey ("e"))
        {
            // rotate right
            transform.Rotate(0f, 3.0f, 0f);
        }

        /* MOVEMENT RESTRICTION */
        if (transform.position.y < 0) transform.position = firstPos;

        /* ROTATION RESTRICTION */
        Vector3 rot = transform.eulerAngles;
        rot.x = 0f;
        rot.z = 0f;
        transform.eulerAngles = rot;
    }
}
