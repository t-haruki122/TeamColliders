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
    private int baseDamage = 10;
    private int remainingAmmo = 100;
    private double damageLevel = 1.0;
    private Weapon weapon;

    private GameObject Player;
    private GameObject Scorpion;

    private bool isFiring = false;
    private bool isAiming = false;

    /*敵関連*/
    private bool isAct = true;

    /*inventorySystem*/
    private InventoryManager inventory;

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
        inventory = new InventoryManager();
        Application.targetFrameRate = 60;
        preHit = hit;
        preCombo = combo;

        // プレイヤーのゲームオブジェクトを取得
        Player = GameObject.FindWithTag("Player");
        if (Player == null){
            Debug.Log("Warning: Player object not set in GM! plz confirm player has its tag");
        }
        else {
            // 武器のゲームオブジェクトを取得
            Scorpion = Player.transform.Find("Skeleton/Hips/Spine/Chest/UpperChest/Right_Shoulder/Right_UpperArm/Right_LowerArm/Right_Hand/Scorpion").gameObject;
        }

        /* プレイヤーを素手に設定 */
        setWeapon(new unarmed());
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

        /* 射撃をしているか(左クリック) */
        // 左クリックを取得(武器を持っていなかったら射撃できなくする)
        isFiring = getHasWeapon()? Input.GetMouseButton(0): false;

        /* ADSをしているか(右クリック) */
        // 右クリックを取得(武器を持っていなかったらADSできなくする)
        isAiming = getHasWeapon()? Input.GetMouseButton(1): false;

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
    public void addPP(RecoverPP item) { pp += item.getItem(); }
    public void resetHit() {
        if (hit > 5) hit -= 5;
        else hit = 0;
    }

    /*弾関連*/ 
    public void reduceAmmo() { --remainingAmmo; }
    public void addAmmo(RecoverAmmo item) { remainingAmmo += (int)item.getItem(); }
    public int getRemainingAmmo() { return remainingAmmo; }

    /*戦闘システム関連*/
    private void setDamageLevel() {
        damageLevel *= damageCoefficient;
    }
    public int getDamage() { return (int) (damageLevel * baseDamage); }
    public bool getIsAct() { return isAct; }
    public void setIsAct(bool isAct) {
        this.isAct = isAct;
    }

    /*武器関連*/
    public bool getHasWeapon() { return (int)weapon.getItem() >= 1; }
    public void setWeapon(Weapon w) {
        this.weapon = w;
        this.updateWeapon();
    }
    private void updateWeapon() {
        if (weapon is unarmed) {
            if (Scorpion == null) Debug.Log("Warning: Scorpion object not set in GM!");
            else Scorpion.SetActive(false);
        }
        else if (weapon is scorpion) {
            if (Scorpion == null) Debug.Log("Warning: Scorpion object not set in GM!");
            else Scorpion.SetActive(true);
        }
        else Debug.Log("Cannot update weapon: Unknown weapon ID: " + this.weapon);
    }
    public bool getIsFiring() { return this.isFiring; }
    public bool getIsAiming() { return this.isAiming; }
}
