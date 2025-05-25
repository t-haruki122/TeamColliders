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

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float threshold = 10f; // 距離の閾値（必要に応じて調整）
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= threshold)
            {
                openGate();
            }
        }
    }

    private void openGate()
    {
        if (InventoryManager.IInstance.hasKey(requiredKeyId))
        {
            Debug.Log("鍵あり。ゲートを開きます。");
            gate.SetActive(false);
        }
        else
        {
            Debug.Log("必要な鍵を持っていません。");
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {

    // }

    private void OnTriggerExit(Collider other)
    { 
        Debug.Log("触れたオブジェクト: " + other.name);

        if (other.CompareTag("Player"))
        {
            collider.isTrigger = false;
        }
    }
}
