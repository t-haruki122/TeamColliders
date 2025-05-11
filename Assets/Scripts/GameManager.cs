using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*singleton*/
    public static GameManager Instance {get; private set;}

    /*member*/
    private int score = 0;
    private int hit = 0;
    private const double hitCoefficient = 0.95;
    private int combo = 0;
    private const int weight = 100;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*hit, comboインクリメンタ，scoreゲッタ*/
    public void addHit() { ++hit; }
    public void addCombo() {++combo; }
    public int getScore() { return score; }

    /*PP取得 内部メソッド*/
    private double getPP() {
        return Math.Pow(hitCoefficient, hit) * (1 + Math.Sqrt(combo / weight));
    }

    /*score加算*/
    public void addScore(int baseScore) {
        score += (int) (baseScore * getPP());
    }


    
}
