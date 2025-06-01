using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowDamage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI showDamage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int damage = GameManager.GMInstance.getDamage();
        showDamage.text = "WEAPON DAMAGE: "+ damage;
    }
}
