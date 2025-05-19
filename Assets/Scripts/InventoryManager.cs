using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private int[] currentItem = new int[inventorySize];


    /*<-+-*-~-=-=-~-*-+-eventMethod-+-*-~-=-=-~-*-+->*/
    void Start()
    {
        slots[0] = slot1.GetComponent<Image>();
        slots[1] = slot2.GetComponent<Image>();
        slots[2] = slot3.GetComponent<Image>();
        slots[3] = slot4.GetComponent<Image>();
        slots[4] = slot5.GetComponent<Image>();
        slots[5] = slot6.GetComponent<Image>();

        itemSprite[(int)ItemID.None] = Resources.Load<Sprite>("Images/pict1");
        //itemSprite[(int)ItemID.Weapon] = Resources.Load<Sprite>("Images/");
        //itemSprite[(int)ItemID.Key1] = Resources.Load<Sprite>("Images/");
        //itemSprite[(int)ItemID.Key2] = Resources.Load<Sprite>("Images/");
        //itemSprite[(int)ItemID.Key3] = Resources.Load<Sprite>("Images/");
        //itemSprite[(int)ItemID.Key4] = Resources.Load<Sprite>("Images/");

        for (int i = 0; i < inventorySize; ++i) {
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
        }
    }

    /*<-+-*-~-=-=-~-*-+-method-+-*-~-=-=-~-*-+->*/
    private void setActiveSlot(int slotIndex) {
        outlines[activeSlot].enabled = false;

        activeSlot = slotIndex;
        outlines[activeSlot].enabled = true;
    }

    public void useItem(int slotIndex) {

    }
}
