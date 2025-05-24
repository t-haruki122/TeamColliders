using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] protected GameObject itemPrefab;
    [SerializeField] protected int itemProperty = 0;
    private Item self;
    private bool used = false;
    private bool isGrounded = false;
    private float verticalVelocity = -1f;
    private const float rayDistance = 0.5f; // 地面との接地判定距離

    // Start is called before the first frame update
    void Start()
    {
        if (!(itemPrefab == null))
        {
            if (itemPrefab.name == "Scorpion") self = new scorpion();
            else if (itemPrefab.name == "RecoverPPS") self = new recoverPPs();
            else if (itemPrefab.name == "RecoverPPM") self = new recoverPPm();
            else if (itemPrefab.name == "RecoverPPL") self = new recoverPPl();
            else if (itemPrefab.name == "RecoverAmmoS") self = new recoverAmmos();
            else if (itemPrefab.name == "RecoverAmmoM") self = new recoverAmmom();
            else if (itemPrefab.name == "RecoverAmmoL") self = new recoverAmmol();
            else if (itemPrefab.name == "Key") self = new Key(itemProperty);
            else if (itemPrefab.name == "SunGlasses") self = new SunGlasses(); // 遊び
            else
            {
                Debug.Log(gameObject.name + ": Invalid itemPrefab: " + itemPrefab.name);
                return;
            }
            Instantiate(itemPrefab, transform.position, Quaternion.identity, transform);
        }
        else
        {
            Debug.Log(gameObject.name + ": ItemPrefab not set to this object");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 地面チェック
        isGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance + 0.01f);

        if (!isGrounded)
        {
            transform.Translate(Vector3.up * verticalVelocity * Time.deltaTime);
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity = 0f; // 地面についたら落下停止
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (used) return;
        if (other.gameObject.CompareTag("Player"))
        {
            /* オブジェクトのクラスを比較して適切な処理をする */
            if (self is Weapon)
            {
                InventoryManager.IInstance.setInventorySlot((int)((Weapon)self).getItem());
                AudioManager.AMInstance.PlayDefaultAcquireSound();
            }
            else if (self is RecoverPP)
            {
                GameManager.GMInstance.addPP((RecoverPP)self);
                AudioManager.AMInstance.PlayRecoverPPSound();
            }
            else if (self is RecoverAmmo)
            {
                GameManager.GMInstance.addAmmo((RecoverAmmo)self);
                AudioManager.AMInstance.PlayRecoverAmmoSound();
            }
            else if (self is Key)
            {
                Debug.Log("Player got key: " + ((Key)self).getItem());
                AudioManager.AMInstance.PlayDefaultAcquireSound();
            }
            else if (self is SunGlasses)
            {
                GameManager.GMInstance.setIsAct(false);
                AudioManager.AMInstance.PlayDefaultAcquireSound();
            }
            else
            {
                Debug.Log(this.gameObject.name + ": Item not set or unknown");
                return;
            }
            used = true;
            // 獲得メッセージ
            MessageStream.MSInstance.addMessage(new AcquireMessage(self));
            // ゲームオブジェクトを破壊
            Destroy(this.gameObject);
        }
    }
}
