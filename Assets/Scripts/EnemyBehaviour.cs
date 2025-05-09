using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; // slider(HPバー)用

public class EnemyBehaviour : MonoBehaviour
{
    // 発見UI用のビックリマークとはてなマークと沈黙マーク
    private GameObject TMP_exclamation;
    private GameObject TMP_question;
    private GameObject TMP_quiet;

    // HPバー用のスライダー
    private Slider enemyHPBar;

    // ゲームオブジェクト
    private GameObject _self;
    private GameObject _target;

    // 値を共有するスクリプト
    private EnemyShot enemyShot;

    // 敵のパラメータ
    [SerializeField] float speed = 1.0f;
    [SerializeField] float _sightAngle = 30.0f;
    [SerializeField] float _maxDistance = 20.0f;
    [SerializeField] int maxHP = 100;

    // 内部記憶_敵の記憶
    private Vector3 playerPositionMemory = new Vector3(0, 0, 0);
    public int HP; // TODO 後でprivateに！

    // 内部記憶_システム変数
    private bool isVisibleMemory = false;
    private int internalFrameCount = -1;

    // ターゲットが円錐の中に入っているか調べる
    private bool isInAngle()
    {   
        // ターゲットまでの向きと距離を計算
        var targetDir = _target.transform.position - _self.transform.position;
        var targetDistance = targetDir.magnitude;

        // cos(θ/2)を計算
        var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // 自身とターゲットへの向きの内積計算
        // ターゲットへの向きベクトルを正規化する必要があることに注意
        var innerProduct = Vector3.Dot(_self.transform.forward, targetDir.normalized);

        // 視界判定
        return innerProduct > cosHalf && targetDistance < _maxDistance;
    }

    // ターゲットとの間にオブジェクトがないか調べる
    private bool isNotObstructed()
    {
        // ターゲットまでの向きを計算
        var targetDir = _target.transform.position - _self.transform.position;

        // レイキャストで衝突判定
        if (Physics.Raycast(_self.transform.position, targetDir, out RaycastHit hit))
        {
            return hit.collider.gameObject == _target;
        }
        return false;
    }

    private void reset()
    {
        TMP_exclamation.SetActive(false);
        TMP_question.SetActive(false);
        TMP_quiet.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _self = this.gameObject;
        _target = GameObject.FindGameObjectWithTag("Player");

        Transform _enemyShot = transform.Find("EnemyShot");
        enemyShot = _enemyShot.GetComponent<EnemyShot>();

        enemyHPBar = transform.Find("StatusUI/Canvas/EnemyHPBar").GetComponent<Slider>();

        TMP_exclamation = transform.Find("StanceUI/TMP_exclamation").gameObject;
        TMP_question = transform.Find("StanceUI/TMP_question").gameObject;
        TMP_quiet = transform.Find("StanceUI/TMP_quiet").gameObject;

        HP = maxHP;

        reset();
    }

    // Update is called once per frame
    void Update()
    {
        // HPが0以下なら自身を破壊する
        // TODO 破壊アニメーション
        if (HP < 0) {
            Destroy(this.gameObject);
            return;
        }

        // HPバーの更新
        enemyHPBar.value = HP;

        // 敵の色の更新
        // デフォルトカラー!!マテリアルの色変えたら変えること！！
        Color color = new Color(0.5f, 0.5f, 0.5f);
        if (HP < maxHP / 2)
        {
            // HP(50%)以下で黄色 -> 赤色に変遷
            float _val = 0.5f - (float)HP / (float)maxHP;
            Debug.Log(_val);
            color = new Color(1f, 1f - 2f * _val, 0.5f - _val);
        }
        transform.Find("Body").GetComponent<Renderer>().material.color = color;
        transform.Find("Body/WingRight").GetComponent<Renderer>().material.color = color;
        transform.Find("Body/WingLeft").GetComponent<Renderer>().material.color = color;
        transform.Find("Body/Halo").GetComponent<Renderer>().material.color = color;

        // プレイヤーが見えるか？
        var isVisible = isInAngle() && isNotObstructed();

        // TODO 見えなくとも、プレイヤーに攻撃されたらisVisible = trueとする。

        // 発見UIの更新
        if (!isVisibleMemory && isVisible)
        {
            // ターゲットを発見
            reset();
            internalFrameCount = 120;
            TMP_exclamation.SetActive(true);
        }
        if (isVisibleMemory && !isVisible)
        {
            // ターゲットが見えなくなった
            reset();
            internalFrameCount = 120;
            TMP_quiet.SetActive(true);
        }
        isVisibleMemory = isVisible;
        
        // 発見UIのフレームカウント
        if (internalFrameCount == 0)
        {
            reset();
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
            playerPositionMemory = _target.transform.position;
            // プレイヤメモリのy座標をデフォルト値に
            playerPositionMemory.Set(playerPositionMemory.x, _self.transform.position.y, playerPositionMemory.z);
            // プレイヤーの位置に移動
            transform.position = Vector3.MoveTowards(transform.position, playerPositionMemory, speed * Time.deltaTime);
            // プレイヤーの方を見る
            transform.LookAt(_target.transform);
            // EnemyShotを有効化
            enemyShot.isActiveEnemyShot = true;
        }
        else // プレイヤーを視認していないとき
        {
            if (playerPositionMemory == new Vector3(0, 0, 0))
            {
                // プレイヤーの位置がわからないので停止、一旦何もしない

                // 回転させてみる？ -> 微妙だったらコメントアウト
                transform.Rotate(0, 45*Time.deltaTime, 0);
            }
            else
            {
                // 記憶したプレイヤーの位置まで動く
                transform.position = Vector3.MoveTowards(transform.position, playerPositionMemory, speed * Time.deltaTime);
                // 動き終わったらメモリーを000 -> 見失った
                if (transform.position == playerPositionMemory)
                {
                    playerPositionMemory = new Vector3(0, 0, 0);
                    reset();
                    internalFrameCount = 120;
                    TMP_question.SetActive(true);
                }
            }
            // enemyshotを無効化
            enemyShot.isActiveEnemyShot = false;
        }

        // 結果表示
        // Debug.Log("isVisible:" + isVisible + " isVisibleMemory:" + isVisibleMemory + " ifc:" + internalFrameCount);
        // Debug.Log("isInAngle: " + isInAngle() + ", isNotObstructed: " + isNotObstructed());
    }
}