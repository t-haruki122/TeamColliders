using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private int itemID = 0;
    /*
    itemIDについて
    0: エラー
    1: 銃
    2: 
    */

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
        Debug.Log("Player got item that has itemID: " + itemID);
        if (other.gameObject.CompareTag("Player")) {
            switch (itemID) {
                case 0: Debug.Log("Error itemID not set for this object"); gameObject.SetActive(false); break;
                case 1: GameManager.GMInstance.setHasWeapon(true); gameObject.SetActive(false); break;
                default: Debug.Log("item ID not found"); gameObject.SetActive(false); break;
            }
        }
    }
}
