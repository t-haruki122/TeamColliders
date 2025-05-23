using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; // slider(HPバー)用

public class ShapeBoss : ShapeEnemy
{
    protected int internalFrameCountForState = 0;
    protected int state = 0;
    protected override void BossSpawn()
    {
        setBaseParams(
            maxHP: 1000,
            score: 10000
        );
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
        if (internalFrameCountForState % 120 == 0) {
            internalFrameCountForState = 0;
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
            lookTarget();
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