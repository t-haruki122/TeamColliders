using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyOpen : MonoBehaviour
{
    [SerializeField]
    private int keyId = (int)ItemID.Key1;
    private void OnTriggerEnter(Collider other)
    { 
        Debug.Log("触れたオブジェクト: " + other.name);

        if (other.CompareTag("Player"))
        {
            InventoryManager.IInstance.setInventorySlot(keyId);
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
    }
}
