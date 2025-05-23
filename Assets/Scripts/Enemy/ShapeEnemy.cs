using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; // slider(HPバー)用

public class ShapeEnemy : baseEnemy
{
    // 発見UI用のビックリマークとはてなマークと沈黙マーク
    protected GameObject TMP_exclamation;
    protected GameObject TMP_question;
    protected GameObject TMP_quiet;

    // ゲームオブジェクト
    protected Slider enemyHPBar;

    // 値を共有するスクリプト
    protected EnemyShot enemyShot;

    // 内部記憶_敵の記憶
    protected Vector3 playerPositionMemory = new Vector3(0, 0, 0);

    // 内部記憶_システム変数
    protected bool isVisible = false;
    protected bool isVisibleMemory = false;
    protected int internalFrameCount = -1;
    protected int stance = 0;

    // 敵のパラメータ
    [SerializeField] protected bool isIdleRotation = true;

    // 敵の初期色
    protected Color defaultColor;

    /*************** METHOD ***************/
    protected void resetUI()
    {
        TMP_exclamation.SetActive(false);
        TMP_question.SetActive(false);
        TMP_quiet.SetActive(false);
    }

    protected void changeColor(Color color)
    {
        if (colliders == null) return;

        foreach (Transform _collider in colliders)
        {
            var renderer = _collider.GetComponent<Renderer>();
            if (renderer == null) continue;

            // マテリアルを個別に複製（他オブジェクトに影響を与えない）
            renderer.material = new Material(renderer.material);
            renderer.material.color = color;
        }
    }

    protected Color getColorFromHP()
    {
        if (HP < maxHP / 2)
        {
            // HP(50%)以下で黄色 -> 赤色に変遷
            float _val = 0.5f - (float)HP / (float)maxHP;
            return new Color(1f, 1f - 2f * _val, 0.5f - _val);
        }
        else return defaultColor;
    }

    protected void updatePlayerPositionMemory()
    {
        // プレイヤーメモリを更新
        playerPositionMemory = target.transform.position;
        // プレイヤメモリのy座標をデフォルト値に
        playerPositionMemory.Set(playerPositionMemory.x, this.transform.position.y, playerPositionMemory.z);
    }

    protected void rotateAtPosition(float angularSpeed) {
        transform.RotateAround(transform.Find("Center").position, new Vector3(0f, 1f, 0f), Time.deltaTime * angularSpeed);
        Vector3 rot = transform.eulerAngles;
        rot.x = 0f;
        rot.z = 0f;
        transform.eulerAngles = rot;
    }

    protected void getStanceUI()
    {
        TMP_exclamation = transform.Find("StanceUI/TMP_exclamation").gameObject;
        TMP_question = transform.Find("StanceUI/TMP_question").gameObject;
        TMP_quiet = transform.Find("StanceUI/TMP_quiet").gameObject;
    }

    protected void updateStanceUI()
    {
        if (internalFrameCount == 0)
        {
            resetUI();
            internalFrameCount = -1;
            return;
        }
        if (internalFrameCount > 0) internalFrameCount--;
        if (stance == 0) return; // 変更なし
        resetUI();
        if (stance == 1)
        {
            TMP_exclamation.SetActive(true);
            stance = 0;
        }
        else if (stance == 2)
        {
            TMP_quiet.SetActive(true);
            stance = 0;
        }
        else if (stance == 3)
        {
            TMP_question.SetActive(true);
            stance = 0;
        }
        else return;
        internalFrameCount = 120;
    }

    /*************** EVENT METHOD ***************/
    protected override void Spawn()
    {
        setBaseParams(
        // 変更なし
        );

        enemyShot = transform.Find("EnemyShot").GetComponent<EnemyShot>();
        enemyShot.isActiveEnemyShot = false;

        enemyHPBar = transform.Find("StatusUI/Canvas/EnemyHPBar").GetComponent<Slider>();

        defaultColor = transform.Find("Collider/Body").GetComponent<Renderer>().material.color;

        getStanceUI();
        resetUI();
    }

    protected override void Act()
    {
        // HPバーの更新
        enemyHPBar.value = (float)HP / (float)maxHP;

        // 色の更新
        Color color = getColorFromHP();
        changeColor(color);

        // プレイヤーが見えるか？
        isVisible = isGetDamageOnFrame || getIsVisible();

        // stanceUIの更新
        if (!isVisibleMemory && isVisible) stance = 1; // 発見
        if (isVisibleMemory && !isVisible) stance = 2; // 見失ったが位置は記憶
        isVisibleMemory = isVisible;
        updateStanceUI();

        // メインビヘイビア
        if (isVisible) // プレイヤーを視認しているとき
        {
            updatePlayerPositionMemory();
            move(playerPositionMemory); // プレイヤーの位置に移動
            lookTarget(); // プレイヤーの方を見る
            enemyShot.isActiveEnemyShot = true; // EnemyShotを有効化 
        }
        else // プレイヤーを視認していないとき
        {
            enemyShot.isActiveEnemyShot = false;
            if (playerPositionMemory == new Vector3(0, 0, 0))
            {
                if (isIdleRotation)
                {
                    transform.RotateAround(transform.Find("Center").position, new Vector3(0f, 1f, 0f), Time.deltaTime * 45f);
                    Vector3 rot = transform.eulerAngles;
                    rot.x = 0f;
                    rot.z = 0f;
                    transform.eulerAngles = rot;
                }
            }
            else
            {
                move(playerPositionMemory); // 記憶したプレイヤーの位置まで動く
                if (transform.position == playerPositionMemory)
                {
                    playerPositionMemory = new Vector3(0, 0, 0);
                    stance = 3; // 完全に見失った
                }
            }
        }
    }
}