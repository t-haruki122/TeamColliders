using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GimmickKey : MonoBehaviour
{
    [SerializeField]
    protected GameObject gate;
    [SerializeField]
    protected int requiredKeyId = (int)ItemID.Key1;
    [SerializeField]
    protected float threshold = 10f; // 距離の閾値（必要に応じて調整）
    [SerializeField]
    protected TextMeshPro textBox;

    protected BoxCollider collider;

    protected int statePrev = -1;

    void Start()
    {
        collider = gate.GetComponent<BoxCollider>();
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return; // プレイヤーが取得できなかった
        
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > threshold) {
            // プレイヤーが遠すぎる
            if (textBox == null) return;
            textBox.text = "";
            statePrev = -1;
            return;
        }

        var IM = InventoryManager.IInstance;
        int state = -1;
        if (IM.getActiveKeyID() == requiredKeyId)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                IM.useItem(IM.getActiveSlot());
                openGate();
                MessageStream.MSInstance.addMessage(new Message("ゲートが開いた!"));
                return;
            }
            else state = 1;
        }
        else
        {
            if (IM.hasKey(requiredKeyId)) state = 2;
            else state = 3;
        }
        
        /* instructionを更新 */
        if (state == statePrev) return; // 更新なし
        statePrev = state;

        if (textBox == null) return; // textBoxがない

        switch (state)
        {
            case 1:  textBox.text = $"Press F to use Key {requiredKeyId}"; break;
            case 2:  textBox.text = $"You have Key {requiredKeyId}...";    break;
            case 3:  textBox.text = $"Key {requiredKeyId} is required";    break;
            default: textBox.text = $"Unknown state";                      break;
        }
    }

    private void openGate()
    {
        Debug.Log("ゲートを開きます。");
        gate.SetActive(false);
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
