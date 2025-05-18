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

    public Vector3 warpLocation = new Vector3(-90, 1, -90);

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("触れたオブジェクト: " + other.name);

        if (other.CompareTag("Player"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false; // 一時的に無効化
                other.transform.position = warpLocation;
                cc.enabled = true;  // 再び有効化
                Debug.Log("ワープさせた！");
            }
        }
    }


}
