using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float angularVelocity = 45f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 center = transform.position;
        Vector3 axis = new Vector3(0f, 1f, 0f);
        transform.RotateAround(center, axis, Time.deltaTime * angularVelocity);
    }
}
