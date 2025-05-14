using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter 発火: " + other.gameObject.name);
        if (other.gameObject.CompareTag("warpCube"))
        {
            transform.position = new Vector3(-90, 1, -90);
        }
    }
}
