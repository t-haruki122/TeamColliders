using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; // slider(HPバー)用

public class ShapeBoss : baseEnemy
{
    // 発見UI用のビックリマークとはてなマークと沈黙マーク
    private GameObject TMP_exclamation;
    private GameObject TMP_question;
    private GameObject TMP_quiet;

    // ゲームオブジェクト
    private Slider enemyHPBar;

    // 値を共有するスクリプト
    private EnemyShot enemyShot;

    // 内部記憶_敵の記憶
    private Vector3 playerPositionMemory = new Vector3(0, 0, 0);

    // 内部記憶_システム変数
    private bool isVisible = false;
    private bool isVisibleMemory = false;
    private int internalFrameCount = -1;
    private int state = 0;

    // 敵のパラメータ
    bool isIdleRotation;
    float angularSpeed = 90f;

    private void resetUI()
    {
        TMP_exclamation.SetActive(false);
        TMP_question.SetActive(false);
        TMP_quiet.SetActive(false);
    }

    protected override void Spawn()
    {
        setBaseParams(
            maxHP: 1000
        );
        Debug.Log(maxHP);
        isIdleRotation = true;

        enemyShot = transform.Find("EnemyShot").GetComponent<EnemyShot>();
        enemyShot.isActiveEnemyShot = false;

        enemyHPBar = transform.Find("StatusUI/Canvas/EnemyHPBar").GetComponent<Slider>();

        TMP_exclamation = transform.Find("StanceUI/TMP_exclamation").gameObject;
        TMP_question = transform.Find("StanceUI/TMP_question").gameObject;
        TMP_quiet = transform.Find("StanceUI/TMP_quiet").gameObject;

        resetUI();
    }

    protected void updatePlayerPositionMemory() {
        // プレイヤーメモリを更新
        playerPositionMemory = target.transform.position;
        // プレイヤメモリのy座標をデフォルト値に
        playerPositionMemory.Set(playerPositionMemory.x, this.transform.position.y, playerPositionMemory.z);
    }

    protected void move(Vector3 targetPos) {
        // プレイヤーの位置に移動
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        transform.LookAt(targetPos);
    }

    protected void rotateAtPosition(float angularSpeed) {
        transform.RotateAround(transform.Find("Center").position, new Vector3(0f, 1f, 0f), Time.deltaTime * angularSpeed);
        Vector3 rot = transform.eulerAngles;
        rot.x = 0f;
        rot.z = 0f;
        transform.eulerAngles = rot;
    }

    protected override void Act()
    {
        // HPバーの更新
        enemyHPBar.value = (float)HP / (float)maxHP;

        // 敵の色の更新
        // デフォルトカラー!!マテリアルの色変えたら変えること！！
        Color color = new Color(0.5f, 0.5f, 0.5f);
        if (HP < maxHP / 2)
        {
            // HP(50%)以下で黄色 -> 赤色に変遷
            float _val = 0.5f - (float)HP / (float)maxHP;
            // Debug.Log(_val);
            color = new Color(1f, 1f - 2f * _val, 0.5f - _val);
        }
        transform.Find("Body").GetComponent<Renderer>().material.color = color;
        transform.Find("Body/WingRight").GetComponent<Renderer>().material.color = color;
        transform.Find("Body/WingLeft").GetComponent<Renderer>().material.color = color;
        transform.Find("Body/Halo").GetComponent<Renderer>().material.color = color;

        /****敵対ビヘイビア****/

        // プレイヤーが見えるか？
        isVisible = isGetDamageOnFrame || getIsVisible();
        Debug.Log(isVisible);
        if (isVisible) isVisibleMemory = true;
        
        if (state == 0 && isVisible){
            // 初発見
            state = 1;
        }
        
        if (state <= 0) return; // プレイヤー未発見
        
        // メインビヘイビア
        internalFrameCount++;
        if (internalFrameCount % 120 == 0) {
            internalFrameCount = 0;
            // ランダムステート
            switch (state) {
                case 1: state = 2; break;
                case 2: state = 3; break;
                case 3: state = 4; break;
                case 4: state = 1; break;
                default: break;
            }
            Debug.Log("state: " + state);
        }
        updatePlayerPositionMemory();

        if (state == 1) {
            // プレイヤーの位置に移動
            move(playerPositionMemory);
            enemyShot.isActiveEnemyShot = false;
        }
        if (state == 2) {
            // プレイヤーに対して攻撃
            transform.LookAt(playerPositionMemory);
            enemyShot.isActiveEnemyShot = true;
        }
        if (state == 3) {
            // 回転しながら攻撃
            rotateAtPosition(135f);
            enemyShot.isActiveEnemyShot = true;
        }
        if (state == 4) {
            // プレイヤーに垂直に移動 TODO
            enemyShot.isActiveEnemyShot = false;
        }
    }
}