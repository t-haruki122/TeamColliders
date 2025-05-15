using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class baseEnemy : MonoBehaviour
{
    /*<-+-*-~-=-=-~-*-+-member-+-*-~-=-=-~-*-+->*/
    /*objects*/
    protected GameObject _self;
    protected GameObject _target;
    /*status*/
    protected float speed = 1.0f;
    protected float _sightAngle = 30.0f;
    protected float _maxDistance = 20.0f;
    protected const int maxHP = 100;
    protected bool isFriendly = false;
    protected bool isIdleRotation = false;
    protected int HP;
    protected int attackDamage = 1; //hitcount per hit 
    protected Item item; //落とす弾のインスタンス
    
    /*<-+-*-~-=-=-~-*-+-method-+-*-~-=-=-~-*-+->*/
    /*ターゲットが円錐の中に入っているか*/
    protected bool isInAngle()
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

    /*ターゲットとの間にオブジェクトがないか*/
    protected bool isNotObstructed()
    {
        // ターゲットまでの向きを計算
        var targetDir = _target.transform.position - _self.transform.position;

        // レイキャストで衝突判定
        int layerMask = ~(1 << LayerMask.NameToLayer("IgnoreRaycast"));
        if (Physics.Raycast(_self.transform.position, targetDir, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            return hit.collider.gameObject == _target;
        }
        return false;
    }
    /*倒された際弾を回復*/
    public void lootAmmo() { 
        if(item != null) GameManager.GMInstance.addAmmo(item); 
    }
}
