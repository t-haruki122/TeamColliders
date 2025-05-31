using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; // slider(HPバー)用

public class ShapeBoss2 : ShapeEnemy
{
    [SerializeField] protected int keyID = 1;
    protected int internalFrameCountForState = 0;
    protected int state = 0;
    private int bossHPstate = 0;
    protected override void BossSpawn()
    {
        setBaseParams(
            maxHP: 1200,
            score: 50000
        );
        setItem(new recoverAmmol(), new Key(keyID));
    }

    protected override void Act()
    {
        // HPバーの更新
        enemyHPBar.value = (float)HP / (float)maxHP;

        if (HP < 0.75f * maxHP && bossHPstate < 1) bossHPstate = 1;
        else if (HP < 0.5f * maxHP && bossHPstate < 2) bossHPstate = 2;
        else if (HP < 0.25f * maxHP && bossHPstate < 3) bossHPstate = 3;

        // 色の更新
        Color color = getColorFromHP();
        changeColor(color);

        // プレイヤーが見えるか？
        isVisible = isGetDamageOnFrame || getIsVisible();

        // stanceUIの更新
        if (!isVisibleMemory && isVisible) stance = 1; // 発見(ボスは一度だけ)
        updateStanceUI();

        if (isVisible) isVisibleMemory = true;
        
        if (state == 0 && isVisible){
            // 初発見
            state = 1;
        }
        
        if (state <= 0) return; // プレイヤー未発見
        
        // メインビヘイビア
        internalFrameCountForState++;
        if (internalFrameCountForState % 180 == 0) {
            internalFrameCountForState = 0;
            // ランダムステート
            switch (state) {
                case 1: state = 2; break;
                case 2: 
                    enemyShot.setShotState(0);
                    state = 3;
                    break;
                case 3:
                    enemyShot.setShotState(1);
                    changePosY(-1f);
                    state = 4;
                    break;
                case 4:
                    changePosY(1f);
                    enemyShot.setShotState(0);
                    state = 5;
                    break;
                case 5:
                    state = 1;
                    break;
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
            lookTarget();
            if (bossHPstate > 0) enemyShot.setShotState(1);
            enemyShot.isActiveEnemyShot = true;
        }
        if (state == 3) {
            // 移動攻撃
            lookTarget();
            rotateAtPosition(-240f);
            if (bossHPstate > 1) enemyShot.setShotState(1);
            enemyShot.isActiveEnemyShot = true;
        }
        if (state == 4) {
            // 回転しながら攻撃
            rotateAtPosition(240f);
            enemyShot.isActiveEnemyShot = true;
        }
        if (state == 5) {
            // プレイヤーに垂直に移動 TODO
            enemyShot.setShotState(0);
            enemyShot.isActiveEnemyShot = false;
        }
    }
    private void changePosY(float y) {
        Vector3 newPosition = transform.position;
        newPosition.y += y;
        transform.position = newPosition;
    }
}