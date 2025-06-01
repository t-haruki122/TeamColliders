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
    private int shotState = 0;  //0:単発, 1:散弾
    private int frameCount = 0;
    private bool isChasingPlayer = true;

    void Start()
    {
        
    }

    void Update()
    {
        if (isChasingPlayer) transform.LookAt(GameObject.FindGameObjectWithTag("PlayerCollider").transform);

        if (!isActiveEnemyShot)
        {
            return;
        }

        frameCount += 1;

        if (frameCount == firingRate)
        {
            frameCount = 0;

            switch (shotState) {
                case 0:
                    baseAct();
                    break;
                case 1:
                    diffuse();
                    break;
            }
            
        }
    }
    public void setShotState(int state) { shotState = state; }
    public void setIsChasingPlayer(bool TF) { isChasingPlayer = TF; }

    private void baseAct() {
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

    private void diffuse() {
        GameObject[] shells = new GameObject[4];
        float[] angles = new float[] {-30f, -10f, 10f, 30f};
        Rigidbody[] shellRbs = new Rigidbody[4];
        
        for (int i = 0; i < 4; ++i) {
            Quaternion bulletQuaternion = transform.rotation * Quaternion.Euler(0, 0, angles[i]);
            shells[i] = Instantiate(shellPrefab, transform.position + transform.right * (angles[i] / 20f), bulletQuaternion);
            shells[i].GetComponent<ShellBehaviour>().shellShooter = transform.parent.gameObject.name;
            shellRbs[i] = shells[i].GetComponent<Rigidbody>();
            shellRbs[i].AddForce(transform.forward * shellSpeed);
        }
        for (int i = 0; i < 4; ++i) Destroy(shells[i], 2f);
    }
}
