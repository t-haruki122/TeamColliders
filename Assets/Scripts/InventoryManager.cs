using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ItemID {
    None,
    Weapon,
    Key1,
    Key2,
    Key3,
    Key4
}

public class InventoryManager : MonoBehaviour
{
    /*singleton*/
    public static InventoryManager IInstance {get; private set;}

    /*<-+-*-~-=-=-~-*-+-member-+-*-~-=-=-~-*-+->*/
    /*slotのオブジェクト本体*/
    [SerializeField] private GameObject slot1;
    [SerializeField] private GameObject slot2;
    [SerializeField] private GameObject slot3;
    [SerializeField] private GameObject slot4;
    [SerializeField] private GameObject slot5;
    [SerializeField] private GameObject slot6;
    /*inventory関連*/
    private const int inventorySize = 6;
    private int activeSlot = 0; //現在選択中のスロット
    private Image[] slots = new Image[inventorySize];
    private Sprite[] itemSprite = new Sprite[inventorySize];
    private Outline[] outlines = new Outline[inventorySize];
    private TextMeshProUGUI[] slotTags = new TextMeshProUGUI[inventorySize];
    private int[] currentItem = new int[inventorySize];


    /*<-+-*-~-=-=-~-*-+-eventMethod-+-*-~-=-=-~-*-+->*/
    void Awake() {
        if (IInstance == null) {
            IInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        slots[0] = slot1.GetComponent<Image>(); 
        slots[1] = slot2.GetComponent<Image>();
        slots[2] = slot3.GetComponent<Image>();
        slots[3] = slot4.GetComponent<Image>();
        slots[4] = slot5.GetComponent<Image>();
        slots[5] = slot6.GetComponent<Image>();

        itemSprite[(int)ItemID.None] = Resources.Load<Sprite>("");
        itemSprite[(int)ItemID.Weapon] = Resources.Load<Sprite>("Images/scorpion");
        itemSprite[(int)ItemID.Key1] = Resources.Load<Sprite>("Images/key");
        itemSprite[(int)ItemID.Key2] = Resources.Load<Sprite>("Images/key");
        itemSprite[(int)ItemID.Key3] = Resources.Load<Sprite>("Images/key");
        itemSprite[(int)ItemID.Key4] = Resources.Load<Sprite>("Images/key");

        for (int i = 0; i < inventorySize; ++i) {
            slotTags[i] = slots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            slotTags[i].enabled = false;
            outlines[i] = slots[i].GetComponent<Outline>();
            outlines[i].enabled = false;
            currentItem[i] = (int)ItemID.None;
        }
        outlines[0].enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { setActiveSlot(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { setActiveSlot(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { setActiveSlot(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { setActiveSlot(3); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { setActiveSlot(4); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { setActiveSlot(5); }
        if (Input.GetKeyDown(KeyCode.F)) { useItem(activeSlot); }

        for (int i = 0; i < inventorySize; ++i) {
            if (itemSprite[i] != null) {
                slots[i].sprite = itemSprite[currentItem[i]];
            }
            setSlotTags(i);
        }
    }

    /*<-+-*-~-=-=-~-*-+-method-+-*-~-=-=-~-*-+->*/
    /*現在のスロットにUIを変更*/
    private void setActiveSlot(int slotIndex) {
        outlines[activeSlot].enabled = false;

        activeSlot = slotIndex;
        outlines[activeSlot].enabled = true;
        activeItem();
        setSlotTags(activeSlot);
    }
    /*現在のアイテムの適用*/
    private void activeItem() {
        switch (currentItem[activeSlot]) {
            case (int)ItemID.None: 
                GameManager.GMInstance.setWeapon(new unarmed());
                break;
            case (int)ItemID.Weapon:
                GameManager.GMInstance.setWeapon(new scorpion());
                break;
            default:
                GameManager.GMInstance.setWeapon(new unarmed());
                break;
        }
    }
    /*消費可能アイテムの使用*/
    public void useItem(int slotIndex) {

    }
    /*inventoryにアイテムを追加する(左詰め)*/
    public void setInventorySlot(int itemId) {
        int topIndex = 0;
        for (int i = 1; i < inventorySize; ++i) {
            if (currentItem[i] == 0) {
                topIndex = i;
                break;
            }
        }
        if (topIndex != 0) {
            currentItem[topIndex] = itemId;
            setSlotTags(topIndex);
        }
    }
    /*アイテムの付属情報*/
    private void setSlotTags(int slot) {
        switch (currentItem[slot]) {
            case (int)ItemID.None:
                break;
            case (int)ItemID.Weapon:
                slotTags[slot].enabled = true;
                slotTags[slot].text = GameManager.GMInstance.getRemainingAmmo().ToString();
                break;
            default:
                break;
        }
    }
    /*Keyを持っているかどうか*/
    public bool hasKey(int keyId) {
        for (int i = 1; i < inventorySize; ++i) {
            if (currentItem[i] == keyId && keyId > 1) return true;
        }
        return false;
    }
}
