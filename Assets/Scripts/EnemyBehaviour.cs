using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // 自分自身
    [SerializeField] private Transform _self;
    // ターゲット
    [SerializeField] private Transform _target;
    // 視野角（度数法）
    [SerializeField] private float _sightAngle;
    // 視界の最大距離
    [SerializeField] private float _maxDistance = float.PositiveInfinity;

    public bool IsVisible()
    {
        var selfPos = _self.position;
        var targetPos = _target.position;

        // 自身の向き（正規化されたベクトル）
        var selfDir = _self.forward;
        
        // ターゲットまでの向きと距離計算
        var targetDir = targetPos - selfPos;
        var targetDistance = targetDir.magnitude;

        // cos(θ/2)を計算
        var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // 自身とターゲットへの向きの内積計算
        // ターゲットへの向きベクトルを正規化する必要があることに注意
        var innerProduct = Vector3.Dot(selfDir, targetDir.normalized);

        // 視界判定
        return innerProduct > cosHalf && targetDistance < _maxDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 視界判定
        var isVisible = IsVisible();

        // 結果表示
        Debug.Log("isVisible = " + isVisible);
    }
}

/*
webサイトからコピー(参照)
https://nekojara.city/unity-object-sight

public class SightCheckerExample : MonoBehaviour
{
    // 自分自身
    [SerializeField] private Transform _self;

    // ターゲット
    [SerializeField] private Transform _target;

    // 視野角（度数法）
    [SerializeField] private float _sightAngle;

    // 視界の最大距離
    [SerializeField] private float _maxDistance = float.PositiveInfinity;

    #region Logic

    /// <summary>
    /// ターゲットが見えているかどうか
    /// </summary>
    public bool IsVisible()
    {
        // 自身の位置
        var selfPos = _self.position;
        // ターゲットの位置
        var targetPos = _target.position;

        // 自身の向き（正規化されたベクトル）
        var selfDir = _self.forward;
        
        // ターゲットまでの向きと距離計算
        var targetDir = targetPos - selfPos;
        var targetDistance = targetDir.magnitude;

        // cos(θ/2)を計算
        var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // 自身とターゲットへの向きの内積計算
        // ターゲットへの向きベクトルを正規化する必要があることに注意
        var innerProduct = Vector3.Dot(selfDir, targetDir.normalized);

        // 視界判定
        return innerProduct > cosHalf && targetDistance < _maxDistance;
    }

    #endregion

    #region Debug

    // 視界判定の結果をGUI出力
    private void OnGUI()
    {
        // 視界判定
        var isVisible = IsVisible();

        // 結果表示
        GUI.Box(new Rect(20, 20, 150, 23), $"isVisible = {isVisible}");
    }

    #endregion
}
*/