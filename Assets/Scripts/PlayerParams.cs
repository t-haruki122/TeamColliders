using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PlayerParams : MonoBehaviour
{
    public int cntHit;

    public bool hasWeapon = false;

    // ゲームオブジェクト(Serialize)
    public TextMeshProUGUI cntHitText;
    public GameObject weapon;

    // Start is called before the first frame update
    void Start()
    {
        cntHit = 0;

        /* 武器を最初から持たせる（テスト用） */
        hasWeapon = false;
    }

    // Update is called once per frame
    void Update()
    {
        // CntHitを画面表示
        cntHitText.SetText("CntHit: " + cntHit);

        // weaponの表示・非表示
        weapon.SetActive(hasWeapon);
    }
}
