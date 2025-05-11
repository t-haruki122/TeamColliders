using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PlayerParams : MonoBehaviour
{
    public int cntHit;

    // ゲームオブジェクト
    public TextMeshProUGUI cntHitText;

    // Start is called before the first frame update
    void Start()
    {
        cntHit = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cntHitText.SetText("CntHit:" + cntHit);
    }
}
