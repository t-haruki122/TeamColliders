using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    /*singleton*/
    public static GameManager GMInstance {get; private set;}

    /*<-+-*-~-=-=-~-*-+-member-+-*-~-=-=-~-*-+->*/
    /*スコア関連*/
    private const double hitCoefficient = 0.95;
    private const int weight = 20;
    private int score = 0;
    private double pp;
    private double insidePP = 1;
    private double outsidePP = 0;
    private int hit = 0;
    private int combo = 0;
    private int preHit;
    private int preCombo;
    private double elapsedTimebonus = 1;
    private const int PlayerScoreLength = 7;
    private string[] playerScoreText = new string[PlayerScoreLength]; 
    private int[] playerScores = new int[PlayerScoreLength]; 
    private bool dataSavedFlag = false;
    private int currentDataIndex = 0;

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

    /*<-+-*-~-=-=-~-*-+-eventMethod-+-*-~-=-=-~-*-+->*/
    void Awake() {
        if (GMInstance == null) {
            GMInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            if (Player == null) {
                Player = GameObject.FindWithTag("Player");
                if (Player == null) Debug.Log("Player not set in GM, while tried to get Player again");
            }
            if (Scorpion == null && Player != null) {
                Scorpion = Player.transform.Find("Skeleton/Hips/Spine/Chest/UpperChest/Right_Shoulder/Right_UpperArm/Right_LowerArm/Right_Hand/Scorpion").gameObject;
                if (Scorpion == null) Debug.Log("Scorpion not set in GM, while tried to get Scorpion again");
                else setWeapon(new unarmed());
            }
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
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
            if (combo > 0) {
                MessageStream.MSInstance.addMessage(new Message($"コンボリセット: {combo} > 0"));
            }
            combo = 0;
            preCombo = 0;
            setPP();
            outsidePP *= hitCoefficient;
            damageLevel = 1 + (damageLevel - 1) / 2; //comboが途切れると増加したダメージが半分になる
            preHit = hit;   
        }

        /* 射撃をしているか(左クリック) */
        // 左クリックを取得(武器を持っていなかったら射撃できなくする)
        isFiring = getHasWeapon()? Input.GetMouseButton(0): false;

        /* ADSをしているか(右クリック) */
        // 右クリックを取得(武器を持っていなかったらADSできなくする)
        isAiming = getHasWeapon()? Input.GetMouseButton(1): false;

        updatePP();
    }

    /*<-+-*-~-=-=-~-*-+-method-+-*-~-=-=-~-*-+->*/
    /*スコア関連*/
    /*hit, combo incrementer*/
    public void addHit() { ++hit; }
    public void addCombo() {++combo; }
    /*主要変数初期化*/
    public void initialize() {
        score = 0;
        insidePP = 1;
        outsidePP = 0;
        hit = 0;
        combo = 0;
        preHit = 0;
        preCombo = 0;
        baseDamage = 10;
        remainingAmmo = 100;
        elapsedTimebonus = 1;
        damageLevel = 1;
        dataSavedFlag = false;
        //MusicManager.MMInstance.StopMusic();
    }
    /*elapsedTiemBonus*/
    public double getElapsedTimeBonus() {
        if (ShowTime.STInstance != null) {
            return 1 + 1 / (1 + Math.Pow(Math.E, ShowTime.STInstance.getElapsedTime() / 150 - 3));
        }
        return 1;
    }

    /*setter, getter*/
    public int getScore() { return score; }
    public int getCombo() { return combo; }
    public int getPlayerScoreLength() { return PlayerScoreLength; }

    private void updatePP() { pp = insidePP + outsidePP; }
    private void setPP() {
        insidePP = Math.Pow(hitCoefficient, hit) * (1.0 + Math.Sqrt((double)combo / (double)weight));
    }

    public double getPP() { return pp; }

    /*add score*/
    public int addScore(int baseScore)
    {
        score += (int)(baseScore * pp);
        return (int)(baseScore * pp);
    }
    
    /*pp recover*/
    public void addPP(RecoverPP item) { outsidePP += item.getItem(); }
    public void resetHit() {
        if (hit > 5) hit -= 5;
        else hit = 0;
    }
    /*スコア集計*/
    /*scoreデータを保存. 第二引数:type=0で名前, type=1でスコア*/
    public void savePlayerData(string data, int type) {
        /*type:0なら名前を保存*/
        if (type == 0) playerScoreText[currentDataIndex] = data;
        /*type:1ならスコアを保存*/
        else if (type == 1) {
            playerScoreText[currentDataIndex] += " : " + data;
            playerScores[currentDataIndex] = int.Parse(data);
            dataSavedFlag = true; //playerNameとscoreの保存完了
            if (++currentDataIndex == PlayerScoreLength) --currentDataIndex; 
        }
    }
    /*スコア順に並び替え*/
    public void sortScore() {
        /*名前とスコアをペアにする*/
        List<KeyValuePair<string, int>> playerDatas = new List<KeyValuePair<string, int>>();
        for (int i = 0; i < PlayerScoreLength; ++i) playerDatas.Add(new KeyValuePair<string, int>(playerScoreText[i], playerScores[i]));

        /*スコア順にソート*/
        var sortedPlayerDatas = playerDatas.OrderByDescending(pair => pair.Value).ToList();
        for (int i = 0; i < PlayerScoreLength; ++i) {
            var (name, score) = sortedPlayerDatas[i];
            playerScoreText[i] = name;
            playerScores[i] = score;
        }
        
    }
    /*playerScoreTextのgetter*/
    public string[] getplayerScoreText() { return playerScoreText; }

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
