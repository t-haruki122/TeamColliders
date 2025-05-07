using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    // 自身のTransform
    [SerializeField] private Transform _self;
    // ターゲットのTransform
    [SerializeField] private Transform _target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        _self.LookAt(_target);
    }
}