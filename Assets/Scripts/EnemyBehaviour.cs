using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // 発見UI用のビックリマークとはてなマーク
    [SerializeField] private GameObject TMP_exclamation;
    [SerializeField] private GameObject TMP_question;

    // ゲームオブジェクト
    private GameObject _self;
    private GameObject _target;

    // 敵のパラメータ
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float _sightAngle = 30.0f;
    [SerializeField] private float _maxDistance = 20.0f;

    // 内部記憶_敵の記憶
    private Vector3 playerPositionMemory = new Vector3(0, 0, 0);

    // 内部記憶_システム変数
    private bool isVisibleMemory = false;
    private int internalFrameCount = -1;

    public bool isInAngle()
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

    public bool isNotObstructed()
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

    // Start is called before the first frame update
    void Start()
    {
        _self = this.gameObject;
        _target = GameObject.FindGameObjectWithTag("Player");
        TMP_exclamation.SetActive(false);
        TMP_question.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 発見UIの更新
        var isVisible = isInAngle() && isNotObstructed();
        if (!isVisibleMemory && isVisible)
        {
            Start();
            // ターゲットを発見
            internalFrameCount = 120;
            TMP_exclamation.SetActive(true);
        }
        if (isVisibleMemory && !isVisible)
        {
            Start();
            // ターゲットを見失った
            internalFrameCount = 120;
            TMP_question.SetActive(true);
        }
        isVisibleMemory = isVisible;
        
        // 発見UIのフレームカウント
        if (internalFrameCount == 0)
        {
            Start();
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
            // プレイヤーの位置に移動
            transform.position = Vector3.MoveTowards(transform.position, playerPositionMemory, speed * Time.deltaTime);
        }
        else // プレイヤーを視認していないとき
        {
            if (playerPositionMemory == new Vector3(0, 0, 0))
            {
                // プレイヤーの位置がわからない(000)ので停止、一旦何もしない
            }
            else
            {
                // 記憶したプレイヤーの位置まで動く
                transform.position = Vector3.MoveTowards(transform.position, playerPositionMemory, speed * Time.deltaTime);
                // 動き終わったらメモリーを000
                if (transform.position == playerPositionMemory)
                {
                    playerPositionMemory = new Vector3(0, 0, 0);
                }
            }
        }

        // 結果表示
        // Debug.Log("isVisible:" + isVisible + " isVisibleMemory:" + isVisibleMemory + " ifc:" + internalFrameCount);
        // Debug.Log("isInAngle: " + isInAngle() + ", isNotObstructed: " + isNotObstructed());
    }
}