using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowPP : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI showPP;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        double pp = GameManager.GMInstance.getPP();
        showPP.text = pp.ToString("F3") + "-"+ getRank(pp);
    }
    
    /*ppに応じてランク付け*/
    private string getRank(double pp) {
        if (pp >= 2.0) return "X";
        else if (pp >= 1.0) return "S";
        else if (pp >= 0.9) return "A";
        else if (pp >= 0.7) return "B";
        else if (pp >= 0.5) return "C";
        else return "D";
    }
}
