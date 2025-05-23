using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の基本的な挙動を管理する基底クラス
/// 各種敵クラスはこのクラスを継承してください
/// </summary>
/// <remarks>
/// Unity標準関数をオーバーラップしています
/// void Start() -> void Spawn()
/// Spawn()内でsetBaseParams()を使用してください
/// void Update() -> void Act()
/// </remarks>
public abstract class baseEnemy : MonoBehaviour
{
    /*<-+-*-~-=-=-~-*-+-member-+-*-~-=-=-~-*-+->*/

    /*status*/
    protected float speed = 1.0f;
    protected float sightAngle = 30.0f;
    protected float maxDistance = 20.0f;
    protected int maxHP = 100;
    protected bool isAct = false;
    protected int attackDamage = 1; //hitcount per hit 
    protected int score = 1000;
    protected RecoverAmmo item; //落とす弾のインスタンス

    /*system*/
    protected int HP;
    protected int damage = 0;
    protected bool isGetDamageOnFrame = false;
    protected GameObject target;
    protected GameObject targetCenter;
    protected Transform colliders;

    /*<-+-*-~-=-=-~-*-+-method-+-*-~-=-=-~-*-+->*/
    /// <summary>ターゲットが見えているかを判定する関数</summary>
    protected bool getIsVisible()
    {
        // ターゲットまでの向きと距離を計算
        var targetDir = target.transform.position - this.transform.position;
        var targetDistance = targetDir.magnitude;
        if (targetDistance > maxDistance) return false; // ターゲットが遠すぎる

        // 視野角の判定
        var cosHalf = Mathf.Cos(sightAngle / 2 * Mathf.Deg2Rad);
        var innerProduct = Vector3.Dot(this.transform.forward, targetDir.normalized);
        if (innerProduct < cosHalf) return false; // ターゲットが視野角外

        // レイキャストで障害物がないか判定
        // target == playerの時
        GameObject targetCollider = GameObject.FindGameObjectWithTag("PlayerCollider");
        int layerMask = ~(1 << LayerMask.NameToLayer("IgnoreRaycast"));
        if (Physics.Raycast(this.transform.position, targetDir, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            // なんでIgnoreRaycastしてるのにPlayerColliderが反応してるの？
            // Debug.Log(hit.collider.gameObject.name + " / " + target.name);
            if (hit.collider.gameObject != targetCollider) return false; // 一番近くのオブジェクトがターゲットじゃない
        }
        else return false; // レイキャスト失敗
        return true; // ターゲットを視認している
    }

    /// <summary>パラメータを敵にあわせて調整</summary>
    /// <param name="speed">敵の移動速度</param>
    /// <param name="sightAngle">敵の視野角度</param>
    /// <param name="maxDistance">敵の視認最大距離</param>
    /// <param name="maxHP">敵の最大HP</param>
    /// <param name="isAct">行動をするかどうか</param>
    /// <param name="HP">現在のHP</param>
    /// <param name="attackDamage">1回の攻撃で与えるダメージ</param>
    protected void setBaseParams(
        float speed = 1f,
        float sightAngle = 45f,
        float maxDistance = 20f,
        int maxHP = 100,
        bool isAct = false,
        int attackDamage = 1, //hitcount per hit
        int score = 1000
    )
    {
        this.speed = speed;
        this.sightAngle = sightAngle;
        this.maxDistance = maxDistance;
        this.maxHP = maxHP;
        this.isAct = isAct;
        this.attackDamage = attackDamage;
        this.score = score;
    }

    protected void setRecoverAmmo(RecoverAmmo item)
    {
        this.item = item;
    }

    protected void lootAmmo()
    {
        if (item != null) GameManager.GMInstance.addAmmo(item);
    }

    protected void move(Vector3 targetPos)
    {
        // ターゲットの位置に移動
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        transform.LookAt(targetPos);
    }

    protected void lookTarget()
    {
        this.transform.LookAt(targetCenter.transform);
    }

    /************ OVERRIDABLE METHOD ************/
    protected abstract void Spawn();
    protected abstract void Act();
    protected virtual void BossSpawn() {}
    protected virtual void OnDeath() {}


    /************ EVENT METHOD ************/
    void Start()
    {
        colliders = this.transform.Find("Collider").transform;
        target = GameObject.FindGameObjectWithTag("Player");
        targetCenter = GameObject.FindGameObjectWithTag("PlayerCollider");

        setRecoverAmmo(new recoverAmmos());

        Spawn();
        BossSpawn();

        HP = maxHP;
    }

    void Update()
    {
        // HPの減算
        isGetDamageOnFrame = damage > 0;
        if (isGetDamageOnFrame)
        {
            HP -= damage;
            // TODO ダメージエフェクト
            damage = 0;
        }

        // HPが0以下なら自身を破壊する
        if (HP < 0)
        {
            int scoreDelta = GameManager.GMInstance.addScore(score);
            MessageStream.MSInstance.addMessage(new KillMessage(gameObject.name, scoreDelta));
            GameManager.GMInstance.addCombo();
            lootAmmo();
            OnDeath();
            Destroy(this.gameObject);
            return;
        }

        isAct = GameManager.GMInstance.getIsAct();
        if (!isAct) return;

        Act();
    }

    /************ SHARE METHOD ************/
    public void addDamage(int deltaDamage) {
        this.damage += deltaDamage;
    }
}
