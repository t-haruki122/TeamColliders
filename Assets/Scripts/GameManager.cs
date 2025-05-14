using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*singleton*/
    public static GameManager GMInstance {get; private set;}

    /*<-+-*-~-=-=-~-*-+-member-+-*-~-=-=-~-*-+->*/
    /*スコア関連*/
    private const double hitCoefficient = 0.95;
    private const int weight = 100;
    private int score = 0;
    private double pp = 1;
    private int hit = 0;
    private int combo = 0;
    private int preHit;
    private int preCombo;

    /*銃関連*/
    private const double damageCoefficient = 1.08;
    private int baseDamage = 100;
    private int remainingAmmo = 100;
    private double damageLevel = 1.0;

    /*<-+-*-~-=-=-~-*-+-eventMethod-+-*-~-=-=-~-*-+->*/
    void Awake() {
        if (GMInstance == null) {
            GMInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        preHit = hit;
        preCombo = combo;
    }

    // Update is called once per frame
    void Update()
    {
        /*hit, combo数に変更があればppを計算*/
        if (preCombo != combo) {
            setPP();
            setDamageLevel();
            preCombo = combo;
        }
        if (preHit != hit ) {
            combo = 0;
            preCombo = 0;
            setPP();
            damageLevel = 1 + (damageLevel - 1) / 2; //comboが途切れると増加したダメージが半分になる
            preHit = hit;   
        }
    }

    /*<-+-*-~-=-=-~-*-+-method-+-*-~-=-=-~-*-+->*/
    /*スコア関連*/
    /*hit, combo incrementer*/
    public void addHit() { ++hit; }
    public void addCombo() {++combo; }

    /*setter, getter*/
    public int getScore() { return score; }
    public int getCombo() { return combo; }

    private void setPP() {
        pp = Math.Pow(hitCoefficient, hit) * (1 + Math.Sqrt(combo / weight));
    }

    public double getPP() { return pp; }

    /*add score*/
    public void addScore(int baseScore) {
        score += (int) (baseScore * pp);
    }
    
    /*pp recover*/
    protected void addPP(double variable) {
        pp += variable;
    }

    /*銃関連*/ 
    public void reduceAmmo() { --remainingAmmo; }
    protected void addAmmo(int num) { remainingAmmo += num; }

    private void setDamageLevel() {
        damageLevel *= damageCoefficient;
    }
    public int getDamage() { return (int) (damageLevel * baseDamage); }
}
