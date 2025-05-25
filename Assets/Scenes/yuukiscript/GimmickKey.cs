using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickKey : MonoBehaviour
{
    [SerializeField]
    private GameObject gate;
    [SerializeField]
    private int requiredKeyId = (int)ItemID.Key1;

    private BoxCollider collider;

    void Start()
    {
        collider = gate.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("触れたオブジェクト: " + other.name);

        if (other.CompareTag("Player"))
        {
            if (InventoryManager.IInstance.hasKey(requiredKeyId))
            {
                collider.isTrigger = true;
                Debug.Log("鍵あり。ゲートを開きます。");
            }
            else
            {
                Debug.Log("必要な鍵を持っていません。");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    { 
        Debug.Log("触れたオブジェクト: " + other.name);

        if (other.CompareTag("Player"))
        {
            collider.isTrigger = false;
        }
    }
}
