using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowCombo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI showCombo;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int combo = GameManager.GMInstance.getCombo();
        showCombo.text = combo.ToString();
    }
}
