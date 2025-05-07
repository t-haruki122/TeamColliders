using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60; // TODO マージする前に消せ！！
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
    }
}
