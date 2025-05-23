using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    public GameObject shellPrefab;
    // public AudioClip sound; // TODO
    public bool isActiveEnemyShot = false;
    public int firingRate = 60;
    public int shellSpeed = 1000;
    public float shellDestroyTime = 5.0f;

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

        if (frameCount == firingRate)
        {
            frameCount = 0;
            GameObject shell = Instantiate(shellPrefab, transform.position, transform.rotation);
            shell.GetComponent<ShellBehaviour>().shellShooter = transform.parent.gameObject.name; // 弾に敵の名前を付与
            Rigidbody shellRb = shell.GetComponent<Rigidbody>();

            // 弾速は自由に設定
            shellRb.AddForce(transform.forward * shellSpeed);

            // 発射音を出す TODO
            // AudioSource.PlayClipAtPoint(sound, transform.position);

            // ５秒後に砲弾を破壊する
            Destroy(shell, shellDestroyTime);
        }
    }
}
