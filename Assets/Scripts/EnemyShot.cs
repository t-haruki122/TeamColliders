using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    public GameObject shellPrefab;
    // public AudioClip sound; // TODO
    public bool isActiveEnemyShot = false;

    private int frameCount = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (!isActiveEnemyShot)
        {
            return;
        }

        frameCount += 1;

        if (frameCount == 60)
        {
            frameCount = 0;
            GameObject shell = Instantiate(shellPrefab, transform.position, transform.rotation);
            Rigidbody shellRb = shell.GetComponent<Rigidbody>();

            // 弾速は自由に設定
            shellRb.AddForce(transform.forward * 500);

            // 発射音を出す TODO
            // AudioSource.PlayClipAtPoint(sound, transform.position);

            // ５秒後に砲弾を破壊する
            Destroy(shell, 5.0f);
        }
    }
}
