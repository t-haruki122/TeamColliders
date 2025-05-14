using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    public GameObject shellPrefab;

    // public AudioClip sound; // TODO
    public bool isActivePlayerShot = true;
    public int firingRate = 15;
    public int shellSpeed = 2000;

    public bool ballisticGapCompensation = true;

    private int frameCount = 0;

    // メインカメラ
    public GameObject mainCamera;


    void Start()
    {

    }

    void Update()
    {
        bool isFire = Input.GetMouseButton(0); // 左クリック
        isActivePlayerShot = isFire;

        if (!isActivePlayerShot)
        {
            frameCount = firingRate - 1;
            return;
        }

        if (!GameManager.GMInstance.hasWeapon)
        {
            frameCount = firingRate - 1;
            return;
        }

        frameCount += 1;

        if (frameCount == firingRate)
        {
            frameCount = 0;

            Vector3 mainCameraDir = mainCamera.transform.rotation * Vector3.forward;
            Quaternion rotation = mainCamera.transform.rotation;
            // クロスヘアと弾道のギャップを補正
            if (ballisticGapCompensation) {
                if (Physics.Raycast(mainCamera.transform.position, mainCameraDir, out RaycastHit hit))
                {
                    if (hit.collider.gameObject.CompareTag("Enemy")) {
                        Debug.Log("Player target: "+ hit.collider.gameObject.name);
                        rotation = Quaternion.LookRotation(hit.collider.transform.GetComponent<Renderer>().bounds.center - transform.position);
                    }
                }
            }
            transform.rotation = rotation;

            // 弾を発射
            GameObject shell = Instantiate(shellPrefab, transform.position, rotation);
            Rigidbody shellRb = shell.GetComponent<Rigidbody>();

            // 弾速は自由に設定
            shellRb.AddForce(transform.forward * shellSpeed);

            // 発射音を出す TODO
            // AudioSource.PlayClipAtPoint(sound, transform.position);

            // ５秒後に砲弾を破壊する
            Destroy(shell, 5.0f);
        }
    }
}
