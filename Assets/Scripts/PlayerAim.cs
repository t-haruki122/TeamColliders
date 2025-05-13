using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    Animator animator;

    // カメラのゲームオブジェクト
    public GameObject playerFollowCamera;
    public GameObject mainCamera;
    public GameObject crossHair;

    // プレイヤーパラメーター用スクリプト
    private PlayerParams playerParams;

    private bool isAiming = false;

    // 他のスクリプトとisAimingを共有するためのメソッド
    public bool getIsAiming() {
        return isAiming;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // プレイヤーパラメータースクリプトを取得
        playerParams = GetComponent<PlayerParams>();
    }

    // Update is called once per frame
    void Update()
    {
        isAiming = Input.GetMouseButton(1); // 右クリック

        // 武器を持っていなかったらADSできなくする
        isAiming = playerParams.hasWeapon? isAiming: false;

        // Debug.Log(isAiming);
        animator.SetBool("isAiming", isAiming);

        // 右クリックを押したらPlayerFollowCameraを使わない、逆も然り
        playerFollowCamera.SetActive(!isAiming);

        // 右クリックでクロスヘアを表示、逆も然り
        crossHair.SetActive(isAiming);

        if (isAiming)
        {
            // エイム中はプレイヤーの向きとカメラの向きを合わせる
            // Aim.animに合わせ，y軸+40度の角度をつける
            Vector3 rot = mainCamera.transform.eulerAngles;
            rot.x = 0f;
            rot.y += 40f;
            rot.z = 0f;
            transform.eulerAngles = rot;
        }
    }
}
