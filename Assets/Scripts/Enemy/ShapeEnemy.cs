using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; // slider(HPバー)用

public class ShapeEnemy : baseEnemy
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

    // 敵のパラメータ
    [SerializeField] bool isIdleRotation = true;

    private void resetUI()
    {
        TMP_exclamation.SetActive(false);
        TMP_question.SetActive(false);
        TMP_quiet.SetActive(false);
    }

    protected override void Spawn()
    {
        setBaseParams(
            // 変更なし
        );

        enemyShot = transform.Find("EnemyShot").GetComponent<EnemyShot>();
        enemyShot.isActiveEnemyShot = false;

        enemyHPBar = transform.Find("StatusUI/Canvas/EnemyHPBar").GetComponent<Slider>();

        TMP_exclamation = transform.Find("StanceUI/TMP_exclamation").gameObject;
        TMP_question = transform.Find("StanceUI/TMP_question").gameObject;
        TMP_quiet = transform.Find("StanceUI/TMP_quiet").gameObject;

        resetUI();
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

        // 発見UIの更新
        if (!isVisibleMemory && isVisible)
        {
            // ターゲットを発見
            resetUI();
            internalFrameCount = 120;
            TMP_exclamation.SetActive(true);
        }
        if (isVisibleMemory && !isVisible)
        {
            // ターゲットが見えなくなった
            resetUI();
            internalFrameCount = 120;
            TMP_quiet.SetActive(true);
        }
        isVisibleMemory = isVisible;
        
        // 発見UIのフレームカウント
        if (internalFrameCount == 0)
        {
            resetUI();
            internalFrameCount = -1;
        }
        if (internalFrameCount > 0)
        {
            internalFrameCount--;
        }

        // メインビヘイビア
        if (isVisible) // プレイヤーを視認しているとき
        {
            // プレイヤーメモリを更新
            playerPositionMemory = target.transform.position;
            // プレイヤメモリのy座標をデフォルト値に
            playerPositionMemory.Set(playerPositionMemory.x, this.transform.position.y, playerPositionMemory.z);
            // プレイヤーの位置に移動
            transform.position = Vector3.MoveTowards(transform.position, playerPositionMemory, speed * Time.deltaTime);
            // プレイヤーの方を見る
            transform.LookAt(target.transform);
            // EnemyShotを有効化
            enemyShot.isActiveEnemyShot = true;
        }
        else // プレイヤーを視認していないとき
        {
            if (playerPositionMemory == new Vector3(0, 0, 0))
            {
                // プレイヤーの位置がわからないので停止、一旦何もしない
                // 回転させてみる？ -> 微妙だったらコメントアウト
                if (isIdleRotation) {
                    transform.RotateAround(transform.Find("Center").position, new Vector3(0f, 1f, 0f), Time.deltaTime * 45f);
                    Vector3 rot = transform.eulerAngles;
                    rot.x = 0f;
                    rot.z = 0f;
                    transform.eulerAngles = rot;
                }
            }
            else
            {
                // 記憶したプレイヤーの位置まで動く
                transform.position = Vector3.MoveTowards(transform.position, playerPositionMemory, speed * Time.deltaTime);
                // 動き終わったらメモリーを000 -> 見失った
                if (transform.position == playerPositionMemory)
                {
                    playerPositionMemory = new Vector3(0, 0, 0);
                    resetUI();
                    internalFrameCount = 120;
                    TMP_question.SetActive(true);
                }
            }
            // enemyshotを無効化
            enemyShot.isActiveEnemyShot = false;
        }
    }
}